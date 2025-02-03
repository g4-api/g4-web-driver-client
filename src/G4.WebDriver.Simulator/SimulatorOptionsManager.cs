using G4.WebDriver.Models;
using G4.WebDriver.Remote;

using System;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Defines an interface allowing the user to set options on the browser.
    /// </summary>
    public class SimulatorOptionsManager : IOptions
    {
        #region *** Fields       ***
        private IWindow _window;
        #endregion

        #region *** Constructors ***
        public SimulatorOptionsManager()
        { }

        public SimulatorOptionsManager(RectModel rect)
        {
            _window = new SimulatorWindow(rect);
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets an object allowing the user to manipulate cookies on the page.
        /// </summary>
        public ICookieJar Cookies => new SimulatorCookieJar();

        /// <summary>
        /// Gets an object allowing the user to manipulate the currently-focused browser window.
        /// </summary>
        public IWindow Window
        {
            get
            {
                _window ??= new SimulatorWindow();
                return _window;
            }
        }

        /// <summary>
        /// Gets an object allowing the user to examining the logs for this driver instance.
        /// </summary>
        public ILogManager Logs => new SimulatorLogs();

        public INetworkManager Network => throw new NotImplementedException();

        /// <summary>
        /// Provides access to the timeouts defined for this driver.
        /// </summary>
        /// <returns>An object implementing the <see cref="ITimeouts" /> interface.</returns>
        public ITimeouts Timeouts { get; set; } = new SimulatorTimeouts();
        #endregion
    }
}
