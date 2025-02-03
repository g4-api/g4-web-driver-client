namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Specifies the behavior of waiting for page loads in the driver.
    /// </summary>
    public static class PageLoadStrategies
    {
        /// <summary>
        /// Indicates the behavior is not set.
        /// </summary>
        public const string Default = "";

        /// <summary>
        /// Waits for pages to load and ready state to be 'complete'.
        /// </summary>
        public const string Normal = "normal";

        /// <summary>
        /// Waits for pages to load and for ready state to be 'interactive' or 'complete'.
        /// </summary>
        public const string Eager = "eager";

        /// <summary>
        /// Does not wait for pages to load, returning immediately.
        /// </summary>
        public const string None = "none";
    }
}
