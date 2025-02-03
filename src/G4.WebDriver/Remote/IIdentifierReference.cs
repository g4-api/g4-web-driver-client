namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents an interface for objects that have an identifier.
    /// </summary>
    public interface IIdentifierReference
    {
        /// <summary>
        /// Gets the unique identifier of the WebElement.
        /// </summary>
        string Id { get; }
    }
}
