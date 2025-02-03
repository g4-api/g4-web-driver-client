namespace G4.WebDriver.Models
{
    /// <summary>
    /// This model is used to represent a null action, typically used for introducing delays or pauses.
    /// It provides properties to define the duration of the null action and its type.
    /// </summary>
    public class NullActionModel : ActionModelBase
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the type of the null action.
        /// </summary>
        public string Type { get; set; }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Provides constants for different types of null actions.
        /// </summary>
        public static class ActionTypes
        {
            /// <summary>
            /// Represents a pause null action.
            /// </summary>
            public const string Pause = "pause";
        }
        #endregion
    }
}
