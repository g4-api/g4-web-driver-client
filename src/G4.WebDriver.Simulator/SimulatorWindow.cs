using G4.WebDriver.Models;
using G4.WebDriver.Remote;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Provides methods for getting and setting the size and position of the browser window.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MockWindow"/> class.
    /// </remarks>
    /// <param name="rect">The RectModel to use when setting the windows size and position.</param>
    public class SimulatorWindow(RectModel rect) : IWindow
    {
        #region *** constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="MockWindow"/> class.
        /// </summary>
        public SimulatorWindow()
            : this(rect: new RectModel { X = 1, Y = 1, Height = 300, Width = 300 })
        { }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets the size and position on the screen of the operating system window
        /// corresponding to the current top-level browsing context.
        /// </summary>
        public RectModel WindowRect { get; private set; } = rect;
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Sets the current window to full screen if it is not already in that state.
        /// </summary>
        public void FullScreen()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// The Minimize Window command invokes the window manager-specific "minimize" operation,
        /// if any, on the window containing the current top-level browsing context.
        /// This typically hides the window in the system tray.
        /// </summary>
        public void Minimize()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// The Maximize Window command invokes the window manager-specific "maximize" operation,
        /// if any, on the window containing the current top-level browsing context.
        /// This typically increases the window to the maximum available size without going full-screen.
        /// </summary>
        public void Maximize()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Sets the size and the position of the operating system window
        /// corresponding to the current top-level browsing context.
        /// </summary>
        /// <param name="rect">The RectModel to use when setting the windows size and position.</param>
        public void SetWindowRect(RectModel rect)
        {
            WindowRect = rect;
        }
        #endregion
    }
}
