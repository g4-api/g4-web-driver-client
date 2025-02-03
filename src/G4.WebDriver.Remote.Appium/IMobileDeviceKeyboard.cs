using G4.WebDriver.Remote.Appium.Models;

namespace G4.WebDriver.Remote.Appium
{
    /// <summary>
    /// Represents keyboard interactions for Appium WebDriver.
    /// </summary>
    public interface IMobileDeviceKeyboard
    {
        #region *** Properties ***
        /// <summary>
        /// Gets a value indicating whether the on-screen keyboard is currently visible.
        /// </summary>
        public bool IsOnScreenKeyboardVisible { get; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Hides the on-screen keyboard if it is currently displayed.
        /// If the keyboard is not visible, this operation has no effect.
        /// </summary>
        void HideKeyboard();

        /// <summary>
        /// Hides the on-screen keyboard using the specified hiding strategy.
        /// </summary>
        /// <param name="strategy">
        /// The hiding strategy to be applied. Possible values include:
        /// 'press', 'pressKey', 'swipeDown', 'tapOut', 'tapOutside', 'default'.
        /// </param>
        void HideKeyboard(string strategy);

        /// <summary>
        /// Hides the on-screen keyboard using the specified hiding strategy and key.
        /// </summary>
        /// <param name="strategy">
        /// The hiding strategy to be applied. Possible values include:
        /// 'press', 'pressKey', 'swipeDown', 'tapOut', 'tapOutside', 'default'.
        /// </param>
        /// <param name="keyName">The key name associated with the hiding strategy.</param>
        void HideKeyboard(string strategy, string keyName);

        /// <summary>
        /// Hides the on-screen keyboard using the parameters specified in the provided model.
        /// </summary>
        /// <param name="model">The model containing parameters for hiding the keyboard.</param>
        void HideKeyboard(HideKeyboardModel model);

        /// <summary>
        /// Shows the on-screen keyboard if it is currently hidden.
        /// </summary>
        /// <remarks>
        /// This method sends a command to show the on-screen keyboard.
        /// If the keyboard is already visible, this operation has no effect.
        /// </remarks>
        void ShowKeyboard();
        #endregion
    }
}
