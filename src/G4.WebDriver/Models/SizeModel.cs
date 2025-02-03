namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the dimensions of a web element.
    /// </summary>
    /// <param name="height">The height of the web element's bounding rectangle in CSS pixels.</param>
    /// <param name="width">The width of the web element's bounding rectangle in CSS pixels.</param>
    public class SizeModel(double height, double width)
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="SizeModel"/> class with default dimensions.
        /// </summary>
        public SizeModel()
            : this(height: 0, width: 0)
        { }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the height of the web element's bounding rectangle in CSS pixels.
        /// </summary>
        public double Height { get; set; } = height;

        /// <summary>
        /// Gets or sets the width of the web element's bounding rectangle in CSS pixels.
        /// </summary>
        public double Width { get; set; } = width;
        #endregion
    }
}
