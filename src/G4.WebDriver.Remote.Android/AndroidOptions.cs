using G4.WebDriver.Attributes;
using G4.WebDriver.Remote.Appium;

namespace G4.WebDriver.Remote.Android
{
    /// <summary>
    /// Represents the options specific to Android for Appium.
    /// </summary>
    [WebDriverOptions(prefix: "appium", optionsKey: "options")]
    public class AndroidOptions : AppiumOptionsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidOptions"/> class.
        /// </summary>
        public AndroidOptions()
        {
            // Add the 'platformName' capability with the value 'android'
            AddCapabilities("platformName", "android");
        }
    }
}
