/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/INavigation.cs
 */
using System;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides methods to navigate through the browser's history and to load URLs.
    /// </summary>
    public interface INavigation
    {
        #region *** Properties ***
        /// <summary>
        /// Gets the title of the current document in the top-level browsing context.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the current URL of the top-level browsing context.
        /// </summary>
        public string Url { get; set; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Navigates the browser to the specified URL.
        /// </summary>
        /// <param name="url">The URL to load. A fully qualified URL is recommended.</param>
        void Open(string url);

        /// <summary>
        /// Navigates the browser to the specified URL.
        /// </summary>
        /// <param name="url">The URL to load, as a Uri object.</param>
        void Open(Uri url);

        /// <summary>
        /// Navigates one step forward in the browser's history.
        /// </summary>
        void Redo();

        /// <summary>
        /// Navigates one step backward in the browser's history.
        /// Equivalent to pressing the browser's back button.
        /// </summary>
        void Undo();

        /// <summary>
        /// Reloads the current page in the browser.
        /// </summary>
        void Update();
        #endregion
    }
}
