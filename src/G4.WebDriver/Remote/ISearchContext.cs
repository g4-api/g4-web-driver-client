/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/ISearchContext.cs
 */
using G4.WebDriver.Models;

using System.Collections.Generic;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Defines the interface used to search for elements.
    /// </summary>
    public interface ISearchContext
    {
        /// <summary>
        /// Finds the first matching element within the current context using the specified <see cref="By"/> locator.
        /// </summary>
        /// <param name="by">The <see cref="By"/> locator to use for finding the element.</param>
        /// <returns>The first <see cref="IWebElement"/> found using the specified locator.</returns>
        IWebElement FindElement(By by);

        /// <summary>
        /// Finds all matching elements within the current context using the specified <see cref="By"/> locator.
        /// </summary>
        /// <param name="by">The <see cref="By"/> locator to use for finding the elements.</param>
        /// <returns>An <see cref="IEnumerable{IWebElement}"/> containing all elements that match the specified locator.</returns>
        IEnumerable<IWebElement> FindElements(By by);
    }
}
