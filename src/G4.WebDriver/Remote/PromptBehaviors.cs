namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Specifies the behavior of handling unexpected alerts.
    /// </summary>
    public static class PromptBehaviors
    {
        /// <summary>
        /// Accept unexpected alerts.
        /// </summary>
        public const string Accept = "accept";

        /// <summary>
        /// Accepts unexpected alerts and notifies the user that the alert has
        /// been accepted by throwing an UnhandledAlertException.
        /// </summary>
        public const string AcceptAndNotify = "AcceptAndNotify";

        /// <summary>
        /// Indicates the behavior is not set.
        /// </summary>
        public const string Default = "";

        /// <summary>
        /// Dismiss unexpected alerts.
        /// </summary>
        public const string Dismiss = "dismiss";

        /// <summary>
        /// Dismisses unexpected alerts and notifies the user that the alert has
        /// been dismissed by throwing an UnhandledAlertException".
        /// </summary>
        public const string DismissAndNotify = "DismissAndNotify";

        /// <summary>
        /// Ignore unexpected alerts, such that the user must handle them.
        /// </summary>
        public const string Ignore = "ignore";
    }
}
