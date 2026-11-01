using G4.WebDriver.Exceptions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Sap;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace G4.WebDriver.Tests
{
    [TestClass]
    [TestCategory("SapDriver")]
    [TestCategory("UnitTest")]
    public class SapDriverTests
    {
        // Names the W3C element-reference key that the SAP server uses when a method returns a control.
        private const string ElementReferenceKey = "element-6066-11e4-a52e-4f735466cecf";

        // Holds the JSON media type of every protocol response that is not an XML document.
        private const string JsonMediaType = "application/json";

        // Holds the XML media type the SAP server uses for object model documents.
        private const string XmlMediaType = "application/xml";

        [TestMethod(DisplayName = "Verify that GetDocument with a locator posts the " +
            "W3C locator and returns the element subtree XML.")]
        public void GetDocumentElementTest()
        {
            // Arrange: answer with the XML subtree of the located element.
            const string Document = "<GuiTextField id=\"wnd[0]/usr/txtUser\" />";
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: Document,
                mediaType: XmlMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);

            // Act: request the subtree of the element located by an XPath locator.
            var actual = driver.GetDocument(By.Xpath("//GuiTextField"));

            // Assert: return the XML body unchanged after a POST on the element document route.
            var requests = messageHandler.GetRequests();
            Assert.AreEqual(Document, actual);
            Assert.HasCount(1, requests);
            Assert.AreEqual(HttpMethod.Post, requests[0].Method);
            Assert.AreEqual("/session/session-id/element/dom", requests[0].Uri.AbsolutePath);

            // Assert: send the W3C locator body the server reads as its location strategy.
            using var body = JsonDocument.Parse(requests[0].Body);
            Assert.AreEqual("xpath", body.RootElement.GetProperty("using").GetString());
            Assert.AreEqual("//GuiTextField", body.RootElement.GetProperty("value").GetString());
        }

        [TestMethod(DisplayName = "Verify that GetDocument raises a WebDriverException when " +
            "the server answers with an XML error document.")]
        public void GetDocumentFailedStatusTest()
        {
            // Arrange: answer with a failed status and the XML error document the server writes on failure.
            const string ErrorDocument = "<Error>The session does not exist.</Error>";
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.InternalServerError,
                content: ErrorDocument,
                mediaType: XmlMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);

            // Act: request the document from a server that cannot produce it.
            var exception = Assert.Throws<WebDriverException>(() => driver.GetDocument());

            // Assert: surface the server's error document so the caller sees why the request failed.
            Assert.Contains("500", exception.Message);
            Assert.Contains(ErrorDocument, exception.Message);
        }

        [TestMethod(DisplayName = "Verify that GetDocument reads the raw XML " +
            "body of the session object model document.")]
        public void GetDocumentSessionTest()
        {
            // Arrange: answer with a raw XML document, the way the server writes it outside the JSON envelope.
            const string Document = "<Session><Window id=\"wnd[0]\" /></Session>";
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: Document,
                mediaType: XmlMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);

            // Act: request the document of every open window of the attached session.
            var actual = driver.GetDocument();

            // Assert: return the XML body unchanged after a GET on the session document route.
            var requests = messageHandler.GetRequests();
            Assert.AreEqual(Document, actual);
            Assert.HasCount(1, requests);
            Assert.AreEqual(HttpMethod.Get, requests[0].Method);
            Assert.AreEqual("/session/session-id/dom", requests[0].Uri.AbsolutePath);
        }

        [TestMethod(DisplayName = "Verify that GetStatusBar maps the status bar " +
            "JSON value to the typed status bar model.")]
        public void GetStatusBarTest()
        {
            // Arrange: answer with a success message in the status bar.
            var json =
                "{\n" +
                "    \"value\": {\n" +
                "        \"messageAsPopup\": false,\n" +
                "        \"messageId\": \"00\",\n" +
                "        \"messageNumber\": \"344\",\n" +
                "        \"messageType\": \"S\",\n" +
                "        \"text\": \"Data was saved\"\n" +
                "    }\n" +
                "}";
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: json,
                mediaType: JsonMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);

            // Act: read the status bar of the attached session.
            var statusBar = driver.GetStatusBar();

            // Assert: request the status bar route once.
            var requests = messageHandler.GetRequests();
            Assert.HasCount(1, requests);
            Assert.AreEqual(HttpMethod.Get, requests[0].Method);
            Assert.AreEqual("/session/session-id/sap/statusbar", requests[0].Uri.AbsolutePath);

            // Assert: map every status bar field from the JSON value.
            Assert.IsFalse(statusBar.MessageAsPopup);
            Assert.AreEqual("00", statusBar.MessageId);
            Assert.AreEqual("344", statusBar.MessageNumber);
            Assert.AreEqual("S", statusBar.MessageType);
            Assert.AreEqual("Data was saved", statusBar.Text);
        }

        [TestMethod(DisplayName = "Verify that InvokeMethod converts an array result into scalars and SAP elements.")]
        public void InvokeMethodArrayResultTest()
        {
            // Arrange: answer with a collection that mixes a number, a string, and a control reference.
            var json = "{\"value\":[7,\"Row\",{\"" + ElementReferenceKey + "\":\"cell-id\"}]}";
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: json,
                mediaType: JsonMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);
            var element = new SapElement(driver, id: "grid-id");

            // Act: call a method whose result is a collection.
            var result = element.InvokeMethod("getCells");

            // Assert: convert each item to the value the caller works with.
            var items = result as object[];
            Assert.IsNotNull(items);
            Assert.HasCount(3, items);
            Assert.AreEqual(7L, items[0]);
            Assert.AreEqual("Row", items[1]);

            // Assert: wrap the control reference as a SAP element owned by the same driver.
            var cell = items[2] as SapElement;
            Assert.IsNotNull(cell);
            Assert.AreEqual("cell-id", cell.Id);
        }

        [TestMethod(DisplayName = "Verify that InvokeMethod returns a SAP element " +
            "when the method returns a control reference.")]
        public void InvokeMethodElementResultTest()
        {
            // Arrange: answer with a control reference, the way the server returns a control result.
            var json = "{\"value\":{\"" + ElementReferenceKey + "\":\"child-id\"}}";
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: json,
                mediaType: JsonMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);
            var element = new SapElement(driver, id: "tree-id");

            // Act: call an allow-listed method with positional arguments.
            var result = element.InvokeMethod("findNodeByKey", "N1", 2);

            // Assert: post the method call to the element's call route.
            var requests = messageHandler.GetRequests();
            Assert.HasCount(1, requests);
            Assert.AreEqual(HttpMethod.Post, requests[0].Method);
            Assert.AreEqual("/session/session-id/sap/element/tree-id/call", requests[0].Uri.AbsolutePath);

            // Assert: send the method name and the arguments in call order.
            using var body = JsonDocument.Parse(requests[0].Body);
            var arguments = body.RootElement.GetProperty("arguments");
            Assert.AreEqual("findNodeByKey", body.RootElement.GetProperty("method").GetString());
            Assert.AreEqual(2, arguments.GetArrayLength());
            Assert.AreEqual("N1", arguments[0].GetString());
            Assert.AreEqual(2, arguments[1].GetInt32());

            // Assert: wrap the returned control as a SAP element.
            var child = result as SapElement;
            Assert.IsNotNull(child);
            Assert.AreEqual("child-id", child.Id);
        }

        [TestMethod(DisplayName = "Verify that InvokeMethod returns null when the method has no result.")]
        public void InvokeMethodEmptyResultTest()
        {
            // Arrange: answer with the empty object the server returns for a method without a result.
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: "{\"value\":{}}",
                mediaType: JsonMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);
            var element = new SapElement(driver, id: "grid-id");

            // Act: call a method without arguments and without a result.
            var result = element.InvokeMethod("pressToolbarButton");

            // Assert: report the missing result as null and send an empty argument list.
            var requests = messageHandler.GetRequests();
            using var body = JsonDocument.Parse(requests[0].Body);
            Assert.IsNull(result);
            Assert.AreEqual(0, body.RootElement.GetProperty("arguments").GetArrayLength());
        }

        [TestMethod(DisplayName = "Verify that sequential SAP method calls target " +
            "each element and keep the registered route template.")]
        public void InvokeMethodSequentialElementsTest()
        {
            // Arrange: share one driver between two elements to expose any shared route or element state.
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: "{\"value\":{}}",
                mediaType: JsonMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);
            var leftElement = new SapElement(driver, id: "left-id");
            var rightElement = new SapElement(driver, id: "right-id");

            // Act: call the same method on the left element first and the right element second.
            leftElement.InvokeMethod("pressToolbarButton");
            rightElement.InvokeMethod("pressToolbarButton");

            // Assert: send each call to its own element route, in call order.
            var requests = messageHandler.GetRequests();
            Assert.HasCount(2, requests);
            Assert.AreEqual("/session/session-id/sap/element/left-id/call", requests[0].Uri.AbsolutePath);
            Assert.AreEqual("/session/session-id/sap/element/right-id/call", requests[1].Uri.AbsolutePath);

            // Assert: keep the registered template's placeholders for later calls.
            Assert.AreEqual(
                "/session/$[session]/sap/element/$[element]/call",
                driver.Invoker.Commands["InvokeSapElementMethod"].Route);
        }

        [TestMethod(DisplayName = "Verify that clearing a SapOptions value removes " +
            "it from the getter and the capabilities.")]
        public void NewCapabilitiesClearedOptionTest()
        {
            // Arrange: set a session path, then clear it to fall back to index selection.
            var options = new SapOptions
            {
                SessionPath = "/app/con[0]/ses[0]",
                SessionIndex = 1
            };
            options.SessionPath = null;

            // Act: convert the options after the session path was cleared.
            var capabilities = options.ConvertToCapabilities();

            // Assert: drop the cleared value from both the getter and the sap:options capability.
            var sapOptions = capabilities.AlwaysMatch["sap:options"] as IDictionary<string, object>;
            Assert.IsNull(options.SessionPath);
            Assert.IsNotNull(sapOptions);
            Assert.IsFalse(sapOptions.ContainsKey("sessionPath"));
            Assert.AreEqual(1, sapOptions["sessionIndex"]);
        }

        [TestMethod(DisplayName = "Verify that SapOptions sends the SAP browser " +
            "name and every set option, including zero indexes.")]
        public void NewCapabilitiesSapOptionsTest()
        {
            // Arrange: select the first session of the first connection and target a grid label.
            var options = new SapOptions
            {
                ConnectionIndex = 0,
                Label = "sap-host",
                SessionIndex = 0,
                SystemName = "DEV"
            };

            // Act: convert the options to the capabilities sent with the new-session request.
            var capabilities = options.ConvertToCapabilities();

            // Assert: carry the SAP browser name that grids and hubs route on.
            Assert.AreEqual("Sap", capabilities.AlwaysMatch["browserName"]);

            // Assert: send every set value under sap:options, keeping the zero indexes the server treats as valid.
            var sapOptions = capabilities.AlwaysMatch["sap:options"] as IDictionary<string, object>;
            Assert.IsNotNull(sapOptions);
            Assert.AreEqual(0, sapOptions["connectionIndex"]);
            Assert.AreEqual(0, sapOptions["sessionIndex"]);
            Assert.AreEqual("sap-host", sapOptions["label"]);
            Assert.AreEqual("DEV", sapOptions["systemName"]);

            // Assert: leave unset options out so the server applies its own selection rules.
            Assert.IsFalse(sapOptions.ContainsKey("sessionPath"));
            Assert.IsFalse(sapOptions.ContainsKey("client"));
        }

        [TestMethod(DisplayName = "Verify that SendVirtualKey without a window leaves the " +
            "window empty so the current window receives the key.")]
        public void SendVirtualKeyCurrentWindowTest()
        {
            // Arrange: record the request of a key sent without a window name.
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: "{\"value\":null}",
                mediaType: JsonMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);

            // Act: send Enter (0) to the current window.
            driver.SendVirtualKey(0);

            // Assert: send key code zero and a null window.
            var requests = messageHandler.GetRequests();
            using var body = JsonDocument.Parse(requests[0].Body);
            Assert.AreEqual(0, body.RootElement.GetProperty("virtualKey").GetInt32());
            Assert.AreEqual(JsonValueKind.Null, body.RootElement.GetProperty("window").ValueKind);
        }

        [TestMethod(DisplayName = "Verify that SendVirtualKey posts the key code and the target window.")]
        public void SendVirtualKeyWindowTest()
        {
            // Arrange: record the request of a key sent to a modal window.
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: "{\"value\":null}",
                mediaType: JsonMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);

            // Act: send Back (3) to the first modal window.
            driver.SendVirtualKey(3, window: "wnd[1]");

            // Assert: post the key to the session's virtual key route.
            var requests = messageHandler.GetRequests();
            Assert.HasCount(1, requests);
            Assert.AreEqual(HttpMethod.Post, requests[0].Method);
            Assert.AreEqual("/session/session-id/sap/vkey", requests[0].Uri.AbsolutePath);

            // Assert: send the key code and the window name the server reads.
            using var body = JsonDocument.Parse(requests[0].Body);
            Assert.AreEqual(3, body.RootElement.GetProperty("virtualKey").GetInt32());
            Assert.AreEqual("wnd[1]", body.RootElement.GetProperty("window").GetString());
        }

        [TestMethod(DisplayName = "Verify that SetProperty posts the property name " +
            "and value to the element's property route.")]
        public void SetPropertyTest()
        {
            // Arrange: record the request of a scroll position set on a table control.
            var messageHandler = new RecordingHttpMessageHandler(
                HttpStatusCode.OK,
                content: "{\"value\":null}",
                mediaType: JsonMediaType);
            using var httpClient = new HttpClient(handler: messageHandler, disposeHandler: true);
            using var driver = NewSapDriver(httpClient);
            var element = new SapElement(driver, id: "table-id");

            // Act: set an allow-listed property on the control.
            element.SetProperty("verticalScrollbar.position", 20);

            // Assert: post the property to the element's property route.
            var requests = messageHandler.GetRequests();
            Assert.HasCount(1, requests);
            Assert.AreEqual(HttpMethod.Post, requests[0].Method);
            Assert.AreEqual("/session/session-id/sap/element/table-id/property", requests[0].Uri.AbsolutePath);

            // Assert: send the property name and value the server reads.
            using var body = JsonDocument.Parse(requests[0].Body);
            Assert.AreEqual("verticalScrollbar.position", body.RootElement.GetProperty("name").GetString());
            Assert.AreEqual(20, body.RootElement.GetProperty("value").GetInt32());
        }

        // Creates a SapDriver bound to the supplied in-memory HTTP client with session creation disabled, so every
        // command goes to the recording handler under the fixed "session-id" session. The caller owns the client;
        // disposing the driver only sends a delete-session request to the same handler.
        private static SapDriver NewSapDriver(HttpClient httpClient)
        {
            // Bind transport to the in-memory handler while keeping an absolute URI for route assertions.
            var invoker = new WebDriverCommandInvoker(
                serverAddress: new Uri(uriString: "http://localhost:5556"),
                httpClient,
                timeout: TimeSpan.FromSeconds(5),
                keepAlive: false)
            {
                Session = new SessionIdModel(opaqueKey: "session-id")
            };

            // Skip session creation because the test controls the opaque session identifier.
            var session = new SessionModel
            {
                StartNewSession = false
            };

            // Attach the invoker session so the driver and its elements fill the session placeholder from it.
            return new SapDriver(invoker, session)
            {
                Session = invoker.Session
            };
        }

        // Records each request's method, URI, and body in a thread-safe queue and answers every request with one
        // fixed response. The handler performs no network I/O and returns snapshots so tests cannot change its history.
        private sealed class RecordingHttpMessageHandler(HttpStatusCode statusCode, string content, string mediaType)
            : HttpMessageHandler
        {
            // Owns the request history shared by every command a test sends.
            private readonly ConcurrentQueue<RecordedRequest> _requests = new();

            // Returns a detached snapshot so assertions observe completed requests without sharing mutable state.
            internal RecordedRequest[] GetRequests()
            {
                return [.. _requests];
            }

            /// <inheritdoc />
            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                // Capture the resolved endpoint and the serialized body before answering.
                var body = request.Content is null
                    ? null
                    : await request.Content.ReadAsStringAsync(cancellationToken);
                _requests.Enqueue(new RecordedRequest(request.Method, request.RequestUri, body));

                // Answer with the configured status, body, and media type.
                return new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(content, Encoding.UTF8, mediaType),
                    RequestMessage = request
                };
            }
        }

        // Holds one recorded request: its HTTP method, resolved URI, and serialized body (null when it has none).
        private sealed record RecordedRequest(HttpMethod Method, Uri Uri, string Body);
    }
}
