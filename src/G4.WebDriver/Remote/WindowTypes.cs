/*
 * RESOURCES
 * https://www.w3.org/TR/webdriver/#contexts
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/ITargetLocator.cs
 */
namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents the type of a new browser window that may be created
    /// </summary>
    public static class WindowTypes
    {
        /// <summary>
        /// Create a new browser window using a new tab.
        /// </summary>
        public const string Tab = "tab";

        /// <summary>
        /// Create a new browser window using a new top-level window.
        /// </summary>
        public const string Window = "window";
    }
}
