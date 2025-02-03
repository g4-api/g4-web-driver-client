using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a command to switch to a window could not be satisfied because the window could not be found.
    /// </summary>
    [WebDriverException(jsonErrorCode: "no such window", HttpStatusCode = HttpStatusCode.NotFound)]
    public class NoSuchWindowException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoSuchWindowException class.
        /// </summary>
        public NoSuchWindowException()
        { }

        /// <inheritdoc />
        public NoSuchWindowException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public NoSuchWindowException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
