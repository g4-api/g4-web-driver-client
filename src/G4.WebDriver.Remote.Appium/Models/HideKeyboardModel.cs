namespace G4.WebDriver.Remote.Appium.Models
{
    /// <summary>
    /// Represents a model for hiding the keyboard on a mobile device, with various strategies and options.
    /// </summary>
    public class HideKeyboardModel
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the key used to hide the keyboard.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the key code used to hide the keyboard.
        /// </summary>
        public string KeyCode { get; set; }

        /// <summary>
        /// Gets or sets the key name used to hide the keyboard.
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Gets or sets the strategy used to hide the keyboard. See <see cref="Strategies"/> for available options.
        /// </summary>
        public string Strategy { get; set; }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Provides a set of predefined strategies for hiding the keyboard.
        /// </summary>
        public static class Strategies
        {
            /// <summary>
            /// Default strategy for hiding the keyboard.
            /// </summary>
            public const string Default = "default";

            /// <summary>
            /// Strategy to press a specific key to hide the keyboard.
            /// </summary>
            public const string Press = "press";

            /// <summary>
            /// Strategy to press a key using its key code to hide the keyboard.
            /// </summary>
            public const string PressKey = "pressKey";

            /// <summary>
            /// Strategy to swipe down to hide the keyboard.
            /// </summary>
            public const string SwipeDown = "swipeDown";

            /// <summary>
            /// Strategy to tap outside the keyboard to hide it.
            /// </summary>
            public const string TapOut = "tapOut";

            /// <summary>
            /// Strategy to tap outside the keyboard area to hide it.
            /// </summary>
            public const string TapOutside = "tapOutside";
        }
        #endregion
    }
}
