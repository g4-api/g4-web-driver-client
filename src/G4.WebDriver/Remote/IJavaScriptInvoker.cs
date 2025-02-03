using G4.WebDriver.Models;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Defines the interface through which the user can invoke JavaScript.
    /// </summary>
    public interface IJavaScriptInvoker
    {
        /// <summary>
        /// Invokes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">The JavaScript code to invoke.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>The value returned by the script.</returns>
        object InvokeScript(string script, params object[] args);

        /// <summary>
        /// Invokes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">The JavaScript code to invoke.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>The value returned by the script.</returns>
        T InvokeScript<T>(string script, params object[] args);

        /// <summary>
        /// Invokes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">A PinnedScriptModel object containing the code to invoke.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>The value returned by the script.</returns>
        object InvokeScript(PinnedScriptModel script, params object[] args);

        /// <summary>
        /// Invokes JavaScript asynchronously in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">The JavaScript code to invoke.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>The value returned by the script.</returns>
        object InvokeAsyncScript(string script, params object[] args);
    }
}
