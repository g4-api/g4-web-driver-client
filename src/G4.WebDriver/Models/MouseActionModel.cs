namespace G4.WebDriver.Models
{
    /// <summary>
    /// This model is used to describe various mouse actions performed during WebDriver interactions.
    /// It provides properties to define the type of mouse action, associated button, coordinates,
    /// duration, and origin. The <see cref="ActionTypes"/> class contains constants for different
    /// types of mouse actions.
    /// </summary>
    public class MouseActionModel : ActionModelBase
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the mouse button associated with the action.
        /// </summary>
        public int? Button { get; set; }

        /// <summary>
        /// Gets or sets the origin of the action.
        /// </summary>
        public object Origin { get; set; }

        /// <summary>
        /// Gets or sets the type of the mouse action.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the X-coordinate of the action.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y-coordinate of the action.
        /// </summary>
        public int Y { get; set; }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Provides constants for different types of mouse actions.
        /// </summary>
        public static class ActionTypes
        {
            /// <summary>
            /// Represents a pointer down action.
            /// </summary>
            public const string PointerDown = "pointerDown";

            /// <summary>
            /// Represents a pointer move action.
            /// </summary>
            public const string PointerMove = "pointerMove";

            /// <summary>
            /// Represents a pointer up action.
            /// </summary>
            public const string PointerUp = "pointerUp";
        }

        /// <summary>
        /// Provides constants for different mouse buttons.
        /// </summary>
        public static class MouseButtons
        {
            /// <summary>
            /// Represents the left mouse button.
            /// </summary>
            public const int Left = 0;

            /// <summary>
            /// Represents the right mouse button.
            /// </summary>
            public const int Right = 2;
        }
        #endregion
    }
}
