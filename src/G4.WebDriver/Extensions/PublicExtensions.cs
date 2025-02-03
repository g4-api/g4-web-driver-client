/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/By.cs
 */
using G4.WebDriver.DeveloperTools;
using G4.WebDriver.DeveloperTools.Models;
using G4.WebDriver.Exceptions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Interactions;
using G4.WebDriver.Support.Ui;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace G4.WebDriver.Extensions
{
    /// <summary>
    /// Provides extension methods for various utility operations used in the G4 WebDriver.
    /// </summary>
    public static class PublicExtensions
    {
        // Initializes the JSON serializer options for serializing and deserializing JSON data
        private static readonly JsonSerializerOptions s_options = new()
        {
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Closes the browser window at the specified index in the list of open windows.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="index">The zero-based index of the window to close.</param>
        /// <returns>Returns the IWebDriver instance after closing the specified window.</returns>
        public static IWebDriver Close(this IWebDriver driver, int index)
        {
            // Check if the specified index is within the valid range of open windows.
            if (index > (driver.WindowHandles.Count - 1))
            {
                // If the index is out of range, return the driver without closing any window.
                return driver;
            }

            // Store the handle of the main window (assumed to be the first one in the list).
            var mainWindow = driver.WindowHandles[0];

            // Get the handle of the window to close, based on the index.
            var window = driver.WindowHandles[index];

            // Switch to the specified window and close it.
            driver.SwitchTo().Window(window).Close();

            // If there are any windows left open, switch back to the main window.
            if (driver.WindowHandles?.Count > 0)
            {
                driver.SwitchTo().Window(mainWindow);
            }

            // Return the driver instance.
            return driver;
        }

        /// <summary>
        /// Closes the browser window with the specified window handle.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="windowHandle">The handle of the window to close.</param>
        /// <returns>Returns the IWebDriver instance after closing the specified window.</returns>
        public static IWebDriver Close(this IWebDriver driver, string windowHandle)
        {
            // Check if the specified window handle exists in the list of open windows.
            var isWindowName = driver
                .WindowHandles
                .Any(i => i.Equals(windowHandle, StringComparison.OrdinalIgnoreCase));

            // If the window handle is not found, return the driver without closing any window.
            if (!isWindowName)
            {
                return driver;
            }

            // Store the handle of the main window (assumed to be the first one in the list).
            var mainWindow = driver.WindowHandles[0];

            // Switch to the specified window handle and close it.
            driver.SwitchTo().Window(windowHandle).Close();

            // If there are any windows left open, switch back to the main window.
            if (driver.WindowHandles?.Count > 0)
            {
                driver.SwitchTo().Window(handle: mainWindow);
            }

            // Return the driver instance.
            return driver;
        }

        /// <summary>
        /// Closes all child windows and switches back to the main window.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver that controls the browser.</param>
        /// <returns>Returns the IWebDriver instance after closing all child windows.</returns>
        public static IWebDriver CloseWindows(this IWebDriver driver)
        {
            // Check if there's only one window open, if so, return the driver as is.
            if (driver.WindowHandles.Count == 1)
            {
                return driver;
            }

            // Store the handle of the main window (assumed to be the first one in the list).
            var mainWindow = driver.WindowHandles[0];

            // Iterate through all open windows.
            foreach (var window in driver.WindowHandles)
            {
                // Skip the main window to avoid closing it.
                if (window == mainWindow)
                {
                    continue;
                }

                // Switch to the child window and close it.
                driver.SwitchTo().Window(window).Close();

                // Brief pause to ensure the window is fully closed before proceeding.
                Thread.Sleep(100);
            }

            // Switch back to the main window.
            driver.SwitchTo().Window(mainWindow);

            // Return the driver instance.
            return driver;
        }

        /// <summary>
        /// Asserts whether an alert dialog exists and is accessible.
        /// </summary>
        /// <param name="driver">The <see cref="IWebDriver"/> instance.</param>
        /// <returns><c>true</c> if an alert dialog exists and is accessible; otherwise, <c>false</c>.</returns>
        public static bool ConfirmAlert(this IWebDriver driver)
        {
            try
            {
                // Attempt to switch to an alert dialog. If successful, an alert exists.
                driver.SwitchTo().Alert();
                return true;
            }
            catch
            {
                // If an exception is thrown, no alert dialog is accessible.
                return false;
            }
        }

        /// <summary>
        /// Converts the <see cref="IWebElement"/> to a dictionary.
        /// </summary>
        /// <param name="element">The <see cref="IWebElement"/> to convert.</param>
        /// <returns>A dictionary representing the <see cref="IWebElement"/>.</returns>
        public static IDictionary<string, object> ConvertToDictionary(this IWebElement element)
        {
            // Define the property name for the dictionary
            const string PropertyName = "element-6066-11e4-a52e-4f735466cecf";

            // Create a new dictionary and add the element ID with the specified property name
            return new Dictionary<string, object>
            {
                [PropertyName] = element.Id
            };
        }

        /// <summary>
        /// Converts the <see cref="ShadowRoot"/> to a dictionary.
        /// </summary>
        /// <param name="element">The <see cref="ShadowRoot"/> to convert.</param>
        /// <returns>A dictionary representing the <see cref="ShadowRoot"/>.</returns>
        public static IDictionary<string, object> ConvertToDictionary(this ShadowRoot element)
        {
            // Define the property name for the dictionary
            const string PropertyName = "shadow-6066-11e4-a52e-4f735466cecf";

            // Create a new dictionary and add the element ID with the specified property name
            return new Dictionary<string, object>
            {
                [PropertyName] = element.Id
            };
        }

        /// <summary>
        /// Switches the driver's context to a frame identified by the specified selector, with a specified timeout.
        /// </summary>
        /// <param name="targetLocator">The ITargetLocator instance used to perform frame switching.</param>
        /// <param name="by">The selector used to find the frame element.</param>
        /// <param name="timeout">The maximum time to wait for the frame element to be located and switched to.</param>
        public static void Frame(this ITargetLocator targetLocator, By by, TimeSpan timeout)
        {
            // Check if the target locator supports wrapping the driver
            if (targetLocator is not IDriverReference wrapsDriver)
            {
                // If the target locator doesn't wrap a driver, the operation cannot proceed
                return;
            }

            // Use WebDriverWait to wait for the frame element to be located and switch to it
            WebDriverUtilities
                .NewWebDriverWait(wrapsDriver.Driver, timeout)
                .Until(d =>
                {
                    try
                    {
                        // Find the frame element using the provided selector
                        var frameElement = d.FindElement(by);

                        // Switch to the located frame element
                        return d.SwitchTo().Frame(frameElement);
                    }
                    catch (Exception e) when (e is NoSuchFrameException)
                    {
                        // Return null if the frame element was not found or if an exception occurs
                        return null;
                    }
                });
        }

        /// <summary>
        /// Retrieves a displayed web element identified by the specified selector, with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <returns>Returns the first found and displayed IWebElement instance.</returns>
        public static IWebElement GetDisplayedElement(this IWebDriver driver, By by)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return GetDisplayedElement(driver, by, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Retrieves a displayed web element identified by the specified selector, with a specified timeout.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed.</param>
        /// <returns>Returns the first found and displayed IWebElement instance.</returns>
        public static IWebElement GetDisplayedElement(this IWebDriver driver, By by, TimeSpan timeout)
        {
            // Use WebDriverWait to wait for the element to be visible within the specified timeout.
            return WebDriverUtilities
                .NewWebDriverWait(driver, timeout)
                .Until(d => ExpectedConditions.ElementIsVisible(by).Invoke(d));
        }

        /// <summary>
        /// Retrieves a collection of displayed web elements identified by the specified selector, with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web elements.</param>
        /// <returns>Returns an IEnumerable of found and displayed IWebElement instances.</returns>
        public static IEnumerable<IWebElement> GetDisplayedElements(this IWebDriver driver, By by)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return GetDisplayedElements(driver, by, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Retrieves a collection of displayed web elements identified by the specified selector, with a specified timeout.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web elements.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be displayed.</param>
        /// <returns>Returns an IEnumerable of found and displayed IWebElement instances. If no elements are found within the timeout, returns an empty collection.</returns>
        public static IEnumerable<IWebElement> GetDisplayedElements(this IWebDriver driver, By by, TimeSpan timeout)
        {
            try
            {
                // Use WebDriverWait to wait for all elements matching the selector to be visible within the specified timeout.
                return WebDriverUtilities
                    .NewWebDriverWait(driver, timeout)
                    .Until(d => ExpectedConditions.VisibilityOfAllElementsLocatedBy(by).Invoke(d));
            }
            catch
            {
                // If an exception occurs (e.g., elements not found within the timeout), return an empty collection.
                return [];
            }
        }

        /// <summary>
        /// Retrieves a web element identified by the specified selector, with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <returns>Returns the found IWebElement instance.</returns>
        public static IWebElement GetElement(this IWebDriver driver, By by)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return GetElement(driver, by, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Retrieves a web element identified by the specified selector, with a specified timeout.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be found.</param>
        /// <returns>Returns the found IWebElement instance.</returns>
        public static IWebElement GetElement(this IWebDriver driver, By by, TimeSpan timeout)
        {
            // Use WebDriverWait to wait for the element to exist within the specified timeout.
            return WebDriverUtilities
                .NewWebDriverWait(driver, timeout)
                .Until(d => ExpectedConditions.ElementExists(by).Invoke(d));
        }

        /// <summary>
        /// Retrieves a collection of web elements identified by the specified selector, with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web elements.</param>
        /// <returns>Returns an IEnumerable of found IWebElement instances.</returns>
        public static IEnumerable<IWebElement> GetElements(this IWebDriver driver, By by)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return GetElements(driver, by, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Retrieves a collection of web elements identified by the specified selector, with a specified timeout.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web elements.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be found.</param>
        /// <returns>Returns an IEnumerable of found IWebElement instances. If no elements are found within the timeout, returns an empty collection.</returns>
        public static IEnumerable<IWebElement> GetElements(this IWebDriver driver, By by, TimeSpan timeout)
        {
            try
            {
                // Use WebDriverWait to wait for all elements matching the selector to be present within the specified timeout.
                return WebDriverUtilities
                    .NewWebDriverWait(driver, timeout)
                    .Until(d => ExpectedConditions.PresenceOfAllElementsLocatedBy(by).Invoke(d));
            }
            catch
            {
                // If an exception occurs (e.g., elements not found within the timeout), return an empty collection.
                return [];
            }
        }

        /// <summary>
        /// Retrieves an enabled web element identified by the specified selector, with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <returns>Returns the first found and enabled IWebElement instance.</returns>
        public static IWebElement GetEnabledElement(this IWebDriver driver, By by)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return GetEnabledElement(driver, by, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Retrieves an enabled web element identified by the specified selector, with a specified timeout.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be enabled and clickable.</param>
        /// <returns>Returns the first found and enabled IWebElement instance.</returns>
        public static IWebElement GetEnabledElement(this IWebDriver driver, By by, TimeSpan timeout)
        {
            // Use WebDriverWait to wait for the element to be enabled and clickable within the specified timeout.
            return WebDriverUtilities
                .NewWebDriverWait(driver, timeout)
                .Until(d => ExpectedConditions.ElementToBeClickable(by).Invoke(d));
        }

        /// <summary>
        /// Retrieves a collection of enabled web elements identified by the specified selector, with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web elements.</param>
        /// <returns>Returns an IEnumerable of found and enabled IWebElement instances.</returns>
        public static IEnumerable<IWebElement> GetEnabledElements(this IWebDriver driver, By by)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return GetEnabledElements(driver, by, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Retrieves a collection of enabled web elements identified by the specified selector, with a specified timeout.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web elements.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be enabled and clickable.</param>
        /// <returns>Returns an IEnumerable of found and enabled IWebElement instances. If no elements are found within the timeout, returns an empty collection.</returns>
        public static IEnumerable<IWebElement> GetEnabledElements(this IWebDriver driver, By by, TimeSpan timeout)
        {
            try
            {
                // Use WebDriverWait to wait for all elements matching the selector to be enabled and clickable within the specified timeout.
                return WebDriverUtilities
                    .NewWebDriverWait(driver, timeout)
                    .Until(d => ExpectedConditions.EnabilityOfAllElementsLocatedBy(by).Invoke(d));
            }
            catch
            {
                // If an exception occurs (e.g., elements not found within the timeout), return an empty collection.
                return [];
            }
        }

        /// <summary>
        /// Retrieves the outer HTML of the specified web element.
        /// </summary>
        /// <param name="element">The IWebElement for which to retrieve the outer HTML.</param>
        /// <returns>Returns a string containing the outer HTML of the element.</returns>
        /// <exception cref="WebDriverException">Thrown if there is an error retrieving the outer HTML.</exception>
        public static string GetOuterHtml(this IWebElement element)
        {
            try
            {
                // Execute JavaScript to get the outerHTML of the specified element.
                var source = element.Driver.InvokeScript("return arguments[0].outerHTML;", element);

                // Return the retrieved outer HTML as a string.
                return $"{source}";
            }
            catch (Exception e)
            {
                // Get the base exception message for better clarity in the error message.
                var message = e.GetBaseException().Message;

                // Throw a WebDriverException with the base exception message
                // and the original exception as the inner exception.
                throw new WebDriverException(message, innerException: e);
            }
        }

        /// <summary>
        /// Retrieves a selected web element identified by the specified selector, with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <returns>Returns the first found and selected IWebElement instance.</returns>
        public static IWebElement GetSelectedElement(this IWebDriver driver, By by)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return GetSelectedElement(driver, by, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Retrieves a selected web element identified by the specified selector, with a specified timeout.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be selected.</param>
        /// <returns>Returns the first found and selected IWebElement instance.</returns>
        public static IWebElement GetSelectedElement(this IWebDriver driver, By by, TimeSpan timeout)
        {
            // Use WebDriverWait to wait for the element to be clickable within the specified timeout.
            return WebDriverUtilities
                .NewWebDriverWait(driver, timeout)
                .Until(d => ExpectedConditions.ElementIsSelected(by).Invoke(d));
        }

        /// <summary>
        /// Retrieves a collection of selected web elements identified by the specified selector, with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web elements.</param>
        /// <returns>Returns an IEnumerable of found and selected IWebElement instances.</returns>
        public static IEnumerable<IWebElement> GetSelectedElements(this IWebDriver driver, By by)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return GetSelectedElements(driver, by, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Retrieves a collection of selected web elements identified by the specified selector, with a specified timeout.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web elements.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be selected.</param>
        /// <returns>Returns an IEnumerable of found and selected IWebElement instances. If no elements are found within the timeout, returns an empty collection.</returns>
        public static IEnumerable<IWebElement> GetSelectedElements(this IWebDriver driver, By by, TimeSpan timeout)
        {
            try
            {
                // Use WebDriverWait to wait for all elements matching the selector to be selected within the specified timeout.
                return WebDriverUtilities
                    .NewWebDriverWait(driver, timeout)
                    .Until(d => ExpectedConditions.SelectedOfAllElementsLocatedBy(by).Invoke(d));
            }
            catch
            {
                // If an exception occurs (e.g., elements not found within the timeout), return an empty collection.
                return [];
            }
        }

        /// <summary>
        /// Retrieves the server address of the WebDriver service associated with the given driver instance.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver from which to retrieve the server address.</param>
        /// <returns>The URI representing the server address of the WebDriver service.</returns>
        public static Uri GetServerAddress(this IWebDriver driver) => driver.Invoker.WebDriverService.ServerAddress;

        /// <summary>
        /// Retrieves the current session ID associated with the WebDriver instance.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver from which to retrieve the session ID.</param>
        /// <returns>
        /// A <see cref="SessionIdModel"/> representing the session ID of the WebDriver.
        /// If the driver implements <see cref="IHasSessionId"/>, the actual session ID is returned.
        /// Otherwise, a new session ID is generated with a "G4-" prefix.
        /// </returns>
        public static SessionIdModel GetSession(this IWebDriver driver)
        {
            // Check if the driver implements the IHasSessionId interface, which provides access to the session ID.
            return driver is IWebDriverSession d
                ? d.Session                                   // Return the existing session ID.
                : new SessionIdModel($"G4-{Guid.NewGuid()}"); // Generate and return a new session ID with a "G4-" prefix.
        }

        /// <summary>
        /// Retrieves the shadow root of the specified web element. The shadow root allows access to the shadow DOM,
        /// which is a separate DOM tree associated with a web component.
        /// </summary>
        /// <param name="element">The IWebElement representing the element with a shadow DOM.</param>
        /// <returns>Returns an ISearchContext representing the shadow root, or null if the shadow root is not found.</returns>
        public static ISearchContext GetShadowRoot(this IWebElement element)
        {
            // JavaScript to retrieve the shadow root of the element.
            const string Script = "return arguments[0].shadowRoot;";

            // The property name used to identify the shadow root in the WebDriver protocol.
            const string PropertyName = "shadow-6066-11e4-a52e-4f735466cecf";

            // Invoke the script to get the shadow root of the element.
            var shadowRoot = element.Driver.InvokeScript(Script, element);

            // If the shadow root is null or empty, return null.
            if (shadowRoot == null || string.IsNullOrEmpty($"{shadowRoot}"))
            {
                return null;
            }

            // Retrieve the shadow root's unique ID from the returned JSON object.
            var id = ((JsonElement)shadowRoot)
                .GetProperty(PropertyName)
                .GetString();

            // Return a new ShadowRoot object that allows interaction with the shadow DOM.
            return new ShadowRoot(element.Driver, id);
        }

        /// <summary>
        /// Retrieves the value of a form element, such as an input or textarea.
        /// </summary>
        /// <param name="element">The IWebElement from which to retrieve the value.</param>
        /// <returns>Returns the value of the element as a string. If the value is null or an error occurs, returns an empty string.</returns>
        public static string GetValue(this IWebElement element)
        {
            try
            {
                // Use JavaScript to retrieve the 'value' property of the element.
                string text = element
                    .Driver
                    .InvokeScript("return arguments[0].value;", element) as string;

                // Return the retrieved value, or an empty string if the value is null or empty.
                return string.IsNullOrEmpty(text) ? string.Empty : text;
            }
            catch
            {
                // Ignore any exceptions that occur during the retrieval of the element value.
                // This prevents exceptions from propagating and affecting the test flow.
            }

            // Return an empty string if an error occurs or if the value could not be retrieved.
            return string.Empty;
        }

        /// <summary>
        /// Attempts to repeatedly find and click an element identified by the specified selector
        /// until the element becomes stale or the timeout is reached.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <returns>Returns the IWebDriver instance after the element is clicked or the timeout is reached.</returns>
        public static IWebDriver InvokeClickUntilStale(this IWebDriver driver, By by)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return InvokeClickUntilStale(driver, by, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Attempts to repeatedly find and click an element identified by the specified selector
        /// until the element becomes stale or the timeout is reached.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be clicked.</param>
        /// <returns>Returns the IWebDriver instance after the element is clicked or the timeout is reached.</returns>
        public static IWebDriver InvokeClickUntilStale(this IWebDriver driver, By by, TimeSpan timeout)
        {
            // Create a new WebDriverWait instance with the specified timeout.
            var webDriverWait = new WebDriverWait<IWebDriver>(driver, timeout);

            // Use WebDriverWait to wait for the element to be found, clicked, and possibly become stale.
            return webDriverWait.Until(d =>
            {
                try
                {
                    // Attempt to find and click the element.
                    d.FindElement(by).Click();
                }
                catch (Exception e) when (e is not StaleElementReferenceException)
                {
                    // If an exception occurs (other than StaleElementReferenceException), return null to keep waiting.
                    return null;
                }

                // Return the driver instance if the element becomes stale or the click succeeds.
                return d;
            });
        }

        /// <summary>
        /// Repeatedly attempts to send keys to an element identified by the specified selector until the element becomes stale or the timeout is reached.
        /// Optionally clears the element's existing text before sending the new keys. Uses a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <param name="text">The text to send to the web element.</param>
        /// <param name="clear">A boolean value indicating whether to clear the element's existing text before sending the new keys.</param>
        /// <returns>Returns the IWebDriver instance after sending the keys to the element.</returns>
        public static IWebDriver InvokeSendKeysUntilStale(this IWebDriver driver, By by, string text, bool clear)
        {
            // Call the overload method with a default timeout of 10 seconds.
            return InvokeSendKeysUntilStale(driver, by, text, clear, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Repeatedly attempts to send keys to an element identified by the specified selector until the element becomes stale or the timeout is reached.
        /// Does not clear the element's existing text. Uses a default timeout of 10 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <param name="text">The text to send to the web element.</param>
        /// <returns>Returns the IWebDriver instance after sending the keys to the element.</returns>
        public static IWebDriver InvokeSendKeysUntilStale(this IWebDriver driver, By by, string text)
        {
            // Call the overload method without clearing the element's existing text, using a default timeout of 10 seconds.
            return InvokeSendKeysUntilStale(driver, by, text, clear: false, timeout: TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Repeatedly attempts to send keys to an element identified by the specified selector until the element becomes stale or the timeout is reached.
        /// Does not clear the element's existing text.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <param name="text">The text to send to the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to accept the keys.</param>
        /// <returns>Returns the IWebDriver instance after sending the keys to the element.</returns>
        public static IWebDriver InvokeSendKeysUntilStale(this IWebDriver driver, By by, string text, TimeSpan timeout)
        {
            // Call the overload method without clearing the element's existing text, using the specified timeout.
            return InvokeSendKeysUntilStale(driver, by, text, clear: false, timeout);
        }

        /// <summary>
        /// Repeatedly attempts to send keys to an element identified by the specified selector until the element becomes stale or the timeout is reached.
        /// Optionally clears the element's existing text before sending the new keys.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the web element.</param>
        /// <param name="text">The text to send to the web element.</param>
        /// <param name="clear">A boolean value indicating whether to clear the element's existing text before sending the new keys.</param>
        /// <param name="timeout">The maximum time to wait for the element to accept the keys.</param>
        /// <returns>Returns the IWebDriver instance after sending the keys to the element.</returns>
        public static IWebDriver InvokeSendKeysUntilStale(this IWebDriver driver, By by, string text, bool clear, TimeSpan timeout)
        {
            // Create a new WebDriverWait instance with the specified timeout.
            var webDriverWait = new WebDriverWait<IWebDriver>(driver, timeout);

            // Use WebDriverWait to wait for the element to be found and send keys.
            return webDriverWait.Until(d =>
            {
                try
                {
                    // Find the element using the provided selector.
                    var element = d.FindElement(by);

                    // If the 'clear' flag is set, clear the element's existing text.
                    if (clear)
                    {
                        element.Clear();
                    }

                    // Send the specified text to the element.
                    element.SendKeys(text);
                }
                catch (Exception e) when (e is not StaleElementReferenceException)
                {
                    // If an exception occurs (other than StaleElementReferenceException), return null to keep waiting.
                    return null;
                }

                // Return the driver instance after successfully sending the keys.
                return d;
            });
        }

        /// <summary>
        /// Tries to move the mouse cursor to the specified web element.
        /// </summary>
        /// <param name="element">The element to which the mouse cursor should be moved.</param>
        /// <returns>Returns the same element after attempting to move the mouse cursor.</returns>
        public static IWebElement MoveToElement(this IWebElement element)
        {
            // Check if the element supports wrapping a driver.
            if (element is not IDriverReference)
            {
                // If the element does not wrap a driver, return the element as is.
                return element;
            }

            // Get the WebDriver instance associated with the element.
            var driver = element.Driver;

            // Create a new ActionSequence for the driver to perform actions like moving the mouse cursor.
            var actions = new ActionSequence(driver);

            try
            {
                // Attempt to move the mouse cursor to the element.
                actions.AddMoveMouseCursor(element).Invoke();
            }
            catch
            {
                // Ignore any exceptions that occur during mouse cursor movement
                // This prevents exceptions from propagating further and affecting the rest of the test.
            }

            // Return the element after the attempt to move the mouse cursor.
            return element;
        }

        /// <summary>
        /// Creates a new WebDriver command for invoking.
        /// </summary>
        /// <param name="invoker">The WebDriver command invoker.</param>
        /// <param name="commandName">The name of the command to create.</param>
        /// <returns>A new WebDriver command model.</returns>
        /// <exception cref="ArgumentNullException">Thrown when invoker is null.</exception>
        /// <exception cref="NotSupportedException">Thrown when the specified command is not supported.</exception>
        public static WebDriverCommandModel NewCommand(this IWebDriverCommandInvoker invoker, string commandName)
        {
            // Check if the invoker is null
            if (invoker == null)
            {
                throw new ArgumentNullException(nameof(invoker), "The invoker cannot be null.");
            }

            // Check if the command name is empty or not present in the invoker's commands
            if (string.IsNullOrEmpty(commandName) || !invoker.Commands.TryGetValue(commandName, out WebDriverCommandModel value))
            {
                throw new NotSupportedException($"The command '{commandName}' is not supported.");
            }

            // Get the command from the invoker's commands
            var command = value;

            // Set the session for the command, if a session is available
            command.Session = invoker.Session?.OpaqueKey;

            // Return the created command
            return command;
        }

        /// <summary>
        /// Creates a new developer tools connection for the specified WebDriver instance.
        /// </summary>
        /// <param name="driver">The WebDriver instance for which to create a developer tools connection.</param>
        /// <returns>A <see cref="DeveloperToolsConnection"/> object representing the developer tools connection.</returns>
        /// <exception cref="WebDriverException">Thrown when the debugger address cannot be resolved.</exception>
        public static DeveloperToolsConnection NewDeveloperToolsConnection(this IWebDriver driver)
        {
            // Resolve the WebSocket debugger address from the WebDriver instance.
            var webSocketDebuggerUrl = ResolveDebuggerAddress(driver);

            // Check if the WebSocket debugger URL is empty or null.
            if (string.IsNullOrEmpty(webSocketDebuggerUrl))
            {
                // Error message to indicate failure in resolving the debugger address.
                const string error500 = "Failed to resolve debugger address for developer tools connection.";

                // Throw an exception if the WebSocket debugger URL could not be resolved.
                throw new WebDriverException(error500);
            }

            // Ensure the WebSocket debugger URL starts with "http://".
            // If it doesn't, prepend "http://".
            webSocketDebuggerUrl = webSocketDebuggerUrl.StartsWith("http://")
                ? webSocketDebuggerUrl
                : $"http://{webSocketDebuggerUrl}";

            // Retrieve the WebSocket debugger URL asynchronously and wait for the result.
            // This call retrieves the actual WebSocket debugger URL required for connection.
            webSocketDebuggerUrl = GetWebSocketDebuggerUrl(endpoint: webSocketDebuggerUrl).GetAwaiter().GetResult();

            // Create and return a new DeveloperToolsConnection using the resolved WebSocket debugger URL.
            return new DeveloperToolsConnection(webSocketDebuggerUrl);
        }

        /// <summary>
        /// Creates a new <see cref="SessionModel"/> from the provided <see cref="CapabilitiesModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="CapabilitiesModel"/> instance.</param>
        /// <returns>A new <see cref="SessionModel"/> instance.</returns>
        public static SessionModel NewSessionModel(this CapabilitiesModel model)
        {
            // Initialize the desired capabilities dictionary.
            var desired = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // Populate the desired capabilities from AlwaysMatch.
            foreach (var capability in model.AlwaysMatch)
            {
                desired[capability.Key] = capability.Value;
            }

            // Populate the desired capabilities from FirstMatch.
            foreach (var capabilities in model.FirstMatch)
            {
                foreach (var capability in capabilities)
                {
                    desired[capability.Key] = capability.Value;
                }
            }

            // If FirstMatch is empty, move the 'browserName' entry to FirstMatch.
            if (model.FirstMatch?.Any() == false)
            {
                const string BrowserName = "browserName";
                _ = desired.TryGetValue(BrowserName, out object browserNameOut);

                // Create a new entry for FirstMatch.
                var entry = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    [BrowserName] = $"{browserNameOut}"
                };

                // Add the entry to FirstMatch and remove it from AlwaysMatch.
                model.FirstMatch = model.FirstMatch.Concat([entry]);
                model.AlwaysMatch.Remove(BrowserName);
            }

            // Return a new SessionModel instance.
            return new SessionModel
            {
                Capabilities = model,
                DesiredCapabilities = desired
            };
        }

        /// <summary>
        /// Opens the specified URL in the browser with a default page load timeout of 60 seconds.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="url">The URL to navigate to.</param>
        /// <returns>Returns the IWebDriver instance after navigating to the specified URL.</returns>
        public static IWebDriver OpenUrl(this IWebDriver driver, string url)
        {
            // Call the overload method with a default page load timeout of 60 seconds.
            return OpenUrl(driver, url, driver.Manage().Timeouts.PageLoad);
        }

        /// <summary>
        /// Opens the specified URL in the browser with a specified page load timeout.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="url">The URL to navigate to.</param>
        /// <param name="timeout">The maximum time to wait for the page to load.</param>
        /// <returns>Returns the IWebDriver instance after navigating to the specified URL.</returns>
        /// <exception cref="Exception">Throws an exception if there is an issue with navigating to the URL.</exception>
        public static IWebDriver OpenUrl(this IWebDriver driver, string url, TimeSpan timeout)
        {
            try
            {
                // Set the page load timeout for the browser.
                driver.Manage().Timeouts.PageLoad = timeout;

                // Navigate to the specified URL using your custom NavigateTo method.
                driver.Navigate().Open(url);

                // Return the driver instance after successful navigation.
                return driver;
            }
            catch
            {
                // If an exception occurs, close and dispose of the driver to clean up resources.
                driver?.Close();
                driver?.Dispose();

                // Re-throw the exception to be handled by the calling code.
                throw;
            }
        }

        /// <summary>
        /// Extension method to save a screenshot of the entire page for a given <see cref="IWebDriver"/>.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="fileName">The file name (including path) to save the screenshot.</param>
        /// <returns>The updated WebDriver instance.</returns>
        public static IWebDriver SaveScreenshot(this IWebDriver driver, string fileName)
        {
            // Ensure the path does not end with a backslash
            fileName = fileName.TrimEnd('\\').TrimEnd('/');

            // Capture and save the screenshot using the WebDriver's built-in functionality
            ((ITakesScreenshot)driver).GetScreenshot().Save(fileName);

            // Return the updated WebDriver instance
            return driver;
        }

        /// <summary>
        /// Extension method to save a screenshot of a specific <see cref="IWebElement"/>.
        /// </summary>
        /// <param name="element">The WebElement instance.</param>
        /// <param name="fileName">The file name (including path) to save the screenshot.</param>
        public static void SaveScreenshot(this IWebElement element, string fileName)
        {
            // Ensure the path does not end with a backslash
            fileName = fileName.TrimEnd('\\');

            // Capture and save the screenshot of the WebElement
            element.GetScreenshot().Save(fileName);
        }

        /// <summary>
        /// Sends a custom WebDriver command using the provided WebDriverCommandModel.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="command">The WebDriverCommandModel containing the command to send.</param>
        public static void SendCommand(this IWebDriver driver, WebDriverCommandModel command)
        {
            // Retrieve the server address from the WebDriver service.
            var serverAddress = driver.Invoker.WebDriverService.ServerAddress.AbsoluteUri;

            // Send the command to the WebDriver server.
            command.Send(baseUrl: serverAddress);
        }

        /// <summary>
        /// Sends a DELETE command to the WebDriver server.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="route">The route on the WebDriver server for the command.</param>
        /// <returns>Returns the response from the WebDriver server as a string.</returns>
        public static string SendDeleteCommand(this IWebDriver driver, string route)
        {
            // Get the full command API endpoint.
            var command = FormatCommandRoute(driver, route);

            // Send the DELETE request and wait for the response.
            var response = WebDriverUtilities.HttpClient
                .DeleteAsync(requestUri: command)
                .GetAwaiter()
                .GetResult();

            // Return the response content as a string.
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends a GET command to the WebDriver server for a specific element route.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="element">The IWebElement to which the command is related.</param>
        /// <param name="route">The route on the WebDriver server for the command.</param>
        /// <returns>Returns the response from the WebDriver server as a string.</returns>
        public static string SendGetCommand(this IWebDriver driver, IWebElement element, string route)
        {
            // Construct the route specific to the element.
            var elementCommandRoute = $"/element/{element.Id}/{route}";

            // Send the GET command with the constructed route.
            return SendGetCommand(driver, route: elementCommandRoute);
        }

        /// <summary>
        /// Sends a GET command to the WebDriver server.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="route">The route on the WebDriver server for the command.</param>
        /// <returns>Returns the response from the WebDriver server as a string.</returns>
        public static string SendGetCommand(this IWebDriver driver, string route)
        {
            // Get the full command API endpoint.
            var command = FormatCommandRoute(driver, route);

            // Send the GET request and wait for the response.
            var response = WebDriverUtilities.HttpClient
                .GetAsync(requestUri: command)
                .GetAwaiter()
                .GetResult();

            // Return the response content as a string.
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends a sequence of keys to the HTML element with a specified delay between each key press.
        /// </summary>
        /// <param name="element">The HTML element to send keys to.</param>
        /// <param name="text">The text representing the keys to send.</param>
        /// <param name="milliseconds">The delay in milliseconds between each key press.</param>
        public static void SendKeys(this IWebElement element, string text, int milliseconds)
        {
            // Convert the milliseconds delay to TimeSpan
            var delay = TimeSpan.FromMilliseconds(milliseconds);

            // Call the internal SendKeys method to handle key sending with the specified delay
            SendKeys(element, text, delay);
        }

        /// <summary>
        /// Sends a sequence of keys to the HTML element with a specified delay between each key press.
        /// </summary>
        /// <param name="element">The HTML element to send keys to.</param>
        /// <param name="text">The text representing the keys to send.</param>
        /// <param name="delay">The delay between each key press.</param>
        public static void SendKeys(this IWebElement element, string text, TimeSpan delay)
        {
            // If delay is zero, send all keys at once
            if (delay == TimeSpan.Zero)
            {
                element.SendKeys(text);
                return;
            }

            // Iterate through each key and send it with the specified delay
            foreach (var key in text)
            {
                // Send the key as text
                element.SendKeys(text: $"{key}");

                // Wait for the specified delay before sending the next key
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// Sends modified keys to a WebElement, where modifiers are pressed down before sending the actual keys.
        /// </summary>
        /// <param name="element">The HTML element to send keys to.</param>
        /// <param name="text">The text representing the keys to send.</param>
        /// <param name="modifiers">A collection of modifier keys to be pressed down.</param>
        public static void SendModifiedKeys(this IWebElement element, string text, IEnumerable<string> modifiers)
        {
            // Create an action sequence to perform the key presses
            var actions = new ActionSequence(element.Driver);

            try
            {
                // Press down each modifier key
                foreach (var modifier in modifiers)
                {
                    var key = Keys.Get(modifier);
                    actions.AddKeyDown(key);
                }

                // Invoke the actions to press down the modifiers
                actions.Invoke();

                // Send the specified keys to the element
                element.SendKeys(text);
            }
            catch (Exception e)
            {
                throw e.GetBaseException();
            }
            finally
            {
                // Clear the actions to release the modifiers
                actions.ClearActions();
            }
        }

        /// <summary>
        /// Performs a native clear on the web element with a default delay between key presses.
        /// </summary>
        /// <param name="element">The web element to clear.</param>
        public static void SendNativeClear(this IWebElement element)
        {
            SendNativeClear(element, delay: TimeSpan.FromMilliseconds(50));
        }

        /// <summary>
        /// Performs a native clear on the web element with a specified delay between key presses.
        /// </summary>
        /// <param name="element">The web element to clear.</param>
        /// <param name="delay">The delay between each backspace key press in milliseconds.</param>
        public static void SendNativeClear(this IWebElement element, TimeSpan delay)
        {
            // Supported HTML tags for native clear
            var supportedTags = new[] { "input" };
            var driver = element.Driver;

            // Check if the element's tag is supported for native clear
            var isTag = supportedTags.Contains(element.TagName, StringComparer.OrdinalIgnoreCase);

            // The element's tag is not supported for native clear
            if (!isTag)
            {
                // Throw a NotSupportedException to indicate the error
                throw new NotSupportedException($"Native clear is not supported for elements with the tag: {element.TagName}");
            }

            try
            {
                // Get the length of the element's value
                var length = element.GetAttribute("value")?.Length ?? 0;

                // Create an action sequence to send backspace key presses with a delay
                var actions = new ActionSequence(driver);

                // Move the mouse cursor to the specified element
                actions.AddMoveMouseCursor(element);

                // Click the specified element
                actions.AddClick(element);

                // Simulate pressing the 'End' key using WebDriver's Keys
                actions.AddKeyPress(Keys.End);

                // Send backspace key presses to clear the content
                for (var i = 0; i < length; i++)
                {
                    actions.AddKeyPress(Keys.Backspace);
                    actions.AddPauseAction(duration: Convert.ToInt32(delay.TotalMilliseconds));
                }

                // Invoke the actions to perform the native clear
                actions.Invoke();
            }
            catch (Exception e)
            {
                // Throw the base exception to indicate the error
                throw e.GetBaseException();
            }
        }

        /// <summary>
        /// Sends a POST command to the WebDriver server with data for a specific element route.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="element">The IWebElement to which the command is related.</param>
        /// <param name="route">The route on the WebDriver server for the command.</param>
        /// <param name="data">The data to send in the POST request.</param>
        /// <returns>Returns the response from the WebDriver server as a string.</returns>
        public static string SendPostCommand(this IWebDriver driver, IWebElement element, string route, object data)
        {
            // Construct the route specific to the element.
            var elementCommandRoute = $"/element/{element.Id}/{route}";

            // Send the POST command with the constructed route and data.
            return SendPostCommand(driver, route: elementCommandRoute, data);
        }

        /// <summary>
        /// Sends a POST command to the WebDriver server with the specified data.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="route">The route on the WebDriver server for the command.</param>
        /// <param name="data">The data to send in the POST request.</param>
        /// <returns>Returns the response from the WebDriver server as a string.</returns>
        public static string SendPostCommand(this IWebDriver driver, string route, object data)
        {
            // Serialize the data to a JSON string.
            var content = JsonSerializer.Serialize(data, s_options);

            // Create the HTTP content for the POST request.
            var stringContent = new StringContent(content, Encoding.UTF8, mediaType: "application/json");

            // Get the full command API endpoint.
            var command = FormatCommandRoute(driver, route);

            // Send the POST request and wait for the response.
            var response = WebDriverUtilities.HttpClient
                .PostAsync(requestUri: command, content: stringContent)
                .GetAwaiter()
                .GetResult();

            // Return the response content as a string.
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Starts a click listener that clicks on elements found by the specified selector at regular intervals.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the elements to click.</param>
        public static void StartClickListener(this IWebDriver driver, By by)
        {
            // Start the click listener with a default interval of 3 seconds and a timeout of 10 minutes.
            StartClickListener(driver, by, interval: TimeSpan.FromSeconds(3), timeout: TimeSpan.FromMinutes(10));
        }

        /// <summary>
        /// Starts a click listener that clicks on elements found by the specified selector at the specified interval.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the elements to click.</param>
        /// <param name="interval">The time interval between each click attempt.</param>
        public static void StartClickListener(this IWebDriver driver, By by, TimeSpan interval)
        {
            // Start the click listener with a specified interval and a default timeout of 10 minutes.
            StartClickListener(driver, by, interval, timeout: TimeSpan.FromMinutes(10));
        }

        /// <summary>
        /// Starts a click listener that clicks on elements found by the specified selector at the specified interval and within the specified timeout period.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="by">The selector used to find the elements to click.</param>
        /// <param name="interval">The time interval between each click attempt.</param>
        /// <param name="timeout">The maximum duration for which the click listener will run.</param>
        public static void StartClickListener(this IWebDriver driver, By by, TimeSpan interval, TimeSpan timeout)
        {
            // Local method to perform the click action on elements found by the selector.
            static void Tick(IWebDriver driver, By by, TimeSpan interval)
            {
                try
                {
                    // Find elements matching the selector.
                    var elements = driver.FindElements(by);

                    // If no elements are found, return immediately.
                    if (!elements.Any())
                    {
                        return;
                    }

                    // Click on each found element.
                    foreach (var element in elements)
                    {
                        element.Click();
                    }
                }
                catch
                {
                    // Ignore exceptions to ensure the listener continues running.
                }
                finally
                {
                    // Pause for the specified interval before the next click attempt.
                    Thread.Sleep(interval);
                }
            }

            // Start a new long-running task to continuously click on elements at the specified interval.
            Task.Factory.StartNew(() =>
            {
                // Initialize the total time elapsed and the expiration time.
                TimeSpan expire = TimeSpan.FromTicks(0);

                // Run the click listener until the timeout is reached or the driver is null.
                while (driver != null || expire <= timeout)
                {
                    Tick(driver, by, interval);
                    expire += interval;
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Submits a form associated with the specified web element. If the element is a submit button, it triggers a click.
        /// Otherwise, it uses JavaScript to submit the form that the element belongs to.
        /// </summary>
        /// <param name="element">The IWebElement to be submitted, typically an input element within a form.</param>
        public static void Submit(this IWebElement element)
        {
            // Constants for string comparison and the JavaScript submit script.
            const StringComparison Comparison = StringComparison.OrdinalIgnoreCase;
            const string Script =
                "var e = arguments[0].ownerDocument.createEvent('Event');" +
                "e.initEvent('submit', true, true);" +
                "if (arguments[0].dispatchEvent(e)) { arguments[0].submit(); }";

            // Retrieve the 'type' attribute of the element to check if it's a submit button.
            var elementType = element.GetAttribute("type");
            var isSubmit = !string.IsNullOrEmpty(elementType) && elementType.Equals("submit", Comparison);

            // If the element is a submit button, trigger a click event.
            if (isSubmit)
            {
                element.Click();
                return;
            }

            // Find the nearest form element that the current element is part of.
            var form = element.FindElement(By.Xpath("./ancestor-or-self::form"));

            // Use JavaScript to submit the form programmatically.
            element.Driver.InvokeScript(Script, form);
        }

        /// <summary>
        /// Submits a form on the web page using the form's ID.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="id">The ID of the form to submit.</param>
        /// <returns>Returns the IWebDriver instance after the form is submitted.</returns>
        public static IWebDriver SubmitForm(this IWebDriver driver, string id)
        {
            // Use JavaScript to submit the form with the specified ID.
            driver.InvokeScript($"document.forms['{id}'].submit();");

            // Return the driver instance after form submission.
            return driver;
        }

        /// <summary>
        /// Submits a form on the web page using the form's index in the document.forms collection.
        /// </summary>
        /// <param name="driver">The instance of the IWebDriver controlling the browser.</param>
        /// <param name="index">The zero-based index of the form in the document.forms collection.</param>
        /// <returns>Returns the IWebDriver instance after the form is submitted.</returns>
        public static IWebDriver SubmitForm(this IWebDriver driver, int index)
        {
            // Use JavaScript to submit the form at the specified index.
            driver.InvokeScript($"document.forms[{index}].submit();");

            // Return the driver instance after form submission.
            return driver;
        }

        /// <summary>
        /// Switches the driver's context to the window identified by the specified index in the list of open window handles.
        /// </summary>
        /// <param name="targetLocator">The ITargetLocator instance used to perform window switching.</param>
        /// <param name="index">The zero-based index of the window in the driver's list of open window handles.</param>
        /// <returns>Returns the IWebDriver instance after switching to the specified window.</returns>
        /// <exception cref="NotImplementedException">Thrown if the target locator does not support wrapping a driver.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified index is out of range of the available window handles.</exception>
        public static IWebDriver Window(this ITargetLocator targetLocator, int index)
        {
            // Check if the target locator supports wrapping a driver
            if (targetLocator is not IDriverReference wrapsDriver)
            {
                // Throw an exception if the target locator does not wrap a driver
                throw new NotImplementedException("Target locator does not support wrapping a driver.");
            }

            // Get the wrapped WebDriver instance
            var driver = wrapsDriver.Driver;

            // Check if there are no open windows
            if (driver.WindowHandles.Count == 0)
            {
                // If no windows are open, return the driver without changing the context
                return driver;
            }

            // Check if the specified index is within the valid range of window handles
            if (index < 0 || index >= driver.WindowHandles.Count)
            {
                // Throw an exception if the index is out of range
                throw new ArgumentOutOfRangeException(nameof(index), "The specified index is out of range of available window handles.");
            }

            // Switch to the window at the specified index
            driver.SwitchTo().Window(driver.WindowHandles[index]);

            // Return the driver after switching to the window
            return driver;
        }

        // Formats a command route by incorporating the WebDriver server address and session ID into the route.
        private static string FormatCommandRoute(IWebDriver driver, string route)
        {
            // Retrieve the server address from the WebDriver service.
            var serverAddress = driver.Invoker.WebDriverService.ServerAddress.AbsoluteUri;

            // Check if the driver implements IHasSessionId to confirm it has a session ID.
            if (driver is not IWebDriverSession)
            {
                // Throw an exception if the driver does not have a session ID.
                throw new WebDriverException("The driver does not have a session ID.");
            }

            // Retrieve the session ID from the driver.
            var session = ((IWebDriverSession)driver).Session;

            // Trim any leading or trailing slashes from the route to ensure proper formatting.
            route = route.Trim('/');

            // Return the formatted command route, including the server address, session ID, and route.
            return $"{serverAddress}session/{session}/{route}";
        }

        // Retrieves the WebSocket debugger URL for the specified endpoint using the DevTools Protocol.
        private static async Task<string> GetWebSocketDebuggerUrl(string endpoint)
        {
            // Retrieves the browser version information and supported DevTools Protocol version from the specified endpoint.
            static async Task<VersionModel> GetVersion(string endpoint)
            {
                // Construct the URL to get version information from the endpoint.
                var versionUrl = $"{endpoint.TrimEnd('/')}/json/version";

                // Create a new HTTP GET request for the version URL.
                var request = new HttpRequestMessage(HttpMethod.Get, versionUrl);

                try
                {
                    // Send the HTTP request asynchronously and get the response.
                    var response = await WebDriverUtilities.HttpClient.SendAsync(request);

                    // Ensure that the HTTP response is successful (status code 200-299).
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a JSON string.
                    var json = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON string to a VersionModel object.
                    return JsonSerializer.Deserialize<VersionModel>(json, s_options);
                }
                catch (Exception e)
                {
                    // Throw the base exception if an error occurs during the request.
                    throw e.GetBaseException();
                }
                finally
                {
                    // Dispose of the HTTP request to release resources.
                    request.Dispose();
                }
            }

            // Retrieve the version information from the endpoint.
            var version = await GetVersion(endpoint);

            // Return the WebSocket debugger URL from the retrieved version information.
            return version.WebSocketDebuggerUrl;
        }

        // Resolves the debugger address for the current Chromium-based driver instance.
        private static string ResolveDebuggerAddress(this IWebDriver driver)
        {
            // Retrieve the capabilities dictionary from the driver.
            var capabilities = ((WebDriverBase)driver).Capabilities.AlwaysMatch;

            // Attempt to resolve the debugger address directly from the capabilities.
            var debuggerAddress = ResolveDebuggerAddress(capabilities);

            // If a debugger address is found, return it.
            if (!string.IsNullOrEmpty(debuggerAddress))
            {
                return debuggerAddress;
            }

            // Extract the command-line arguments from the capabilities.
            var arguments = GetArguments(capabilities);

            // Attempt to resolve the debugger address and port from the command-line arguments.
            var (address, port) = ResolveDebuggerAddress(arguments);

            // If no port is found, return an empty string.
            if (string.IsNullOrEmpty(port))
            {
                return string.Empty;
            }

            // Return the debugger address in the format "address:port".
            return $"{address}:{port}";
        }

        // Resolves the debugger address from the provided WebDriver capabilities dictionary.
        private static string ResolveDebuggerAddress(IDictionary<string, object> capabilities)
        {
            // Find the first key in the capabilities dictionary that matches the pattern for options (e.g., ":chromeOptions").
            var optionsKey = capabilities
                .Keys
                .FirstOrDefault(i => Regex.IsMatch(input: i, pattern: "(?<=:\\w+)Options$"));

            // Check if an options key was found.
            var isOptions = !string.IsNullOrEmpty(optionsKey);

            // If an options key is found, cast the associated value to a dictionary; otherwise, use an empty dictionary.
            var options = isOptions
                ? (Dictionary<string, object>)capabilities[optionsKey]
                : [];

            // Check if the "debuggerAddress" key exists in the options dictionary and retrieve its value if present.
            var isDebuggerAddress = options.TryGetValue("debuggerAddress", out object debuggerAddress);

            // Return the debugger address if found; otherwise, return an empty string.
            return isDebuggerAddress
                ? $"{debuggerAddress}"
                : string.Empty;
        }

        // Resolves the debugger address and port from a list of command-line arguments.
        private static (string Address, string Port) ResolveDebuggerAddress(IEnumerable<string> arguments)
        {
            // Regex pattern to capture the value after '='.
            const string Pattern = @"(?<==)\w+";

            // If the arguments collection is null or empty, return empty strings for both address and port.
            if (arguments?.Any() != true)
            {
                return (string.Empty, string.Empty);
            }

            // Default address if not specified.
            const string DefaultAddress = "--remote-debugging-address=localhost";

            // Find the argument for the remote debugging address; use the default if not found.
            var debuggingAddressArg = arguments.FirstOrDefault(i => i.StartsWith("--remote-debugging-address", StringComparison.OrdinalIgnoreCase)) ?? DefaultAddress;

            // Find the argument for the remote debugging port; use an empty string if not found.
            var debuggingPortArg = arguments.FirstOrDefault(i => i.StartsWith("--remote-debugging-port", StringComparison.OrdinalIgnoreCase)) ?? string.Empty;

            // Extract the value for the debugging address using regex.
            var debuggingAddress = Regex.Match(input: debuggingAddressArg, Pattern)?.Value;

            // Extract the value for the debugging port using regex.
            var debuggingPort = Regex.Match(input: debuggingPortArg, Pattern)?.Value;

            // Return the resolved debugger address and port.
            return (debuggingAddress, debuggingPort);
        }

        // Extracts the list of arguments from the provided capabilities dictionary.
        private static IEnumerable<string> GetArguments(IDictionary<string, object> capabilities)
        {
            // Find the first key in the capabilities dictionary that matches the pattern for options (e.g., ":chromeOptions").
            var optionsKey = capabilities
                .Keys
                .FirstOrDefault(i => Regex.IsMatch(input: i, pattern: "(?<=:\\w+)Options$"));

            // Check if an options key was found.
            var isOptions = !string.IsNullOrEmpty(optionsKey);

            // If an options key is found, cast the associated value to a dictionary; otherwise, use an empty dictionary.
            var options = isOptions
                ? (Dictionary<string, object>)capabilities[optionsKey]
                : [];

            // Check if the "args" key exists in the options dictionary and retrieve its value if present.
            var isArguments = options.TryGetValue("args", out object arguments);

            // Return the list of arguments if found; otherwise, return an empty collection.
            return isArguments
                ? (IEnumerable<string>)arguments
                : [];
        }
    }
}
