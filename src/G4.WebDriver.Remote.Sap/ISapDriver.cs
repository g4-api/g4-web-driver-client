using G4.WebDriver.Exceptions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Sap.Models;

namespace G4.WebDriver.Remote.Sap
{
    /// <summary>
    /// Represents the session-level SAP GUI commands that the W3C WebDriver protocol does not cover.
    /// </summary>
    /// <remarks>
    /// Every command runs against the SAP GUI session that the WebDriver session is attached to. Server failures
    /// surface as the matching WebDriver exceptions.
    /// </remarks>
    public interface ISapDriver
    {
        #region *** Methods      ***
        /// <summary>
        /// Gets the SAP object model document of every open window of the attached session.
        /// </summary>
        /// <returns>The XML document built from the SAP GUI Scripting object tree.</returns>
        string GetDocument();

        /// <summary>
        /// Gets the SAP object model document of the subtree of the element located by the specified locator.
        /// </summary>
        /// <param name="by">The locator of the element whose subtree is returned.</param>
        /// <returns>The XML document of the element subtree.</returns>
        /// <exception cref="NoSuchElementException">Thrown when no element matches the locator.</exception>
        string GetDocument(By by);

        /// <summary>
        /// Gets the main window's status bar of the attached session.
        /// </summary>
        /// <returns>The message type, ID, number, and text of the status bar.</returns>
        SapStatusBarModel GetStatusBar();

        /// <summary>
        /// Sends a SAP virtual key to the current window of the attached session.
        /// </summary>
        /// <param name="virtualKey">The SAP virtual key code, from 0 to 99, such as 0 for Enter or 3 for Back.</param>
        void SendVirtualKey(int virtualKey);

        /// <summary>
        /// Sends a SAP virtual key to the specified window of the attached session.
        /// </summary>
        /// <param name="virtualKey">The SAP virtual key code, from 0 to 99, such as 0 for Enter or 3 for Back.</param>
        /// <param name="window">The window name, such as <c>wnd[1]</c>; null targets the current window.</param>
        /// <exception cref="NoSuchWindowException">Thrown when the window is not open.</exception>
        void SendVirtualKey(int virtualKey, string window);
        #endregion
    }
}
