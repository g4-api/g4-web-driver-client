using System;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the element does not have a shadow root.
    /// </summary>
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
