using G4.WebDriver.Remote;

using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

#pragma warning disable S3011 // Necessary to interact with internal fields for sending native actions.
namespace G4.WebDriver.Extensions
{
    /// <summary>
    /// Provides extension methods for UI automation using WebDriver.
    /// </summary>
    public static class UiaExtensions
    {
        // Initialize the static HttpClient instance for sending requests to the WebDriver server
        private static HttpClient HttpClient => new();

        // Initialize the static JsonSerializerOptions instance for deserializing JSON responses
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Gets the UI Automation attribute value of a web element.
        /// </summary>
        /// <param name="element">The web element to get the attribute from.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <returns>The attribute value as a string.</returns>
        public static string GetUiaAttribute(this IWebElement element, string name)
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(element);

            // Get the WebDriver instance from the web element
            var driver = element.Driver;

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the attribute command
            var requestUri = $"{url}/session/{sessionId}/element/{elementId}/attribute/{name}";

            // Create the HTTP request for the attribute
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Read the response content and deserialize the JSON response
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent, JsonSerializerOptions);

            // Try to get the value from the response object
            var isValue = responseObject.TryGetValue("value", out var value);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Return the value as a string if it exists, otherwise return an empty string
            return isValue ? $"{value}" : string.Empty;
        }

        /// <summary>
        /// Sends a native click command to a web element.
        /// </summary>
        /// <param name="element">The web element to click.</param>
        public static void SendNativeClick(this IWebElement element)
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(element);

            // Get the WebDriver instance from the web element
            var driver = element.Driver;

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the native click command
            var requestUri = $"{url}/session/{sessionId}/user32/element/{elementId}/click";

            // Create the HTTP request for the native click
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sends a native double-click command to a web element.
        /// </summary>
        /// <param name="element">The web element to double-click.</param>
        public static void SendNativeDoubleClick(this IWebElement element)
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(element);

            // Get the WebDriver instance from the web element
            var driver = element.Driver;

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the native double-click command
            var requestUri = $"{url}/session/{sessionId}/user32/element/{elementId}/dclick";

            // Create the HTTP request for the native double-click
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sets focus on a web element.
        /// </summary>
        /// <param name="element">The web element to set focus on.</param>
        public static void SetFocus(this IWebElement element)
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(element);

            // Get the WebDriver instance from the web element
            var driver = element.Driver;

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(driver);

            // Construct the route for the native focus command
            var requestUri = $"{url}/session/{sessionId}/user32/element/{elementId}/focus";

            // Create the HTTP request for the native focus
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();
        }

        // Gets the remote server URI from the WebDriver command executor.
        private static string GetRemoteServerUri(IWebDriver driver)
        {
            // Get the remote server URI from the command executor using reflection
            return driver.GetServerAddress().AbsoluteUri.Trim('/');
        }

        // Gets the session ID and element ID from a web element.
        private static (string SessionId, string ElementId) GetRouteData(IWebElement element)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            // Get the WebDriver instance from the web element
            var driver = element.Driver;

            // Get the session ID from the WebDriver
            var sessionId = $"{((IWebDriverSession)driver).Session}";

            // Return the session ID and element ID as a tuple
            return (sessionId, element.Id);
        }
    }
}