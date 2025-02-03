using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a modal dialog was open, blocking this operation.
    /// </summary>
    [WebDriverException(jsonErrorCode: "unexpected alert open", HttpStatusCode = HttpStatusCode.InternalServerError)]
    public class UnexpectedAlertOpenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the UnexpectedAlertOpenException class.
        /// </summary>
        public UnexpectedAlertOpenException()
        { }

        /// <inheritdoc />
        public UnexpectedAlertOpenException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public UnexpectedAlertOpenException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
