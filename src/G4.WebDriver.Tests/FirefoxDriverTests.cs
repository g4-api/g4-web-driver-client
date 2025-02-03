/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESOURCES
 */
using G4.WebDriver.Exceptions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Chromium;
using G4.WebDriver.Remote.Firefox;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace G4.WebDriver.Tests
{
    [TestClass]
    [TestCategory("FirefoxDriver")]
    [TestCategory("UnitTest")]
    public class FirefoxDriverTests : TestBase<FirefoxDriver>
    {
        #region *** Logs            ***
        [TestMethod(displayName: "Verify that the Firefox WebDriver throws a InvalidArgumentException " +
            "for an invalid log type.")]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void GetLogInvalidLogTypeTest()
        {
            // Set up FirefoxOptions and define invalid log preferences.
            var options = new FirefoxOptions();

            // Apply logging preferences to the FirefoxDriver options.
            options.SetLoggingPreferences(new LogPreferencesModel
            {
                LogType = "invalid",
                LogLevel = ChromiumOptionsBase.ChromiumLogLevels.All
            });

            // Invoke the FirefoxDriver and attempt to retrieve logs of an invalid type.
            Invoke(TestContext, options, "Logs.html", (driver) =>
            {
                // Wait for the page to load.
                Thread.Sleep(1500);

                // Attempt to retrieve logs of an invalid type, which should throw a WebDriverException.
                driver.Manage().Logs.GetLog("invalid");
            });
        }

        [TestMethod(displayName: "Verify that logs can be retrieved from the Firefox WebDriver.")]
        public void GetLogTest()
        {
            // Set up FirefoxOptions and define log preferences.
            var options = new FirefoxOptions();

            // Apply logging preferences to the FirefoxDriver options.
            options.SetLoggingPreferences(new LogPreferencesModel
            {
                LogLevel = FirefoxOptions.FirefoxLogLevels.Trace
            });

            // Invoke the FirefoxDriver and retrieve logs of the specified type.
            Invoke(TestContext, options, "Logs.html", (driver) =>
            {
                // Wait for the page to load.
                Thread.Sleep(1500);

                // Retrieve logs of the specified type.
                var driverLogs = driver.Manage().Logs.GetLog("browser").ToList();

                // Assert that logs were retrieved and are not empty.
                Assert.IsTrue(driverLogs.Count != 0, $"Expected any logs but found none.");
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

        [TestMethod(displayName: "Verify that the FirefoxWebDriverService starts, is ready, and correctly stops.")]
        [TestCategory("Local")]
        public void NewFirefoxWebDriverServiceTest()
        {
            // Retrieve the local Grid endpoint path from the test context properties
            var binariesPath = TestContext.Properties["Grid.Endpoint"].ToString();

            // Create a new FirefoxWebDriverService instance with the specified binaries path
            var service = new FirefoxWebDriverService(binariesPath);

            // Start the Firefox WebDriver service
            service.StartService();

            // Verify that the Firefox WebDriver service is ready after starting
            Assert.IsTrue(service.Ready, "Firefox WebDriver service is not ready after starting.");

            // Dispose of the Firefox WebDriver service
            service.Dispose();

            // Verify that the Firefox WebDriver service is not ready after disposing
            Assert.IsTrue(!service.Ready, "Firefox WebDriver service is still ready after disposing.");
        }

        [TestMethod(displayName: "Verify that a new FirefoxDriver session can be created and disposed successfully.")]
        [TestCategory("Local")]
        public void NewDefaultSessionTest()
        {
            // Create a new FirefoxDriver instance with the path from the test context properties
            var driver = new FirefoxDriver($"{TestContext.Properties["Grid.Endpoint"]}");

            // Verify that the FirefoxDriver instance is not null after creation
            Assert.IsTrue(driver != null, "FirefoxDriver instance is null after creation. Expected a valid driver instance.");

            // Dispose of the FirefoxDriver instance
            driver.Dispose();
        }

        [TestMethod(displayName: "Verify that a new FirefoxDriver session can be created with options and disposed successfully.")]
        [TestCategory("Local")]
        public void NewSessionWithOptionsTest()
        {
            // Create FirefoxWebDriverService options with the binaries path from the test context properties
            var options = new FirefoxWebDriverService(binariesPath: $"{TestContext.Properties["Grid.Endpoint"]}");

            // Create a new FirefoxDriver instance using the specified options
            var driver = new FirefoxDriver(options);

            // Verify that the FirefoxDriver instance is not null after creation with options
            Assert.IsTrue(driver != null, "FirefoxDriver instance is null after creation with options. Expected a valid driver instance.");

            // Dispose of the FirefoxDriver instance
            driver.Dispose();
        }

        [TestMethod(displayName: "Verify that a new FirefoxDriver session can be created with FirefoxWebDriverService and disposed successfully.")]
        [TestCategory("Local")]
        public void NewSessionWithServiceTest()
        {
            // Create an FirefoxWebDriverService instance with the binaries path from the test context properties
            var service = new FirefoxWebDriverService(binariesPath: $"{TestContext.Properties["Grid.Endpoint"]}");

            // Create a new FirefoxDriver instance using the specified service
            var driver = new FirefoxDriver(service);

            // Verify that the FirefoxDriver instance is not null after creation with the service
            Assert.IsTrue(driver != null, "FirefoxDriver instance is null after creation with FirefoxWebDriverService. Expected a valid driver instance.");

            // Dispose of the FirefoxDriver instance
            driver.Dispose();
        }

        [TestMethod(displayName: "Verify that a new FirefoxDriver session can be created, quit, and disposed successfully.")]
        [TestCategory("Local")]
        public void NewSessionQuitDisposeTest()
        {
            // Create a new FirefoxDriver instance with the path from the test context properties
            var driver = new FirefoxDriver($"{TestContext.Properties["Grid.Endpoint"]}");

            // Verify that the FirefoxDriver instance is not null after creation
            Assert.IsTrue(driver != null, "FirefoxDriver instance is null after creation. Expected a valid driver instance.");

            // Quit the FirefoxDriver session
            driver.Quit();

            // Dispose of the FirefoxDriver instance
            driver.Dispose();
        }

        [TestMethod(displayName: "Verify that a new FirefoxDriver session can be created, closed, quit, and disposed successfully.")]
        [TestCategory("Local")]
        public void NewSessionCloseQuitDisposeTest()
        {
            // Create a new FirefoxDriver instance with the path from the test context properties
            var driver = new FirefoxDriver($"{TestContext.Properties["Grid.Endpoint"]}");

            // Verify that the FirefoxDriver instance is not null after creation
            Assert.IsTrue(driver != null, "FirefoxDriver instance is null after creation. Expected a valid driver instance.");

            // Close the FirefoxDriver window
            driver.Close();

            // Quit the FirefoxDriver session
            driver.Quit();

            // Dispose of the FirefoxDriver instance
            driver.Dispose();
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
        #endregion
    }
}
