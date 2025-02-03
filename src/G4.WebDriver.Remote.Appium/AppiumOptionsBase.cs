namespace G4.WebDriver.Remote.Appium
{
    /// <summary>
    /// Provides base configuration options for Appium-based WebDriver instances.
    /// </summary>
    public abstract class AppiumOptionsBase : WebDriverOptionsBase
    {
        /// <summary>
        /// Gets the vendor prefix used for vendor-specific capability names in Appium.
        /// </summary>
        public override string VendorOptionsKey => "appium";
    }
}
