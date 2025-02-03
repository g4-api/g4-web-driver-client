namespace G4.WebDriver.Models
{
    /// <summary>
    /// This model is used to describe keyboard actions performed during input interactions.
    /// It provides properties to define the type of keyboard action and the associated value.
    /// </summary>
    public class KeyboardActionModel : ActionModelBase
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the type of keyboard action.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the keyboard action.
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Provides constants for different types of keyboard actions.
        /// </summary>
        public static class ActionTypes
        {
            /// <summary>
            /// Represents a key down action.
            /// </summary>
            public const string KeyDown = "keyDown";

            /// <summary>
            /// Represents a key up action.
            /// </summary>
            public const string KeyUp = "keyUp";
        }
        #endregion
    }
}
