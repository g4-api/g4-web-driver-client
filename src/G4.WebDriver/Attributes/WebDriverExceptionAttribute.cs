using System;
using System.Net;

namespace G4.WebDriver.Attributes
{
    /// <summary>
    /// Specifies an <see cref="Exception"/> call as a WebDriver exception.
    /// </summary>
    /// <param name="jsonErrorCode">The JSON Error Code returned by the WebDriver.</param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class WebDriverExceptionAttribute(string jsonErrorCode) : Attribute
    {
        /// <summary>
        /// Gets or sets the JSON Error Code.
        /// </summary>
        public string JsonErrorCode { get; set; } = jsonErrorCode;

        /// <summary>
        /// The <see cref="System.Net.HttpStatusCode"/> the WebDriverException is associated with.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
