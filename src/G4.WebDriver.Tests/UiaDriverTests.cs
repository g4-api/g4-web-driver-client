using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Uia;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace G4.WebDriver.Tests
{
    [TestClass]
    [TestCategory("UiaDriver")]
    [TestCategory("WindowsAppTest")]
    public class UiaDriverTests : TestBase<UiaDriver>
    {
        [TestMethod(DisplayName = "Verify that repeated UIA attribute commands resolve each attribute from an isolated route template.")]
        [TestCategory("UnitTest")]
        public void GetAttributeSequentialNamesTest()
        {
            // Arrange: record local requests so attribute route expansion is verified without a live UIA server.
            var messageHandler = new RecordingHttpMessageHandler();
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            var invoker = NewCommandInvoker(httpClient);
            using var driver = NewRemoteDriver(invoker);
            var element = new UiaElement(driver, id: "right-id");

            // Act: request two attributes through one element and one command registry.
            var name = element.GetAttribute("Name");
            var automationId = element.GetAttribute("AutomationId");

            // Assert: retain both successful responses so route verification covers completed command invocations.
            Assert.AreEqual("captured", name);
            Assert.AreEqual("captured", automationId);

            // Assert: preserve each changing attribute in its own endpoint instead of reusing the first expanded route.
            var requestUris = messageHandler.GetRequestUris();
            Assert.HasCount(2, requestUris);
            Assert.AreEqual("/session/session-id/element/right-id/attribute/Name", requestUris[0].AbsolutePath);
            Assert.AreEqual("/session/session-id/element/right-id/attribute/AutomationId", requestUris[1].AbsolutePath);

            // Assert: keep the registered route reusable after every invocation.
            var template = invoker.Commands["GetUser32Attribute"];
            Assert.AreEqual(
                "/session/$[session]/element/$[element]/attribute/$[attributeName]",
                template.Route);
        }

        [TestMethod(DisplayName = "Verify that invoking a UIA command preserves the caller-owned route and registered template.")]
        [TestCategory("UnitTest")]
        public void InvokeCommandPreservesCallerRouteTest()
        {
            // Arrange: create one invocation-owned focus command whose route still contains both placeholders.
            var messageHandler = new RecordingHttpMessageHandler();
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            var invoker = NewCommandInvoker(httpClient);
            var command = invoker.NewCommand(commandName: "SetUser32Focus");
            command.Element = "right-id";

            // Act: send the command through the production invoker where route expansion occurs.
            invoker.Invoke(command);

            // Assert: send the resolved right-element endpoint while retaining the caller-owned route.
            var requestUris = messageHandler.GetRequestUris();
            Assert.HasCount(1, requestUris);
            Assert.AreEqual(
                "/session/session-id/user32/element/right-id/focus",
                requestUris[0].AbsolutePath);

            // Assert: keep both caller and registry routes reusable after transport completes.
            const string ExpectedRoute = "/session/$[session]/user32/element/$[element]/focus";
            Assert.AreEqual(ExpectedRoute, command.Route);
            Assert.AreEqual(ExpectedRoute, invoker.Commands["SetUser32Focus"].Route);
        }

        [TestMethod(DisplayName = "Verify that a new application driver can be instantiated and perform UI interactions correctly.")]
        public void NewApplicationDriverTest()
        {
            // Arrange: redirect the test driver to the UIA binaries used by the Windows application fixture.
            var binariesPath = $"{TestContext.Properties["Grid.Endpoint"]}";
            TestContext.Properties["Grid.Endpoint"] = Path.Combine(binariesPath, "UiaDriverServer");

            // Arrange: configure the application and stable automation identifiers needed for the interaction.
            var options = new UiaOptions
            {
                App = "E:\\Binaries\\Automation\\SimpleWpfTestApp.exe"
            };
            var textbox = By.Xpath("//Edit[@AutomationId='InputTextBoxAutomationId']");
            var button = By.Xpath("//Button[@AutomationId='SubmitButtonAutomationId']");
            var label = By.Xpath("//Text[@AutomationId='OutputLabelAutomationId']");

            // Act: exercise text entry and submission through the complete UIA driver lifecycle.
            Invoke(
                TestContext,
                options,
                url: string.Empty,
                action: driver =>
                {
                    // Act: enter and submit text before querying the resulting label state.
                    driver.FindElement(textbox).SendKeys("Foo");
                    driver.FindElement(button).Click();

                    // Assert: verify the application received the input and published the expected label.
                    var actual = driver.FindElement(label).GetAttribute("Name");
                    Assert.AreEqual("You entered: Foo", actual, $"Output label text mismatch. Expected 'You entered: Foo' but got '{actual}'.");
                }
            );
        }

        [TestMethod(DisplayName = "Verify that new commands own independent request state while the registered template remains unchanged.")]
        [TestCategory("UnitTest")]
        public void NewCommandIndependentInstancesTest()
        {
            // Arrange: register UIA command templates in an invoker that performs no network request for this copy test.
            var messageHandler = new RecordingHttpMessageHandler();
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            var invoker = NewCommandInvoker(httpClient);

            // Act: create two commands from one template and mutate only the first command.
            var leftCommand = invoker.NewCommand(commandName: "SetUser32Focus");
            var rightCommand = invoker.NewCommand(commandName: "SetUser32Focus");
            leftCommand.Element = "left-id";
            leftCommand.Route = leftCommand.Route
                .Replace("$[session]", leftCommand.Session)
                .Replace("$[element]", leftCommand.Element);

            // Assert: expose distinct instances so request state cannot leak between invocations.
            Assert.AreNotSame(leftCommand, rightCommand);
            Assert.AreNotSame(leftCommand, invoker.Commands["SetUser32Focus"]);
            Assert.AreNotSame(rightCommand, invoker.Commands["SetUser32Focus"]);
            Assert.IsNull(rightCommand.Element);

            // Assert: retain placeholders in both the untouched command and registered template.
            const string ExpectedRoute = "/session/$[session]/user32/element/$[element]/focus";
            Assert.AreEqual(ExpectedRoute, rightCommand.Route);
            Assert.AreEqual(ExpectedRoute, invoker.Commands["SetUser32Focus"].Route);
        }

        [TestMethod(DisplayName = "Verify that a new UiaDriver can be instantiated and is not ready by default.")]
        public void NewDesktopDriverTest()
        {
            // Arrange: redirect the test driver to the UIA binaries used by the desktop fixture.
            var binariesPath = $"{TestContext.Properties["Grid.Endpoint"]}";
            TestContext.Properties["Grid.Endpoint"] = Path.Combine(binariesPath, "UiaDriverServer");

            // Act: instantiate the desktop driver through the shared lifecycle without opening an application URL.
            Invoke(
                TestContext,
                options: default,
                url: string.Empty,
                action: driver =>
                {
                    // Assert: retain the single-Windows-instance service state and a valid driver instance.
                    Assert.IsFalse(
                        driver.Invoker.WebDriverService.Ready,
                        "WebDriverService is ready even though UIA supports only one Windows instance.");

                    Assert.IsNotNull(driver, "Driver instance is null. Expected a valid driver instance to be created.");
                }
            );
        }

        [TestMethod(DisplayName = "Verify that parallel UIA focus commands retain their own element routes.")]
        [TestCategory("UnitTest")]
        public async Task SetFocusParallelElementsTestAsync()
        {
            // Arrange: share one invoker across two UIA elements so concurrent command ownership is exercised directly.
            var messageHandler = new RecordingHttpMessageHandler();
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            var invoker = NewCommandInvoker(httpClient);
            using var driver = NewRemoteDriver(invoker);
            var leftElement = new UiaElement(driver, id: "left-id");
            var rightElement = new UiaElement(driver, id: "right-id");

            var cancellationToken = new CancellationTokenSource().Token;

            // Act: invoke both focus requests concurrently to expose any shared route or element state.
            await Task.WhenAll(
                Task.Run(action: leftElement.SetFocus, cancellationToken),
                Task.Run(action: rightElement.SetFocus, cancellationToken));

            // Assert: receive exactly one request for each element regardless of completion order.
            var requestUris = messageHandler.GetRequestUris();
            var actualPaths = Array.ConvertAll(requestUris, uri => uri.AbsolutePath);
            var expectedPaths = new[]
            {
                "/session/session-id/user32/element/left-id/focus",
                "/session/session-id/user32/element/right-id/focus"
            };

            Assert.HasCount(2, requestUris);
            Assert.AreSequenceEqual(expectedPaths, actualPaths);

            // Assert: retain the registered placeholders after concurrent invocation.
            Assert.AreEqual(
                "/session/$[session]/user32/element/$[element]/focus",
                invoker.Commands["SetUser32Focus"].Route);
        }

        [TestMethod(DisplayName = "Verify that sequential UIA focus commands target the left and right elements independently.")]
        [TestCategory("UnitTest")]
        public void SetFocusSequentialElementsTest()
        {
            // Arrange: share one invoker across left and right UIA elements to reproduce the recorded twin-panel flow.
            var messageHandler = new RecordingHttpMessageHandler();
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            var invoker = NewCommandInvoker(httpClient);
            using var driver = NewRemoteDriver(invoker);
            var leftElement = new UiaElement(driver, id: "left-id");
            var rightElement = new UiaElement(driver, id: "right-id");

            // Act: focus the left element first and the right element second through the production UiaElement path.
            leftElement.SetFocus();
            rightElement.SetFocus();

            // Assert: preserve each element identifier in the exact request order used by the recording.
            var requestUris = messageHandler.GetRequestUris();
            Assert.HasCount(2, requestUris);
            Assert.AreEqual(
                "/session/session-id/user32/element/left-id/focus",
                requestUris[0].AbsolutePath);
            Assert.AreEqual(
                "/session/session-id/user32/element/right-id/focus",
                requestUris[1].AbsolutePath);

            // Assert: retain the registered placeholders for later focus actions.
            Assert.AreEqual(
                "/session/$[session]/user32/element/$[element]/focus",
                invoker.Commands["SetUser32Focus"].Route);
        }

        // Creates a test-only command invoker whose templates cover the changing UIA routes exercised in this class.
        // The caller owns the supplied HTTP client, while each command invocation owns its copied request state.
        private static WebDriverCommandInvoker NewCommandInvoker(HttpClient httpClient)
        {
            // Bind transport to an in-memory handler while retaining a complete absolute URI for route assertions.
            var invoker = new WebDriverCommandInvoker(
                serverAddress: new Uri(uriString: "http://localhost:4444"),
                httpClient,
                timeout: TimeSpan.FromSeconds(5),
                keepAlive: false)
            {
                Session = new SessionIdModel(opaqueKey: "session-id")
            };

            // Register the element and attribute templates needed to detect placeholder contamination.
            invoker.AddCommand(
                commandName: "GetUser32Attribute",
                command: new WebDriverCommandModel
                {
                    Method = HttpMethod.Get,
                    Route = "/session/$[session]/element/$[element]/attribute/$[attributeName]"
                });
            invoker.AddCommand(
                commandName: "SetUser32Focus",
                command: new WebDriverCommandModel
                {
                    Method = HttpMethod.Get,
                    Route = "/session/$[session]/user32/element/$[element]/focus"
                });

            return invoker;
        }

        // Creates a remote driver with session creation disabled so UiaElement uses the supplied deterministic invoker.
        // Disposal uses the same local handler and releases no external process or desktop resource.
        private static RemoteWebDriver NewRemoteDriver(WebDriverCommandInvoker invoker)
        {
            // Disable remote session allocation because the test controls the opaque session identifier.
            var session = new SessionModel
            {
                StartNewSession = false
            };

            // Attach the invoker session to the driver before any UiaElement captures it.
            return new RemoteWebDriver(invoker, session)
            {
                Session = invoker.Session
            };
        }

        // Captures request endpoints in a thread-safe queue and returns a stable WebDriver response.
        // The handler performs no network I/O and exposes snapshots so tests cannot mutate its owned request history.
        private sealed class RecordingHttpMessageHandler : HttpMessageHandler
        {
            // Owns the request URI history shared by sequential and parallel command tests.
            private readonly ConcurrentQueue<Uri> _requestUris = new();

            // Returns a detached request snapshot so assertions observe completed requests without sharing mutable state.
            internal Uri[] GetRequestUris()
            {
                return [.. _requestUris];
            }

            /// <inheritdoc />
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                // Capture the resolved endpoint before returning a successful protocol response.
                _requestUris.Enqueue(request.RequestUri);

                // Return a stable value that supports both focus and attribute response parsing.
                var response = new HttpResponseMessage(statusCode: HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        content: "{\"value\":\"captured\"}",
                        encoding: Encoding.UTF8,
                        mediaType: "application/json"),
                    RequestMessage = request
                };

                return Task.FromResult(result: response);
            }
        }
    }
}
