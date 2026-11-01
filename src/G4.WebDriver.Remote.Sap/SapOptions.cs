using G4.WebDriver.Attributes;

using System;
using System.Collections.Generic;

namespace G4.WebDriver.Remote.Sap
{
    /// <summary>
    /// Provides the options for a SAP GUI WebDriver session, sent as the <c>sap:options</c> capability.
    /// </summary>
    /// <remarks>
    /// The server attaches the new WebDriver session to exactly one running SAP GUI session. Set
    /// <see cref="SessionPath"/>, or <see cref="ConnectionIndex"/> and <see cref="SessionIndex"/>, to choose it
    /// directly; <see cref="SystemName"/>, <see cref="Client"/>, and <see cref="ConnectionDescription"/> narrow the
    /// candidates instead. When nothing is set and exactly one session is open, the server attaches to it; when the
    /// choice is ambiguous, session creation fails. Every value is written straight to the vendor options, so an
    /// index of zero is sent while an unset (null) value is omitted.
    /// </remarks>
    [WebDriverOptions(prefix: "sap", optionsKey: "options")]
    public class SapOptions : WebDriverOptionsBase
    {
        #region *** Constants    ***
        /// <summary>
        /// The browser name sent with SAP sessions, which grids and hubs can route on.
        /// </summary>
        public const string SapBrowserName = "Sap";
        #endregion

        #region *** Fields       ***
        // Holds the SAP option values read back by the property getters; the base options mirror every change.
        private readonly Dictionary<string, object> _sapOptions = new(StringComparer.OrdinalIgnoreCase);
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="SapOptions"/> class with the SAP browser name and no session
        /// selection, so the server attaches to the only open SAP GUI session.
        /// </summary>
        public SapOptions()
        {
            // Name the browser up front so every session request carries a routable SAP browser name.
            BrowserName = SapBrowserName;
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the SAP Logon client, such as <c>100</c>, used to narrow the candidate sessions.
        /// </summary>
        /// <remarks>
        /// Null leaves the client out of the selection.
        /// </remarks>
        public string Client
        {
            get => GetOption<string>(instance: this, key: "client");
            set => SetOption(instance: this, key: "client", value);
        }

        /// <summary>
        /// Gets or sets the SAP Logon connection description used to narrow the candidate sessions.
        /// </summary>
        /// <remarks>
        /// Null leaves the description out of the selection.
        /// </remarks>
        public string ConnectionDescription
        {
            get => GetOption<string>(instance: this, key: "connectionDescription");
            set => SetOption(instance: this, key: "connectionDescription", value);
        }

        /// <summary>
        /// Gets or sets the zero-based index of the SAP GUI connection, used together with
        /// <see cref="SessionIndex"/>.
        /// </summary>
        /// <remarks>
        /// Null leaves the index out of the request; zero is a valid index and is always sent.
        /// </remarks>
        public int? ConnectionIndex
        {
            get => GetOption<int?>(instance: this, key: "connectionIndex");
            set => SetOption(instance: this, key: "connectionIndex", value);
        }

        /// <summary>
        /// Gets or sets the label that targets a SAP driver on a specific machine when the session goes through a grid.
        /// </summary>
        /// <remarks>
        /// The SAP driver server ignores this value; only grids and hubs read it. Null sends no label.
        /// </remarks>
        public string Label
        {
            get => GetOption<string>(instance: this, key: "label");
            set => SetOption(instance: this, key: "label", value);
        }

        /// <summary>
        /// Gets or sets the zero-based index of the session within the connection chosen by
        /// <see cref="ConnectionIndex"/>.
        /// </summary>
        /// <remarks>
        /// Null leaves the index out of the request; zero is a valid index and is always sent.
        /// </remarks>
        public int? SessionIndex
        {
            get => GetOption<int?>(instance: this, key: "sessionIndex");
            set => SetOption(instance: this, key: "sessionIndex", value);
        }

        /// <summary>
        /// Gets or sets the SAP GUI Scripting path of the session, in the form <c>/app/con[n]/ses[n]</c>.
        /// </summary>
        /// <remarks>
        /// When set, it selects the session directly and takes the place of the indexes. Null leaves it out.
        /// </remarks>
        public string SessionPath
        {
            get => GetOption<string>(instance: this, key: "sessionPath");
            set => SetOption(instance: this, key: "sessionPath", value);
        }

        /// <summary>
        /// Gets or sets the SAP system ID, such as <c>DEV</c>, used to narrow the candidate sessions.
        /// </summary>
        /// <remarks>
        /// Null leaves the system out of the selection.
        /// </remarks>
        public string SystemName
        {
            get => GetOption<string>(instance: this, key: "systemName");
            set => SetOption(instance: this, key: "systemName", value);
        }
        #endregion

        #region *** Methods      ***
        // Reads one SAP option for a property getter; an option that was never set, or was cleared, returns the
        // type's default (null for strings and nullable indexes). The helper only reads the instance's option state.
        private static T GetOption<T>(SapOptions instance, string key)
        {
            // Return the stored value when present so the getter reflects the latest assignment.
            return instance._sapOptions.TryGetValue(key, out var value)
                ? (T)value
                : default;
        }

        // Stores one SAP option and mirrors it into the base vendor options, which the base class always serializes
        // under sap:options regardless of default values. A null value removes the option from both stores so the
        // server falls back to its own selection rules.
        private static void SetOption(SapOptions instance, string key, object value)
        {
            // Remove a cleared option from both stores so no stale value reaches the session request.
            if (value is null)
            {
                instance._sapOptions.Remove(key);
                instance.DeleteOption(key);

                return;
            }

            // Record the value for the getter and the capability conversion together so both views stay in sync.
            instance._sapOptions[key] = value;
            instance.AddOptions(key, value);
        }
        #endregion
    }
}
