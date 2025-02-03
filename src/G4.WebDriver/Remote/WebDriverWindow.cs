using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents a set of WebDriver commands related to window manipulation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="WebDriverWindow"/> class.
    /// </remarks>
    /// <param name="driver">Instance of the driver currently in use.</param>
    public class WebDriverWindow(IWebDriver driver) : IWindow
    {
        #region *** Fields     ***
        // The WebDriver instance used for browser automation.
        private readonly IWebDriver _driver = driver;
        #endregion

        #region *** Properties ***
        /// <summary>
        /// Gets the size and position of the browser window.
        /// </summary>
        /// <returns>The <see cref="RectModel"/> representing the window size and position.</returns>
        public RectModel WindowRect => _driver
            .Invoker
            .Invoke<RectModel>(_driver.Invoker.NewCommand(nameof(WebDriverCommands.GetWindowRect)));
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Puts the window in full-screen mode.
        /// </summary>
        public void FullScreen()
        {
            // Get the invoker from the WebDriver.
            var invoker = _driver.Invoker;

            // Create a new command for full-screen operation.
            var command = invoker.NewCommand(nameof(WebDriverCommands.FullScreenWindow));

            // Invoke the full-screen command.
            invoker.Invoke(command);
        }

        /// <summary>
        /// Maximizes the current browser window.
        /// </summary>
        public void Maximize()
        {
            // Get the invoker from the WebDriver instance
            var invoker = _driver.Invoker;

            // Mount the command for maximizing the window
            var command = invoker.NewCommand(nameof(WebDriverCommands.MaximizeWindow));

            // Invoke the command
            invoker.Invoke(command);
        }

        /// <summary>
        /// Minimizes the current browser window.
        /// </summary>
        public void Minimize()
        {
            // Get the invoker from the WebDriver instance
            var invoker = _driver.Invoker;

            // Mount the command for minimizing the window
            var command = invoker.NewCommand(nameof(WebDriverCommands.MinimizeWindow));

            // Invoke the command
            invoker.Invoke(command);
        }

        /// <summary>
        /// Sets the size and position of the browser window.
        /// </summary>
        /// <param name="rect">The <see cref="RectModel"/> containing the window size and position.</param>
        /// <exception cref="ArgumentNullException">Thrown when the rect is null.</exception>
        public void SetWindowRect(RectModel rect)
        {
            // Get the invoker from the driver
            var invoker = _driver.Invoker;

            // Create a new SetWindowRect command
            var command = invoker.NewCommand(nameof(WebDriverCommands.SetWindowRect));

            // Set the data for the command as the provided rect
            command.Data = rect ?? throw new ArgumentNullException(nameof(rect), "The `rect` parameter cannot be null.");

            // Invoke the command using the invoker
            invoker.Invoke(command);
        }
        #endregion
    }
}
