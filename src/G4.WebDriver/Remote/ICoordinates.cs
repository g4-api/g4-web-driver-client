/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Interactions/ICoordinates.cs
 */
using G4.WebDriver.Models;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents the coordinates of an element within different contexts such as the DOM, the viewport, and the screen.
    /// </summary>
    public interface ICoordinates
    {
        /// <summary>
        /// Gets an auxiliary locator that provides additional location information.
        /// </summary>
        object AuxiliaryLocator { get; }

        /// <summary>
        /// Gets the location of the element within the DOM.
        /// </summary>
        PointModel LocationInDom { get; }

        /// <summary>
        /// Gets the location of the element within the viewport.
        /// </summary>
        PointModel LocationInViewPort { get; }

        /// <summary>
        /// Gets the location of the element on the screen.
        /// </summary>
        PointModel LocationOnScreen { get; }
    }
}
