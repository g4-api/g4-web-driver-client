using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the command could not be completed because the element is not pointer- or keyboard interactable.
    /// </summary>
    [WebDriverException(jsonErrorCode: "element not interactable", HttpStatusCode = HttpStatusCode.BadRequest)]
    public class ElementNotInteractableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ElementNotInteractableException class.
        /// </summary>
        public ElementNotInteractableException()
        { }

        /// <inheritdoc />
        public ElementNotInteractableException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public ElementNotInteractableException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
