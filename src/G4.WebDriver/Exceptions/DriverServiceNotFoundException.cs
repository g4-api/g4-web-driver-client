using System;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the driver service cannot be found (down or inaccessible).
    /// </summary>
    public class DriverServiceNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DriverServiceNotFoundException class.
        /// </summary>
        public DriverServiceNotFoundException()
        { }

        /// <inheritdoc />
        public DriverServiceNotFoundException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public DriverServiceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
