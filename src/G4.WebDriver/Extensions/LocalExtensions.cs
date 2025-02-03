using G4.WebDriver.Exceptions;
using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace G4.WebDriver.Extensions
{
    /// <summary>
    /// Provides extension methods for various utility operations used in the G4 WebDriver.
    /// </summary>
    internal static class LocalExtensions
    {
        // Initailize JSON options for serialization and deserialization
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Adds a range of elements to the end of the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="collection">The list to which the elements will be added.</param>
        /// <param name="range">The collection of elements to add to the list.</param>
        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> range)
        {
            // Iterate through each element in the range and add it to the collection
            foreach (var item in range)
            {
                // Add the current item to the collection
                collection.Add(item);
            }
        }

        /// <summary>
        /// Asserts whether two objects are equal by comparing their serialized string representations.
        /// </summary>
        /// <param name="source">The source object to compare.</param>
        /// <param name="target">The target object to compare.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise, <c>false</c>.</returns>
        public static bool AssertAreEqual(this object source, object target)
        {
            return AssertAreEqual(source, target, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Asserts whether two objects are equal by comparing their serialized string representations using the specified comparison type.
        /// </summary>
        /// <param name="source">The source object to compare.</param>
        /// <param name="target">The target object to compare.</param>
        /// <param name="comparisonType">The type of string comparison to use.</param>
        /// <returns><c>true</c> if the objects are equal based on the specified comparison; otherwise, <c>false</c>.</returns>
        public static bool AssertAreEqual(this object source, object target, StringComparison comparisonType)
        {
            // Handle null cases explicitly to avoid unnecessary work.
            if (source == null && target == null)
            {
                return true;
            }
            if (source == null || target == null)
            {
                return false;
            }

            try
            {
                // Check if the objects are of the same type before serialization.
                if (source.GetType() != target.GetType())
                {
                    return false;
                }

                // Handle simple cases directly.
                switch (source)
                {
                    // Compare strings using the specified comparison type.
                    case string sourceString when target is string targetString:
                        return string.Equals(sourceString, targetString, comparisonType);

                    // Compare integers directly.
                    case int sourceInt when target is int targetInt:
                        return sourceInt == targetInt;

                    // Compare long integers directly.
                    case long sourceLong when target is long targetLong:
                        return sourceLong == targetLong;

                    // Compare boolean values directly.
                    case bool sourceBool when target is bool targetBool:
                        return sourceBool == targetBool;

                    // Compare bytes directly.
                    case byte sourceByte when target is byte targetByte:
                        return sourceByte == targetByte;

                    // Compare double values using epsilon to account for floating-point precision issues.
                    case double sourceDouble when target is double targetDouble:
                        return Math.Abs(sourceDouble - targetDouble) < double.Epsilon;

                    // Compare float values using epsilon to account for floating-point precision issues.
                    case float sourceFloat when target is float targetFloat:
                        return Math.Abs(sourceFloat - targetFloat) < float.Epsilon;

                    // Compare decimal values directly.
                    case decimal sourceDecimal when target is decimal targetDecimal:
                        return sourceDecimal == targetDecimal;

                    // Compare characters directly.
                    case char sourceChar when target is char targetChar:
                        return sourceChar == targetChar;

                    // Compare DateTime values directly.
                    case DateTime sourceDateTime when target is DateTime targetDateTime:
                        return sourceDateTime == targetDateTime;

                    // Compare GUIDs directly.
                    case Guid sourceGuid when target is Guid targetGuid:
                        return sourceGuid == targetGuid;
                }

                // Serialize the source and target objects to JSON strings.
                var serializedSource = JsonSerializer.Serialize(source);
                var serializedTarget = JsonSerializer.Serialize(target);

                // Order the characters in the serialized strings alphabetically.
                var orderedSource = serializedSource.Order();
                var orderedTarget = serializedTarget.Order();

                // Compare the concatenated ordered strings using the specified comparison type.
                return string.Concat(orderedSource).Equals(string.Concat(orderedTarget), comparisonType);
            }
            catch
            {
                // Return false if an exception occurs during comparison.
                return false;
            }
        }

        /// <summary>
        /// Asserts whether the input string is valid JSON.
        /// </summary>
        /// <param name="input">The input string to validate.</param>
        /// <returns><c>true</c> if the input string is valid JSON; otherwise, <c>false</c>.</returns>
        public static bool AssertJson(this string input)
        {
            // Process the input string
            input = input.Trim();

            // Set up conditions
            var isObj = input.StartsWith('{') && input.EndsWith('}');
            var isArr = input.StartsWith('[') && input.EndsWith(']');

            // Parse
            if (isObj || isArr)
            {
                try
                {
                    JsonDocument.Parse(input);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the specified argument is null or its default value.
        /// </summary>
        /// <param name="argument">The object to check.</param>
        /// <returns>True if the argument is null or the default value; otherwise, false.</returns>
        public static bool AssertNullOrDefault(this object argument)
        {
            // Check if the argument is null
            if (argument == null)
            {
                return true;
            }

            // Get the type of the argument
            var argumentType = argument.GetType();

            // Check if the argument is a value type and if it is equal to its default value
            if (argumentType.IsValueType)
            {
                return Activator.CreateInstance(argumentType).Equals(argument);
            }

            // If not null and not default, return false
            return false;
        }

        /// <summary>
        /// Checks if the specified argument is null or its default value, considering a specified expected type.
        /// </summary>
        /// <param name="argument">The object to check.</param>
        /// <param name="expectedType">The expected type to compare against.</param>
        /// <returns>True if the argument is null or the default value of the expected type; otherwise, false.</returns>
        public static bool AssertNullOrDefault(this object argument, Type expectedType)
        {
            // Check if the argument is null
            if (argument == null)
            {
                return true;
            }

            // If the expected type is nullable, return false because we are not interested
            // in default null values for nullable types
            if (Nullable.GetUnderlyingType(expectedType) != null)
            {
                return false;
            }

            // Get the type of the argument
            var argumentType = argument.GetType();

            // Check if the argument is a value type and if it is equal to its default value
            if (argumentType.IsValueType && argumentType != expectedType)
            {
                return Activator.CreateInstance(argumentType).Equals(argument);
            }

            // If not null and not default, return false
            return false;
        }

        /// <summary>
        /// Checks if the specified argument of a generic type is null or its default value.
        /// </summary>
        /// <typeparam name="T">The type of the argument to check.</typeparam>
        /// <param name="argument">The object to check.</param>
        /// <returns>True if the argument is null or the default value; otherwise, false.</returns>
        public static bool AssertNullOrDefault<T>(this T argument)
        {
            // Check if the argument is equal to the default value of its type
            if (Equals(argument, default(T)))
            {
                return true;
            }

            // Get the method type
            var methodType = typeof(T);

            // If the type is nullable, return false because we are not interested
            // in default null values for nullable types
            if (Nullable.GetUnderlyingType(methodType) != null)
            {
                return false;
            }

            // Get the type of the argument
            var argumentType = argument.GetType();

            // Check if the argument is a value type and if it is equal to its default value
            if (argumentType.IsValueType && argumentType != methodType)
            {
                return Activator.CreateInstance(argumentType).Equals(argument);
            }

            // If not null and not default, return false
            return false;
        }

        /// <summary>
        /// Converts a JsonElement to a dictionary representation.
        /// </summary>
        /// <param name="element">The JsonElement to convert.</param>
        /// <returns>A dictionary representing the JsonElement.</returns>
        public static IDictionary<string, object> ConvertToDictionary(this JsonElement element)
        {
            // Get the raw JSON text
            var json = element.GetRawText();

            // Deserialize the JSON into a dictionary
            return JsonSerializer.Deserialize<IDictionary<string, object>>(json);
        }

        /// <summary>
        /// Converts an object to JSON using the default naming policy.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public static string ConvertToJson(this object obj)
        {
            return ConvertToJson(obj, namingPolicy: JsonNamingPolicy.CamelCase);
        }

        /// <summary>
        /// Converts an object to JSON using the specified naming policy.
        /// </summary>
        /// <param name="namingPolicy">The policy used to convert a property's name to another format.</param>
        /// <returns>A string that represents the current object.</returns>
        [SuppressMessage(
            category: "Performance",
            checkId: "CA1869:Cache and reuse 'JsonSerializerOptions' instances",
            Justification = "Creating new settings with a different convention is necessary to prevent unintentional mutation and maintain consistency.")]
        public static string ConvertToJson(this object obj, JsonNamingPolicy namingPolicy)
        {
            // Set default naming policy to CamelCase if not provided
            namingPolicy ??= JsonNamingPolicy.CamelCase;

            // Configure JSON serialization options
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = namingPolicy,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };

            // Serialize the object to JSON using the specified options
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Gets the number of seconds since the Unix epoch for a given DateTime object.
        /// </summary>
        /// <param name="dateTime">The DateTime object.</param>
        /// <returns>The number of seconds since the Unix epoch.</returns>
        public static long GetEpochSeconds(this DateTime dateTime)
        {
            // Set up the zero date
            var zeroDate = DateTime.UnixEpoch;

            // Calculate the time span
            var span = dateTime.ToUniversalTime().Subtract(zeroDate);

            // Return the number of seconds
            return Convert.ToInt64(span.TotalSeconds);
        }

        /// <summary>
        /// Gets the number of seconds since the Unix epoch for a given nullable DateTime object.
        /// </summary>
        /// <param name="dateTime">The nullable DateTime object.</param>
        /// <returns>The number of seconds since the Unix epoch if the DateTime is not null; otherwise, 0.</returns>
        public static long GetEpochSeconds(this DateTime? dateTime)
        {
            // Set up the zero date
            var zeroDate = DateTime.UnixEpoch;

            // Calculate the time span
            var span = dateTime?.ToUniversalTime().Subtract(zeroDate);

            // Return the number of seconds
            return Convert.ToInt64(span?.TotalSeconds);
        }

        /// <summary>
        /// Merges multiple dictionaries into one dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionaries.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionaries.</typeparam>
        /// <param name="dictionary">The target dictionary where the merge results will be stored.</param>
        /// <param name="dictionaries">The dictionaries to merge into the target dictionary.</param>
        public static void Merge<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, params IDictionary<TKey, TValue>[] dictionaries)
        {
            // Loop through each dictionary to merge
            foreach (var _dictionary in dictionaries)
            {
                // Loop through keys in the current dictionary
                foreach (var key in _dictionary.Keys)
                {
                    // Merge key-value pair into the target dictionary
                    dictionary[key] = _dictionary[key];
                }
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="WebDriverErrorModel"/> from the given <see cref="WebDriverResponseModel"/>.
        /// </summary>
        /// <param name="responseModel">The response model containing the value from which the error model will be created.</param>
        /// <returns>A new instance of <see cref="WebDriverErrorModel"/> if the response contains a valid error object; otherwise, returns the default value.</returns>
        public static WebDriverErrorModel NewErrorModel(this WebDriverResponseModel responseModel)
        {
            // Ensure the value is of type JsonElement.
            if (responseModel.Value is not JsonElement value)
            {
                return default;
            }

            // Ensure the JsonElement is an object.
            if (value.ValueKind != JsonValueKind.Object)
            {
                return default;
            }

            // Try to extract the "error", "message", and "stacktrace" properties.
            var isError = value.TryGetProperty("error", out JsonElement error);
            var isMessage = value.TryGetProperty("message", out JsonElement message);
            var isStackTrace = value.TryGetProperty("stacktrace", out JsonElement stackTrace);

            // If "error" is not present, return the default WebDriverErrorModel.
            if (!isError)
            {
                return default;
            }

            // Construct and return a new WebDriverErrorModel.
            return new WebDriverErrorModel
            {
                Error = error.GetString(),
                Message = isMessage ? message.GetString() : string.Empty,
                StackTrace = isStackTrace ? stackTrace.GetString() : string.Empty,
                StatusCode = (int)responseModel.WebDriverResponse.StatusCode
            };
        }

        /// <summary>
        /// Reads the contents of a Stream and returns them as a string.
        /// </summary>
        /// <param name="stream">The Stream to read.</param>
        /// <returns>The contents of the Stream as a string.</returns>
        public static string ReadAsString(this Stream stream)
        {
            // Set up StreamReader
            using StreamReader reader = new(stream);

            // Read the Stream and return the contents as a string
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Resolves the value of a JsonElement based on its value kind.
        /// </summary>
        /// <param name="jsonElement">The JsonElement to resolve.</param>
        /// <returns>The resolved value of the JsonElement.</returns>
        public static object ResolveType(this JsonElement jsonElement) => jsonElement.ValueKind switch
        {
            // If the JsonElement is a string, return the string.
            JsonValueKind.String => jsonElement.GetString(),

            // If the JsonElement is a number, try to parse it as a long.
            // If parsing as a long is successful, return the long value; otherwise, return it as a double.
            JsonValueKind.Number => jsonElement.TryGetInt64(out long longOut)
                ? longOut
                : jsonElement.GetDouble(),

            // If the JsonElement is a boolean (true), return the boolean value.
            JsonValueKind.True => jsonElement.GetBoolean(),

            // If the JsonElement is a boolean (false), return the boolean value.
            JsonValueKind.False => jsonElement.GetBoolean(),

            // If the JsonElement is an array, return the JsonElement itself for further processing.
            JsonValueKind.Array => jsonElement,

            // If the JsonElement is an object, return the JsonElement itself for further processing.
            JsonValueKind.Object => jsonElement,

            // If the JsonElement is undefined, return null.
            JsonValueKind.Undefined => null,

            // If the JsonElement is null, return null.
            JsonValueKind.Null => null,

            // Default case: return the JsonElement itself.
            _ => jsonElement
        };

        /// <summary>
        /// Sends the WebDriver command using the specified base URL with a default timeout of 1 minute and keepAlive as false.
        /// </summary>
        /// <param name="model">The WebDriver command model to send.</param>
        /// <param name="baseUrl">The base URL to send the command to.</param>
        /// <returns>The HTTP response message from the server.</returns>
        public static HttpResponseMessage Send(this WebDriverCommandModel model, string baseUrl)
        {
            // Call the overloaded Send method with default timeout and keepAlive values
            return Send(
                model,
                httpClient: WebDriverUtilities.HttpClient,
                baseUrl,
                timeout: TimeSpan.FromMinutes(1),
                keepAlive: false);
        }

        /// <summary>
        /// Sends the WebDriver command using the specified base URL with a specified timeout and keepAlive as false.
        /// </summary>
        /// <param name="model">The WebDriver command model to send.</param>
        /// <param name="baseUrl">The base URL to send the command to.</param>
        /// <param name="timeout">The timeout for the HTTP request.</param>
        /// <returns>The HTTP response message from the server.</returns>
        public static HttpResponseMessage Send(this WebDriverCommandModel model, string baseUrl, TimeSpan timeout)
        {
            // Call the overloaded Send method with the specified timeout and default keepAlive value
            return Send(model, httpClient: WebDriverUtilities.HttpClient, baseUrl, timeout, keepAlive: false);
        }

        /// <summary>
        /// Sends the WebDriver command using the specified base URL with a specified timeout and keepAlive value.
        /// </summary>
        /// <param name="model">The WebDriver command model to send.</param>
        /// <param name="baseUrl">The base URL to send the command to.</param>
        /// <param name="timeout">The timeout for the HTTP request.</param>
        /// <param name="keepAlive">Indicates whether to keep the connection alive.</param>
        /// <returns>The HTTP response message from the server.</returns>
        public static HttpResponseMessage Send(
            this WebDriverCommandModel model, string baseUrl, TimeSpan timeout, bool keepAlive)
        {
            // Call the overloaded Send method with the specified timeout and keepAlive values
            return Send(model, httpClient: WebDriverUtilities.HttpClient, baseUrl, timeout, keepAlive);
        }

        /// <summary>
        /// Sends an HTTP request based on the WebDriver command model.
        /// </summary>
        /// <param name="model">The WebDriver command model containing the command details.</param>
        /// <param name="httpClient">The HTTP client used to send the request. If null, a default client is used.</param>
        /// <param name="baseUrl">The base URL for the WebDriver server.</param>
        /// <param name="timeout">The timeout for the HTTP request.</param>
        /// <param name="keepAlive">Indicates whether to keep the connection alive after the request.</param>
        /// <returns>The HTTP response message.</returns>
        /// <exception cref="WebDriverConnectionException">Thrown when there is a connection issue.</exception>
        /// <exception cref="WebDriverTimeoutException">Thrown when the request times out.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the request is unauthorized.</exception>
        public static HttpResponseMessage Send(
            this WebDriverCommandModel model,
            HttpClient httpClient,
            string baseUrl,
            TimeSpan timeout,
            bool keepAlive)
        {
            // Use default HTTP client if none is provided
            httpClient ??= WebDriverUtilities.HttpClient;

            // Trim trailing '/' from the base URL if present
            baseUrl = baseUrl.Trim().EndsWith('/') ? baseUrl[0..^1] : baseUrl;

            // Create the request body JSON, or an empty object if no data is provided
            var requestBody = model.Data == null
                ? "{}"
                : JsonSerializer.Serialize(model.Data, s_jsonOptions);

            // Determine the request content based on the HTTP method and presence of data
            var content = model.Data == null && model.Method != HttpMethod.Post
                ? null
                : new StringContent(requestBody, Encoding.UTF8, model.ContentType);

            // Construct the endpoint URL
            var endpoint = baseUrl + model.Route;

            // Create the HTTP request message
            var request = new HttpRequestMessage(model.Method, endpoint)
            {
                Content = content
            };

            // Parse the base URL into a URI object
            var uri = new Uri(baseUrl);

            // If the URI contains user credentials, add them to the Authorization header
            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                var credentialsBuffer = Encoding.UTF8.GetBytes(uri.UserInfo);
                var credentialsBase64 = Convert.ToBase64String(credentialsBuffer);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentialsBase64);
            }

            // Add connection headers to keep the connection alive or close it
            request.Headers.Add("Connection", keepAlive ? "keep-alive" : "close");
            request.Headers.Add("Keep-Alive", "timeout=600");

            try
            {
                // Send the HTTP request and wait for the response
                using var cancellationTokenSource = new CancellationTokenSource(timeout);
                var response = httpClient
                    .SendAsync(request, cancellationTokenSource.Token)
                    .GetAwaiter()
                    .GetResult();

                // Handle Unauthorized response (HTTP 401)
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var error401 = response.Content.ReadAsStringAsync().Result;
                    throw new UnauthorizedAccessException(error401);
                }

                // Return the response for other status codes
                return response;
            }
            catch (HttpRequestException e)
            {
                // Handle general HTTP request exceptions
                throw new WebDriverConnectionException(e.Message, e);
            }
            catch (TaskCanceledException)
            {
                // Handle task cancellation due to request timeout
                var error408 = "Invocation of WebDriver command failed. " +
                    $"Command: '{model.Route}', " +
                    $"Method: '{model.Method}', " +
                    $"Session: '{model.Session}', " +
                    $"{(string.IsNullOrEmpty(model.Element) ? "" : $"Element: '{model.Element}', ")}" +
                    "Reason: Request Timeout.";
                throw new WebDriverTimeoutException(error408, timeout);
            }
            catch (UnauthorizedAccessException e)
            {
                // Handle unauthorized access exception
                var error401 = "Permission denied while trying to perform the action. " +
                    $"Action: '{model.Route}', " +
                    $"Method: '{model.Method}', " +
                    $"Session: '{model.Session}', " +
                    $"{(string.IsNullOrEmpty(model.Element) ? "" : $"Element: '{model.Element}', ")}" +
                    $"Reason: Unauthorized access - {e.Message}";
                throw new UnauthorizedAccessException(error401);
            }
        }

        /// <summary>
        /// Sets the data for the <see cref="WebDriverCommandModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="WebDriverCommandModel"/> instance to set the data for.</param>
        /// <param name="data">The data to set.</param>
        /// <returns>The modified <see cref="WebDriverCommandModel"/> instance with the new data set.</returns>
        public static WebDriverCommandModel SetData(this WebDriverCommandModel model, object data)
        {
            // Set the data property of the model.
            model.Data = data;

            // Return the modified model to allow for fluent chaining.
            return model;
        }

        /// <summary>
        /// Sets the element for the <see cref="WebDriverCommandModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="WebDriverCommandModel"/> instance to set the element for.</param>
        /// <param name="element">The element identifier to set.</param>
        /// <returns>The modified <see cref="WebDriverCommandModel"/> instance with the new element set.</returns>
        public static WebDriverCommandModel SetElement(this WebDriverCommandModel model, string element)
        {
            // Set the element property of the model.
            model.Element = element;

            // Return the modified model to allow for fluent chaining.
            return model;
        }

        /// <summary>
        /// Sets the HTTP method for the <see cref="WebDriverCommandModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="WebDriverCommandModel"/> instance to set the HTTP method for.</param>
        /// <param name="method">The <see cref="HttpMethod"/> to set.</param>
        /// <returns>The modified <see cref="WebDriverCommandModel"/> instance with the new HTTP method set.</returns>
        public static WebDriverCommandModel SetMethod(this WebDriverCommandModel model, HttpMethod method)
        {
            // Set the HTTP method property of the model.
            model.Method = method;

            // Return the modified model to allow for fluent chaining.
            return model;
        }

        /// <summary>
        /// Sets the route for the <see cref="WebDriverCommandModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="WebDriverCommandModel"/> instance to set the route for.</param>
        /// <param name="route">The route to set.</param>
        /// <returns>The modified <see cref="WebDriverCommandModel"/> instance with the new route set.</returns>
        public static WebDriverCommandModel SetRoute(this WebDriverCommandModel model, string route)
        {
            // Set the route property of the model.
            model.Route = route;

            // Return the modified model to allow for fluent chaining.
            return model;
        }

        /// <summary>
        /// Sets the session identifier for the <see cref="WebDriverCommandModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="WebDriverCommandModel"/> instance to set the session identifier for.</param>
        /// <param name="session">The session identifier to set.</param>
        /// <returns>The modified <see cref="WebDriverCommandModel"/> instance with the new session identifier set.</returns>
        public static WebDriverCommandModel SetSession(this WebDriverCommandModel model, string session)
        {
            // Set the session property of the model.
            model.Session = session;

            // Return the modified model to allow for fluent chaining.
            return model;
        }

        /// <summary>
        /// Attempts to get the value associated with the specified key in a case-insensitive manner.
        /// If the dictionary is already using a case-insensitive comparer, it utilizes the standard TryGetValue.
        /// Otherwise, it performs a case-insensitive search.
        /// </summary>
        /// <typeparam name="TValue">The type of the value associated with the key.</typeparam>
        /// <param name="data">The dictionary to search.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public static bool TryGetValueCaseInsensitive<TValue>(this IDictionary<string, TValue> data, string key, out TValue value)
        {
            // Attempt to get the value using the standard TryGetValue
            if (data.TryGetValue(key, out value))
            {
                return true;
            }

            // Determine if the dictionary's comparer is case-insensitive
            bool isCaseInsensitive = false;

            // Check if the dictionary uses a case-insensitive comparer
            if (data is Dictionary<string, TValue> concreteDict)
            {
                // Check if the dictionary uses a case-insensitive comparer
                isCaseInsensitive = concreteDict.Comparer.Equals(StringComparer.OrdinalIgnoreCase) ||
                    concreteDict.Comparer.Equals(StringComparer.InvariantCultureIgnoreCase) ||
                    concreteDict.Comparer.Equals(StringComparer.CurrentCultureIgnoreCase);
            }
            else if (data is IReadOnlyDictionary<string, TValue> readOnlyDict)
            {
                // Attempt to access the comparer via reflection for IReadOnlyDictionary implementations
                var comparerProperty = typeof(IReadOnlyDictionary<string, TValue>).GetProperty("Comparer");

                // Check if the comparer is case-insensitive for the IReadOnlyDictionary implementation
                if (comparerProperty?.GetValue(readOnlyDict) is IEqualityComparer<string> comparer)
                {
                    isCaseInsensitive = comparer.Equals(StringComparer.OrdinalIgnoreCase) ||
                        comparer.Equals(StringComparer.InvariantCultureIgnoreCase) ||
                        comparer.Equals(StringComparer.CurrentCultureIgnoreCase);
                }
            }

            if (isCaseInsensitive)
            {
                // If the comparer is case-insensitive and key wasn't found, return false
                value = default;
                return false;
            }

            // Perform a case-insensitive search manually
            foreach (var (k, v) in data.Select(i => (i.Key, i.Value)))
            {
                if (string.Equals(k, key, StringComparison.OrdinalIgnoreCase))
                {
                    value = v;
                    return true;
                }
            }

            // If no match is found, set value to default and return false
            value = default;
            return false;
        }
    }
}
