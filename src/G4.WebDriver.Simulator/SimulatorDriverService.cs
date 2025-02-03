using G4.WebDriver.Models;
using G4.WebDriver.Remote;

using System;
using System.Diagnostics;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Represents a simulated implementation of a WebDriver service for the G4 WebDriver.
    /// </summary>
    public class SimulatorDriverService : IWebDriverService
    {
        #region *** Properties ***
        /// <inheritdoc />
        public bool HideCommandPromptWindow { get; set; }

        /// <inheritdoc />
        public bool Ready { get; set; }

        /// <inheritdoc />
        public Process Process { get; }

        /// <inheritdoc />
        public Uri ServerAddress { get; set; }

        /// <inheritdoc />
        public bool SuppressDiagnostic { get; set; }

        /// <inheritdoc />
        public TimeSpan Timeout { get; set; }

        /// <inheritdoc />
        public WebDriverStatusModel Status { get; set; } = new();
        #endregion

        #region *** Methods    ***
        /// <inheritdoc />
        public void Dispose()
        {
            // Call the protected Dispose method with disposing set to true.
            Dispose(disposing: true);

            // Suppress finalization to prevent the garbage collector from calling the finalizer.
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            // Since this is a simulator, there's no actual resource to dispose.
            // If there were managed resources, they would be disposed here when disposing is true.
        }

        /// <inheritdoc />
        public WebDriverStatusModel GetStatus()
        {
            // Return the current status of the simulator service.
            return Status;
        }

        /// <inheritdoc />
        public void DeployWebDriver()
        {
            // In the simulator, this method is left empty as there's no actual deployment.
        }

        /// <inheritdoc />
        public void StartService()
        {
            // Since this is a simulator, there's no actual service to start.
            // In a real implementation, code to start the WebDriver process would be here.
        }

        /// <inheritdoc />
        public void StopService()
        {
            // Since this is a simulator, there's no actual service to stop.
            // In a real implementation, code to stop the WebDriver process would be here.
        }
        #endregion
    }
}
