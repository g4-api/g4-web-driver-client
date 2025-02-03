/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Edge/EdgeOptions.cs
 * https://learn.microsoft.com/en-us/microsoft-edge/webdriver-chromium/capabilities-edge-options
 */
using G4.WebDriver.Attributes;
using G4.WebDriver.Remote.Chromium;
using G4.WebDriver.Remote.Chromium.Models;

namespace G4.WebDriver.Remote.Edge
{
    /// <summary>
    /// Represents the options specific to Microsoft Edge for configuring WebDriver sessions.
    /// Inherits from <see cref="ChromiumOptionsBase"/> to provide options specific to Chromium-based browsers.
    /// </summary>
    [WebDriverOptions(prefix: "ms", optionsKey: "edgeOptions")]
    public class EdgeOptions : ChromiumOptionsBase
    {
        #region *** Constants    ***
        // Constant for Microsoft Edge browser name.
        private const string MicrosoftEdge = "MicrosoftEdge";

        // Constant for WebView2 component.
        private const string WebView2 = "webview2";
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeOptions"/> class.
        /// Sets the browser name to "MicrosoftEdge" and adds it to capabilities.
        /// </summary>
        public EdgeOptions()
        {
            // Set the browser name to Microsoft Edge.
            BrowserName = MicrosoftEdge;

            // Add browser name to capabilities.
            AddCapabilities("browserName", MicrosoftEdge);
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets whether to create a WebView session used for launching an
        /// Edge (Chromium) WebView-based application on desktop.
        /// </summary>
        public bool UseWebView
        {
            get => BrowserName == WebView2;
            set => BrowserName = value ? WebView2 : MicrosoftEdge;
        }

        /// <summary>
        /// Gets or sets an address of a Windows Device Portal server to connect to, in the form of hostname/ip:port, for example 127.0.0.1:50080.
        /// For more information, see Remote Debugging - Windows 10 devices.
        /// </summary>
        [VendorCapability(capabilityName: "wdpAddress")]
        public string WdpAddress { get; set; }

        /// <summary>
        /// Gets or sets an optional password to use when connecting to a Windows Device Portal server.
        /// Required if the server has authentication enabled.
        /// </summary>
        [VendorCapability(capabilityName: "wdpPassword")]
        public string WdpPassword { get; set; }

        /// <summary>
        /// Gets or sets an optional user name to use when connecting to a Windows Device Portal server.
        /// Required if the server has authentication enabled.
        /// </summary>
        [VendorCapability(capabilityName: "wdpUsername")]
        public string WdpUsername { get; set; }

        /// <summary>
        /// Gets or sets the required process ID to use if attaching to a running WebView2 UWP app, for example 36590.
        /// This information can be found in browserProcessId on 'http://[Device Portal URL]/msedge'.
        /// </summary>
        [VendorCapability(capabilityName: "wdpProcessId")]
        public int WdpProcessId { get; set; }

        /// <summary>
        /// Gets or sets an optional dictionary that can be used to configure the WebView2 environment when launching a WebView2 app.
        /// For more information, <see cref="ChromiumWebViewOptionsModel"/> object.
        /// </summary>
        [VendorCapability(capabilityName: "webviewOptions")]
        public ChromiumWebViewOptionsModel WebViewOptions { get; set; }

        /// <summary>
        /// Gets or sets an application user model ID of a Microsoft Edge app package to launch, for example Microsoft.MicrosoftEdge.Stable_8wekyb3d8bbwe!MSEDGE.
        /// Use windowsApp instead of binary when connecting to a Windows 10X device or emulator using Windows Device Portal.
        /// </summary>
        [VendorCapability(capabilityName: "windowsApp")]
        public string WindowsApp { get; set; }
        #endregion
    }
}
