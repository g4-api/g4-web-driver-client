using G4.WebDriver.Models;

namespace G4.WebDriver.Remote.Appium
{
    /// <summary>
    /// Represents an interface for interacting with geolocation in the context of Appium WebDriver.
    /// </summary>
    public interface IGeolocation
    {
        /// <summary>
        /// Gets the current geolocation.
        /// </summary>
        /// <returns>The current geolocation.</returns>
        GeolocationModel GetGeolocation();

        /// <summary>
        /// Sets the geolocation to the specified values.
        /// </summary>
        /// <param name="geolocationModel">The geolocation values to set.</param>
        void SetGeolocation(GeolocationModel geolocationModel);
    }
}
