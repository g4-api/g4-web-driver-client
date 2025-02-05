using G4.WebDriver.Attributes;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Represents a command invoker for the simulator WebDriver, implementing the <see cref="IWebDriverCommandInvoker"/> interface.
    /// This invoker is used to simulate WebDriver commands without actual browser interaction.
    /// </summary>
    public class SimulatorCommandInvoker : IWebDriverCommandInvoker
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorCommandInvoker"/> class.
        /// </summary>
        public SimulatorCommandInvoker()
        {
            // Initialize the Commands dictionary with commands from the specified container
            Commands ??= GetCommandsFromContainer(typeof(WebDriverCommands));
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets the WebDriver service associated with the simulator.
        /// </summary>
        public IWebDriverService WebDriverService => new SimulatorDriverService();

        /// <summary>
        /// Gets the dictionary of WebDriver commands.
        /// </summary>
        public IDictionary<string, WebDriverCommandModel> Commands { get; }

        /// <summary>
        /// Gets the address of the WebDriver server.
        /// </summary>
        public Uri ServerAddress => new("http://simulator.local.io");

        /// <summary>
        /// Gets or sets the session identifier model.
        /// </summary>
        public SessionIdModel Session { get; set; } = new SessionIdModel
        {
            OpaqueKey = $"simulator-{Guid.NewGuid()}"
        };
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Adds a new command to the invoker's command dictionary.
        /// </summary>
        /// <param name="commandName">The name of the command to add.</param>
        /// <param name="command">The command model to add.</param>
        /// <returns>The current instance of <see cref="IWebDriverCommandInvoker"/>.</returns>
        public IWebDriverCommandInvoker AddCommand(string commandName, WebDriverCommandModel command)
        {
            // Since this is a simulator, we can simply return 'this' without adding commands
            return this;
        }

        /// <summary>
        /// Adds commands from the specified container type.
        /// </summary>
        /// <param name="container">The type containing command definitions.</param>
        /// <returns><c>true</c> if commands were added; otherwise, <c>false</c>.</returns>
        public bool AddFromContainer(Type container)
        {
            // Since this is a simulator, we can return true without actually adding commands
            return true;
        }

        /// <summary>
        /// Invokes a command by name without any data.
        /// </summary>
        /// <param name="commandName">The name of the command to invoke.</param>
        /// <returns>The response model from the command execution.</returns>
        public WebDriverResponseModel Invoke(string commandName)
        {
            // Throw NotImplementedException to indicate that this method is not implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invokes a command by name with the specified data.
        /// </summary>
        /// <param name="commandName">The name of the command to invoke.</param>
        /// <param name="data">The data to pass to the command.</param>
        /// <returns>The response model from the command execution.</returns>
        public WebDriverResponseModel Invoke(string commandName, object data)
        {
            // Return a new response model with the current session's opaque key
            return new WebDriverResponseModel
            {
                Session = Session.OpaqueKey
            };
        }

        /// <summary>
        /// Invokes a command using the specified command model.
        /// </summary>
        /// <param name="command">The command model to invoke.</param>
        /// <returns>The response model from the command execution.</returns>
        public WebDriverResponseModel Invoke(WebDriverCommandModel command)
        {
            // Return a new response model with the current session's opaque key
            return new WebDriverResponseModel
            {
                Session = Session.OpaqueKey
            };
        }

        /// <summary>
        /// Invokes a command using the specified command model and returns a result of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result to return.</typeparam>
        /// <param name="command">The command model to invoke.</param>
        /// <returns>The result of the command execution.</returns>
        public T Invoke<T>(WebDriverCommandModel command)
        {
            // Throw NotImplementedException to indicate that this method is not implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a command from the invoker's command dictionary.
        /// </summary>
        /// <param name="commandName">The name of the command to remove.</param>
        /// <returns><c>true</c> if the command was removed; otherwise, <c>false</c>.</returns>
        public bool RemoveCommand(string commandName)
        {
            // Throw NotImplementedException to indicate that this method is not implemented
            throw new NotImplementedException();
        }

        // Retrieves commands from the specified container type.
        private static Dictionary<string, WebDriverCommandModel> GetCommandsFromContainer(Type container)
        {
            // Check if the container type is marked with the WebDriverCommandsContainerAttribute
            if (container.GetCustomAttribute<WebDriverCommandsContainerAttribute>() == null)
            {
                return [];
            }

            // Get all public static properties from the container type
            var containerCommands = container.GetProperties(BindingFlags.Public | BindingFlags.Static);
            var commands = new Dictionary<string, WebDriverCommandModel>();

            // Iterate over each property to find command definitions
            foreach (var containerCommand in containerCommands)
            {
                var attribute = containerCommand.GetCustomAttribute<WebDriverCommandAttribute>();
                var isCommandAttribute = attribute != null;
                var isCommand = isCommandAttribute && containerCommand.PropertyType == typeof(WebDriverCommandModel);

                if (!isCommand)
                {
                    continue;
                }

                // Add the command to the dictionary using the attribute's command name
                commands[attribute.Command] = (WebDriverCommandModel)containerCommand.GetValue(null);
            }

            // Return the dictionary of commands found in the container type (if any)
            return commands;
        }
        #endregion
    }
}
