using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the element does not have a shadow root.
    /// </summary>
    /// <remarks>
    /// Mapped to the WebDriver "no such shadow root" error, so a service response with that error is raised as
    /// this exception instead of a generic <see cref="WebDriverException"/>.
    /// </remarks>
    [WebDriverException(jsonErrorCode: "no such shadow root", HttpStatusCode = HttpStatusCode.NotFound)]
    public class NoSuchShadowRootException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NoSuchShadowRootException class.
        /// </summary>
        public NoSuchShadowRootException()
        { }

        /// <inheritdoc />
        public NoSuchShadowRootException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public NoSuchShadowRootException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
