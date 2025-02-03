namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents an interface for objects that provide information about their display status.
    /// </summary>
    public interface IDisplayable
    {
        /// <summary>
        /// Gets a value indicating whether the WebElement is displayed on the page.
        /// </summary>
        /// <remarks>
        /// This method represents a protocol extension and is not an official command.It is designed for web platforms by default.
        /// To adapt this functionality for other platforms, override this method as needed.
        /// </remarks>
        bool Displayed { get; }
    }
}
