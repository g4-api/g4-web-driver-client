namespace G4.WebDriver.Models
{
    /// <summary>
    /// The dimensions and coordinates of the given web element.
    /// </summary>
    public class RectModel
    {
        /// <summary>
        /// Height of the web element's bounding rectangle in CSS pixels.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// Width of the web element's bounding rectangle in CSS pixels.
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// X axis position of the top-left corner of the web element
        /// relative to the current browsing context's document element in CSS pixels.
        /// </summary>
        public double? X { get; set; }

        /// <summary>
        /// Y axis position of the top-left corner of the web element
        /// relative to the current browsing context's document element in CSS pixels.
        /// </summary>
        public double? Y { get; set; }
    }
}
