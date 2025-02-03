using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the target for mouse interaction
    /// is not in the browser's viewport and cannot be brought into that viewport.
    /// </summary>
    [WebDriverException(jsonErrorCode: "move target out of bounds", HttpStatusCode = HttpStatusCode.InternalServerError)]
    public class MoveTargetOutOfBoundsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MoveTargetOutOfBoundsException class.
        /// </summary>
        public MoveTargetOutOfBoundsException()
        { }

        /// <inheritdoc />
        public MoveTargetOutOfBoundsException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public MoveTargetOutOfBoundsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
