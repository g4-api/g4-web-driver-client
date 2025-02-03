/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/CookieJar.cs
 */
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents a collection of cookies in the WebDriver session, allowing for managing browser cookies.
    /// </summary>
    /// <param name="driver">The WebDriver instance used to interact with the browser.</param>
    public class CookieJar(IWebDriver driver) : ICookieJar, IDriverReference
    {
        #region *** Fields     ***
        // Command invoker to send WebDriver commands to the browser.
        private readonly IWebDriverCommandInvoker _invoker = driver.Invoker;

        // JSON serializer options for deserializing cookie data with case-insensitive
        // property names and camelCase naming policy.
        private static readonly JsonSerializerOptions s_options = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        #endregion

        #region *** Properties ***
        /// <inheritdoc />
        public ReadOnlyCollection<CookieModel> Cookies => GetCookies(_invoker);

        /// <inheritdoc />
        public IWebDriver Driver { get; } = driver;
        #endregion

        #region *** Methods    ***
        /// <inheritdoc />
        public void AddCookie(CookieModel cookie)
        {
            // Create an anonymous object with the cookie data.
            var data = new
            {
                Cookie = cookie
            };

            // Invoke the command to add the cookie to the browser's cookie store.
            _invoker.Invoke(nameof(WebDriverCommands.AddCookie), data);
        }

        /// <inheritdoc />
        public void DeleteCookies()
        {
            // Invoke the command to delete all cookies.
            _invoker.Invoke(nameof(WebDriverCommands.DeleteCookies));
        }

        /// <inheritdoc />
        public void DeleteCookie(CookieModel cookie)
        {
            // Delete the cookie by its name.
            DeleteCookie(cookie?.Name);
        }

        /// <inheritdoc />
        public void DeleteCookie(string name)
        {
            // Create a new command to delete the named cookie.
            var command = _invoker.NewCommand(nameof(WebDriverCommands.DeleteNamedCookie));

            // Replace the placeholder with the actual cookie name in the command route.
            command.Route = command.Route.Replace($"$[{nameof(name)}]", name);

            // Invoke the command to delete the named cookie.
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public CookieModel GetCookie(string name)
        {
            // Create a new command to retrieve the named cookie.
            var command = _invoker.NewCommand(nameof(WebDriverCommands.GetNamedCookie));

            // Replace the placeholder with the actual cookie name in the command route.
            command.Route = command.Route.Replace($"$[{nameof(name)}]", name);

            // Invoke the command and return the retrieved cookie.
            return _invoker.Invoke<CookieModel>(command);
        }

        // Retrieves all cookies from the browser's cookie store.
        private static ReadOnlyCollection<CookieModel> GetCookies(IWebDriverCommandInvoker invoker)
        {
            // Invoke the command to retrieve all cookies.
            var cookies = invoker
                .Invoke(nameof(WebDriverCommands.GetCookies))
                .Value;

            // Serialize the cookies to JSON and then deserialize them into a collection of CookieModel objects.
            var jsonData = JsonSerializer.Serialize(cookies);
            var cookiesCollection = JsonSerializer.Deserialize<IEnumerable<CookieModel>>(jsonData, s_options);

            // Return the collection of cookies as a read-only collection.
            return new ReadOnlyCollection<CookieModel>(new List<CookieModel>(cookiesCollection));
        }
        #endregion
    }
}
