using G4.WebDriver.Exceptions;
using G4.WebDriver.Remote;
using G4.WebDriver.Support.Ui;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;

namespace G4.WebDriver.Extensions
{
    /// <summary>
    /// Provides utility methods for WebDriver operations.
    /// </summary>
    public static class WebDriverUtilities
    {
        #region *** Fields  ***
        /// <summary>
        /// Gets a new instance of HttpClient configured with custom user agent.
        /// </summary>
        public static readonly HttpClient HttpClient = NewHttpClient();
        #endregion

        #region *** Methods ***
        /// <summary>
        /// Gets a free port number starting from a default port (9515).
        /// </summary>
        /// <returns>A free port number.</returns>
        public static int GetFreePort() => GetFreePort(9515);

        /// <summary>
        /// Gets a free port number starting from the specified default port.
        /// </summary>
        /// <param name="defaultPort">The default port number to start searching for a free port.</param>
        /// <returns>A free port number.</returns>
        public static int GetFreePort(int defaultPort)
        {
            // Initialize the port with the default value
            var port = defaultPort;

            // Create a new socket for checking port availability
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Create a local endpoint with any IP address and port 0
                var localEP = new IPEndPoint(IPAddress.Any, 0);

                // Bind the socket to the local endpoint to find an available port
                socket.Bind(localEP);

                // Get the actual local endpoint with the assigned port
                localEP = (IPEndPoint)socket.LocalEndPoint;

                // Update the port with the assigned port from the local endpoint
                port = localEP.Port;
            }
            finally
            {
                // Close the socket to release the port
                socket.Close();
            }

            // Return the obtained free port
            return port;
        }

        /// <summary>
        /// Creates a new WebDriverWait instance with a specified timeout and a set of common exceptions to ignore.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="timeout">The maximum time to wait for a condition to be met.</param>
        /// <returns>Returns an IWaitable<IWebDriver> instance configured with the specified timeout and common exceptions to ignore.</returns>
        public static IWaitable<IWebDriver> NewWebDriverWait(IWebDriver driver, TimeSpan timeout)
        {
            // Define a set of common exceptions to ignore during the wait.
            var ignoreExceptions = new[]
            {
                typeof(StaleElementReferenceException),
                typeof(NoSuchElementException),
                typeof(WebDriverException),
                typeof(NullReferenceException)
            };

            // Call the overload method with the specified timeout and common exceptions to ignore.
            return NewWebDriverWait(driver, timeout, ignoreExceptions);
        }

        /// <summary>
        /// Creates a new WebDriverWait instance with a specified timeout and a custom set of exceptions to ignore.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="timeout">The maximum time to wait for a condition to be met.</param>
        /// <param name="ignoreExceptions">The types of exceptions to ignore during the wait.</param>
        /// <returns>Returns an IWaitable<IWebDriver> instance configured with the specified timeout and exceptions to ignore.</returns>
        public static IWaitable<IWebDriver> NewWebDriverWait(IWebDriver driver, TimeSpan timeout, params Type[] ignoreExceptions)
        {
            // Create a new WebDriverWait instance with the specified timeout.
            var webDriverWait = new WebDriverWait<IWebDriver>(driver, timeout);

            // Add each exception type to the list of exceptions to ignore during the wait.
            foreach (var exceptionType in ignoreExceptions)
            {
                webDriverWait.IgnoreExceptions.Add(exceptionType);
            }

            // Return the configured WebDriverWait instance.
            return webDriverWait;
        }

        // Creates a new instance of HttpClient with optional user credentials and custom user agent.
        private static HttpClient NewHttpClient()
        {
            // Create HttpClientHandler instance.
            var httpClientHandler = new HttpClientHandler();

            // Get the current assembly version and runtime identifier.
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var platformName = RuntimeInformation.RuntimeIdentifier;

            // Construct the user agent string.
            var userAgent = $"G4 Engine/4.0.0.0 (.NET {assemblyVersion} {platformName})";

            // Create a new instance of HttpClient using the configured HttpClientHandler.
            var client = new HttpClient(httpClientHandler);

            // Add the user agent to the default request headers.
            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            // Return the created HttpClient instance.
            return client;
        }
        #endregion
    }
}
