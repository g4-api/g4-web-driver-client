using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace G4.WebDriver.DeveloperTools
{
    /// <summary>
    /// Represents a connection to a WebSocket debugger endpoint, providing methods to communicate with
    /// the debugger using the DevTools Protocol.
    /// </summary>
    /// <param name="webSocketDebuggerUrl">The WebSocket debugger URL to connect to.</param>
    /// <param name="timeout">The timeout duration for WebSocket operations.</param>
    public class DeveloperToolsConnection(string webSocketDebuggerUrl, TimeSpan timeout) : IDisposable
    {
        #region *** Fields       ***
        // JSON serializer options used for serializing and deserializing JSON data.
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        // The WebSocket client used for communication
        private readonly ClientWebSocket _clientWebSocket = new();

        // The timeout period for WebSocket operations
        private readonly TimeSpan _timeout = timeout;

        // The WebSocket URL to connect to
        private readonly Uri _webSocketUrl = new(webSocketDebuggerUrl);

        // A counter for command IDs used in communication
        private int _commandId = 1;
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="DeveloperToolsConnection"/> class.
        /// Uses a default timeout of 5 seconds for WebSocket operations.
        /// </summary>
        /// <param name="webSocketDebuggerUrl">The WebSocket debugger URL to connect to.</param>
        public DeveloperToolsConnection(string webSocketDebuggerUrl)
            : this(webSocketDebuggerUrl, TimeSpan.FromSeconds(5))
        { }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Attaches to a target specified by its ID and retrieves the session and target information.
        /// </summary>
        /// <param name="targetId">The ID of the target to attach to.</param>
        /// <returns>A tuple containing the session ID and the target model.</returns>
        public (string Session, DeveloperToolsTargetModel Target) AttachToTarget(string targetId)
        {
            // Create a command to attach to the target with the specified targetId and flatten set to true
            var command = new DeveloperToolsCommandModel
            {
                Id = _commandId++,
                Method = "Target.attachToTarget",
                Parameters = new Dictionary<string, object>
                {
                    ["targetId"] = targetId,
                    ["flatten"] = true
                }
            };

            // Send the command to the WebSocket and wait for the response
            var jsonResponse = SendCommand(command, _timeout);

            // Extract the session ID from the JSON response
            var session = jsonResponse.RootElement.GetProperty("params").GetProperty("sessionId").GetString();

            // Extract the target info as a JSON string from the JSON response
            var target = jsonResponse.RootElement.GetProperty("params").GetProperty("targetInfo").GetRawText();

            // Deserialize the target info JSON string into a DeveloperToolsTargetModel and return the session ID and target model
            return (session, JsonSerializer.Deserialize<DeveloperToolsTargetModel>(target, s_jsonOptions));
        }

        /// <summary>
        /// Synchronously connects to the WebSocket debugger URL and returns the current instance.
        /// </summary>
        /// <returns>The current <see cref="DeveloperToolsConnection"/> instance.</returns>
        public DeveloperToolsConnection Connect()
        {
            // Attempt to connect to the WebSocket using the provided URL and no cancellation token
            _clientWebSocket
                .ConnectAsync(_webSocketUrl, CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            // Return the current instance of DeveloperToolsConnection
            return this;
        }

        /// <summary>
        /// Closes the connection to the WebSocket if it is open.
        /// </summary>
        public void Disconnect()
        {
            // Gracefully close the WebSocket connection
            _clientWebSocket?
                .CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure, statusDescription: "Closing", CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="DeveloperToolsConnection"/> class.
        /// </summary>
        public void Dispose()
        {
            // Call the protected Dispose method to release resources
            Dispose(true);

            // Prevent the garbage collector from calling the object's finalizer
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DeveloperToolsConnection"/> class.
        /// </summary>
        /// <param name="disposing">A boolean indicating whether the method is being called from the Dispose method (true) or from the finalizer (false).</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check if the method is being called from the Dispose method and not from the finalizer
            // Exit if disposing is false, as we only need to release unmanaged resources
            if (!disposing)
            {
                return;
            }

            // Gracefully close the WebSocket connection
            _clientWebSocket?
                .CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure, statusDescription: "Closing", CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            // Dispose of the WebSocket client to release all managed resources
            _clientWebSocket?.Dispose();
        }

        /// <summary>
        /// Retrieves the list of targets from the WebSocket debugger and returns them
        /// as a collection of <see cref="DeveloperToolsTargetModel"/>.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="DeveloperToolsTargetModel"/> representing the targets.</returns>
        public IEnumerable<DeveloperToolsTargetModel> GetTargets()
        {
            // Create a command to get the list of targets
            var command = new DeveloperToolsCommandModel
            {
                Id = _commandId++,
                Method = "Target.getTargets"
            };

            // Send the command to the WebSocket and wait for the response
            var jsonResponse = SendCommand(command, _timeout);

            // Extract the targetInfos array from the JSON response as a raw JSON string
            var targets = jsonResponse.RootElement.GetProperty("result").GetProperty("targetInfos").GetRawText();

            // Deserialize the JSON string into an array of DeveloperToolsTargetModel and return it
            return JsonSerializer.Deserialize<DeveloperToolsTargetModel[]>(targets, s_jsonOptions);
        }

        /// <summary>
        /// Sends a command to the WebSocket and retrieves the response as a <see cref="JsonDocument"/>.
        /// </summary>
        /// <param name="command">The command to be sent to the WebSocket.</param>
        /// <returns>A <see cref="JsonDocument"/> representing the response from the WebSocket.</returns>
        public JsonDocument SendCommand(DeveloperToolsCommandModel command)
        {
            // Call the overloaded SendCommand method with the default timeout
            return SendCommand(command, _timeout);
        }

        /// <summary>
        /// Sends a command to the WebSocket with a specified timeout and retrieves the response as a <see cref="JsonDocument"/>.
        /// </summary>
        /// <param name="command">The command to be sent to the WebSocket.</param>
        /// <param name="timeout">The timeout duration for the WebSocket operations.</param>
        /// <returns>A <see cref="JsonDocument"/> representing the response from the WebSocket.</returns>
        public JsonDocument SendCommand(DeveloperToolsCommandModel command, TimeSpan timeout)
        {
            // Increment and set the command ID for the new command
            command.Id = _commandId++;

            // Serialize the command object to a JSON string
            var requestContent = JsonSerializer.Serialize(command, s_jsonOptions);

            // Convert the JSON string to a byte array
            var buffer = Encoding.UTF8.GetBytes(requestContent);

            // Create a cancellation token with the specified timeout
            var cancellationToken = new CancellationTokenSource(timeout).Token;

            // Send the command to the WebSocket
            _clientWebSocket
                .SendAsync(buffer, messageType: WebSocketMessageType.Text, endOfMessage: true, cancellationToken)
                .GetAwaiter()
                .GetResult();

            // Allocate a buffer to receive the response
            var resultBuffer = new byte[4096];

            // Receive the response from the WebSocket
            var webSocketReceiveResult = _clientWebSocket
                .ReceiveAsync(resultBuffer, cancellationToken)
                .GetAwaiter()
                .GetResult();

            // Convert the received byte array to a string
            var resultContent = Encoding.UTF8.GetString(resultBuffer, 0, webSocketReceiveResult.Count);

            try
            {
                // Parse the result string into a JsonDocument
                return JsonDocument.Parse(resultContent);
            }
            catch (Exception e)
            {
                // Handle any exceptions that occur during parsing
                var message = $"{e.GetBaseException().Message}";
                var error501 = $"An error occurred while sending the command or processing the response. " +
                    $"Please check the WebSocket connection and the command format. Error details: {message}";
                var result = new
                {
                    Error = error501
                };

                // Serialize the error result to a JSON string
                var resultBody = JsonSerializer.Serialize(result, s_jsonOptions);

                // Return the error result as a JsonDocument
                return JsonDocument.Parse(resultBody);
            }
        }
        #endregion
    }
}
