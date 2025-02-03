/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Chromium/ChromiumAndroidOptions
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Internal/AndroidOptions.cs
 */
using System;

namespace G4.WebDriver.Remote.Chromium
{
    /// <summary>
    /// Represents options specific to Chromium-based browsers on Android devices.
    /// </summary>
    public class ChromiumAndroidOptions
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumAndroidOptions"/> class with the specified Android package.
        /// </summary>
        /// <param name="androidPackage">The package name of the Android application to be launched.</param>
        /// <exception cref="ArgumentException">Thrown when the provided <paramref name="androidPackage"/> is <c>null</c> or empty.</exception>
        public ChromiumAndroidOptions(string androidPackage)
        {
            // Check if the androidPackage is null or empty and throw an exception if it is.
            if (string.IsNullOrEmpty(androidPackage))
            {
                const string error400 = "Android package name cannot be null or empty.";
                throw new ArgumentException(error400, nameof(androidPackage));
            }

            // Set the AndroidPackage property to the provided value.
            AndroidPackage = androidPackage;
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the activity name of the Android application to be launched.
        /// </summary>
        public string AndroidActivity { get; set; }

        /// <summary>
        /// Gets or sets the serial number of the Android device to be used.
        /// </summary>
        public string AndroidDeviceSerial { get; set; }

        /// <summary>
        /// Gets or sets the package name of the Android application.
        /// </summary>
        public string AndroidPackage { get; set; }

        /// <summary>
        /// Gets or sets the process name of the Android application to be started.
        /// </summary>
        public string AndroidProcess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the running application on the Android device.
        /// </summary>
        public bool UseRunningApp { get; set; }
        #endregion
    }
}
