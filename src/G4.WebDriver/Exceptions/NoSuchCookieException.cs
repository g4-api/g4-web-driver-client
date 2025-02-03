using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when no cookie matching the given path name was
    /// found amongst the associated cookies of the current browsing context's active document.
    /// </summary>
    [WebDriverException(jsonErrorCode: "no such cookie", HttpStatusCode = HttpStatusCode.NotFound)]
    public class NoSuchCookieException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoSuchCookieException class.
        /// </summary>
        public NoSuchCookieException()
        { }

        /// <inheritdoc />
        public NoSuchCookieException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public NoSuchCookieException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
