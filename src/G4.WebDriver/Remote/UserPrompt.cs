/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Alert.cs
 */
using G4.WebDriver.Extensions;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents an alert dialog or a user prompt in the browser.
    /// Provides methods to interact with alerts, such as accepting, dismissing, and sending text to them.
    /// </summary>
    /// <param name="driver">The WebDriver instance used to interact with the browser.</param>
    public class UserPrompt(IWebDriver driver) : IUserPrompt
    {
        #region *** Fields     ***
        // Command invoker to send WebDriver commands to the browser.
        private readonly IWebDriverCommandInvoker _invoker = driver.Invoker;
        #endregion

        #region *** Properties ***
        /// <inheritdoc />
        public string Text => _invoker
            .Invoke(_invoker.NewCommand(nameof(WebDriverCommands.GetAlertText)))
            .Value
            .ToString();
        #endregion

        #region *** Methods    ***
        /// <inheritdoc />
        public void Approve()
        {
            // Create a new command to accept the alert
            var command = _invoker.NewCommand(nameof(WebDriverCommands.AlertAccept));

            // Invoke the command to accept the alert
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public void Close()
        {
            // Create a new command to dismiss the alert
            var command = _invoker.NewCommand(nameof(WebDriverCommands.AlertDismiss));

            // Invoke the command to dismiss the alert
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public void SendKeys(string text)
        {
            // Create a new command to send text to the alert
            var command = _invoker
                .NewCommand(nameof(WebDriverCommands.SendAlertText))
                .SetData(new { Text = text });

            // Invoke the command to send text to the alert
            _invoker.Invoke(command);
        }
        #endregion
    }
}
