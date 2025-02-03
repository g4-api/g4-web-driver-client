/*
 * RESOURCES
 * https://www.w3.org/TR/webdriver/#contexts
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/ITargetLocator.cs
 */
namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Defines the interface through which the user can manipulate user prompts.
    /// </summary>
    public interface IUserPrompt
    {
        #region *** Properties ***
        /// <summary>
        /// Gets the text of the alert dialog.
        /// </summary>
        string Text { get; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Accepts the alert dialog (equivalent to clicking "OK").
        /// </summary>
        void Approve();

        /// <summary>
        /// Dismisses the alert dialog (equivalent to clicking "Cancel").
        /// </summary>
        void Close();

        /// <summary>
        /// Sends text to the alert dialog's input field.
        /// </summary>
        /// <param name="text">The text to send to the alert input field.</param>
        void SendKeys(string text);
        #endregion
    }
}
