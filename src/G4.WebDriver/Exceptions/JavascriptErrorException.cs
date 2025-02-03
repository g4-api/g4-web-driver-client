using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an error occurred while executing JavaScript supplied by the user.
    /// </summary>
    [WebDriverException(jsonErrorCode: "javascript error", HttpStatusCode = HttpStatusCode.InternalServerError)]
    public class JavascriptErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the JavascriptErrorException class.
        /// </summary>
        public JavascriptErrorException()
        { }

        /// <inheritdoc />
        public JavascriptErrorException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public JavascriptErrorException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
