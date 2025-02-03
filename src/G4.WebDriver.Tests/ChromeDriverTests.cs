/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESOURCES
 */
using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Chrome;
using G4.WebDriver.Remote.Chromium;
using G4.WebDriver.Tests.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace G4.WebDriver.Tests
{
    [TestClass]
    [TestCategory("ChromeDriver")]
    [TestCategory("UnitTest")]
    public class ChromeDriverTests : TestBase<ChromeDriver>
    {
        #region *** Logs            ***
        [TestMethod(displayName: "Verify that the Chrome WebDriver throws a InvalidArgumentException " +
            "for an invalid log type.")]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void GetLogInvalidLogTypeTest()
        {
            // Set up ChromeOptions and define invalid log preferences.
            var options = new ChromeOptions();

            // Apply logging preferences to the ChromeDriver options.
            options.SetLoggingPreferences(new LogPreferencesModel
            {
                LogType = "invalid",
                LogLevel = ChromiumOptionsBase.ChromiumLogLevels.All
            });

            // Invoke the ChromeDriver and attempt to retrieve logs of an invalid type.
            Invoke(TestContext, options, "Logs.html", (driver) =>
            {
                // Wait for the page to load.
                Thread.Sleep(1500);

                // Attempt to retrieve logs of an invalid type, which should throw a WebDriverException.
                driver.Manage().Logs.GetLog("invalid");
            });
        }

        [TestMethod(displayName: "Verify that logs can be retrieved from the Chrome WebDriver.")]
        #region *** Data Set ***
        [DataRow("browser")]
        [DataRow("driver")]
        [DataRow("performance")]
        #endregion
        public void GetLogTest(string logType)
        {
            // Set up ChromeOptions and define log preferences.
            var options = new ChromeOptions();

            // Apply logging preferences to the ChromeDriver options.
            options.SetLoggingPreferences(new LogPreferencesModel
            {
                LogType = logType,
                LogLevel = ChromiumOptionsBase.ChromiumLogLevels.All
            });

            // Invoke the ChromeDriver and retrieve logs of the specified type.
            Invoke(TestContext, options, "Logs.html", (driver) =>
            {
                // Wait for the page to load.
                Thread.Sleep(1500);

                // Retrieve logs of the specified type.
                var driverLogs = driver.Manage().Logs.GetLog(logType).ToList();

                // Assert that logs were retrieved and are not empty.
                Assert.IsTrue(driverLogs.Count != 0, $"Expected logs of type '{logType}' but found none.");
            });
        }
        #endregion

        #region *** Sessions        ***
        [TestMethod(displayName: "Verify that the WebDriverService starts, is ready, and correctly stops.")]
        [TestCategory("Local")]
        #region *** Data Set ***
        [DataRow("msedgedriver.exe", 9517)]
        #endregion
        public void NewWebDriverServiceDefaultTest(string executableName, int port)
        {
            // Retrieve the local Grid endpoint path from the test context properties
            var binariesPath = TestContext.Properties["Grid.Endpoint"].ToString();

            // Create a new WebDriverService instance with the specified executable and port
            var service = new WebDriverService(binariesPath, executableName, port);

            // Start the WebDriver service
            service.StartService();

            // Verify that the WebDriver service is ready after starting
            Assert.IsTrue(service.Ready, "WebDriver service is not ready after starting.");

            // Dispose of the WebDriver service
            service.Dispose();

            // Verify that the WebDriver service's server address matches the specified port
            Assert.IsTrue(service.ServerAddress.Port == port, $"WebDriver service server address port mismatch. Expected port: {port}, but got: {service.ServerAddress.Port}.");

            // Verify that the WebDriver service is not ready after disposing
            Assert.IsTrue(!service.Ready, "WebDriver service is still ready after disposing.");
        }

        [TestMethod(displayName: "Verify that the ChromeWebDriverService starts, is ready, and correctly stops.")]
        [TestCategory("Local")]
        public void NewChromeWebDriverServiceTest()
        {
            // Retrieve the local Grid endpoint path from the test context properties
            var binariesPath = TestContext.Properties["Grid.Endpoint"].ToString();

            // Create a new ChromeWebDriverService instance with the specified binaries path
            var service = new ChromeWebDriverService(binariesPath);

            // Start the Chrome WebDriver service
            service.StartService();

            // Verify that the Chrome WebDriver service is ready after starting
            Assert.IsTrue(service.Ready, "Chrome WebDriver service is not ready after starting.");

            // Dispose of the Chrome WebDriver service
            service.Dispose();

            // Verify that the Chrome WebDriver service is not ready after disposing
            Assert.IsTrue(!service.Ready, "Chrome WebDriver service is still ready after disposing.");
        }

        [TestMethod(displayName: "Verify that a new ChromeDriver session can be created and disposed successfully.")]
        [TestCategory("Local")]
        public void NewDefaultSessionTest()
        {
            // Create a new ChromeDriver instance with the path from the test context properties
            var driver = new ChromeDriver($"{TestContext.Properties["Grid.Endpoint"]}");

            // Verify that the ChromeDriver instance is not null after creation
            Assert.IsTrue(driver != null, "ChromeDriver instance is null after creation. Expected a valid driver instance.");

            // Dispose of the ChromeDriver instance
            driver.Dispose();
        }

        [TestMethod(displayName: "Verify that a new ChromeDriver session can be created with options and disposed successfully.")]
        [TestCategory("Local")]
        public void NewSessionWithOptionsTest()
        {
            // Create ChromeWebDriverService options with the binaries path from the test context properties
            var options = new ChromeWebDriverService(binariesPath: $"{TestContext.Properties["Grid.Endpoint"]}");

            // Create a new ChromeDriver instance using the specified options
            var driver = new ChromeDriver(options);

            // Verify that the ChromeDriver instance is not null after creation with options
            Assert.IsTrue(driver != null, "ChromeDriver instance is null after creation with options. Expected a valid driver instance.");

            // Dispose of the ChromeDriver instance
            driver.Dispose();
        }

        [TestMethod(displayName: "Verify that a new ChromeDriver session can be created with ChromeWebDriverService and disposed successfully.")]
        [TestCategory("Local")]
        public void NewSessionWithServiceTest()
        {
            // Create an ChromeWebDriverService instance with the binaries path from the test context properties
            var service = new ChromeWebDriverService(binariesPath: $"{TestContext.Properties["Grid.Endpoint"]}");

            // Create a new ChromeDriver instance using the specified service
            var driver = new ChromeDriver(service);

            // Verify that the ChromeDriver instance is not null after creation with the service
            Assert.IsTrue(driver != null, "ChromeDriver instance is null after creation with ChromeWebDriverService. Expected a valid driver instance.");

            // Dispose of the ChromeDriver instance
            driver.Dispose();
        }

        [TestMethod(displayName: "Verify that a new ChromeDriver session can be created, quit, and disposed successfully.")]
        [TestCategory("Local")]
        public void NewSessionQuitDisposeTest()
        {
            // Create a new ChromeDriver instance with the path from the test context properties
            var driver = new ChromeDriver($"{TestContext.Properties["Grid.Endpoint"]}");

            // Verify that the ChromeDriver instance is not null after creation
            Assert.IsTrue(driver != null, "ChromeDriver instance is null after creation. Expected a valid driver instance.");

            // Quit the ChromeDriver session
            driver.Quit();

            // Dispose of the ChromeDriver instance
            driver.Dispose();
        }

        [TestMethod(displayName: "Verify that a new ChromeDriver session can be created, closed, quit, and disposed successfully.")]
        [TestCategory("Local")]
        public void NewSessionCloseQuitDisposeTest()
        {
            // Create a new ChromeDriver instance with the path from the test context properties
            var driver = new ChromeDriver($"{TestContext.Properties["Grid.Endpoint"]}");

            // Verify that the ChromeDriver instance is not null after creation
            Assert.IsTrue(driver != null, "ChromeDriver instance is null after creation. Expected a valid driver instance.");

            // Close the ChromeDriver window
            driver.Close();

            // Quit the ChromeDriver session
            driver.Quit();

            // Dispose of the ChromeDriver instance
            driver.Dispose();
        }

        [TestMethod(displayName: "Verify that a new ChromeDriver session can be created with an extension and disposed successfully.")]
        [TestCategory("Local")]
        public void NewSessionAddExtensionTest()
        {
            // Retrieve the driver binaries path from the test context properties
            var driverBinaries = $"{TestContext.Properties["Grid.Endpoint"]}";

            // Create ChromeOptions and add an extension to it
            var options = new ChromeOptions();
            options.AddExtensions("Binaries/RhinoApiExtensions.crx");

            // Create a new ChromeDriver instance with the specified binaries and options
            var driver = new ChromeDriver(driverBinaries, options);

            // Close the ChromeDriver window
            driver.Close();

            // Quit the ChromeDriver session
            driver.Quit();

            // Dispose of the ChromeDriver instance
            driver.Dispose();

            // Verify that the ChromeDriver instance is not null after all operations
            Assert.IsTrue(driver != null, "ChromeDriver instance is null after adding an extension and performing operations. Expected a valid driver instance.");
        }
        #endregion

        #region *** Custom Commands ***
        [DoNotParallelize]
        [RetryableTestMethod(
            numberOfAttempts: 5,
            displayName: "Verify that CDP commands can be invoked using ChromeDriver to navigate to a URL.")]
        public void InvokeCdpTest() => Invoke(TestContext, url: string.Empty, action: (driver) =>
        {
            // Send a CDP command to navigate to a specific URL
            driver.SendDeveloperToolsCommand(new CdpRequestModel
            {
                Command = "Page.navigate",
                Parameters = new Dictionary<string, object>
                {
                    ["url"] = "https://example.com"
                }
            });

            // Wait for 3 seconds before retrieving the current URL
            Thread.Sleep(3000);

            // Retrieve the current URL after navigation
            var url = driver.Navigate().Url;

            // Verify that the current URL contains "example.com" after navigation
            Assert.IsTrue(url.Contains("example.com"), $"The URL '{url}' does not contain 'example.com' after navigating using CDP command.");
        });

        [TestMethod(displayName: "Verify that cast sinks can be retrieved using Developer Tools in ChromeDriver.")]
        public void GetCastSinksTest() => Invoke(TestContext, url: string.Empty, action: (driver) =>
        {
            // Retrieve the list of available cast sinks using Developer Tools
            var sinks = driver.DeveloperTools.Cast.GetSinks();

            // Verify that there is at least one cast sink available
            Assert.IsTrue(sinks.Any(), "No cast sinks were found. Expected at least one cast sink to be available.");
        });

        [TestMethod(displayName: "Verify that an ArgumentException is thrown when selecting a null cast sink.")]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectCastSinksExceptionTest() => Invoke(TestContext, url: string.Empty, action: (driver) =>
        {
            // Attempt to set a null cast sink, which should throw an ArgumentException
            driver.DeveloperTools.Cast.SetSinkToUse(null);
        });

        [TestMethod(displayName: "Verify that a cast sink can be selected using Developer Tools in ChromeDriver.")]
        public void SelectCastSinksTest() => Invoke(TestContext, url: string.Empty, action: (driver) =>
        {
            // Retrieve the list of available cast sinks using Developer Tools
            var sinks = driver.DeveloperTools.Cast.GetSinks();

            // Get the name of the first available sink
            var name = sinks?.First().Name;

            // Verify that the name of the sink is not null or empty
            Assert.IsTrue(!string.IsNullOrEmpty(name), "The cast sink name is null or empty. Expected a valid sink name.");

            // Set the sink to use for casting
            driver.DeveloperTools.Cast.SetSinkToUse(name);
        });

        [TestMethod(displayName: "Verify that an ArgumentException is thrown when starting tab mirroring with a null sink name.")]
        [ExpectedException(typeof(ArgumentException))]
        public void StartCastTabMirroringExceptionTest() => Invoke(TestContext, url: string.Empty, action: (driver) =>
        {
            // Attempt to start tab mirroring with a null sink name, which should throw an ArgumentException
            driver.DeveloperTools.Cast.StartTabMirroring(null);
        });

        [TestMethod(displayName: "Verify that tab mirroring can be started using a valid cast " +
            "sink name with Developer Tools in ChromeDriver.")]
        public void StartCastTabMirroringTest() => Invoke(TestContext, url: string.Empty, action: (driver) =>
        {
            // Retrieve the list of available cast sinks using Developer Tools
            var sinks = driver.DeveloperTools.Cast.GetSinks();

            // Get the name of the first available sink
            var name = sinks?.First().Name;

            // Verify that the name of the sink is not null or empty
            Assert.IsTrue(!string.IsNullOrEmpty(name), "The cast sink name is null or empty. Expected a valid sink name.");

            // Wait for 3 seconds before starting tab mirroring
            Thread.Sleep(3000);

            // Start tab mirroring with the selected sink
            driver.DeveloperTools.Cast.StartTabMirroring(name);
        });

        [TestMethod(displayName: "Verify that an ArgumentException is thrown when starting desktop mirroring with a null sink name.")]
        [ExpectedException(typeof(ArgumentException))]
        public void StartDesktopMirroringExceptionTest() => Invoke(TestContext, url: string.Empty, action: (driver) =>
        {
            // Attempt to start desktop mirroring with a null sink name, which should throw an ArgumentException
            driver.DeveloperTools.Cast.StartDesktopMirroring(null);
        });

        [Ignore(message: "The test is blocked by the browser screen selection.")]
        [TestMethod(displayName: "Verify that desktop mirroring can be started using a valid cast sink name with Developer Tools in ChromeDriver.")]
        public void StartDesktopMirroringTest() => Invoke(TestContext, url: string.Empty, action: (driver) =>
        {
            // Retrieve the list of available cast sinks using Developer Tools
            var sinks = driver.DeveloperTools.Cast.GetSinks();

            // Get the name of the first available sink
            var name = sinks?.First().Name;

            // Verify that the name of the sink is not null or empty
            Assert.IsTrue(!string.IsNullOrEmpty(name), "The cast sink name is null or empty. Expected a valid sink name.");

            // Start desktop mirroring with the selected sink
            driver.DeveloperTools.Cast.StartDesktopMirroring(name);
        });
        #endregion

        #region *** Developer Tools ***
        [TestMethod(displayName: "Verify that CDP commands can be sent to navigate to a URL using ChromeDriver.")]
        [TestCategory("Local")]
        public void SendCdpCommandTest()
        {
            // Obtain a free port for remote debugging
            var port = WebDriverUtilities.GetFreePort();

            // Set up ChromeOptions with the remote debugging port
            var options = new ChromeOptions
            {
                Arguments =
                [
                    $"--remote-debugging-port={port}"
                ]
            };

            // Invoke the test with the given options and URL
            Invoke(TestContext, options, "DeveloperTools.html", (driver) =>
            {
                // Define the expected URL for navigation
                const string expected = "https://example.com";

                // Establish a new Developer Tools connection
                using var cdpClient = driver.NewDeveloperToolsConnection();
                var targets = cdpClient.Connect().GetTargets();

                // Retrieve the target ID of the first available target
                var target = targets.First().TargetId;
                var sessionId = cdpClient.AttachToTarget(target).Session;

                // Create a command to navigate to the specified URL
                var command = new DeveloperToolsCommandModel
                {
                    Method = "Page.navigate",
                    Parameters = new Dictionary<string, object>
                    {
                        ["url"] = expected
                    },
                    SessionId = sessionId
                };

                // Send the command to navigate to the URL
                cdpClient.SendCommand(command);

                // Retrieve the actual URL after navigation
                var actual = driver.Navigate().Url.TrimEnd('/');

                // Assert that the actual URL matches the expected URL
                Assert.AreEqual(expected, actual, "The URL after navigating using the CDP command does not match the expected URL.");
            });
        }
        #endregion

        #region *** Cookies         ***
        [TestMethod(displayName: "Verify that a cookie can be added successfully.")]
        public void AddCookieTest() => Invoke(TestContext, "Cookies.html", (driver) =>
        {
            // Create a new cookie with a specific name and value.
            var cookie = new CookieModel
            {
                Name = "TestCookie",
                Value = "TestValue"
            };

            // Add the cookie to the browser.
            driver.Manage().Cookies.AddCookie(cookie);

            // Retrieve the cookie by its name.
            var actual = driver.Manage().Cookies.GetCookie("TestCookie");

            // Assert that the cookie value matches the expected value.
            Assert.IsTrue(actual.Value == "TestValue", "The cookie value does not match the expected value.");
        });

        [TestMethod(displayName: "Verify that all cookies can be deleted successfully.")]
        public void DeleteCookiesTest() => Invoke(TestContext, "Cookies.html", (driver) =>
        {
            // Delete all cookies in the browser.
            driver.Manage().Cookies.DeleteCookies();

            // Assert that no cookies remain.
            Assert.IsTrue(driver.Manage().Cookies.Cookies.Count == 0, "Cookies were not deleted successfully.");
        });

        [TestMethod(displayName: "Verify that deleting a specific cookie works as expected.")]
        [ExpectedException(typeof(NoSuchCookieException))]
        public void DeleteCookieTest() => Invoke(TestContext, "Cookies.html", (driver) =>
        {
            // Create a cookie model representing the cookie to be deleted.
            var cookie = new CookieModel
            {
                Name = "NID"
            };

            // Delete the specific cookie.
            driver.Manage().Cookies.DeleteCookie(cookie);

            // Attempt to retrieve the deleted cookie, expecting an exception to be thrown.
            driver.Manage().Cookies.GetCookie("NID");
        });

        [TestMethod(displayName: "Verify that cookies can be retrieved successfully.")]
        public void GetCookiesTest() => Invoke(TestContext, "Cookies.html", (driver) =>
        {
            // Retrieve all cookies from the browser.
            var cookies = driver.Manage().Cookies.Cookies;

            // Assert that the cookies collection is not empty.
            Assert.IsTrue(cookies.Count != 0, "No cookies were retrieved.");
        });

        [TestMethod(displayName: "Verify that a specific cookie can be retrieved successfully.")]
        public void GetCookieTest() => Invoke(TestContext, "Cookies.html", (driver) =>
        {
            // Retrieve a specific cookie by its name.
            var cookie = driver.Manage().Cookies.GetCookie("NID");

            // Assert that the cookie value is not empty.
            Assert.IsTrue(cookie.Value.Length > 0, "The cookie value is empty.");
        });
        #endregion

        #region *** Navigation      ***
        [TestMethod(displayName: "Verify that the current page title can be retrieved successfully.")]
        public void GetTitleTest() => Invoke(TestContext, "Navigation.html", (driver) =>
        {
            // Retrieve the current page title.
            var title = driver.Navigate().Title;

            // Assert that the title is not empty.
            Assert.IsTrue(title.Length > 0, "The page title is empty.");
        });

        [TestMethod(displayName: "Verify that the current page URL can be retrieved successfully.")]
        public void GetUrlTest() => Invoke(TestContext, "Navigation.html", (driver) =>
        {
            // Retrieve the current page URL.
            var url = driver.Navigate().Url;

            // Assert that the URL is not empty.
            Assert.IsTrue(url.Contains("Navigation.html", StringComparison.OrdinalIgnoreCase), "The page URL is empty.");
        });

        [TestMethod(displayName: "Verify that a URL can be opened successfully.")]
        public void OpenUrlTest() => Invoke(TestContext, "Navigation.html", (driver) =>
        {
            // Retrieve the current page URL.
            var url = driver.Navigate().Url;

            // Open the URL with an added query parameter.
            driver.Navigate().Open($"{url}?param=1");

            // Assert that the URL contains the specified query parameter.
            Assert.IsTrue(driver.Navigate().Url.Contains("param=1", StringComparison.OrdinalIgnoreCase),
                "The URL does not contain the expected query parameter 'param=1'.");
        });

        [TestMethod(displayName: "Verify Redo and Undo navigation with query parameters.")]
        public void RedoUndoNavigationTest() => Invoke(TestContext, "Navigation.html", (driver) =>
        {
            // Retrieve the current page URL.
            var url = driver.Navigate().Url;

            // Open the URL with an added query parameter.
            driver.Navigate().Open($"{url}?param=1");

            // Assert that the URL contains the specified query parameter.
            Assert.IsTrue(driver.Navigate().Url.Contains("param=1", StringComparison.OrdinalIgnoreCase),
                "The URL does not contain the expected query parameter 'param=1' after navigation.");

            // Perform the Undo navigation action.
            driver.Navigate().Undo();

            // Assert that the URL no longer contains the query parameter after Undo.
            Assert.IsTrue(!driver.Navigate().Url.Contains("param=1", StringComparison.OrdinalIgnoreCase),
                "The URL still contains the query parameter 'param=1' after undoing the navigation.");

            // Perform the Redo navigation action.
            driver.Navigate().Redo();

            // Assert that the URL contains the query parameter again after Redo.
            Assert.IsTrue(driver.Navigate().Url.Contains("param=1", StringComparison.OrdinalIgnoreCase),
                "The URL does not contain the expected query parameter 'param=1' after redoing the navigation.");
        });

        [TestMethod(displayName: "Verify that the browser can be refreshed successfully.")]
        public void RefreshTest() => Invoke(TestContext, "Navigation.html", (driver) =>
        {
            // Refresh the page.
            driver.Navigate().Update();

            // Assert that the URL remains the same after refreshing the page.
            Assert.IsTrue(driver.Navigate().Url.Contains("?refreshed=true", StringComparison.OrdinalIgnoreCase),
                "The URL changed after refreshing the page.");
        });
        #endregion

        #region *** Timeouts        ***
        [TestMethod(displayName: "Verify that a page load timeout is triggered correctly.")]
        [ExpectedException(typeof(WebDriverTimeoutException))]
        public void PageLoadTimeoutTest() => Invoke(TestContext, url: string.Empty, (driver) =>
        {
            // Set the page load timeout to 5 seconds.
            driver.Manage().Timeouts.PageLoad = TimeSpan.FromSeconds(5);

            // Attempt to open a page that will not load.
            driver.Navigate().Open("http://localhost:9002/test/static/Timeouts.html?blockLoad=true");

            // This point should not be reached if a page load timeout occurs.
            Assert.Fail("Expected a WebDriverTimeoutException, but the operation completed without timing out.");
        });

        [TestMethod(displayName: "Verify that a find element timeout is triggered correctly.")]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindElementTimeoutTest() => Invoke(TestContext, "Timeouts.html", (driver) =>
        {
            // Create a stopwatch to measure the time taken to locate an element.
            var stopwatch = new Stopwatch();

            // Set the implicit wait timeout to 5 seconds.
            driver.Manage().Timeouts.Implicit = TimeSpan.FromSeconds(5);

            try
            {
                // Start measuring time to locate the element.
                stopwatch.Start();

                // Attempt to find an element that does not exist.
                driver.FindElement(By.CssSelector("#nonexistent"));
            }
            catch (NoSuchElementException)
            {
                // Re-throw the expected exception to match the test's ExpectedException attribute.
                throw;
            }
            finally
            {
                // Stop the stopwatch after attempting to find the element.
                stopwatch.Stop();

                // Assert that the find element timeout occurred in more than 5 seconds.
                Assert.IsTrue(stopwatch.ElapsedTicks > 5 * TimeSpan.TicksPerSecond,
                    "The find element operation took longer than expected.");
            }
        });

        [TestMethod(displayName: "Verify that an async script execution timeout is triggered correctly.")]
        [ExpectedException(typeof(ScriptTimeoutException))]
        public void AsyncScriptTimeoutTest() => Invoke(TestContext, "Timeouts.html", (driver) =>
        {
            // Set the script execution timeout to 5 seconds.
            driver.Manage().Timeouts.Script = TimeSpan.FromSeconds(5);

            // Attempt to execute an async script that will not complete.
            driver.InvokeAsyncScript(@"
                var callback = arguments[arguments.length - 1];
                setTimeout(function() {
                    callback('Script completed after delay');
                }, 10000); // 10-second delay
            ");

            // This point should not be reached if an async script execution timeout occurs.
            Assert.Fail("Expected a ScriptTimeoutException, but the operation completed without timing out.");
        });

        [TestMethod(displayName: "Verify that a script execution timeout is triggered correctly.")]
        [ExpectedException(typeof(ScriptTimeoutException))]
        public void ScriptTimeoutTest() => Invoke(TestContext, "Timeouts.html", (driver) =>
        {
            // Set the script execution timeout to 5 seconds.
            driver.Manage().Timeouts.Script = TimeSpan.FromSeconds(5);

            // Attempt to execute a script that will not complete.
            driver.InvokeScript(@"
                var start = new Date().getTime();
                // Busy-wait loop for 10 seconds
                while (new Date().getTime() - start < 10000) {
                    // Blocking the main thread intentionally
                }
                return 'Script completed after delay';
            ");

            // This point should not be reached if a script execution timeout occurs.
            Assert.Fail("Expected a ScriptTimeoutException, but the operation completed without timing out.");
        });
        #endregion
    }
}
