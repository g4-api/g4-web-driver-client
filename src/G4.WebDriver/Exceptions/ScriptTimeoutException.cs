using G4.WebDriver.Attributes;
using G4.WebDriver.Remote;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a script did not complete before its timeout expired.
    /// </summary>
    [WebDriverException(jsonErrorCode: "script timeout", HttpStatusCode = HttpStatusCode.RequestTimeout)]
    public class ScriptTimeoutException : Exception, ITimeoutReference
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the ScriptTimeoutException class.
        /// </summary>
        public ScriptTimeoutException()
        { }

        /// <inheritdoc />
        public ScriptTimeoutException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptTimeoutException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="timeout">The timeout duration that caused the exception.</param>
        public ScriptTimeoutException(string message, TimeSpan timeout)
            : base(message)
        {
            Timeout = timeout;
        }

        /// <inheritdoc />
        public ScriptTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        { }
        #endregion

        #region *** Properties   ***
        /// <inheritdoc />
        public TimeSpan Timeout { get; }
        #endregion
    }
}
