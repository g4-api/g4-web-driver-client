using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Appium.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace G4.WebDriver.Remote.Appium
{
    /// <summary>
    /// Base class for Appium drivers, providing geolocation and keyboard control functionalities for mobile devices.
    /// </summary>
    /// <param name="invoker">The command invoker for executing WebDriver commands.</param>
    /// <param name="session">The session model representing the current WebDriver session.</param>
    public abstract class AppiumDriverBase(IWebDriverCommandInvoker invoker, SessionModel session)
        : RemoteWebDriver(invoker, session), IGeolocation, IMobileDeviceKeyboard
    {
        #region *** Properties ***
        /// <inheritdoc />
        public bool IsOnScreenKeyboardVisible
        {
            get => ((JsonElement)Invoker.Invoke(Invoker.NewCommand("IsKeyboardShown")).Value).GetBoolean();
        }
        #endregion

        #region *** Methods    ***
        /// <inheritdoc />
        public GeolocationModel GetGeolocation()
        {
            // Invoke the custom command to get geolocation and deserialize the result to GeolocationModel
            return Invoker.Invoke("GetGeolocation").New<GeolocationModel>();
        }

        /// <inheritdoc />
        public void HideKeyboard()
        {
            // Hide the keyboard using default settings.
            HideKeyboard(new HideKeyboardModel());
        }

        /// <inheritdoc />
        public void HideKeyboard(string strategy)
        {
            // Hide the keyboard using the specified strategy.
            HideKeyboard(new HideKeyboardModel
            {
                Strategy = strategy
            });
        }

        /// <inheritdoc />
        public void HideKeyboard(string strategy, string keyName)
        {
            // Hide the keyboard using the specified strategy and key name.
            HideKeyboard(new HideKeyboardModel
            {
                Strategy = strategy,
                Key = keyName
            });
        }

        /// <inheritdoc />
        public void HideKeyboard(HideKeyboardModel model)
        {
            // Invoke the custom command to hide the keyboard with the specified model.
            Invoker.Invoke("HideKeyboard", data: model);
        }

        /// <inheritdoc />
        public void SetGeolocation(GeolocationModel geolocationModel)
        {
            // Invoke the custom command to set geolocation with the specified values.
            Invoker.Invoke("SetGeolocation", data: geolocationModel);
        }

        /// <inheritdoc />
        public void ShowKeyboard()
        {
            // Method to show the keyboard is not implemented.
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override IEnumerable<(string Name, WebDriverCommandModel Command)> GetCustomCommands()
        {
            // Helper function to create a WebDriverCommandModel with the specified HTTP method and route.
            static WebDriverCommandModel GetCommand(HttpMethod method, string route) => new()
            {
                Method = method,
                Route = route
            };

            // Return a collection of custom commands for geolocation and device keyboard interactions.
            return
            [
                // Session > Geolocation
                ("GetGeolocation", GetCommand(HttpMethod.Get, "/session/$[session]/location")),
                ("SetGeolocation", GetCommand(HttpMethod.Post, "/session/$[session]/location")),

                // Commands > Device > Keys
                ("HideKeyboard", GetCommand(HttpMethod.Post, "/session/$[session]/appium/device/hide_keyboard")),
                ("IsKeyboardShown", GetCommand(HttpMethod.Get, "/session/$[session]/appium/device/is_keyboard_shown")),
                ("ShowKeyboard", GetCommand(HttpMethod.Post, "/session/$[session]/location"))
            ];
        }
        #endregion
    }
}
