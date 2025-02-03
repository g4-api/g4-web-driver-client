namespace G4.WebDriver.Remote
{
    public static class KnownCapabilities
    {
        /// <summary>
        /// Indicates whether untrusted and self-signed TLS certificates are
        /// implicitly trusted on navigation for the duration of the session.
        /// </summary>
        public const string AcceptInsecureCerts = "acceptInsecureCerts";

        /// <summary>
        /// Identifies the user agent.
        /// </summary>
        public const string BrowserName = "browserName";

        /// <summary>
        /// Identifies the version of the user agent.
        /// </summary>
        public const string BrowserVersion = "browserVersion";

        /// <summary>
        /// Defines the current session's page load strategy.
        /// </summary>
        public const string PageLoadStrategy = "pageLoadStrategy";

        /// <summary>
        /// Identifies the operating system of the endpoint node.
        /// </summary>
        public const string PlatformName = "platformName";

        /// <summary>
        /// Defines the current session's proxy configuration.
        /// </summary>
        public const string Proxy = "proxy";

        /// <summary>
        /// Describes the current session's strict file interactability.
        /// </summary>
        public const string StrictFileInteractability = "strictFileInteractability";

        /// <summary>
        /// Describes the timeouts imposed on certain session operations.
        /// </summary>
        public const string SessionTimeouts = "timeouts";

        /// <summary>
        /// Indicates whether the remote end supports all of the resizing and repositioning commands.
        /// </summary>
        public const string WindowRect = "setWindowRect";

        /// <summary>
        /// Describes the current session's user prompt handler. Defaults to the dismiss and notify state.
        /// </summary>
        public const string UnhandledPromptBehavior = "unhandledPromptBehavior";

        /// <summary>
        /// Capability name used to get a value indicating whether to request URL of a WebSocket
        /// connection for bidirectional communication with a driver.
        /// </summary>
        public static readonly string WebSocketUrl = "webSocketUrl";
    }
}
