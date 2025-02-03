using G4.WebDriver.Models;

using System;
using System.Collections.Generic;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides a way to send commands to the remote end.
    /// </summary>
    public interface IWebDriverCommandInvoker : IWebDriverSession
    {
        #region *** Properties ***
        /// <summary>
        /// Gets the collection of commands loaded by the invoker.
        /// </summary>
        IDictionary<string, WebDriverCommandModel> Commands { get; }

        /// <summary>
        /// Gets the underline WebDriverService instance of the IWebDriverCommandInvoker.
        /// </summary>
        IWebDriverService WebDriverService { get; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Attempts to add a command to the repository of commands known to the invoker.
        /// </summary>
        /// <param name="commandName">The name of the command to attempt to add.</param>
        /// <param name="command">The HttpCommandModel describing the command to add.</param>
        /// <remarks>If the command already exists, it will be overwritten</remarks>
        IWebDriverCommandInvoker AddCommand(string commandName, WebDriverCommandModel command);

        /// <summary>
        /// Attempts to add a commands to the repository of commands known to the invoker.
        /// </summary>
        /// <param name="container">The commands container that holds the commands to add.</param>
        /// <returns><see cref="true"/> if the new command has been added successfully; otherwise, <see cref="false"/>.</returns>
        bool AddFromContainer(Type container);

        /// <summary>
        /// Invokes a command
        /// </summary>
        /// <param name="commandName">The command to invoke.</param>
        /// <returns>A new CommandResponseModel object.</returns>
        WebDriverResponseModel Invoke(string commandName);

        /// <summary>
        /// Invokes a command
        /// </summary>
        /// <param name="commandName">The command to invoke.</param>
        /// <param name="data">The data to send as the request body.</param>
        /// <returns>A new CommandResponseModel object.</returns>
        WebDriverResponseModel Invoke(string commandName, object data);

        /// <summary>
        /// Invokes a command.
        /// </summary>
        /// <param name="command">The command to invoke.</param>
        /// <returns>A new CommandResponseModel object.</returns>
        WebDriverResponseModel Invoke(WebDriverCommandModel command);

        /// <summary>
        /// Invokes a command.
        /// </summary>
        /// <param name="command">The command to invoke.</param>
        /// <returns>A new model of type T object.</returns>
        T Invoke<T>(WebDriverCommandModel command);

        /// <summary>
        /// Attempts to remove a command to the repository of commands known to the invoker.
        /// </summary>
        /// <param name="commandName">The name of the command to attempt to add.</param>
        /// <returns><see cref="true"/> if the new command has been removed successfully; otherwise, <see cref="false"/>.</returns>
        bool RemoveCommand(string commandName);
        #endregion
    }
}
