/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Edge/EdgeOptions.cs
 * https://learn.microsoft.com/en-us/microsoft-edge/webdriver-chromium/capabilities-edge-options
 */
using G4.WebDriver.Attributes;

using System.Collections.Generic;

namespace G4.WebDriver.Remote.Chromium.Models
{
    /// <summary>
    /// Used to configure the WebView2 environment when launching a WebView2 app.
    /// </summary>
    public class ChromiumWebViewOptionsModel : CapabilitiesDictionaryBase
    {
        #region *** Properties ***
        /// <summary>
        /// Gets or sets a list of command-line arguments that WebView2 will pass to the browser process on launch.
        /// Arguments with an associated value should be separated by an = sign (for example, ['start-maximized', 'log-level=0']).
        /// </summary>
        [KnownCapability(capabilityName: "additionalBrowserArguments")]
        public IEnumerable<string> AdditionalBrowserArguments { get; set; }

        /// <summary>
        /// Gets or sets the path to a folder containing a fixed version WebView2 runtime to use.
        /// For more information about using a fixed version runtime distribution with WebView2,
        /// see <seealso cref="https://learn.microsoft.com/en-us/microsoft-edge/webview2/concepts/distribution#the-fixed-version-runtime-distribution-mode">Distribute a WebView2 app and the WebView2 Runtime</seealso>.
        /// </summary>
        [KnownCapability(capabilityName: "browserExecutableFolder")]
        public string BrowserExecutableFolder { get; set; }

        /// <summary>
        /// Gets or sets a preferred WebView2 evergreen runtime distribution to use. Can be "stable" or "canary".
        /// </summary>
        [KnownCapability(capabilityName: "releaseChannelPreference")]
        public string ReleaseChannelPreference { get; set; }

        /// <summary>
        /// Gets or sets the path to the user data folder that WebView2 will use.
        /// If userDataFolder isn't specified, Microsoft Edge WebDriver will create a temporary user data folder.
        /// For more information about managing the user data folder with WebView2, see <seealso cref="https://learn.microsoft.com/en-us/microsoft-edge/webview2/concepts/user-data-folder">Manage the user data folder</seealso>.
        /// </summary>
        [KnownCapability(capabilityName: "userDataFolder")]
        public string UserDataFolder { get; set; }
        #endregion
    }
}
