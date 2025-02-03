namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the coordinates of a web element on the page, specifically the position of the top-left corner.
    /// </summary>
    /// <param name="x">The X-coordinate of the top-left corner of the element, in CSS pixels.</param>
    /// <param name="y">The Y-coordinate of the top-left corner of the element, in CSS pixels.</param>
    public class PointModel(int x, int y)
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="PointModel"/> class with default coordinates (0, 0).
        /// </summary>
        public PointModel()
            : this(x: 0, y: 0)
        { }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the X-coordinate of the top-left corner of the element, relative to
        /// the current browsing context's document element, in CSS pixels.
        /// </summary>
        public int X { get; set; } = x;

        /// <summary>
        /// Gets or sets the Y-coordinate of the top-left corner of the element, relative to
        /// the current browsing context's document element, in CSS pixels.
        /// </summary>
        public int Y { get; set; } = y;
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Moves the point by the specified offsets in the X and Y directions.
        /// </summary>
        /// <param name="x">The amount to move the point along the X-axis.</param>
        /// <param name="y">The amount to move the point along the Y-axis.</param>
        /// <returns>Returns the updated <see cref="PointModel"/> instance.</returns>
        public PointModel MoveBy(int x, int y)
        {
            // Adjust the X-coordinate by the specified offset.
            X += x;

            // Adjust the Y-coordinate by the specified offset.
            Y += y;

            // Return the updated instance for method chaining or further use.
            return this;
        }
        #endregion
    }
}
