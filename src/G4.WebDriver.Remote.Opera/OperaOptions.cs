using G4.WebDriver.Attributes;
using G4.WebDriver.Remote.Chromium;

namespace G4.WebDriver.Remote.Opera
{
    /// <summary>
    /// Represents the options specific to Opera for configuring WebDriver sessions.
    /// Inherits from <see cref="ChromiumOptionsBase"/> to provide options specific to Chromium-based browsers.
    /// </summary>
    [WebDriverOptions(prefix: "goog", optionsKey: "chromeOptions")]
    public class OperaOptions : ChromiumOptionsBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="OperaOptions"/> class.
        /// Sets the browser name to "opera" and adds it to the capabilities.
        /// </summary>
        public OperaOptions()
        {
            // Set the browser name to Opera.
            BrowserName = "opera";

            // Add the browser name to the capabilities.
            AddCapabilities("browserName", BrowserName);
        }
        #endregion
    }
}
