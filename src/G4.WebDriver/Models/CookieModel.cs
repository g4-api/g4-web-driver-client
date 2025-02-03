/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Cookie.cs
 */
using G4.WebDriver.Extensions;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents a cookie in the browser.
    /// </summary>
    /// <param name="expirationDate">The cookie expiration date. If not provided, defaults to 1 day.</param>
    public class CookieModel(DateTime? expirationDate)
    {
        #region *** Fields       ***
        // Define serialization options for converting cookies to and from JSON.
        private static readonly JsonSerializerOptions s_options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Creates a new instance of the CookieModel class.
        /// </summary>
        public CookieModel()
            : this(DateTime.UtcNow.AddDays(1))
        { }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the domain the cookie is visible to.
        /// Defaults to the current browsing context's active document's URL domain if omitted when adding a cookie.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Domain { get; set; }

        /// <summary>
        /// When the cookie expires, specified in seconds since Unix Epoch.
        /// Must not be set if omitted when adding a cookie.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Expiry { get; set; } = expirationDate?.GetEpochSeconds();

        /// <summary>
        /// Gets or sets whether the cookie is an HTTP only cookie. Defaults to false if omitted when adding a cookie.
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>
        /// Gets or sets the name of the cookie.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the cookie path. Defaults to "/" if omitted when adding a cookie.
        /// </summary>
        public string Path { get; set; } = "/";

        /// <summary>
        /// Gets or sets whether the cookie applies to a SameSite policy.
        /// Defaults to None if omitted when adding a cookie. Can be set to either Lax or Strict.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SameSite { get; set; }

        /// <summary>
        /// Gets or sets whether the cookie is a secure cookie. Defaults to false if omitted when adding a cookie.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// Gets or sets the cookie value.
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Converts a dictionary containing raw cookie data into a CookieModel instance.
        /// </summary>
        /// <param name="rawCookie">The dictionary containing raw cookie data.</param>
        /// <returns>Returns a CookieModel instance populated with the dictionary data.</returns>
        public static CookieModel ConvertFromDictionary(IDictionary<string, object> rawCookie)
        {
            // Serialize the dictionary to JSON using the specified serialization options.
            var json = JsonSerializer.Serialize(rawCookie, s_options);

            // Deserialize the JSON into a CookieModel instance and return it.
            return JsonSerializer.Deserialize<CookieModel>(json);
        }
        #endregion
    }
}
