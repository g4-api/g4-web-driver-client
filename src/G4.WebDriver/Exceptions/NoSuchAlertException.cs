using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an attempt was made to operate on a modal dialog when one was not open.
    /// </summary>
    [WebDriverException(jsonErrorCode: "no such alert", HttpStatusCode = HttpStatusCode.InternalServerError)]
    public class NoSuchAlertException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoSuchAlertException class.
        /// </summary>
        public NoSuchAlertException()
        { }

        /// <inheritdoc />
        public NoSuchAlertException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public NoSuchAlertException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
