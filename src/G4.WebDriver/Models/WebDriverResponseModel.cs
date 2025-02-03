using G4.WebDriver.Extensions;

using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents a response from the WebDriver, encapsulating the session, value, and status code.
    /// </summary>
    public class WebDriverResponseModel
    {
        #region *** Fields     ***
        // Options for JSON serialization, enabling case-insensitive property names.
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };
        #endregion

        #region *** Properties ***
        /// <summary>
        /// Gets or sets the session ID associated with the WebDriver response.
        /// </summary>
        public string Session { get; set; }

        /// <summary>
        /// Gets or sets the value returned by the WebDriver, typically the result of a command execution.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the original HTTP response from the WebDriver. This property is ignored during JSON serialization.
        /// </summary>
        [JsonIgnore]
        public HttpResponseMessage WebDriverResponse { get; set; }

        /// <summary>
        /// Gets or sets the status code of the WebDriver response. If the status code is not provided, it defaults to -1.
        /// </summary>
        public int WebDriverStatusCode { get; set; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Creates a new instance of the specified type from the WebDriver response value.
        /// </summary>
        /// <typeparam name="T">The type to which the value should be deserialized.</typeparam>
        /// <returns>A new instance of the specified type, populated with data from the response value.</returns>
        public T New<T>()
        {
            // Serialize the value object to JSON
            var json = JsonSerializer.Serialize(Value);

            // Deserialize the JSON back to the specified type
            return JsonSerializer.Deserialize<T>(json, s_jsonOptions);
        }

        /// <summary>
        /// Creates a new <see cref="WebDriverResponseModel"/> from the given HTTP response message.
        /// </summary>
        /// <param name="response">The HTTP response message received from the WebDriver.</param>
        /// <returns>A new instance of <see cref="WebDriverResponseModel"/> populated with data from the response.</returns>
        public static WebDriverResponseModel New(HttpResponseMessage response)
        {
            // Read the response content as a string
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            // Check if the content is valid JSON
            if (!content.AssertJson())
            {
                return new WebDriverResponseModel();
            }

            // Parse the JSON document
            var jsonDocument = JsonDocument.Parse(content);
            var isValue = jsonDocument.RootElement.TryGetProperty("value", out JsonElement value);

            // Determine the nature of the value element (null, undefined, object, array)
            var isNull = value.ValueKind == JsonValueKind.Null;
            var isUndefined = value.ValueKind == JsonValueKind.Undefined;
            var isObject = value.ValueKind == JsonValueKind.Object;
            var isArray = value.ValueKind == JsonValueKind.Array;

            // If no valid value is found, return a response model with minimal information
            if (!isValue || isNull || isUndefined)
            {
                return new WebDriverResponseModel
                {
                    WebDriverResponse = response,
                    WebDriverStatusCode = (int)response.StatusCode
                };
            }

            // Initialize the session element
            JsonElement session = default;

            // If the value is an object, attempt to extract the session ID
            if (isObject)
            {
                // Attempt to retrieve the session ID from the value object
                _ = value.TryGetProperty("sessionId", out session);
            }

            // Create and return a new WebDriverResponseModel with the extracted data
            return new WebDriverResponseModel
            {
                Session = session.ValueKind == JsonValueKind.Undefined ? string.Empty : session.GetString(),
                Value = isObject || isArray ? JsonDocument.Parse($"{value}").RootElement : value,
                WebDriverStatusCode = (int)response.StatusCode,
                WebDriverResponse = response
            };
        }
        #endregion
    }
}
