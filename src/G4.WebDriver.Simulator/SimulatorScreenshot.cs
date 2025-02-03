using G4.WebDriver.Models;
using G4.WebDriver.Remote;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Represents a simulator for capturing screenshots.
    /// </summary>
    public class SimulatorScreenshot : ITakesScreenshot
    {
        #region *** Fields       ***
        // A base64-encoded image data (simulated screenshot)
        const string Base64 =
            "iVBORw0KGgoAAAANSUhEUgAAAEMAAAAVCAYAAAAdHVOZAAAC+0lEQVRYhe2YMUjjUBjHf5Xb" +
            "5S1BuwRuCdihix1cAh3sZKSgm1gU2vEmdellzHXR4HBjKwcet1UoxqV1KGS5IV063EGXgywt" +
            "dAk46NgbbGNtWpMDRTn7hywv33vf9/75/v+XJHZ7ezuwLIter8d7xeLiIuvr68TOzs4GiUQC" +
            "RVFeu6ZXQ6/Xo16vs3Bzc/OuiQBYXl5maWmJhdcu5K0gFovNyRjHP5DhYAqB8C8TZ/x2y/TH" +
            "nGOBOHZmLwVAn+q+IHfRD83cv8ghxLTY+zWEEJit6DuZhWhkdKvkRIZ2pYPneXieR6fSJjNJ" +
            "yEsiq8GVzSM6ujaXtedLEYkM50cBq9jgfEvyx6StE8pZg69TnmzqyMM7Sj1flQArSZK1S+zu" +
            "2FjXxSrq6M+UIgIZDnYJ9PTk5iS2v3mPCPJnTMhk1OZCCMR+lanCaJkh7a6iFi0ufz7MdpoG" +
            "eloNhnar5Hw556h2gyHTEE5G16WNhhyPtmAALRMln6QxlFdjpYAy6SctE7EODc/jYHX2Uqm0" +
            "juVLxcEu6aiBeAczUSB5fZ/Pu05SSEST84ufJk7TgKLKqK8CEro6HBJxQKiwVlX0mosL0LIx" +
            "sjLyZEzLxsiW2R2RtLpLOWtgRzDYcDLiMkks3Iit9hh93N+gfQyU7MOqgTbDe4KQkYcbc5oG" +
            "2obKpEj7bhtqBRRfJgqFGrTd8PUjdEYKtQhGM9ho4UeohLwC1h93ZoRWOeH8SxnyhxG0LaFu" +
            "aBhNE7uksbkW9CtJTkK2TGcoy9E1zdsmEUkmqZ0yWinz+JxvmWRKGuWdp5s7ldahZPua7V/k" +
            "giYa3+akAoXPM8x1DNLaJlrJwMhuok7zsVUVvVbg+0gWQzON8h7yITzkvthzT8YUCiI/GtRp" +
            "eOcRdH5Ap5JDEWJs3jbSxLalrU/o+QyHG+rTTzGuspkFpkjkHikOfpXJJQSjjFql86QxjxA7" +
            "PT0d7O3thUf+56jX6/Nvk3HMyRjDnIwxLAwGg9eu4U3g7u6O2Pwf6MM/0L+IuiyvFlZ+IwAA" +
            "AABJRU5ErkJggg==";
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Captures a simulated screenshot.
        /// </summary>
        /// <returns>A <see cref="ScreenshotModel"/> containing the simulated screenshot in Base64 format.</returns>
        public ScreenshotModel GetScreenshot() => new(Base64);
        #endregion
    }
}
