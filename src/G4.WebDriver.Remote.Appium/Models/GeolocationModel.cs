namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the geolocation coordinates including latitude, longitude, and altitude.
    /// </summary>
    public class GeolocationModel
    {
        /// <summary>
        /// Gets or sets the altitude of the geolocation in meters above sea level.
        /// </summary>
        public double Altitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of the geolocation in decimal degrees.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of the geolocation in decimal degrees.
        /// </summary>
        public double Longitude { get; set; }
    }
}
