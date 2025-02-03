/*
 * RESOURCES
 * https://www.w3.org/TR/webdriver/#screen-capture
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Screenshot.cs
 */
using System;
using System.IO;
using System.Text.Json.Serialization;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents a screenshot model with base64 and byte array representations.
    /// </summary>
    public class ScreenshotModel
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenshotModel"/> class.
        /// </summary>
        public ScreenshotModel()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenshotModel"/> class with base64 data.
        /// </summary>
        /// <param name="base64">The base64 representation of the screenshot.</param>
        public ScreenshotModel(string base64)
        {
            // Set the base64 property
            Base64 = base64;

            // Convert base64 to byte array and set the Bytes property
            Bytes = Convert.FromBase64String(base64);
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the base64 representation of the screenshot.
        /// </summary>
        public string Base64 { get; set; }

        /// <summary>
        /// Gets or sets the byte array representation of the screenshot.
        /// </summary>
        [JsonIgnore]
        public byte[] Bytes { get; set; }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Saves the screenshot with a timestamped file name.
        /// </summary>
        public void Save() => Save(path: $"Image-{DateTime.UtcNow:yyyyMMddhhmmssfff}.png");

        /// <summary>
        /// Saves the screenshot to the specified file path.
        /// </summary>
        /// <param name="path">The file path where the screenshot will be saved.</param>
        public void Save(string path)
        {
            // Ensure Bytes property is populated with the correct data
            Bytes = (Bytes?.Length == 0) && !string.IsNullOrEmpty(Base64)
                ? Convert.FromBase64String(Base64)
                : Bytes;

            // Trim and ensure the path has a .png extension
            path = path.Trim();
            path = path.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                ? path
                : $"{path}.png";

            // Write the screenshot bytes to the specified file path
            if (Bytes != null)
            {
                File.WriteAllBytes(path, Bytes);
            }
        }
        #endregion
    }
}
