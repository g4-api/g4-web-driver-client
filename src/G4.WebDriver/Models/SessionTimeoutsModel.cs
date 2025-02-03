namespace G4.WebDriver.Models
{
    /// <summary>
    /// Timeout types that may be set using the Set Timeouts command.
    /// </summary>
    public class SessionTimeoutsModel
    {
        /// <summary>
        /// Specifies a time to wait for the element location strategy to complete when locating an element.
        /// </summary>
        public int Implicit { get; set; }

        /// <summary>
        /// Provides the timeout limit used to interrupt an explicit navigation attempt.
        /// </summary>
        public int PageLoad { get; set; } = 300000;

        /// <summary>
        /// Specifies when to interrupt a script that is being evaluated.
        /// A null value implies that scripts should never be interrupted, but instead run indefinitely.
        /// </summary>
        public int Script { get; set; } = 30000;
    }
}
