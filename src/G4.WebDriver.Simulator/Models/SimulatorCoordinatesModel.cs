using G4.WebDriver.Remote;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the coordinates model for the simulator, implementing <see cref="ICoordinates"/>.
    /// Provides fixed coordinate points for simulation purposes.
    /// </summary>
    public class SimulatorCoordinatesModel : ICoordinates
    {
        /// <summary>
        /// Gets the location on the screen, relative to the origin of the screen.
        /// </summary>
        public PointModel LocationOnScreen => new()
        {
            X = 1, // X-coordinate on the screen
            Y = 2  // Y-coordinate on the screen
        };

        /// <summary>
        /// Gets the location in the viewport, relative to the origin of the viewport.
        /// </summary>
        public PointModel LocationInViewPort => new()
        {
            X = 3, // X-coordinate in the viewport
            Y = 4  // Y-coordinate in the viewport
        };

        /// <summary>
        /// Gets the location in the DOM, relative to the origin of the document.
        /// </summary>
        public PointModel LocationInDom => new()
        {
            X = 5, // X-coordinate in the DOM
            Y = 6  // Y-coordinate in the DOM
        };

        /// <summary>
        /// Gets an auxiliary locator, which provides additional information for locating elements.
        /// </summary>
        public object AuxiliaryLocator => new
        {
            X = 9, // X-coordinate of the auxiliary locator
            Y = 10 // Y-coordinate of the auxiliary locator
        };
    }
}
