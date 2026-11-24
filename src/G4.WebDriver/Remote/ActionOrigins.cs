namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides the named origins an input action measures its coordinates from.
    /// </summary>
    /// <remarks>
    /// An action can also use an element reference as its origin; that origin is an object, not a name, so it
    /// is not listed here.
    /// </remarks>
    public static class ActionOrigins
    {
        /// <summary>
        /// Represents the current position of the pointer, so the coordinates are a relative move.
        /// </summary>
        /// <remarks>Valid for pointer actions only; a wheel action does not accept it.</remarks>
        public const string Pointer = "pointer";

        /// <summary>
        /// Represents the top-left corner of the viewport, so the coordinates are an absolute position.
        /// </summary>
        public const string Viewport = "viewport";
    }
}
