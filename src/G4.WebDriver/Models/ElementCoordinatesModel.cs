using G4.WebDriver.Remote;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the coordinates of an element in various contexts, including the DOM, the viewport, and the screen.
    /// Implements the <see cref="ICoordinates"/> interface.
    /// </summary>
    public class ElementCoordinatesModel : ICoordinates
    {
        /// <inheritdoc />
        public object AuxiliaryLocator { get; set; }

        /// <inheritdoc />
        public PointModel LocationInDom { get; set; }

        /// <inheritdoc />
        public PointModel LocationInViewPort { get; set; }

        /// <inheritdoc />
        public PointModel LocationOnScreen { get; set; }
    }
}
