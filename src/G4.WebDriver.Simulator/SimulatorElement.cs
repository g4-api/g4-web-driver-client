using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Uia;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Represents a simulated WebElement for testing or no browser purposes.
    /// </summary>
    public class SimulatorElement : IWebElement, ILocatable, IUser32Element
    {
        #region *** Fields       ***
        private static readonly Random s_random = new();
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorElement"/> class with default values.
        /// </summary>
        /// <param name="parent">The parent SimulatorDriver.</param>
        public SimulatorElement(SimulatorDriver parent)
            : this(parent, tagName: "div")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorElement"/> class with the specified tag name.
        /// </summary>
        /// <param name="parent">The parent SimulatorDriver.</param>
        /// <param name="tagName">The HTML tag name.</param>
        public SimulatorElement(SimulatorDriver parent, string tagName)
            : this(parent, tagName, text: "Mock: Positive Element 20")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorElement"/> class with the specified tag name and text.
        /// </summary>
        /// <param name="parent">The parent SimulatorDriver.</param>
        /// <param name="tagName">The HTML tag name.</param>
        /// <param name="text">The text content of the element.</param>
        public SimulatorElement(SimulatorDriver parent, string tagName, string text)
            : this(parent, tagName, text, enabled: true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorElement"/> class with the specified properties.
        /// </summary>
        /// <param name="parent">The parent SimulatorDriver.</param>
        /// <param name="tagName">The HTML tag name.</param>
        /// <param name="text">The text content of the element.</param>
        /// <param name="enabled">Indicates whether the element is enabled.</param>
        public SimulatorElement(SimulatorDriver parent, string tagName, string text, bool enabled)
            : this(parent, tagName, text, enabled, selected: true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorElement"/> class with the specified properties.
        /// </summary>
        /// <param name="parent">The parent SimulatorDriver.</param>
        /// <param name="tagName">The HTML tag name.</param>
        /// <param name="text">The text content of the element.</param>
        /// <param name="enabled">Indicates whether the element is enabled.</param>
        /// <param name="selected">Indicates whether the element is selected.</param>
        public SimulatorElement(SimulatorDriver parent, string tagName, string text, bool enabled, bool selected)
            : this(parent, tagName, text, enabled, selected, displayed: true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorElement"/> class with the specified properties.
        /// </summary>
        /// <param name="parent">The parent SimulatorDriver.</param>
        /// <param name="tagName">The HTML tag name.</param>
        /// <param name="text">The text content of the element.</param>
        /// <param name="enabled">Indicates whether the element is enabled.</param>
        /// <param name="selected">Indicates whether the element is selected.</param>
        /// <param name="displayed">Indicates whether the element is displayed.</param>
        public SimulatorElement(SimulatorDriver parent, string tagName, string text, bool enabled, bool selected, bool displayed)
        {
            // Set properties of the simulated element
            Driver = parent;
            TagName = tagName;
            Text = text;
            Enabled = enabled;
            Selected = selected;
            Displayed = displayed;
            Location = new PointModel(1, 2);
            Size = new SizeModel(10, 11);
            Rect = new RectModel
            {
                Height = 10,
                Width = 11,
                X = 1,
                Y = 2
            };
            Attributes = new Dictionary<string, string>()
            {
                [SimulatorLocators.Index] = "0",
                [SimulatorLocators.Null] = null,
                ["text"] = "Mock Text 20",
                ["href"] = "http://m.from-href.io/"
            };
            Id = $"simulator-{Guid.NewGuid()}";
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the collection of the SimulatorElement attributes.
        /// </summary>
        public IDictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Provides location of the element using various frames of reference.
        /// </summary>
        public ICoordinates Coordinates { get; } = new SimulatorCoordinatesModel();

        /// <summary>
        /// Gets a value indicating whether or not this element is displayed.
        /// </summary>
        public bool Displayed { get; }

        /// <summary>
        /// Defines the interface through which the user controls the browser.
        /// </summary>
        public IWebDriver Driver { get; }

        /// <inheritdoc />
        public bool Enabled { get; }

        /// <summary>
        /// Gets the underline ID of an element or session as returned by the driver.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this element is state is invalid.
        /// </summary>
        public bool IsInvalidState { get; set; }

        /// <summary>
        /// Gets a Point object containing the coordinates of the upper-left
        /// corner of this element relative to the upper-left corner of the page.
        /// </summary>
        public PointModel Location { get; }

        /// <summary>
        /// Gets the location of an element on the screen, scrolling it into view
        /// if it is not currently on the screen.
        /// </summary>
        public PointModel LocationOnView { get; } = new(1, 1);

        /// <inheritdoc />
        public RectModel Rect { get; }

        /// <inheritdoc />
        public bool Selected { get; }

        /// <summary>
        /// Gets a Size object containing the height and width
        /// of this element.
        /// </summary>
        public SizeModel Size { get; }

        /// <inheritdoc />
        public string TagName { get; }

        /// <inheritdoc />
        public string Text { get; }
        #endregion

        #region *** Methods      ***
        /// <inheritdoc />
        public IWebElement Clear()
        {
            // Confirm that the action on the element is allowed
            ConfirmActionOnElement(element: this);

            // Clear the text content by setting the "value" attribute to an empty string
            Attributes["value"] = string.Empty;

            // Return the current WebElement after the text content has been cleared
            return this;
        }

        /// <inheritdoc />
        public void Click()
        {
            // Confirm that the action on the element is allowed
            ConfirmActionOnElement(element: this);
        }

        /// <inheritdoc />
        public IWebElement FindElement(By by)
        {
            // Use the GetElement method to find the first element based on the specified criteria
            return GetElement((SimulatorDriver)Driver, by);
        }

        /// <inheritdoc />
        public IEnumerable<IWebElement> FindElements(By by)
        {
            try
            {
                // Attempt to get the elements based on the specified criteria
                return GetElements(parent: (SimulatorDriver)Driver, by);
            }
            catch (Exception e) when (e is NoSuchElementException)
            {
                // If no elements are found, return an empty collection
                return new ReadOnlyCollection<IWebElement>([]);
            }
        }

        /// <inheritdoc />
        public IEnumerable<IWebElement> FindElementsByLinkText(string linkText)
        {
            // Use the FindElements method with the By.LinkText criteria to find elements with the specified link text
            return FindElements(By.LinkText(linkText));
        }

        /// <inheritdoc />
        public string GetAttribute(string attributeName)
        {
            // Check if the attribute exists in the stored attributes
            if (Attributes.TryGetValue(attributeName, out string value))
            {
                return value;
            }

            // Handle special case for the "Exception" attribute
            if (attributeName.Equals(SimulatorLocators.Exception))
            {
                // Throw a WebDriverException for the "Exception" attribute
                throw new WebDriverException();
            }

            // Return a mock attribute value if the attribute is not found
            return attributeName.Equals(nameof(value), StringComparison.OrdinalIgnoreCase)
                ? value
                : $"mock attribute value {new Random().Next(0, 1000)}";
        }

        /// <inheritdoc />
        public string GetCssValue(string propertyName)
        {
            // Return a mock CSS value for demonstration purposes
            return "Mock CSS Value";
        }

        /// <inheritdoc />
        public string GetProperty(string propertyName)
        {
            // Return a mock JavaScript property value for demonstration purposes
            return "mockJavaScriptProperty";
        }

        /// <inheritdoc />
        public void MoveToElement()
        {
            // No actual action is performed.
        }

        /// <inheritdoc />
        public void MoveToElement(MousePositionInputModel positionData)
        {
            // No actual action is performed.
        }

        /// <inheritdoc />
        public void SendClick()
        {
            // No actual action is performed.
        }

        /// <inheritdoc />
        public void SendDoubleClick()
        {
            // No actual action is performed.
        }

        /// <inheritdoc />
        public void SendKeys(string text)
        {
            ConfirmActionOnElement(element: this);
            Attributes["value"] = text;
        }

        /// <inheritdoc />
        public void SetFocus()
        {
            // No actual action is performed.
        }

        /// <summary>
        /// Submits the WebElement (mock method - does not perform any action).
        /// </summary>
        [SuppressMessage(
            category: "Performance",
            checkId: "CA1822:Mark members as static",
            Justification = "This method is designed to be an instance method.")]
        public void Submit()
        {
            // This is a mock method and should not perform any action.
            // Submit functionality is not applicable for the simulated WebElement.
            // If needed, implement specific behavior based on the requirements.
        }

        /// <summary>
        /// Captures a screenshot of the current WebElement.
        /// </summary>
        /// <returns>A <see cref="ScreenshotModel"/> containing the screenshot in Base64 format and its raw byte representation.</returns>
        public ScreenshotModel GetScreenshot() => new SimulatorScreenshot().GetScreenshot();

        // Confirms whether an action can be performed on the WebElement.
        private static void ConfirmActionOnElement(SimulatorElement element)
        {
            // Check if the WebElement is not displayed
            if (!element.Displayed)
            {
                // Throw an exception indicating that the element is not interactable
                throw new ElementNotInteractableException();
            }

            // Check if the WebElement is in an invalid state
            if (element.IsInvalidState)
            {
                // Throw an exception indicating that the element is in an invalid state
                throw new InvalidElementStateException();
            }
        }

        // Gets the first element matching the specified By criteria.
        internal static SimulatorElement GetElement(SimulatorDriver parent, By by)
        {
            // Use the Get method to find the first element based on the specified criteria
            return GetElement(by, parent);
        }

        // Gets a collection of elements matching the specified By criteria.
        internal static ReadOnlyCollection<SimulatorElement> GetElements(SimulatorDriver parent, By by)
        {
            // Create a list to store the found elements
            var elements = new List<SimulatorElement>
            {
                // Get the first element based on the specified criteria
                GetElement(by, parent),

                // Get the second element based on the specified criteria (example - adjust based on requirements)
                GetElement(by, parent)
            };

            // Create a ReadOnlyCollection from the list, excluding null elements
            return new ReadOnlyCollection<SimulatorElement>(elements.Where(i => i != null).ToList());
        }

        // Gets a SimulatorElement based on the specified By criteria and parent SimulatorDriver.
        private static SimulatorElement GetElement(By by, SimulatorDriver parent)
        {
            // Convert By value to lowercase for case-insensitive comparison
            var byValue = by.Value.ToLower();

            // Define reflection flags
            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic;

            // Get the method in SimulatorElement class with a description matching the By value
            var method = typeof(SimulatorElement).GetMethodByDescription(byValue, flags);

            // Check if the method is found
            var isMethod = method != default;

            // Check if the By value is whitelisted
            var isWhiteList = !isMethod && parent.LocatorsWhiteList.Any(i => Regex.IsMatch(byValue, i));

            // Return a new SimulatorElement if the By value is whitelisted
            if (isWhiteList)
            {
                return new SimulatorElement(parent);
            }

            // Throw NoSuchElementException if neither a method nor a whitelisted locator is found
            if (!isMethod)
            {
                throw new NoSuchElementException();
            }

            try
            {
                // Invoke the method to create a SimulatorElement based on the By criteria and parent
                return method.GetParameters().Length == 1
                    ? (SimulatorElement)method.Invoke(null, [parent])
                    : (SimulatorElement)method.Invoke(null, null);
            }
            catch (Exception e)
            {
                // Throw the base exception if an error occurs during method invocation
                throw e.GetBaseException();
            }
        }

        // Gets a SimulatorElement representing a file input element using reflection.
        [Description(SimulatorLocators.File)]
        private static SimulatorElement GetFile(SimulatorDriver parent)
        {
            // Create a SimulatorElement representing a file input element
            var fileElement = new SimulatorElement(
                parent, tagName: "INPUT", text: string.Empty, enabled: true, selected: false, displayed: true);

            // Set the type attribute to "file" to indicate it is a file input element
            fileElement.Attributes["type"] = "file";

            return fileElement;
        }

        // Gets a SimulatorElement representing a positive element using reflection.
        [Description(SimulatorLocators.Positive)]
        private static SimulatorElement GetPositive(SimulatorDriver parent) => Positive(parent);

        // Gets a SimulatorElement representing a negative element using reflection.
        [Description(SimulatorLocators.Negative)]
        private static SimulatorElement GetNegative(SimulatorDriver parent) => Negative(parent);

        // Gets a null SimulatorElement using reflection.
        [Description(SimulatorLocators.Null)]
        private static SimulatorElement GetNull() => null;

        // Gets a SimulatorElement representing a stale element using reflection.
        [Description(SimulatorLocators.Stale)]
        private static SimulatorElement GetStale()
            => throw new StaleElementReferenceException("Attempted to access a stale element reference");

        // Gets a SimulatorElement representing a scenario with no matching element using reflection.
        [Description(SimulatorLocators.None)]
        private static SimulatorElement GetNoSuchElement()
            => throw new NoSuchElementException("No matching element found");

        // Gets a SimulatorElement representing a scenario where a WebDriver exception occurs using reflection.
        [Description(SimulatorLocators.Exception)]
        private static SimulatorElement GetException()
            => throw new WebDriverException("An unexpected WebDriver exception occurred");

        // Gets a SimulatorElement representing a positive element with a random selection using reflection.
        [Description(SimulatorLocators.RandomPositive)]
        private static SimulatorElement GetRandomPositive(SimulatorDriver parent) => RandomElement(parent, 90);

        // Gets a SimulatorElement representing a negative element with a random selection using reflection.
        [Description(SimulatorLocators.RandomNegative)]
        private static SimulatorElement GetRandomNegative(SimulatorDriver parent) => RandomElement(parent, 10);

        // Gets a SimulatorElement representing an existing element with a random selection using reflection.
        [Description(SimulatorLocators.RandomExists)]
        private static SimulatorElement GetRandomExists(SimulatorDriver parent)
        {
            return RandomElements(parent, 90).FirstOrDefault();
        }

        // Gets a SimulatorElement representing a non-existing element with a random selection using reflection.
        [Description(SimulatorLocators.RandomNotExists)]
        private static SimulatorElement GetRandomNotExists(SimulatorDriver parent)
        {
            return RandomElements(parent, 1).FirstOrDefault();
        }

        // Gets a SimulatorElement representing a null element with a random selection using reflection.
        [Description(SimulatorLocators.RandomNull)]
        private static SimulatorElement GetRandomNull(SimulatorDriver parent)
        {
            return RandomElement(parent, 95, () => (SimulatorElement)parent.FindElement(By.Custom.Null()));
        }

        // Gets a SimulatorElement representing a scenario where no matching element is found with a random selection using reflection.
        [Description(SimulatorLocators.RandomNoSuchElement)]
        private static SimulatorElement GetRandomNoSuchElement(SimulatorDriver parent)
        {
            return RandomElement(parent, positiveRatio: 95, () => (SimulatorElement)parent.FindElement(By.Custom.None()));
        }

        // Gets a SimulatorElement representing a scenario where a stale element is encountered with a random selection using reflection.
        [Description(SimulatorLocators.RandomStale)]
        private static SimulatorElement GetRandomStale(SimulatorDriver parent)
        {
            return RandomElement(parent, positiveRatio: 95, ()
                => (SimulatorElement)parent.FindElement(By.Custom.Stale()));
        }

        // Gets a SimulatorElement representing a scenario where an element is focused using reflection.
        [Description(SimulatorLocators.Focused)]
        private static SimulatorElement GetFocused(SimulatorDriver parent) => Positive(parent);

        // Gets a SimulatorElement representing a scenario where an element is in an invalid state using reflection.
        [Description(SimulatorLocators.InvalidElementState)]
        private static SimulatorElement GetInvalidState(SimulatorDriver parent)
        {
            // Create a positive SimulatorElement using the Positive method
            var element = Positive(parent);

            // Set the IsInvalidState property to true to simulate an element in an invalid state
            element.IsInvalidState = true;

            // Return the created SimulatorElement
            return element;
        }

        // Gets a SimulatorElement representing a select element using reflection.
        [Description(SimulatorLocators.SelectElement)]
        private static SimulatorElement GetSelectElement(SimulatorDriver parent)
        {
            // Create a new SimulatorElement representing a select element
            return new SimulatorElement(parent, tagName: "select");
        }

        // Creates a SimulatorElement representing options for a SimulatorDriver.
        [Description(SimulatorLocators.Option)]
        private static SimulatorElement GetOptions(SimulatorDriver parent)
        {
            // Create a new SimulatorElement representing an option
            var option = new SimulatorElement(parent, tagName: "option");

            // Set the value attribute of the option
            option.Attributes["value"] = "SimulatorValue";

            // Return the created option element
            return option;
        }

        // Gets a SimulatorElement representing an input element using reflection.
        [Description(SimulatorLocators.Input)]
        private static SimulatorElement GetInput(SimulatorDriver parent)
        {
            // Create a new SimulatorElement representing an input element
            return new SimulatorElement(parent, tagName: "input");
        }

        // Gets a SimulatorElement representing the body element using reflection.
        [Description(SimulatorLocators.Body)]
        private static SimulatorElement GetBody(SimulatorDriver parent)
        {
            // Create a new SimulatorElement representing the body element
            return new SimulatorElement(parent, tagName: "BODY");
        }

        // Gets a new SimulatorElement with inner text containing a URL
        [Description(SimulatorLocators.Url)]
        private static SimulatorElement GetUrl(SimulatorDriver parent)
        {
            // Create a new SimulatorElement with inner text containing a URL
            return new SimulatorElement(parent, tagName: "DIV", "Inner text with URL: http://positive.io/20");
        }

        // Gets a random SimulatorElement using reflection with a specified positive ratio.
        public static SimulatorElement GetRandom(SimulatorDriver parent, int positiveRatio)
        {
            // Return a random SimulatorElement obtained based on the specified positive ratio
            return RandomElement(parent, positiveRatio);
        }

        // Gets a random SimulatorElement using reflection with a specified positive ratio and a factory method for creating elements.
        private static SimulatorElement RandomElement(SimulatorDriver parent, int positiveRatio)
        {
            // Call the RandomElement method with the specified parent, positive ratio, and a factory method for creating elements
            return RandomElement(parent, positiveRatio, lambda: () => Negative(parent));
        }

        // Gets a random SimulatorElement using reflection with a specified positive ratio and a factory method for creating elements.
        // This method is used by reflection and should not be directly invoked in normal usage.
        // It generates a random score and compares it with the positive ratio to determine whether to create a positive or negative SimulatorElement.
        private static SimulatorElement RandomElement(SimulatorDriver parent, int positiveRatio, Func<SimulatorElement> lambda)
        {
            // Generate a random score within the range [0, 100)
            var score = 0;

            // Use the lock statement to synchronize access to the shared resource (s_random)
            lock (s_random)
            {
                // Generate a random score within the range [0, 100)
                score = s_random.Next(0, 100);
            }

            // Return a positive or negative SimulatorElement based on the generated score and positive ratio
            return (score <= positiveRatio) ? Positive(parent) : lambda.Invoke();
        }

        // Gets a collection of random SimulatorElements using reflection with a specified existence ratio and a factory method for creating elements.
        private static ReadOnlyCollection<SimulatorElement> RandomElements(SimulatorDriver parent, int existsRatio)
        {
            return RandomElements(parent, existsRatio,
                factory: () => new ReadOnlyCollection<SimulatorElement>([]));
        }

        // Gets a collection of random SimulatorElements using reflection with a specified existence ratio and a factory method for creating elements.
        // This method is used by reflection and should not be directly invoked in normal usage.
        // It generates a random score and compares it with the existence ratio to determine whether to create a positive or negative collection of SimulatorElements.
        private static ReadOnlyCollection<SimulatorElement> RandomElements(
            SimulatorDriver parent, int existsRatio, Func<ReadOnlyCollection<SimulatorElement>> factory)
        {
            // Generate a random score within the range [0, 100)
            var score = 0;

            // Use the lock statement to synchronize access to the shared resource (s_random)
            lock (s_random)
            {
                // Generate a random score within the range [0, 100)
                score = s_random.Next(0, 100);
            }

            // Return a positive or negative collection of SimulatorElements based on the generated score and existence ratio
            return (score <= existsRatio)
                ? new ReadOnlyCollection<SimulatorElement>([Positive(parent)])
                : factory.Invoke();
        }

        // Creates a positive instance of the SimulatorElement class.
        private static SimulatorElement Positive(SimulatorDriver parent) => new(parent);

        // gets a negative 'DIV' element
        private static SimulatorElement Negative(SimulatorDriver parent)
            => new(parent, tagName: "div", text: "Mock: Negative Element 20", enabled: false, selected: false, displayed: false);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see cref="true"/> if the specified object is equal to the current object; otherwise, <see cref="true"/>.</returns>
        public override bool Equals(object obj)
        {
            // Check if the object is a SimulatorElement
            if (obj is not SimulatorElement otherElement)
            {
                return false;
            }

            // Use StringComparison.OrdinalIgnoreCase for case-insensitive string comparison
            const StringComparison Compare = StringComparison.OrdinalIgnoreCase;

            // Compare tag name
            var isElementTag = TagName.Equals(otherElement.TagName, Compare);

            // Compare size (height and width)
            var isSize = Rect.Height == otherElement.Rect.Height && Rect.Width == otherElement.Rect.Width;

            // Compare position (X and Y)
            var isPosition = Rect.X == otherElement.Rect.X && Rect.Y == otherElement.Rect.Y;

            // Compare text
            var isText = Text.Equals(otherElement.Text, Compare);

            // Compare display status
            var isDisplay = Displayed == otherElement.Displayed;

            // Return true if all comparisons are true
            return isElementTag && isSize && isPosition && isText && isDisplay;
        }

        /// <summary>
        /// Returns a hash code for the current <see cref="SimulatorElement"/> instance.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode() => HashCode.Combine(
            Id,
            TagName,
            Text,
            Enabled,
            Displayed,
            Selected);
        #endregion
    }
}
