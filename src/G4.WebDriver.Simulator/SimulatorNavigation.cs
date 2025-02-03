using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Remote;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Creates a new instance of the SimulatorNavigation class.
    /// </summary>
    /// <param name="driver">The parent MockWebDriver.</param>
    public class SimulatorNavigation(SimulatorDriver driver) : INavigation
    {
        #region *** Fields     ***
        private readonly SimulatorDriver _driver = driver;

        [SuppressMessage(
            category: "Minor Code Smell",
            checkId: "S1075:URIs should not be hardcoded",
            Justification = "This is not a real URL, it's placeholder text.")]
        private string _url = "http://positive.io/20";
        #endregion

        #region *** Properties ***
        /// <summary>
        /// Gets the title of the current browser window.
        /// </summary>
        public string Title => GetTitle(_driver);

        /// <summary>
        /// Gets the URL of the current top-level browsing context.
        /// </summary>
        public string Url
        {
            get => GetUrl(_driver, _url);
            set => _url = value;
        }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Move back a single entry in the browser's history.
        /// </summary>
        public void Undo()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Move a single "item" forward in the browser's history.
        /// </summary>
        /// <remarks>
        /// Does nothing if we are on the latest page viewed.
        /// </remarks>
        public void Redo()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load. It is best to use a fully qualified URL.</param>
        public void Open(string url)
        {
            DoNavigateTo(url);
        }

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load.</param>
        public void Open(Uri url)
        {
            DoNavigateTo(url: url.AbsoluteUri);
        }

        private void DoNavigateTo(string url)
        {
            // get method to execute
            var method = GetType().GetMethodByDescription(actual: url);

            // default
            if (method == default)
            {
                UrlPositive(url);
                return;
            }

            // invoke
            try
            {
                var instance = method.IsStatic ? null : this;
                method.Invoke(instance, [url]);
            }
            catch (Exception e)
            {
                throw e.InnerException ?? e;
            }
        }

        /// <summary>
        /// Refreshes the current page.
        /// </summary>
        public void Update()
        {
            // Method intentionally left empty.
        }

        // Gets the title of the page from the SimulatorDriver.
        private static string GetTitle(SimulatorDriver driver)
        {
            // Check if the capabilities contain a key indicating an exception should be thrown
            return driver.Capabilities?.AlwaysMatch?.ContainsKey("ExceptionOnTitle") == true
                ? throw new WebDriverException()    // If the key is found, throw a WebDriverException
                : "Mock G4™ API Page Title 20"; // Otherwise, return a mock page title
        }

        // Gets the URL from the SimulatorDriver or returns the provided URL.
        private static string GetUrl(SimulatorDriver driver, string url)
        {
            // Check if the capabilities contain a key indicating an exception should be thrown
            return driver.Capabilities?.AlwaysMatch?.ContainsKey("ExceptionOnUrl") == true
                ? throw new WebDriverException() // If the key is found, throw a WebDriverException
                : url;                           // Otherwise, return the provided URL
        }

        // Throws a WebDriverException with the provided URL as the message if the URL is not empty.
        [Description("exception")]
        private static void UrlException(string url)
        {
            throw new WebDriverException(string.IsNullOrEmpty(url) ? string.Empty : url);
        }

        // Throws an ArgumentNullException if the provided URL is null or empty.
        [Description("none|null")]
        private static void UrlNoneOrNull(string url)
        {
            throw new ArgumentNullException(nameof(url), $"Argument [{nameof(url)}] cannot be null.");
        }

        // Sets the URL value to the provided value if it is positive.
        [Description("positive")]
        private void UrlPositive(string url) => _url = url;
        #endregion
    }
}
