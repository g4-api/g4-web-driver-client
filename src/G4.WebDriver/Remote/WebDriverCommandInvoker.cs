using G4.WebDriver.Attributes;
using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Models.Events;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Invokes WebDriver commands and processes command responses.
    /// </summary>
    public class WebDriverCommandInvoker : IWebDriverCommandInvoker
    {
        #region *** Fields       ***
        // The default string comparison for JSON error codes.
        private const StringComparison Compare = StringComparison.OrdinalIgnoreCase;

        // List of exception types with WebDriverExceptionAttribute.
        private static ReadOnlyCollection<Type> s_exceptionTypes = new(new List<Type>(Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(i => i.GetCustomAttribute<WebDriverExceptionAttribute>() != null)));

        // JSON serialization options.
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        // Gets the list of exception types with the WebDriverExceptionAttribute.
        private static ReadOnlyCollection<Type> ExceptionTypes
        {
            get
            {
                // If the exception types list is empty, retrieve and update it.
                if (s_exceptionTypes?.Count == 0)
                {
                    return s_exceptionTypes;
                }

                // Retrieve the list of exception types with the WebDriverExceptionAttribute.
                var list = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(i => i.GetCustomAttribute<WebDriverExceptionAttribute>() != null)
                    .ToList();

                // Update the exception types list.
                s_exceptionTypes = new(list);
                return s_exceptionTypes;
            }
        }

        // Gets the timeout duration for WebDriver commands.
        private readonly TimeSpan _timeout;

        // Gets a value indicating whether to use keep-alive for the WebDriver connection.
        private readonly bool _keepAlive;

        // Gets the HttpClient used for WebDriver communication.
        private readonly HttpClient _httpClient;
        #endregion

        #region *** Events       ***
        /// <summary>
        /// Event raised before a WebDriver command is invoked.
        /// </summary>
        public event EventHandler<WebDriverRequestEventArgs> CommandInvoking;

        /// <summary>
        /// Event raised after a WebDriver command is invoked.
        /// </summary>
        public event EventHandler<WebDriverResponseEventArgs> CommandInvoked;
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the
        /// specified <see cref="WebDriverService"/>. The default timeout is set to 1 minute.
        /// </summary>
        /// <param name="driverService">The WebDriver service.</param>
        public WebDriverCommandInvoker(WebDriverService driverService)
            : this(
                  serverAddress: driverService.ServerAddress,
                  httpClient: WebDriverUtilities.HttpClient,
                  timeout: TimeSpan.FromMinutes(1),
                  keepAlive: true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the
        /// specified <see cref="Remote.WebDriverService"/> and timeout.
        /// </summary>
        /// <param name="driverService">The WebDriver service.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public WebDriverCommandInvoker(WebDriverService driverService, TimeSpan timeout)
            : this(serverAddress: driverService.ServerAddress, httpClient: WebDriverUtilities.HttpClient, timeout, keepAlive: true)
        {
            WebDriverService = driverService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the specified server address.
        /// The default timeout is set to 1 minute.
        /// </summary>
        /// <param name="serverAddress">The address of the WebDriver server.</param>
        public WebDriverCommandInvoker(Uri serverAddress)
            : this(serverAddress, httpClient: WebDriverUtilities.HttpClient, timeout: TimeSpan.FromMinutes(1), keepAlive: true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the specified server
        /// address and timeout.
        /// The default value for keep-alive is set to true.
        /// </summary>
        /// <param name="serverAddress">The address of the WebDriver server.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public WebDriverCommandInvoker(Uri serverAddress, TimeSpan timeout)
            : this(serverAddress, httpClient: WebDriverUtilities.HttpClient, timeout, keepAlive: true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the specified server address,
        /// timeout, and keep-alive setting.
        /// </summary>
        /// <param name="serverAddress">The address of the WebDriver server.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        /// <param name="keepAlive">A value indicating whether to use keep-alive for the WebDriver connection.</param>
        public WebDriverCommandInvoker(Uri serverAddress, TimeSpan timeout, bool keepAlive)
            : this(serverAddress, WebDriverUtilities.HttpClient, timeout, keepAlive)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the
        /// specified <see cref="WebDriverService"/> and <see cref="HttpClient"/>. The default timeout is set to 1 minute.
        /// </summary>
        /// <param name="driverService">The WebDriver service.</param>
        /// <param name="httpClient">The HTTP client to be used for WebDriver communication.</param>
        public WebDriverCommandInvoker(WebDriverService driverService, HttpClient httpClient)
            : this(serverAddress: driverService.ServerAddress, httpClient, timeout: TimeSpan.FromMinutes(1), keepAlive: true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the
        /// specified <see cref="Remote.WebDriverService"/>, <see cref="HttpClient"/>, and timeout.
        /// </summary>
        /// <param name="driverService">The WebDriver service.</param>
        /// <param name="httpClient">The HTTP client to be used for WebDriver communication.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public WebDriverCommandInvoker(WebDriverService driverService, HttpClient httpClient, TimeSpan timeout)
            : this(serverAddress: driverService.ServerAddress, httpClient, timeout, keepAlive: true)
        {
            WebDriverService = driverService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the specified server address and <see cref="HttpClient"/>.
        /// The default timeout is set to 1 minute.
        /// </summary>
        /// <param name="serverAddress">The address of the WebDriver server.</param>
        /// <param name="httpClient">The HTTP client to be used for WebDriver communication.</param>
        public WebDriverCommandInvoker(Uri serverAddress, HttpClient httpClient)
            : this(serverAddress, httpClient, timeout: TimeSpan.FromMinutes(1), keepAlive: true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the specified server address, <see cref="HttpClient"/>, and timeout.
        /// The default value for keep-alive is set to true.
        /// </summary>
        /// <param name="serverAddress">The address of the WebDriver server.</param>
        /// <param name="httpClient">The HTTP client to be used for WebDriver communication.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public WebDriverCommandInvoker(Uri serverAddress, HttpClient httpClient, TimeSpan timeout)
            : this(serverAddress, httpClient, timeout, keepAlive: true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverCommandInvoker"/> class using the specified
        /// server address, <see cref="HttpClient"/>, timeout, and keep-alive setting.
        /// </summary>
        /// <param name="serverAddress">The address of the WebDriver server.</param>
        /// <param name="httpClient">The HTTP client to be used for WebDriver communication.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        /// <param name="keepAlive">A value indicating whether to use keep-alive for the WebDriver connection.</param>
        public WebDriverCommandInvoker(Uri serverAddress, HttpClient httpClient, TimeSpan timeout, bool keepAlive)
        {
            _timeout = timeout;       // The timeout duration for WebDriver commands.
            _keepAlive = keepAlive;   // value indicating whether to use keep-alive for the WebDriver connection.
            _httpClient = httpClient; // The HttpClient used for WebDriver communication.

            // The address of the WebDriver server.
            ServerAddress = serverAddress;

            // Get initial commands from the WebDriverCommands container.
            var initialCommands = GetCommandsFromContainer(typeof(WebDriverCommands));

            // If Commands is null, create a new ConcurrentDictionary.
            Commands ??= new ConcurrentDictionary<string, WebDriverCommandModel>();

            // Merge initial commands into the Commands dictionary.
            Commands.Merge(initialCommands);
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets the dictionary of WebDriver commands with their associated names.
        /// </summary>
        public IDictionary<string, WebDriverCommandModel> Commands { get; }

        /// <summary>
        /// Gets the WebDriver service used by the command invoker.
        /// </summary>
        public IWebDriverService WebDriverService { get; }

        /// <summary>
        /// Gets or sets the session ID model associated with the WebDriver command invoker.
        /// </summary>
        public SessionIdModel Session { get; set; }

        /// <summary>
        /// Gets the address of the WebDriver server.
        /// </summary>
        public Uri ServerAddress { get; }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Adds a custom WebDriver command to the invoker's dictionary of commands.
        /// </summary>
        /// <param name="commandName">The name of the custom WebDriver command.</param>
        /// <param name="command">The WebDriver command model to be added.</param>
        /// <returns>The current instance of the <see cref="IWebDriverCommandInvoker"/>.</returns>
        public IWebDriverCommandInvoker AddCommand(string commandName, WebDriverCommandModel command)
        {
            // Check if the commandName is not empty and the command is not the default value.
            if (!string.IsNullOrEmpty(commandName) && command != default)
            {
                // Add the custom WebDriver command to the Commands dictionary.
                Commands[commandName] = command;
            }

            // Return the current instance of the IWebDriverCommandInvoker.
            return this;
        }

        public bool AddFromContainer(Type container)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invokes a WebDriver command by name.
        /// </summary>
        /// <param name="commandName">The name of the WebDriver command to be invoked.</param>
        /// <returns>The response model containing the result of the command invoked without a request payload.</returns>
        public WebDriverResponseModel Invoke(string commandName)
        {
            // Route payload-free callers through the complete overload so command ownership has one implementation.
            return Invoke(commandName, data: null);
        }

        /// <summary>
        /// Invokes a WebDriver command by name and with the specified data.
        /// </summary>
        /// <param name="commandName">The name of the WebDriver command to be invoked.</param>
        /// <param name="data">The optional request payload assigned only to this command invocation.</param>
        /// <returns>The response model containing the result of the WebDriver command execution.</returns>
        public WebDriverResponseModel Invoke(string commandName, object data)
        {
            // Reject an unsupported command before allocating invocation-owned request state.
            if (string.IsNullOrEmpty(commandName) || !Commands.TryGetValue(commandName, out var commandOut))
            {
                var message = $"The WebDriver command with the name '{commandName}' is not supported or not found.";
                throw new NotSupportedException(message);
            }

            // Copy the registered template so this invocation owns every mutable request value.
            var command = commandOut.Copy();

            // Apply the request-specific payload and session without changing the reusable template.
            command.Data = data;
            command.Session = Session?.OpaqueKey;

            return Invoke(command);
        }

        /// <summary>
        /// Invokes a WebDriver command and processes the command response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the command response into.</typeparam>
        /// <param name="command">The WebDriver command to be invoked.</param>
        /// <returns>The deserialized result of the command response, or the default value for the specified type if the response is null.</returns>
        public T Invoke<T>(WebDriverCommandModel command)
        {
            // Invoke the WebDriver command and retrieve the command response.
            var commandResponse = Invoke(command);

            // Convert the command response value to a string.
            var responseBody = $"{commandResponse.Value}";

            // If the command response value is null, return the default value for the specified type.
            if (commandResponse.Value == null)
            {
                return default;
            }

            // If the command response value is not valid JSON, resolve its type and return.
            if (!responseBody.AssertJson())
            {
                return (T)((JsonElement)commandResponse.Value).ResolveType();
            }

            // Deserialize the JSON response into the specified type using JSON options.
            return JsonSerializer.Deserialize<T>(responseBody, s_jsonOptions);
        }

        /// <summary>
        /// Invokes a WebDriver command and returns the command response model.
        /// </summary>
        /// <param name="command">The WebDriver command to be invoked.</param>
        /// <returns>The response model containing the result of the WebDriver command execution.</returns>
        public WebDriverResponseModel Invoke(WebDriverCommandModel command)
        {
            // Require a command before allocating the isolated request sent to the remote endpoint.
            ArgumentNullException.ThrowIfNull(
                argument: command,
                paramName: nameof(command));

            // Copy caller-owned state so route expansion and transport metadata never escape this invocation.
            var invocation = command.Copy();
            invocation.ContentType = "application/json";

            // Resolve placeholders only on the invocation copy so registered templates remain reusable.
            invocation.Route = invocation
                .Route
                .Replace("$[session]", invocation.Session)
                .Replace("$[element]", invocation.Element);

            // Normalize the server address once so event observers and transport use the same endpoint root.
            var serverAddress = ServerAddress.AbsoluteUri.Trim('/');

            // Publish the resolved invocation without exposing or mutating the registered command template.
            CommandInvoking?.Invoke(sender: this, e: new(serverAddress, invocation));

            // Send the isolated request so concurrent invocations cannot exchange route, element, or payload state.
            var response = invocation.Send(_httpClient, baseUrl: serverAddress, _timeout, _keepAlive);

            // Publish the completed transport response after the request lifecycle finishes.
            CommandInvoked?.Invoke(sender: this, e: new(serverAddress, response));

            // Translate an anonymous 404 into the established missing-session failure contract.
            if (response.StatusCode == HttpStatusCode.NotFound && string.IsNullOrEmpty(invocation.Session))
            {
                var error404 = response.Content.ReadAsStringAsync().Result;
                throw new NoSuchSessionException(error404);
            }

            // Convert the transport result before applying the shared WebDriver error mapping.
            var webDriverResponse = WebDriverResponseModel.New(response);

            // Enforce protocol errors before returning the successful response to the caller.
            AssertWebDriverResponse(webDriverResponse);

            return webDriverResponse;
        }

        /// <summary>
        /// Removes a command from the command dictionary based on the specified command name.
        /// </summary>
        /// <param name="commandName">The name of the command to be removed.</param>
        /// <returns><c>true</c> if the command is successfully removed or if the specified command name is null or empty;otherwise, <c>false</c>.</returns>
        public bool RemoveCommand(string commandName)
        {
            // If the specified command name is null or empty, return false.
            if (string.IsNullOrEmpty(commandName))
            {
                return false;
            }

            // If the command dictionary does not contain the specified command name, return true.
            if (!Commands.ContainsKey(commandName))
            {
                return true;
            }

            // Remove the command with the specified name and return the result of the removal operation.
            return Commands.Remove(commandName);
        }

        // Asserts and handles WebDriver response errors by checking the response and throwing appropriate exceptions.
        private static void AssertWebDriverResponse(WebDriverResponseModel webDriverResponse)
        {
            // Get the error model from the WebDriver response.
            var error = webDriverResponse.NewErrorModel();

            // If there is no error, return without throwing an exception.
            if (error == null)
            {
                return;
            }

            // If the response is successful and there is no error, return without throwing an exception.
            if (webDriverResponse.WebDriverResponse.IsSuccessStatusCode && error == null)
            {
                return;
            }

            // Find the corresponding exception type based on the JSON error code.
            var exceptionType = ExceptionTypes
                .FirstOrDefault(i => i.GetCustomAttribute<WebDriverExceptionAttribute>().JsonErrorCode.Equals(error.Error, Compare));

            // If no matching exception type is found, throw a generic WebDriverException.
            if (exceptionType == default)
            {
                throw new WebDriverException(error.Message);
            }

            // Create an instance of the exception type with the error message and throw the exception.
            throw (Exception)Activator.CreateInstance(exceptionType, error.Message);
        }

        // Retrieves WebDriver commands from a specified commands container type.
        private static Dictionary<string, WebDriverCommandModel> GetCommandsFromContainer(Type container)
        {
            // Check if the specified container type has the WebDriverCommandsContainerAttribute.
            if (container.GetCustomAttribute<WebDriverCommandsContainerAttribute>() == null)
            {
                // Return an empty dictionary if the container type is not a valid commands container.
                return [];
            }

            // Retrieve public static properties of the container type.
            var containerCommands = container.GetProperties(BindingFlags.Public | BindingFlags.Static);
            var commands = new Dictionary<string, WebDriverCommandModel>();

            // Iterate through the container properties to identify and collect WebDriver commands.
            foreach (var containerCommand in containerCommands)
            {
                // Get the WebDriverCommandAttribute of the container property, if present.
                var attribute = containerCommand.GetCustomAttribute<WebDriverCommandAttribute>();

                // Check if the property is a valid WebDriver command by verifying the attribute and property type.
                var isCommandAttribute = attribute != null;
                var isCommand = isCommandAttribute && containerCommand.PropertyType == typeof(WebDriverCommandModel);

                // Skip to the next property if it is not a valid WebDriver command.
                if (!isCommand)
                {
                    continue;
                }

                // Add the WebDriver command to the dictionary with the command name as the key.
                commands[attribute.Command] = (WebDriverCommandModel)containerCommand.GetValue(null);
            }

            // Return the dictionary containing WebDriver commands from the container type.
            return commands;
        }
        #endregion
    }
}
