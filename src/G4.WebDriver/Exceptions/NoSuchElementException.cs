using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an element could not be located on the page using the given search parameters.
    /// </summary>
    [WebDriverException(jsonErrorCode: "no such element", HttpStatusCode = HttpStatusCode.NotFound)]
    public class NoSuchElementException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoSuchElementException class.
        /// </summary>
        public NoSuchElementException()
        { }

        /// <inheritdoc />
        public NoSuchElementException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public NoSuchElementException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
