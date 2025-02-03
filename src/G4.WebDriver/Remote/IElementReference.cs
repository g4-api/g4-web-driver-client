namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides an interface for wrapping a WebDriver element.
    /// This interface exposes a property that returns the underlying <see cref="IWebElement"/>.
    /// </summary>
    public interface IElementReference
    {
        /// <summary>
        /// Gets the wrapped <see cref="IWebElement"/> instance.
        /// </summary>
        IWebElement Element { get; }
    }
}
