using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading;

namespace G4.WebDriver.Remote.Uia
{
    /// <summary>
    /// Represents a WebDriver implementation for UI Automation (UIA), extending the <see cref="RemoteWebDriver"/>.
    /// </summary>
    public class UiaDriver : RemoteWebDriver
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the UiaDriver class.
        /// </summary>
        public UiaDriver()
            : this(new UiaOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified options.
        /// </summary>
        /// <param name="options">The UiaOptions to be used with the Uia driver.</param>
        public UiaDriver(UiaOptions options)
            : this(options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified options.
        /// </summary>
        /// <param name="session">The SessionModel to be used with the Uia driver.</param>
        public UiaDriver(SessionModel session)
            : this(new UiaWebDriverService(), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified driver service.
        /// </summary>
        /// <param name="driverService">The UiaWebDriverService used to initialize the driver.</param>
        public UiaDriver(UiaWebDriverService driverService)
            : this(driverService, new UiaOptions(), TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing Uia.DriverServer.exe.</param>
        public UiaDriver(string binariesPath)
            : this(binariesPath, new UiaOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing Uia.DriverServer.exe.</param>
        /// <param name="options">The UiaOptions to be used with the Uia driver.</param>
        public UiaDriver(string binariesPath, UiaOptions options)
            : this(binariesPath, options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing Uia.DriverServer.exe.</param>
        /// <param name="session">The SessionModel to be used with the Uia driver.</param>
        public UiaDriver(string binariesPath, SessionModel session)
            : this(new UiaWebDriverService(binariesPath), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="driverService">The UiaWebDriverService used to initialize the driver.</param>
        /// <param name="options">The UiaOptions to be used with the Uia driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public UiaDriver(UiaWebDriverService driverService, UiaOptions options, TimeSpan timeout)
            : this(driverService, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="driverService">The UiaWebDriverService used to initialize the driver.</param>
        /// <param name="session">The SessionModel to be used with the Uia driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public UiaDriver(UiaWebDriverService driverService, SessionModel session, TimeSpan timeout)
            : base(new WebDriverCommandInvoker(driverService, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified remote server address.
        /// Uses default <see cref="UiaOptions"/> and timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        public UiaDriver(Uri remoteServerAddress)
            : this(remoteServerAddress, new UiaOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified remote server address and options.
        /// Uses default timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Uia driver.</param>
        public UiaDriver(Uri remoteServerAddress, UiaOptions options)
            : this(remoteServerAddress, options, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified remote server address, options, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Uia driver.</param>
        /// <param name="timeout">The command execution timeout.</param>
        public UiaDriver(Uri remoteServerAddress, UiaOptions options, TimeSpan timeout)
            : this(remoteServerAddress, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class using the specified remote server address, session, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public UiaDriver(Uri remoteServerAddress, SessionModel session, TimeSpan timeout)
            : this(new WebDriverCommandInvoker(remoteServerAddress, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class using the specified WebDriver command invoker and session.
        /// </summary>
        /// <param name="invoker">The WebDriver command invoker for the driver.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        public UiaDriver(IWebDriverCommandInvoker invoker, SessionModel session)
            : base(invoker, session)
        {
            // Additional initialization logic can be added here if needed.
        }
        #endregion

        #region *** Methods      ***
        // Initialize the static HttpClient instance for sending requests to the WebDriver server
        private static HttpClient HttpClient => new();

        // Initialize the static JsonSerializerOptions instance for deserializing JSON responses
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Sends input scan codes to the WebDriver server.
        /// </summary>
        /// <param name="codes">The scan codes to send.</param>
        public void SendInputs(params string[] codes)
        {
            SendInputs(1, codes);
        }

        /// <summary>
        /// Sends input scan codes to the WebDriver server multiple times.
        /// </summary>
        /// <param name="repeat">The number of times to repeat the input.</param>
        /// <param name="codes">The scan codes to send.</param>
        public void SendInputs(int repeat, params string[] codes)
        {
            // Get the session ID from the WebDriver
            var sessionId = $"{Session}";

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver: this);

            // Construct the route for the input simulation command
            var requestUri = $"{url}/session/{sessionId}/user32/inputs";

            // Create the request body with the scan codes to send to the server for input simulation
            var requestBody = new ScanCodesInputModel
            {
                ScanCodes = codes
            };

            // Send the HTTP request and ensure the response is successful for the specified number of repeats
            for (var i = 0; i < repeat; i++)
            {
                // Create the HTTP content for the request body
                var content = new StringContent(
                    content: JsonSerializer.Serialize(requestBody, JsonSerializerOptions),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json");

                // Create the HTTP request for the input simulation
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = content
                };

                // Send the HTTP request
                var response = HttpClient.Send(request);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Wait for a short interval before sending the next input
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Sends text input to the WebDriver server.
        /// </summary>
        /// <param name="text">The text to send.</param>
        public void SendKeys(string text)
        {
            SendKeys(repeat: 1, text);
        }

        /// <summary>
        /// Sends each character of the specified text as a key input with a delay between each keystroke.
        /// </summary>
        /// <param name="text">The text to be sent as key inputs.</param>
        /// <param name="delay">The delay to wait between sending each key.</param>
        public void SendKeys(string text, TimeSpan delay)
        {
            // Iterate through each character in the provided text.
            foreach (var character in text)
            {
                // Send the current character as a key input.
                // 'repeat: 1' indicates that the key is sent once.
                SendKeys(repeat: 1, text: $"{character}");

                // Wait for the specified delay before sending the next key.
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// Sends text input to the WebDriver server multiple times.
        /// </summary>
        /// <param name="repeat">The number of times to repeat the input.</param>
        /// <param name="text">The text to send.</param>
        public void SendKeys(int repeat, string text)
        {
            // Get the session ID from the WebDriver
            var sessionId = $"{Session}";

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver: this);

            // Construct the route for the text input command
            var requestUri = $"{url}/session/{sessionId}/user32/value";

            // Create the request body with the text to send to the server for input simulation
            var requestBody = new TextInputModel
            {
                Text = text
            };

            // Send the HTTP request and ensure the response is successful for the specified number of repeats
            for (var i = 0; i < repeat; i++)
            {
                // Create the HTTP content for the request body
                var content = new StringContent(
                    content: JsonSerializer.Serialize(requestBody, JsonSerializerOptions),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json");

                // Create the HTTP request for the text input
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = content
                };

                // Send the HTTP request
                var response = HttpClient.Send(request);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Wait for a short interval before sending the next input
                Thread.Sleep(100);
            }
        }

        // Gets the remote server URI from the WebDriver command executor.
        private static string GetRemoteServerUri(IWebDriver driver)
        {
            // Get the remote server URI from the command executor using reflection
            return driver.GetServerAddress().AbsoluteUri.Trim('/');
        }

        /// <summary>
        /// Model for sending scan codes as input.
        /// </summary>
        private sealed class ScanCodesInputModel
        {
            /// <summary>
            /// Gets or sets the collection of scan codes.
            /// </summary>
            /// <value>A collection of scan codes required for the operation.</value>
            [Required]
            public IEnumerable<string> ScanCodes { get; set; }
        }

        /// <summary>
        /// Model for sending text input.
        /// </summary>
        private sealed class TextInputModel
        {
            /// <summary>
            /// Gets or sets the text to be input.
            /// </summary>
            /// <value>The text to be input.</value>
            [Required]
            public string Text { get; set; }
        }
        #endregion
    }
}
