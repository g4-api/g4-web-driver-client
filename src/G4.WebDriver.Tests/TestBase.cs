using G4.WebDriver.Extensions;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Edge;
using G4.WebDriver.Remote.Uia;
using G4.WebDriver.Tests.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace G4.WebDriver.Tests
{
    [TestClass]
    [DeploymentItem("Binaries/", "Binaries")]
    [DeploymentItem("Pages/", "Pages")]
    [DeploymentItem("appsettings.json")]
    public abstract class TestBase<T> where T : IWebDriver
    {
        #region *** Properties ***
        /// <summary>
        /// Gets or sets the context of the TestClass.
        /// </summary>
        public TestContext TestContext { get; set; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Invokes a specified action on a WebDriver instance after navigating to the given URL.
        /// Uses the default options for the WebDriver.
        /// </summary>
        /// <param name="context">The test context that provides configuration and properties for the test.</param>
        /// <param name="url">The URL to navigate to before invoking the action.</param>
        /// <param name="action">The action to perform on the WebDriver instance.</param>
        public void Invoke(TestContext context, string url, Action<T> action)
        {
            // Invoke the overloaded method with null options, which uses the default WebDriver options.
            Invoke(context, options: new EdgeOptions(), url, action);
        }

        /// <summary>
        /// Invokes a specified action on a WebDriver instance after navigating to the given URL.
        /// Allows specifying WebDriver options for customization.
        /// </summary>
        /// <param name="context">The test context that provides configuration and properties for the test.</param>
        /// <param name="options">The options to configure the WebDriver instance (can be null to use defaults).</param>
        /// <param name="url">The URL to navigate to before invoking the action.</param>
        /// <param name="action">The action to perform on the WebDriver instance.</param>
        public void Invoke(TestContext context, WebDriverOptionsBase options, string url, Action<T> action)
        {
            // Determine the path to the WebDriver binaries based on whether the test is running locally or remotely.
            var binariesPath = $"{context.Properties["Grid.Endpoint"]}";

            // Initialize the WebDriver instance using the determined binaries path and provided options.
            var driver = NewDriver(context, options, binariesPath);

            // If the provided URL is not a full URL, prepend it with the local test server URL.
            url = url.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? url
                : $"http://localhost:9002/test/static/{url}";

            try
            {
                // Open the specified URL in the browser.
                if (!string.IsNullOrEmpty(url))
                {
                    driver.Navigate().Open(url);
                }

                // Invoke the provided action, passing the WebDriver instance.
                action.Invoke((T)driver);
            }
            catch (Exception e)
            {
                // Rethrow the base exception in case of an error.
                throw e.GetBaseException();
            }
            finally
            {
                // Dispose of the WebDriver instance to close the browser and clean up resources.
                driver.Dispose();
            }
        }

        // Creates a new instance of <see cref="IWebDriver"/> configured for the test context, either locally or remotely.
        private static IWebDriver NewDriver(TestContext context, WebDriverOptionsBase options, string driverBinaries)
        {
            // Determine the build name based on the "Build.Name" property.
            var buildName = context.Properties.Get(key: "Build.Name", defaultValue: string.Empty);
            buildName = string.IsNullOrEmpty(buildName)
                ? $"Development.{DateTime.UtcNow:yyyy.MM.dd}"
                : buildName;

            // If the driver binaries path is a URL, create a remote WebDriver instance.
            if (driverBinaries.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                // Define BrowserStack options for the WebDriver session.
                var browserstackOptions = new Dictionary<string, object>
                {
                    ["resolution"] = "1920x1080",
                    //["username"] = $"{context.Properties["BrowserStack.Username"]}",
                    //["accessKey"] = $"{context.Properties["BrowserStack.Password"]}",
                    ["browserVersion"] = "latest",
                   // ["osVersion"] = "11",
                   // ["os"] = "Windows",
                    ["local"] = "true",
                    ["sessionName"] = GetTestName(),
                    ["buildName"] = buildName,
                    ["projectName"] = $"{context.Properties["Project.Name"]}",
                    ["seleniumVersion"] = $"{context.Properties["Integration.Selenium.Version"]}"
                };

                // Define capabilities for the WebDriver session, including BrowserStack options.
                options.AddCapabilities(browserstackOptions);

                // Create a new instance of the RemoteWebDriver type using the provided driver binaries and options.
                return new RemoteWebDriver(driverBinaries, options.ConvertToCapabilities().NewSessionModel());
            }

            // Create a new instance of the WebDriver type specified by the generic type parameter.
            var driver = (T)Activator.CreateInstance(typeof(T), driverBinaries, options);

            // Set the default timeouts for the WebDriver instance.
            driver.Manage().Timeouts.Implicit = TimeSpan.FromSeconds(15);
            driver.Manage().Timeouts.PageLoad = TimeSpan.FromSeconds(15);
            if(options is not UiaOptions)
            {
                driver.Manage().Timeouts.Script = TimeSpan.FromSeconds(15);
            }

            // Return the configured WebDriver instance.
            return driver;
        }

        // Gets the display name of the currently executing test method if available
        // otherwise, "Test method name not found".
        private static string GetTestName()
        {
            // Define a constant string to hold the message indicating that the test method name was not found
            const string MethodNameNotFound = "Test method name not found";

            // Initialize variables
            var counter = 0;
            var method = new StackFrame(counter++).GetMethod();
            var isTestMethod = method.GetCustomAttribute<TestMethodAttribute>() != null;

            // Iterate through the call stack to find the currently executing test method
            while (!isTestMethod)
            {
                method = new StackFrame(counter++).GetMethod();
                if (method == null)
                {
                    // If no test method is found, return default value
                    return MethodNameNotFound;
                }

                var attribute = method.GetCustomAttribute<TestMethodAttribute>();

                // Check if the current method is a test method
                isTestMethod = attribute != default;
                if (isTestMethod)
                {
                    // Return the display name of the test method if available
                    return string.IsNullOrEmpty(attribute.DisplayName)
                        ? MethodNameNotFound
                        : attribute.DisplayName;
                }
            }

            // Return the default message indicating that the test method name was not found
            return MethodNameNotFound;
        }
        #endregion
    }
}
