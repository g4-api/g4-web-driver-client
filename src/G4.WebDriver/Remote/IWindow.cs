using G4.WebDriver.Models;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides methods for getting and setting the size and position of the browser window.
    /// </summary>
    public interface IWindow
    {
        #region *** Properties ***
        /// <summary>
        /// Gets the size and position on the screen of the operating system window
        /// corresponding to the current top-level browsing context.
        /// </summary>
        RectModel WindowRect { get; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Sets the current window to full screen if it is not already in that state.
        /// </summary>
        void FullScreen();

        /// <summary>
        /// Invokes the window manager-specific "maximize" operation,
        /// if any, on the window containing the current top-level browsing context.
        /// This typically increases the window to the maximum available size without going full-screen.
        /// </summary>
        void Maximize();

        /// <summary>
        /// The Minimize Window command invokes the window manager-specific "minimize" operation,
        /// if any, on the window containing the current top-level browsing context.
        /// This typically hides the window in the system tray.
        /// </summary>
        void Minimize();

        /// <summary>
        /// Sets the size and the position of the operating system window
        /// corresponding to the current top-level browsing context.
        /// </summary>
        /// <param name="rect">The RectModel to use when setting the windows size and position.</param>
        void SetWindowRect(RectModel rect);
        #endregion
    }
}
