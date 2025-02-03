using G4.WebDriver.Remote;

using System.Collections.Generic;

namespace G4.WebDriver.Simulator
{
    public class SimulatorOptions : WebDriverOptionsBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Create a new instance of SimulatorOptions.
        /// </summary>
        public SimulatorOptions()
            : this(new Dictionary<string, object>())
        { }

        /// <summary>
        /// Create a new instance of SimulatorOptions
        /// </summary>
        /// <param name="capabilities">A collection of capabilities to add to the driver capabilities collection.</param>
        public SimulatorOptions(IDictionary<string, object> capabilities)
        {
            BrowserName = "simulator";
            AddCapabilities(capabilities);
        }
        #endregion
    }
}
