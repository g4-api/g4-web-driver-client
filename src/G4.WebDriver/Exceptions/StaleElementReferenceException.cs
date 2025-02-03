using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the referenced element is no longer attached to the DOM.
    /// </summary>
    [WebDriverException(jsonErrorCode: "stale element reference", HttpStatusCode = HttpStatusCode.NotFound)]
    public class StaleElementReferenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the StaleElementReferenceException class.
        /// </summary>
        public StaleElementReferenceException()
        { }

        /// <inheritdoc />
        public StaleElementReferenceException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public StaleElementReferenceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
