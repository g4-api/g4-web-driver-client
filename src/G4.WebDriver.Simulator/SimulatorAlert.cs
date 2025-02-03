using G4.WebDriver.Models;
using G4.WebDriver.Remote;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Creates new instance of this MockAlert object.
    /// </summary>
    /// <param name="driver">Parent driver under which this alert exists.</param>
    public class SimulatorAlert(SimulatorDriver driver) : IUserPrompt
    {
        #region *** Fields     ***
        private readonly SimulatorDriver driver = driver;
        #endregion

        #region *** Properties ***
        /// <summary>
        /// Gets the text of the alert.
        /// </summary>
        public string Text => "mock alert text.";
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Accepts the alert.
        /// </summary>
        public void Approve()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Dismisses the alert.
        /// </summary>
        public void Close()
        {
            // setup conditions
            var hasKey = driver.Capabilities.AlwaysMatch.ContainsKey(SimulatorCapabilities.HasAlert);
            var hasAlert = hasKey && (bool)driver.Capabilities.AlwaysMatch[SimulatorCapabilities.HasAlert];

            // dismiss
            if (hasAlert)
            {
                driver.Capabilities.AlwaysMatch[SimulatorCapabilities.HasAlert] = false;
            }
        }

        /// <summary>
        /// Sends keys to the alert.
        /// </summary>
        /// <param name="text">The keystrokes to send.</param>
        public void SendKeys(string text)
        {
            // Method intentionally left empty.
        }
        #endregion
    }
}
