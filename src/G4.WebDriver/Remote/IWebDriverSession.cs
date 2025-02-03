using G4.WebDriver.Models;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides a mechanism for maintaining a session of a WebDriverServer.
    /// </summary>
    public interface IWebDriverSession
    {
        /// <summary>
        /// Gets the session ID of the WebDriverServer session.
        /// </summary>
        SessionIdModel Session { get; set; }
    }
}
