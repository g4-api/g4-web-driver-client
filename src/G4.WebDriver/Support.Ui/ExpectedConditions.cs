using G4.WebDriver.Exceptions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace G4.WebDriver.Support.Ui
{
    /// <summary>
    /// Provides a collection of common expected conditions used in WebDriver wait operations.
    /// </summary>
    public static class ExpectedConditions
    {
        /// <summary>
        /// An expectation for checking that an alert is present on the page.
        /// </summary>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IUserPrompt"/>
        /// if an alert is present, or null if no alert is present.
        /// </returns>
        public static Func<IWebDriver, IUserPrompt> AlertIsPresent() => (driver) =>
        {
            try
            {
                // Attempt to switch to the alert if it is present.
                return driver.SwitchTo().Alert();
            }
            catch (Exception e) when (e is NoSuchAlertException)
            {
                // If no alert is present, return null.
                return null;
            }
        };

        /// <summary>
        /// An expectation for checking that an element's attribute matches a specified regular expression pattern.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the element.</param>
        /// <param name="attribute">The attribute of the element to match against the regular expression pattern.</param>
        /// <param name="regex">The regular expression pattern to match against the attribute value.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IWebElement"/>
        /// if the attribute matches the regex pattern, or null if it does not match.
        /// </returns>
        public static Func<IWebDriver, IWebElement> AttributeMatches(By by, string attribute, [StringSyntax(StringSyntaxAttribute.Regex)]string regex)
            => (driver) =>
            {
                // Find the element using the specified locator.
                var element = driver.FindElement(by);

                // Check if the element's attribute value matches the specified regular expression pattern.
                if (Regex.IsMatch(element.GetAttribute(attribute), regex))
                {
                    return element; // Return the element if it matches the pattern.
                }

                // Return null if the attribute does not match the pattern.
                return null;
            };

        /// <summary>
        /// An expectation for checking that an element is present on the DOM of a page.
        /// This does not necessarily mean that the element is visible.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the element.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IWebElement"/>
        /// if the element is found. Throws <see cref="NoSuchElementException"/> if the element is not present.
        /// </returns>
        public static Func<IWebDriver, IWebElement> ElementExists(By by) => (driver) =>
        {
            // Use the driver to find the element using the specified locator.
            return driver.FindElement(by);
        };

        /// <summary>
        /// An expectation for checking that a specified element is visible and selected.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the element.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IWebElement"/>
        /// if the element is visible and selected, or null if the element is not selected or a 
        /// <see cref="StaleElementReferenceException"/> is encountered.
        /// </returns>
        public static Func<IWebDriver, IWebElement> ElementIsSelected(By by) => (driver) =>
        {
            try
            {
                // Attempt to find the element and check if it is visible.
                var element = ElementIfVisible(driver.FindElement(by));

                // Return the element if it is selected; otherwise, return null.
                return element.Selected ? element : null;
            }
            catch (Exception e) when (e is StaleElementReferenceException)
            {
                // If a StaleElementReferenceException occurs, return null.
                return null;
            }
        };

        /// <summary>
        /// An expectation for checking that a specified element is visible on the page.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the element.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IWebElement"/>
        /// if the element is visible, or null if the element is not visible or a <see cref="StaleElementReferenceException"/> is encountered.
        /// </returns>
        public static Func<IWebDriver, IWebElement> ElementIsVisible(By by) => (driver) =>
        {
            try
            {
                // Find the element using the provided locator and check if it is visible using the ElementIfVisible method.
                return ElementIfVisible(driver.FindElement(by));
            }
            catch (Exception e) when (e is StaleElementReferenceException)
            {
                // Return null if a StaleElementReferenceException occurs, indicating the element is no longer attached to the DOM.
                return null;
            }
        };

        /// <summary>
        /// An expectation for checking that an element is visible and enabled such that you can click it.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the element.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IWebElement"/>
        /// if the element is visible and clickable (enabled), or null if it is not.
        /// </returns>
        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By by) => (driver) =>
        {
            try
            {
                // Attempt to find the element and check if it is visible.
                var element = ElementIfVisible(driver.FindElement(by));

                // If the element is not visible, return null.
                if (element == null)
                {
                    return null;
                }

                // Return the element if it is enabled (clickable); otherwise, return null.
                return (element.Enabled) ? element : null;
            }
            catch (Exception e) when (e is StaleElementReferenceException)
            {
                // If a StaleElementReferenceException occurs, return null.
                return null;
            }
        };

        /// <summary>
        /// An expectation for checking that all elements present on the web page that match the specified locator are enabled.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the elements.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IEnumerable{IWebElement}"/>
        /// if all elements are enabled, or null if any element is disabled or a <see cref="StaleElementReferenceException"/> is encountered.
        /// </returns>
        public static Func<IWebDriver, IEnumerable<IWebElement>> EnabilityOfAllElementsLocatedBy(By by) => (driver) =>
        {
            try
            {
                // Find all elements using the specified locator.
                var elements = driver.FindElements(by);

                // If any element is not enabled, return null.
                if (!elements.All(i => i.Enabled))
                {
                    return null;
                }

                // Return the list of enabled elements.
                return elements;
            }
            catch (Exception e) when (e is StaleElementReferenceException)
            {
                // If a StaleElementReferenceException occurs, return null.
                return null;
            }
        };

        /// <summary>
        /// An expectation for checking that a frame is available to switch to, and then switches to it.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the frame element.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns the <see cref="IWebDriver"/> 
        /// after switching to the specified frame, or null if the frame is not found or a <see cref="NoSuchFrameException"/> is encountered.
        /// </returns>
        public static Func<IWebDriver, IWebDriver> FrameToBeAvailableAndSwitchToIt(By by) => (driver) =>
        {
            try
            {
                // Attempt to find the frame element using the specified locator.
                var frameElement = driver.FindElement(by);

                // Switch to the found frame and return the WebDriver instance.
                return driver.SwitchTo().Frame(frameElement);
            }
            catch (Exception e) when (e is NoSuchFrameException)
            {
                // If the frame is not found, return null.
                return null;
            }
        };

        /// <summary>
        /// An expectation for checking that all elements present on the web page that match the specified locator are invisible.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the elements.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IEnumerable{IWebElement}"/>
        /// if all elements are invisible, or null if any element is visible or a <see cref="StaleElementReferenceException"/> is encountered.
        /// </returns>
        public static Func<IWebDriver, IEnumerable<IWebElement>> InvisibilityOfAllElementsLocatedBy(By by) => (driver) =>
        {
            try
            {
                // Find all elements using the specified locator.
                var elements = driver.FindElements(by);

                // If any element is visible, return null.
                if (elements.Any(i => i.Displayed))
                {
                    return null;
                }

                // Return the list of invisible elements.
                return elements;
            }
            catch (Exception e) when (e is StaleElementReferenceException)
            {
                // If a StaleElementReferenceException occurs, return null.
                return null;
            }
        };

        /// <summary>
        /// An expectation for checking that an element is either invisible or not present on the DOM.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the element.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns <c>true</c> if the element is invisible or not present,
        /// or <c>false</c> if the element is visible.
        /// </returns>
        public static Func<IWebDriver, bool> InvisibilityOfElementLocated(By by) => (driver) =>
        {
            try
            {
                // Attempt to find the element using the specified locator.
                var element = driver.FindElement(by);

                // Return true if the element is not visible.
                return !element.Displayed;
            }
            catch (Exception e) when (e is NoSuchElementException || e is StaleElementReferenceException)
            {
                // If a NoSuchElementException or StaleElementReferenceException occurs, return true.
                // This indicates that the element is not present or no longer attached to the DOM.
                return true;
            }
        };

        /// <summary>
        /// An expectation for checking that the web page has completely loaded.
        /// </summary>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns the <see cref="IWebDriver"/>
        /// if the page is fully loaded, or null if it is not yet loaded.
        /// </returns>
        public static Func<IWebDriver, IWebDriver> PageHasBeenLoaded() => (driver) =>
        {
            const StringComparison Compare = StringComparison.OrdinalIgnoreCase; // Define a case-insensitive comparison.

            // If the WebDriver is for APPIUM, return the driver since this check is for web page readiness.
            if (driver.GetType().Name.Contains("APPIUM", Compare))
            {
                return driver;
            }

            // Check if the document's ready state is 'complete' to ensure the page has fully loaded.
            var isComplete = driver.InvokeScript("return document.readyState") as string;
            if (isComplete.Equals("complete", Compare))
            {
                return driver; // Return the driver if the page is fully loaded.
            }

            // Return null if the page is not yet loaded.
            return null;
        };

        /// <summary>
        /// An expectation for checking that all elements present on the web page that match the specified locator are visible.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the elements.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IEnumerable{IWebElement}"/>
        /// if elements are found, or null if no elements are found or if a <see cref="StaleElementReferenceException"/> is encountered.
        /// </returns>
        public static Func<IWebDriver, IEnumerable<IWebElement>> PresenceOfAllElementsLocatedBy(By by) => (driver) =>
        {
            try
            {
                // Find all elements using the specified locator.
                var elements = driver.FindElements(by);

                // If no elements are found, return null.
                if (!elements.Any())
                {
                    return null;
                }

                // Return the list of found elements.
                return elements;
            }
            catch (Exception e) when (e is StaleElementReferenceException)
            {
                // If a StaleElementReferenceException occurs, return null.
                // This can happen if the elements are no longer attached to the DOM.
                return null;
            }
        };

        /// <summary>
        /// An expectation for checking that all elements present on the web page that match the specified locator are selected.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the elements.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IEnumerable{IWebElement}"/>
        /// if all elements are selected, or null if any element is not selected or a <see cref="StaleElementReferenceException"/> is encountered.
        /// </returns>
        public static Func<IWebDriver, IEnumerable<IWebElement>> SelectedOfAllElementsLocatedBy(By by) => (driver) =>
        {
            try
            {
                // Find all elements using the specified locator.
                var elements = driver.FindElements(by);

                // If any element is not selected, return null.
                if (!elements.All(i => i.Selected))
                {
                    return null;
                }

                // Return the list of selected elements.
                return elements;
            }
            catch (Exception e) when (e is StaleElementReferenceException)
            {
                // If a StaleElementReferenceException occurs, return null.
                return null;
            }
        };

        /// <summary>
        /// An expectation for checking that the text of an element matches a specified regular expression pattern.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the element.</param>
        /// <param name="regex">The regular expression pattern to match against the element's text.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IWebElement"/>
        /// if the text matches the regex pattern, or null if it does not match.
        /// </returns>
        public static Func<IWebDriver, IWebElement> TextMatches(By by, [StringSyntax(StringSyntaxAttribute.Regex)] string regex)
            => (driver) =>
            {
                // Find the element using the specified locator.
                var element = driver.FindElement(by);

                // Check if the element's text matches the specified regular expression pattern.
                if (Regex.IsMatch(element.Text, regex))
                {
                    return element; // Return the element if it matches the pattern.
                }

                // Return null if the text does not match the pattern.
                return null;
            };

        /// <summary>
        /// An expectation for checking that the current URL matches a specified regular expression pattern.
        /// </summary>
        /// <param name="regex">The regular expression pattern to match against the current URL.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns <c>true</c> if the current URL matches the regex pattern,
        /// or <c>false</c> otherwise.
        /// </returns>
        public static Func<IWebDriver, bool> UrlMatches([StringSyntax(StringSyntaxAttribute.Regex)]string regex)
            => (driver) =>
            {
                // Check if the current URL matches the specified regular expression pattern.
                return Regex.IsMatch(driver.Navigate().Url, regex, RegexOptions.IgnoreCase);
            };

        /// <summary>
        /// An expectation for checking that all elements present on the web page that match the specified locator are visible.
        /// </summary>
        /// <param name="by">The locating mechanism to use for finding the elements.</param>
        /// <returns>
        /// A function that takes an <see cref="IWebDriver"/> instance and returns an <see cref="IEnumerable{IWebElement}"/>
        /// if all elements are visible, or null if any element is not visible or a <see cref="StaleElementReferenceException"/> is encountered.
        /// </returns>
        public static Func<IWebDriver, IEnumerable<IWebElement>> VisibilityOfAllElementsLocatedBy(By by) => (driver) =>
        {
            try
            {
                // Find all elements using the specified locator.
                var elements = driver.FindElements(by);

                // If any element is not visible, return null.
                if (!elements.All(i => i.Displayed))
                {
                    return null;
                }

                // Return the list of visible elements.
                return elements;
            }
            catch (Exception e) when (e is StaleElementReferenceException)
            {
                // If a StaleElementReferenceException occurs, return null.
                return null;
            }
        };

        // Checks if the specified element is visible on the page.
        private static IWebElement ElementIfVisible(IWebElement element)
            => element.Displayed
                ? element // Return the element if it is visible.
                : null;   // Return null if the element is not visible.
    }
}
