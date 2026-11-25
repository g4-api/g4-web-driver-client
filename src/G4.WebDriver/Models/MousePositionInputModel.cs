using System;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the input model for specifying a mouse position.
    /// </summary>
    /// <remarks>
    /// The alignment picks a point of the element's bounding rectangle, and the offsets move that point. An offset the
    /// caller does not set defaults by alignment: one pixel for an edge or corner alignment, so the default top-left
    /// point lands inside the element, and zero for <c>MiddleCenter</c>, so a center point lands exactly on the center.
    /// The UIA driver applies the offsets to every alignment.
    /// </remarks>
    public class MousePositionInputModel
    {
        #region *** Constants    ***
        // The alignment that names the element center, which needs no nudge to land inside the element.
        private const string CenterAlignment = "MiddleCenter";

        // The offset the center alignment gets when the caller sets none, so the point stays on the exact center.
        private const int CenterDefaultOffset = 0;

        // The offset an edge or corner alignment gets when the caller sets none, one pixel inside the element.
        private const int EdgeDefaultOffset = 1;
        #endregion

        #region *** Fields       ***
        // The horizontal offset the caller set, or null while it is left to the alignment's default.
        private int? _offsetX;

        // The vertical offset the caller set, or null while it is left to the alignment's default.
        private int? _offsetY;
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the alignment for the mouse position.
        /// </summary>
        /// <value>The alignment for the mouse position (e.g., TopLeft, MiddleCenter).</value>
        public string Alignment { get; set; } = "TopLeft";

        /// <summary>
        /// Gets or sets the offset from the X-coordinate.
        /// </summary>
        /// <value>
        /// The offset the caller set; otherwise 0 for <c>MiddleCenter</c> and 1 for every other alignment.
        /// </value>
        public int OffsetX
        {
            get => _offsetX ?? GetDefaultOffset(Alignment);
            set => _offsetX = value;
        }

        /// <summary>
        /// Gets or sets the offset from the Y-coordinate.
        /// </summary>
        /// <value>
        /// The offset the caller set; otherwise 0 for <c>MiddleCenter</c> and 1 for every other alignment.
        /// </value>
        public int OffsetY
        {
            get => _offsetY ?? GetDefaultOffset(Alignment);
            set => _offsetY = value;
        }

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
        #endregion

        #region *** Methods      ***
        // Gets the offset an alignment gets when the caller sets none. The center needs no nudge; every edge or
        // corner alignment is nudged one pixel inside the element. The comparison ignores case, as the driver does.
        private static int GetDefaultOffset(string alignment)
        {
            var isCenter = string.Equals(alignment, CenterAlignment, StringComparison.OrdinalIgnoreCase);
            return isCenter ? CenterDefaultOffset : EdgeDefaultOffset;
        }
        #endregion
    }
}
