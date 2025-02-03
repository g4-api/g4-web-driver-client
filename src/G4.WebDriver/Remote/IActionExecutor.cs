/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/IActionExecutor.cs
 */
using G4.WebDriver.Remote.Interactions;

using System.Collections.Generic;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents an interface for invoking actions on a WebDriver instance.
    /// </summary>
    public interface IActionsInvoker
    {
        #region *** Properties ***
        /// <summary>
        /// Gets a value indicating whether the invoker is active.
        /// </summary>
        bool IsInvoker { get; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Invokes a sequence of actions on the WebDriver instance.
        /// </summary>
        /// <param name="sequence">The sequence of actions to be invoked.</param>
        void InvokeActions(IEnumerable<ActionSequence> sequence);

        /// <summary>
        /// Resets the input state of the WebDriver instance.
        /// </summary>
        void ResetInputState();
        #endregion
    }
}
