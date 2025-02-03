using G4.WebDriver.Remote;

using System;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the driver service was not started within the given timeout period.
    /// </summary>
    public class DriverServiceStartTimeoutException : Exception, ITimeoutReference
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverServiceStartTimeoutException"/> class.
        /// </summary>
        public DriverServiceStartTimeoutException()
        { }

        /// <inheritdoc />
        public DriverServiceStartTimeoutException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverServiceStartTimeoutException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="timeout">The timeout duration that caused the exception.</param>
        public DriverServiceStartTimeoutException(string message, TimeSpan timeout)
            : base(message)
        {
            Timeout = timeout;
        }

        /// <inheritdoc />
        public DriverServiceStartTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        { }
        #endregion

        #region *** Properties   ***
        /// <inheritdoc />
        public TimeSpan Timeout { get; }
        #endregion
    }
}
