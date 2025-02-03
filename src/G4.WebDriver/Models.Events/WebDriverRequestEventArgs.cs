using System;
using System.Runtime.Serialization;

namespace G4.WebDriver.Models.Events
{
    /// <summary>
    /// Fires right before the WebDriver command is sent.
    /// </summary>
    /// <param name="serverAddress">The remote end's URL address.</param>
    /// <param name="webDriverCommand">The WebDriverCommandModel to send.</param>
    [DataContract]
    public class WebDriverRequestEventArgs(string serverAddress, WebDriverCommandModel webDriverCommand) : EventArgs
    {
        /// <summary>
        /// Gets or sets the remote end's URL address.
        /// </summary>
        [DataMember]
        public string ServerAddress { get; set; } = serverAddress;

        /// <summary>
        /// Gets or sets the WebDriverCommandModel to send.
        /// </summary>
        [DataMember]
        public WebDriverCommandModel WebDriverCommand { get; set; } = webDriverCommand;
    }
}
