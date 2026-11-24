namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides the pointer types a pointer input source can declare.
    /// </summary>
    /// <remarks>
    /// The type is fixed when the source is created and tells the driver which device the pointer actions
    /// imitate, which in turn decides the events the application receives (mouse, pen or touch events).
    /// </remarks>
    public static class PointerTypes
    {
        /// <summary>
        /// Represents a mouse pointer, the default pointer type.
        /// </summary>
        public const string Mouse = "mouse";

        /// <summary>
        /// Represents a pen (stylus) pointer.
        /// </summary>
        public const string Pen = "pen";

        /// <summary>
        /// Represents a touch pointer, one finger on a touch screen.
        /// </summary>
        public const string Touch = "touch";
    }
}
