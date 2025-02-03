using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the command could not be completed because the element is in an invalid state,
    /// e.g. attempting to clear an element that isn't both editable and resettable.
    /// </summary>
    [WebDriverException(jsonErrorCode: "invalid element state", HttpStatusCode = HttpStatusCode.BadRequest)]
    public class InvalidElementStateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidElementStateException class.
        /// </summary>
        public InvalidElementStateException()
        { }

        /// <inheritdoc />
        public InvalidElementStateException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public InvalidElementStateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
