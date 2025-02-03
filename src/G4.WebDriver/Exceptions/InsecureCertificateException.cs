using G4.WebDriver.Attributes;

using System;
using System.Net;

namespace G4.WebDriver.Exceptions
{
    /// <summary>
    /// The exception that is thrown when navigation caused the user agent to hit a certificate warning,
    /// which is usually the result of an expired or invalid TLS certificate.
    /// </summary>
    [WebDriverException(jsonErrorCode: "insecure certificate", HttpStatusCode = HttpStatusCode.BadRequest)]
    public class InsecureCertificateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InsecureCertificateException class.
        /// </summary>
        public InsecureCertificateException()
        { }

        /// <inheritdoc />
        public InsecureCertificateException(string message)
            : base(message)
        { }

        /// <inheritdoc />
        public InsecureCertificateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
