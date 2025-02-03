namespace G4.WebDriver.Remote.Chromium.DeveloperTools
{
    /// <summary>
    /// Provides access to various developer tools functionalities for a ChromiumDriver instance.
    /// </summary>
    /// <param name="driver">The ChromiumDriver instance to interact with.</param>
    public class ChromiumDeveloperToolsDomains(ChromiumDriverBase driver)
    {
        #region *** Properties ***
        /// <summary>
        /// Gets the Cast object, which provides access to Cast, Presentation API,
        /// and Remote Playback API functionalities through Chrome DevTools Protocol (CDP).
        /// </summary>
        public CastDomain Cast { get; } = new CastDomain(driver);
        #endregion
    }
}
