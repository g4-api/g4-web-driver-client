using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the Element Click command could not be
    /// completed because the element receiving the events is obscuring the
    /// element that was requested clicked.
    /// </summary>
    [WebDriverException(jsonErrorCode: "element click intercepted", HttpStatusCode = HttpStatusCode.BadRequest)]
    public class ElementClickInterceptedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ElementClickInterceptedException class.
        /// </summary>
        public ElementClickInterceptedException()
        { }

        /// <inheritdoc />
        public ElementClickInterceptedException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public ElementClickInterceptedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
