using G4.WebDriver.Models;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents an interface for objects that can capture screenshots.
    /// </summary>
    public interface ITakesScreenshot
    {
        /// <summary>
        /// Captures a screenshot of the current WebElement.
        /// This method invokes the GetElementScreenshot WebDriver command for the specific element and processes the response.
        /// </summary>
        /// <returns>A <see cref="ScreenshotModel"/> containing the screenshot in Base64 format and its raw byte representation.</returns>
        ScreenshotModel GetScreenshot();
    }
}
