using G4.WebDriver.Attributes;
using G4.WebDriver.Remote.Chromium;

namespace G4.WebDriver.Remote.Chrome
{
    /// <summary>
    /// Class to manage options specific to <see cref="ChromeOptions"/>
    /// </summary>
    [WebDriverOptions(prefix: "goog", optionsKey: "chromeOptions")]
    public class ChromeOptions : ChromiumOptionsBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeOptions"/> class.
        /// </summary>
        public ChromeOptions()
        {
            BrowserName = "chrome";
            AddCapabilities("browserName", BrowserName);
        }
        #endregion
    }
}
