using G4.WebDriver.Models;

namespace G4.WebDriver.Remote.Uia
{
    /// <summary>
    /// Represents a web element that can interact with the user32.dll for native Windows UI automation.
    /// </summary>
    public interface IUser32Element
    {
        /// <summary>
        /// Gets the UI Automation attribute value of a web element.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <returns>The attribute value as a string.</returns>
        string GetAttribute(string name);

        /// <summary>
        /// Moves the mouse pointer over the specified web element using default mouse position data.
        /// </summary>
        /// <remarks>This method is designed for Windows environments only, utilizing the user32.dll for native mouse operations.</remarks>
        void MoveToElement();

        /// <summary>
        /// Moves the mouse pointer over the specified web element using the provided mouse position data.
        /// </summary>
        /// <param name="positionData">The mouse position data to use when moving the mouse.</param>
        /// <remarks>This method is designed for Windows environments only, utilizing the user32.dll for native mouse operations.</remarks>
        void MoveToElement(MousePositionInputModel positionData);

        /// <summary>
        /// Sends a native click command to a web element.
        /// </summary>
        void SendClick();

        /// <summary>
        /// Sends a native double-click command to a web element.
        /// </summary>
        void SendDoubleClick();

        /// <summary>
        /// Sets focus on a web element.
        /// </summary>
        void SetFocus();
    }
}
