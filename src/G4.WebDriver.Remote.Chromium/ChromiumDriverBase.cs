using G4.WebDriver.Models;
using G4.WebDriver.Remote.Chromium.DeveloperTools;

using System.Text.Json;

namespace G4.WebDriver.Remote.Chromium
{
    /// <summary>
    /// Represents the base class for Chromium-based drivers, providing access to Chromium Developer
    /// Tools and other browser-specific features.
    /// </summary>
    public abstract class ChromiumDriverBase : RemoteWebDriver
    {
        #region *** Fields       ***
        // JSON serializer options for handling Chromium developer tools commands and responses.
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumDriverBase"/> class with the specified command invoker and session.
        /// </summary>
        /// <param name="invoker">The command invoker for sending WebDriver commands.</param>
        /// <param name="session">The WebDriver session model.</param>
        protected ChromiumDriverBase(IWebDriverCommandInvoker invoker, SessionModel session)
            : base(invoker, session)
        {
            // Initialize Developer Tools domains.
            DeveloperTools = new ChromiumDeveloperToolsDomains(this);
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets the Chromium Developer Tools domains associated with this driver.
        /// </summary>
        public ChromiumDeveloperToolsDomains DeveloperTools { get; }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Invokes a Chromium Developer Tools Protocol (CDP) command and returns the response.
        /// </summary>
        /// <param name="cdpRequest">The CDP request model containing the command and parameters.</param>
        /// <returns>A <see cref="CdpResponseModel"/> containing the response from the CDP command.</returns>
        public CdpResponseModel SendDeveloperToolsCommand(CdpRequestModel cdpRequest)
        {
            // Invoke the "InvokeCdp" command using the WebDriver command invoker.
            var response = Invoker.Invoke("InvokeCdp", cdpRequest);

            // Deserialize the response value into a CdpResponseModel using JSON options.
            var value = (JsonElement)response.Value;

            // Return the deserialized CdpResponseModel.
            return value.Deserialize<CdpResponseModel>(s_jsonOptions);
        }
        #endregion
    }
}
