using System;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a command to create or find a session could not be satisfied because the session could not be found.
    /// </summary>
    public class NoSuchSessionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoSuchSessionException class.
        /// </summary>
        public NoSuchSessionException()
        { }

        /// <inheritdoc />
        public NoSuchSessionException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public NoSuchSessionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
