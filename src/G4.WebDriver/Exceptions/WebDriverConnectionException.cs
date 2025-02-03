using System;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the driver service connection is no longer valid or had a problem.
    /// </summary>
    public class WebDriverConnectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DriverServiceNotFoundException class.
        /// </summary>
        public WebDriverConnectionException()
        { }

        /// <inheritdoc />
        public WebDriverConnectionException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public WebDriverConnectionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
