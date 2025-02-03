using G4.WebDriver.Models;
using G4.WebDriver.Remote;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Represents a simulated implementation of a cookie jar for the G4 WebDriver.
    /// </summary>
    public class SimulatorCookieJar : ICookieJar
    {
        #region *** Fields       ***
        // The internal list of cookies managed by the simulator.
        private readonly IList<CookieModel> _cookieJar;
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorCookieJar"/> class.
        /// </summary>
        public SimulatorCookieJar()
        {
            // Initialize the cookie jar with some mock cookies.
            _cookieJar =
            [
                new CookieModel { Name = "simulator-cookie-01", Value = "mock cookie 01 value" },
                new CookieModel { Name = "simulator-cookie-02", Value = "mock cookie 02 value" }
            ];
        }
        #endregion

        #region *** Properties   ***
        /// <inheritdoc />
        /// <summary>
        /// Gets a read-only collection of all cookies in the cookie jar.
        /// </summary>
        public ReadOnlyCollection<CookieModel> Cookies => new(_cookieJar);
        #endregion

        #region *** Methods      ***
        /// <inheritdoc />
        /// <summary>
        /// Adds a cookie to the cookie jar.
        /// </summary>
        /// <param name="cookie">The <see cref="CookieModel"/> to add.</param>
        public void AddCookie(CookieModel cookie)
        {
            // Add the provided cookie to the internal cookie jar.
            _cookieJar.Add(cookie);
        }

        /// <inheritdoc />
        /// <summary>
        /// Deletes all cookies from the cookie jar.
        /// </summary>
        public void DeleteCookies()
        {
            // Clear all cookies from the internal cookie jar.
            _cookieJar.Clear();
        }

        /// <inheritdoc />
        /// <summary>
        /// Deletes a specific cookie from the cookie jar.
        /// </summary>
        /// <param name="cookie">The <see cref="CookieModel"/> to delete.</param>
        public void DeleteCookie(CookieModel cookie)
        {
            // Remove the specified cookie from the internal cookie jar.
            _cookieJar.Remove(cookie);
        }

        /// <inheritdoc />
        /// <summary>
        /// Deletes a cookie by its name.
        /// </summary>
        /// <param name="name">The name of the cookie to delete.</param>
        public void DeleteCookie(string name)
        {
            // Find the cookie with the specified name.
            var cookie = _cookieJar.FirstOrDefault(c => c.Name == name);

            // If the cookie exists, remove it from the cookie jar.
            if (cookie != null)
            {
                _cookieJar.Remove(cookie);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Retrieves a cookie by its name.
        /// </summary>
        /// <param name="name">The name of the cookie to retrieve.</param>
        /// <returns>The <see cref="CookieModel"/> with the specified name, or <c>null</c> if not found.</returns>
        public CookieModel GetCookie(string name)
        {
            // Return the first cookie that matches the specified name, or null if not found.
            return _cookieJar.FirstOrDefault(c => c.Name == name);
        }
        #endregion
    }
}
