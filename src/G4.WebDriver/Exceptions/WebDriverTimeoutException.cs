using G4.WebDriver.Attributes;
using G4.WebDriver.Remote;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the an operation did not complete before its timeout expired.
    /// </summary>
    [WebDriverException(jsonErrorCode: "timeout", HttpStatusCode = HttpStatusCode.NotFound)]
    public class WebDriverTimeoutException : Exception, ITimeoutReference
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverTimeoutException"/> class.
        /// </summary>
        public WebDriverTimeoutException()
        { }

        /// <inheritdoc />
        public WebDriverTimeoutException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverTimeoutException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="timeout">The timeout duration that caused the exception.</param>
        public WebDriverTimeoutException(string message, TimeSpan timeout)
            : base(message)
        {
            Timeout = timeout;
        }

        /// <inheritdoc />
        public WebDriverTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        { }
        #endregion

        #region *** Properties   ***
        /// <inheritdoc />
        public TimeSpan Timeout { get; }
        #endregion
    }
}
