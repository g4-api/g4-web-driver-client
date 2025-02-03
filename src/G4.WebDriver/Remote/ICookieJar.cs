/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/ICookieJar.cs
 */
using G4.WebDriver.Models;

using System.Collections.ObjectModel;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides an interface for managing browser cookies within a WebDriver session.
    /// </summary>
    public interface ICookieJar
    {
        #region *** Properties ***
        /// <summary>
        /// Gets a collection of all cookies currently stored in the browser.
        /// </summary>
        ReadOnlyCollection<CookieModel> Cookies { get; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Adds a cookie to the browser's cookie store.
        /// </summary>
        /// <param name="cookie">The cookie to add.</param>
        void AddCookie(CookieModel cookie);

        /// <summary>
        /// Deletes a specific cookie from the browser's cookie store.
        /// </summary>
        /// <param name="cookie">The cookie to delete.</param>
        void DeleteCookie(CookieModel cookie);

        /// <summary>
        /// Deletes a cookie by its name from the browser's cookie store.
        /// </summary>
        /// <param name="name">The name of the cookie to delete.</param>
        void DeleteCookie(string name);

        /// <summary>
        /// Deletes all cookies from the browser's cookie store.
        /// </summary>
        void DeleteCookies();

        /// <summary>
        /// Retrieves a cookie by its name from the browser's cookie store.
        /// </summary>
        /// <param name="name">The name of the cookie to retrieve.</param>
        /// <returns>The <see cref="CookieModel"/> with the specified name, or <c>null</c> if no such cookie exists.</returns>
        CookieModel GetCookie(string name);
        #endregion
    }
}
