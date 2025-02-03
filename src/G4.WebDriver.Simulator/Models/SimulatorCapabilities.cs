namespace G4.WebDriver.Models
{
    /// <summary>
    /// Provides constants for simulator-specific capabilities used in the G4 WebDriver.
    /// </summary>
    public static class SimulatorCapabilities
    {
        /// <summary>
        /// Represents the capability to handle multiple child windows.
        /// </summary>
        public const string ChildWindows = "ChildWindows";

        /// <summary>
        /// Indicates whether the simulator should simulate an alert dialog.
        /// </summary>
        public const string HasAlert = "HasAlert";

        /// <summary>
        /// Determines if an exception should be thrown when the simulator is closed.
        /// </summary>
        public const string ThrowOnClose = "ThrowOnClose";
    }
}
