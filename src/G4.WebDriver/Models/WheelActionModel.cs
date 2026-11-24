using G4.WebDriver.Remote;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Describes one action of a wheel input source, as it is sent in a WebDriver "Perform Actions" request.
    /// </summary>
    /// <remarks>
    /// A wheel source accepts only two actions: a scroll and a pause. This model carries the scroll;
    /// <see cref="NullActionModel"/> already carries the pause, so it is not repeated here. The model is
    /// registered on <see cref="ActionModelBase"/> as a derived type, which is what lets a list of mixed
    /// actions keep the scroll members when it is serialized.
    /// </remarks>
    public class WheelActionModel : ActionModelBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="WheelActionModel"/> class with a duration of zero.
        /// </summary>
        /// <remarks>
        /// The driver reads the duration of every scroll and rejects one that is sent as null, so the model
        /// starts with zero, which scrolls at once. Set <see cref="ActionModelBase.Duration"/> to spread the
        /// scroll over a number of milliseconds.
        /// </remarks>
        public WheelActionModel()
        {
            // Give the scroll a numeric duration, because the request writes a missing duration as null.
            Duration = 0;
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the horizontal scroll amount in pixels; a positive value scrolls right.
        /// </summary>
        public int DeltaX { get; set; }

        /// <summary>
        /// Gets or sets the vertical scroll amount in pixels; a positive value scrolls down.
        /// </summary>
        public int DeltaY { get; set; }

        /// <summary>
        /// Gets or sets the point the scroll starts from: the viewport, or an element reference.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="ActionOrigins.Viewport"/>. An element reference makes the driver scroll the
        /// nearest scrollable container under the element's center. A wheel source does not accept
        /// <see cref="ActionOrigins.Pointer"/>; the driver answers it with an "invalid argument" error.
        /// </remarks>
        public object Origin { get; set; } = ActionOrigins.Viewport;

        /// <summary>
        /// Gets or sets the type of the wheel action.
        /// </summary>
        /// <remarks>Defaults to <see cref="ActionTypes.Scroll"/>, the only action specific to a wheel source.</remarks>
        public string Type { get; set; } = ActionTypes.Scroll;

        /// <summary>
        /// Gets or sets the horizontal offset, in pixels, of the scroll start point from the origin.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the vertical offset, in pixels, of the scroll start point from the origin.
        /// </summary>
        public int Y { get; set; }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Provides constants for the types of wheel actions.
        /// </summary>
        public static class ActionTypes
        {
            /// <summary>
            /// Represents a scroll action, which turns the wheel by the action's delta values.
            /// </summary>
            public const string Scroll = "scroll";
        }
        #endregion
    }
}
