using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an illegal attempt was made to set a cookie under a different domain than the current page.
    /// </summary>
    [WebDriverException(jsonErrorCode: "invalid cookie domain", HttpStatusCode = HttpStatusCode.BadRequest)]
    public class InvalidCookieDomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidCookieDomainException class.
        /// </summary>
        public InvalidCookieDomainException()
        { }

        /// <inheritdoc />
        public InvalidCookieDomainException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public InvalidCookieDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
