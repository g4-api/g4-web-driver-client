namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the input model for specifying a mouse position.
    /// </summary>
    public class MousePositionInputModel
    {
        /// <summary>
        /// Gets or sets the alignment for the mouse position.
        /// </summary>
        /// <value>The alignment for the mouse position (e.g., TopLeft, MiddleCenter).</value>
        public string Alignment { get; set; } = "TopLeft";

        /// <summary>
        /// Gets or sets the offset from the X-coordinate.
        /// </summary>
        /// <value>The offset from the X-coordinate.</value>
        public int OffsetX { get; set; } = 1;

        /// <summary>
        /// Gets or sets the offset from the Y-coordinate.
        /// </summary>
        /// <value>The offset from the Y-coordinate.</value>
        public int OffsetY { get; set; } = 1;

        /// <summary>
        /// Gets or sets the X-coordinate of the mouse position.
        /// </summary>
        /// <value>The X-coordinate of the mouse position.</value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y-coordinate of the mouse position.
        /// </summary>
        /// <value>The Y-coordinate of the mouse position.</value>
        public int Y { get; set; }
    }
}
