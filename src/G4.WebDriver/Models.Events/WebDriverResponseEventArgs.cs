using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace G4.WebDriver.Models.Events
{
    /// <summary>
    /// Fires right after the WebDriver command response was received.
    /// </summary>
    /// <param name="serverAddress">The remote end's URL address.</param>
    /// <param name="response">The HttpResponseMessage received from the WebDriver server.</param>
    [DataContract]
    public class WebDriverResponseEventArgs(string serverAddress, HttpResponseMessage response) : EventArgs
    {
        /// <summary>
        /// Gets or sets the HttpResponseMessage received from the WebDriver server.
        /// </summary>
        [DataMember]
        public HttpResponseMessage Response { get; set; } = response;

        /// <summary>
        /// Gets or sets the remote end's URL address.
        /// </summary>
        [DataMember]
        public string ServerAddress { get; set; } = serverAddress;
    }
}
