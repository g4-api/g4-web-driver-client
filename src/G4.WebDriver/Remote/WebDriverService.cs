/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/DriverService.cs
 */
using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Models.Events;

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents a WebDriver service implementation.
    /// </summary>
    public class WebDriverService : IWebDriverService
    {
        #region *** Fields       ***
        // The JSON options for deserializing the WebDriver status model
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // The arguments for the WebDriver executable
        private readonly string _arguments;

        // The path to the WebDriver binaries
        private readonly string _binariesPath;

        // The download URL for the WebDriver executable
        private readonly string _downloadUrl;

        // The name of the WebDriver executable
        private readonly string _executableName;

        // The port on which the WebDriver service will run
        private readonly int _port;
        #endregion

        #region *** Events       ***
        /// <summary>
        /// Occurs when the WebDriver service is disposed.
        /// </summary>
        public event EventHandler<EventArgs> ServiceDisposed;

        /// <summary>
        /// Occurs when the WebDriver service is started.
        /// </summary>
        public event EventHandler<WebDriverProcessStartedEventArgs> ServiceStarted;

        /// <summary>
        /// Occurs when the WebDriver service is starting.
        /// </summary>
        public event EventHandler<WebDriverProcessStartingEventArgs> ServiceStarting;
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverService"/> class with default arguments and download URL.
        /// </summary>
        /// <param name="binariesPath">The path to the WebDriver binaries.</param>
        /// <param name="executableName">The name of the WebDriver executable.</param>
        public WebDriverService(string binariesPath, string executableName)
            : this(binariesPath, executableName, arguments: string.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverService"/> class with default download URL.
        /// </summary>
        /// <param name="binariesPath">The path to the WebDriver binaries.</param>
        /// <param name="executableName">The name of the WebDriver executable.</param>
        /// <param name="arguments">The arguments for the WebDriver executable.</param>
        public WebDriverService(string binariesPath, string executableName, string arguments)
            : this(binariesPath, executableName, arguments, -1)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverService"/> class with default download URL.
        /// </summary>
        /// <param name="binariesPath">The path to the WebDriver binaries.</param>
        /// <param name="executableName">The name of the WebDriver executable.</param>
        /// <param name="port">The port on which the WebDriver service will run.</param>
        public WebDriverService(string binariesPath, string executableName, int port)
            : this(binariesPath, executableName, arguments: string.Empty, port)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverService"/> class with default download URL.
        /// </summary>
        /// <param name="binariesPath">The path to the WebDriver binaries.</param>
        /// <param name="executableName">The name of the WebDriver executable.</param>
        /// <param name="arguments">The arguments for the WebDriver executable.</param>
        /// <param name="port">The port on which the WebDriver service will run.</param>
        public WebDriverService(string binariesPath, string executableName, string arguments, int port)
            : this(binariesPath, executableName, arguments, port, downloadUrl: string.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverService"/> class.
        /// </summary>
        /// <param name="binariesPath">The path to the WebDriver binaries.</param>
        /// <param name="executableName">The name of the WebDriver executable.</param>
        /// <param name="arguments">The arguments for the WebDriver executable.</param>
        /// <param name="port">The port on which the WebDriver service will run.</param>
        /// <param name="downloadUrl">The download URL for the WebDriver executable.</param>
        public WebDriverService(
            string binariesPath, string executableName, string arguments, int port, string downloadUrl)
        {
            // Set the constructor parameters
            _arguments = arguments;
            _binariesPath = binariesPath.Equals(".") ? Environment.CurrentDirectory : binariesPath;
            _executableName = executableName;
            _port = port;
            _downloadUrl = downloadUrl;

            // Add any necessary validation logic here.
            ConfirmConstructor(binariesPath, executableName, port);

            // Set the ServerAddress property
            ServerAddress = new($"http://localhost:{port}");
        }

        // Validates the constructor parameters for the WebDriverService class.
        private static void ConfirmConstructor(string binariesPath, string executableName, int port)
        {
            // Define a message template for exceptions
            var errorMsgTemplate = "Validation failed for constructor parameters: " +
                $"BinariesPath: {binariesPath}, ExecutableName: {executableName}, Port: {port}. Reason: $[reason]";

            // Validate binariesPath
            if (string.IsNullOrEmpty(binariesPath))
            {
                throw new ArgumentException(
                    errorMsgTemplate.Replace("$[reason]", "BinariesPath cannot be null or empty."),
                    nameof(binariesPath));
            }

            // Check if binariesPath is a remote URL
            if (Regex.IsMatch(input: binariesPath, pattern: "^(http(s)?)://"))
            {
                return;
            }

            // Validate executableName
            if (string.IsNullOrEmpty(executableName))
            {
                throw new ArgumentException(
                    errorMsgTemplate.Replace("$[reason]", "ExecutableName cannot be null or empty."),
                    nameof(executableName));
            }

            // Validate port
            if (port < 1)
            {
                throw new ArgumentException(
                    errorMsgTemplate.Replace("$[reason]", "Port must be a positive integer."),
                    nameof(port));
            }

            // Check if the executable file exists at the specified path
            var path = Path.Combine(binariesPath, executableName);
            if (!File.Exists(path))
            {
                throw new DriverServiceNotFoundException(
                    errorMsgTemplate.Replace("$[reason]", $"Executable file not found at path: {path}"));
            }
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets a value indicating whether to hide the command prompt window when starting the WebDriver service.
        /// </summary>
        public bool HideCommandPromptWindow { get; set; }

        /// <summary>
        /// Formats the parameter for the WebDriver service port.
        /// </summary>
        public virtual string PortParameterFormat => "--port={0}";

        /// <summary>
        /// Gets the process associated with the WebDriver service.
        /// </summary>
        public Process Process { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the WebDriver service is ready.
        /// </summary>
        public bool Ready => GetStatus(ServerAddress).Ready;

        /// <summary>
        /// Gets or sets the server address for the WebDriver service.
        /// </summary>
        public Uri ServerAddress { get; set; }

        /// <summary>
        /// Gets the status of the WebDriver service.
        /// </summary>
        public WebDriverStatusModel Status => GetStatus(ServerAddress);

        /// <summary>
        /// Gets or sets a value indicating whether to suppress diagnostic information.
        /// </summary>
        public bool SuppressDiagnostic { get; set; }

        /// <summary>
        /// Gets or sets the timeout duration for various WebDriver operations.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Imports the WebDriver if a download URL is provided.
        /// </summary>
        /// <remarks> the download URL is not provided (null or empty), no action is taken.</remarks>
        public void DeployWebDriver()
        {
            // No download URL provided, skip the import
            if (string.IsNullOrEmpty(_downloadUrl))
            {
                DeployWebDriver(_downloadUrl);
                return;
            }

            // If the download URL is provided, but the implementation is not yet available.
            throw new NotImplementedException("Importing WebDriver with a download URL is not yet implemented.");
        }

        /// <summary>
        /// Imports the WebDriver from the specified download URL.
        /// </summary>
        /// <param name="downloadUrl">The URL from which to download the WebDriver.</param>
        /// <remarks>This method is meant to be overridden in derived classes to provide specific implementation details.</remarks>
        protected virtual void DeployWebDriver(string downloadUrl)
        {
            // Default implementation does nothing.
        }

        /// <summary>
        /// Disposes of the resources used by the WebDriver service.
        /// </summary>
        public void Dispose()
        {
            // Call the Dispose method with true to release both managed and unmanaged resources
            Dispose(true);

            // Suppress finalization to avoid unnecessary calls to the finalizer
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the resources used by the WebDriver service.
        /// </summary>
        /// <param name="disposing">True if called from the Dispose method; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            // If not disposing (i.e., called from the finalizer), return without further action
            if (!disposing)
            {
                return;
            }

            // If the process is not null, kill and dispose it
            Process?.Kill();
            Process?.Dispose();

            // Invoke ServiceDisposed event after disposing resources
            ServiceDisposed?.Invoke(sender: this, e: EventArgs.Empty);
        }

        /// <summary>
        /// Starts the WebDriver service.
        /// </summary>
        public void StartService()
        {
            // If the process is already running, return without taking any action
            if (Process != null)
            {
                return;
            }

            // If the binaries path is a remote URL, invoke ServiceStarted event and return
            if (Regex.IsMatch(input: _binariesPath, pattern: "^(http(s)?)://"))
            {
                ServiceStarted?.Invoke(sender: this, e: new(process: default));
                return;
            }

            // Configure the process start info
            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(_binariesPath, _executableName),
                Arguments = string.IsNullOrEmpty(_arguments) ? string.Format(PortParameterFormat, _port) : _arguments,
                UseShellExecute = false,
                CreateNoWindow = HideCommandPromptWindow
            };
            startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);

            // Initialize the process with the configured start info
            Process = new Process { StartInfo = startInfo };

            // Invoke ServiceStarting event before starting the process
            ServiceStarting?.Invoke(sender: this, e: new(startInfo));

            // Start the process
            Process.Start();

            // Wait for the service to be ready within the specified timeout
            WaitForService(ServerAddress, Timeout);

            // Invoke ServiceStarted event after the process has started
            ServiceStarted?.Invoke(sender: this, e: new(Process));
        }

        /// <summary>
        /// Stops the WebDriver service.
        /// </summary>
        public void StopService()
        {
            // If the process is not null, kill and dispose it
            Process?.Kill();
            Process?.Dispose();
        }

        // Waits for the WebDriver service to be ready within the specified timeout.
        private static void WaitForService(Uri serverAddress, TimeSpan timeout)
        {
            // Flag indicating whether the service is ready
            var isReady = false;

            // Calculate the initial timeout based on the current time and the specified timeout
            var initTimeout = DateTime.UtcNow.Add(timeout);

            // Keep checking the service status until it becomes ready or the timeout is reached
            while (!isReady && DateTime.UtcNow < initTimeout)
            {
                isReady = GetStatus(serverAddress).Ready;
            }

            // If the service is ready, return without further action
            if (isReady)
            {
                return;
            }

            // If the service does not become ready within the timeout, throw an exception
            var msg = $"Waiting for the WebDriver service at {serverAddress} to become ready has timed out.";
            throw new DriverServiceStartTimeoutException(msg);
        }

        // Retrieves the status of the WebDriver service at the specified server address.
        private static WebDriverStatusModel GetStatus(Uri serverAddress)
        {
            // Create a default status model with "Ready" set to false
            var statusModel = new WebDriverStatusModel
            {
                Ready = false
            };

            try
            {
                // Set up a cancellation token with a timeout of 5000 milliseconds
                var cancellationToken = new CancellationTokenSource(5000).Token;

                // Construct the URL for the GetStatus route relative to the server address
                var url = new Uri(serverAddress, new Uri(WebDriverCommands.GetStatus.Route, UriKind.Relative));

                // Create an HTTP GET request message
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Disable the "Connection: close" header in the request
                request.Headers.ConnectionClose = false;

                // Send the HTTP request and wait for the response asynchronously
                var response = WebDriverUtilities.HttpClient.SendAsync(request, cancellationToken).GetAwaiter().GetResult();

                // If the response is not successful, return the default status model
                if (!response.IsSuccessStatusCode)
                {
                    // Log the unsuccessful response
                    Console.WriteLine($"GetStatus request failed with status code: {response.StatusCode}");

                    return statusModel;
                }

                // Read the response content as a string
                var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // Parse the JSON document from the response content
                var value = JsonDocument.Parse(responseContent).RootElement.GetProperty("value");

                // Deserialize the JSON value into a WebDriverStatusModel using the configured JSON options
                return JsonSerializer.Deserialize<WebDriverStatusModel>($"{value}", s_jsonOptions);
            }
            catch (HttpRequestException httpException)
            {
                // Log the HTTP request exception and return the default status model
                Console.WriteLine($"GetStatus request failed: {httpException.Message}");
                return statusModel;
            }
            catch (JsonException jsonException)
            {
                // Log the JSON parsing exception and return the default status model
                Console.WriteLine($"Failed to parse JSON response: {jsonException.Message}");
                return statusModel;
            }
            catch (Exception generalException)
            {
                // Log any other exceptions and return the default status model
                Console.WriteLine($"An unexpected error occurred: {generalException.Message}");
                return statusModel;
            }
        }
        #endregion
    }
}
