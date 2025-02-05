using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace G4.WebDriver.Remote.Uia
{
    public class UiaElement(IWebDriver driver, string id) : WebElement(driver, id), IUser32Element
    {
        // Initialize the static HttpClient instance for sending requests to the WebDriver server
        private static HttpClient HttpClient => new();

        // Initialize the static JsonSerializerOptions instance for deserializing JSON responses
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        /// <inheritdoc />
        new public string GetAttribute(string name)
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(this);

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(Driver);

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

        /// <inheritdoc />
        public void MoveToElement()
        {
            // Call the overload with a default MousePositionInputModel.
            MoveToElement(new MousePositionInputModel());
        }

        /// <inheritdoc />
        public void MoveToElement(MousePositionInputModel positionData)
        {
            // Retrieve the session ID and element ID from the web element for routing.
            var (sessionId, elementId) = GetRouteData(this);

            // Get the URI of the remote server from the command executor.
            var url = GetRemoteServerUri(Driver);

            // Construct the request URI for the native mouse move command.
            var requestUri = $"{url}/session/{sessionId}/user32/element/{elementId}/mouse/move";

            // Serialize the mouse position input model to JSON.
            var jsonData = JsonSerializer.Serialize(positionData, JsonSerializerOptions);

            // Create the HTTP content using the serialized JSON, specifying the content type.
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            // Construct the HTTP POST request with the target URI and content.
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = content
            };

            // Send the HTTP request to move the mouse.
            var response = HttpClient.Send(request);

            // Ensure that the HTTP response indicates a successful request.
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public void SendClick()
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(this);

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(Driver);

            // Construct the route for the native click command
            var requestUri = $"{url}/session/{sessionId}/user32/element/{elementId}/click";

            // Create the HTTP request for the native click
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public void SendDoubleClick()
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(this);

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(Driver);

            // Construct the route for the native double-click command
            var requestUri = $"{url}/session/{sessionId}/user32/element/{elementId}/dclick";

            // Create the HTTP request for the native double-click
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            // Send the HTTP request
            var response = HttpClient.Send(request);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public void SetFocus()
        {
            // Get the session ID and element ID from the web element
            var (sessionId, elementId) = GetRouteData(this);

            // Get the remote server URI from the command executor
            var url = GetRemoteServerUri(Driver);

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
            // Get the WebDriver instance from the web element
            var driver = element.Driver;

            // Get the session ID from the WebDriver
            var sessionId = $"{((IWebDriverSession)driver).Session}";

            // Return the session ID and element ID as a tuple
            return (sessionId, element.Id);
        }
    }
}
