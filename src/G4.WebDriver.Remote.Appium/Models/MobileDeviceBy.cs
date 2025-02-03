using G4.WebDriver.Models;

using System;

namespace G4.WebDriver.Remote.Appium.Models
{
    /// <summary>
    /// A model for a lookup of an individual element and collections of elements.
    /// </summary>
    /// <remarks>This class was designed in a way that it can be extended using extension methods.</remarks>
    /// <param name="using">An enumerated attribute deciding what technique should be used to search for elements in the current browsing context.</param>
    /// <param name="value">The selector to use with the searching technique.</param>
    public class MobileDeviceBy(string @using, string value) : By(@using, value)
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the MobileDeviceBy class with default values.
        /// </summary>
        public MobileDeviceBy()
            : this(@using: string.Empty, value: string.Empty)
        { }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Creates a By object for locating elements using Android Data Matcher expressions.
        /// </summary>
        /// <param name="dataMatcher">The Android Data Matcher expression to locate the element.</param>
        /// <returns>A By object representing the Android Data Matcher expression.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the dataMatcher parameter is null or empty.</exception>
        public static By AndroidDataMatcher(string dataMatcher)
        {
            // Check if dataMatcher is null or empty
            if (string.IsNullOrEmpty(dataMatcher))
            {
                const string error = "The 'dataMatcher' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(dataMatcher), message: error);
            }

            // Create a By object for locating elements using Android Data Matcher expressions
            return new By
            {
                Description = $"By.AndroidDataMatcher: {dataMatcher}",
                Using = "-android datamatcher",
                Value = dataMatcher
            };
        }

        /// <summary>
        /// Creates a By object for locating elements using Android UI Automator expressions.
        /// </summary>
        /// <param name="uiAutomator">The Android UI Automator expression to locate the element.</param>
        /// <returns>A By object representing the Android UI Automator expression.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the uiAutomator parameter is null or empty.</exception>
        /// <remarks>
        /// Use the UI Automator API, in particular the UiSelector class to locate elements.
        /// In Appium you send the Java code, as a string, to the server, which executes it in the application's environment, returning the element or elements.
        /// </remarks>
        /// <seealso cref="https://developer.android.com/reference/android/support/test/uiautomator/UiSelector.html"/>
        public static By AndroidUiAutomator(string uiAutomator)
        {
            // Check if uiAutomator is null or empty
            if (string.IsNullOrEmpty(uiAutomator))
            {
                const string error = "The 'uiAutomator' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(uiAutomator), message: error);
            }

            // Create a By object for locating elements using Android UI Automator expressions
            return new By
            {
                Description = $"By.AndroidUIAutomator: {uiAutomator}",
                Using = "-android uiautomator",
                Value = uiAutomator
            };
        }

        /// <summary>
        /// Creates a By object for locating elements using Android ViewMatcher expressions.
        /// </summary>
        /// <param name="viewMatcher">The Android ViewMatcher expression to locate the element.</param>
        /// <returns>A By object representing the Android ViewMatcher expression.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the viewMatcher parameter is null or empty.</exception>
        public static By AndroidViewMatcher(string viewMatcher)
        {
            // Check if viewMatcher is null or empty
            if (string.IsNullOrEmpty(viewMatcher))
            {
                const string error = "The 'viewMatcher' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(viewMatcher), message: error);
            }

            // Create a By object for locating elements using Android ViewMatcher expressions
            return new By
            {
                Description = $"By.AndroidViewMatcher: {viewMatcher}",
                Using = "-android viewmatcher",
                Value = viewMatcher
            };
        }

        /// <summary>
        /// Creates a By object for locating elements using iOS Automation expressions.
        /// </summary>
        /// <param name="iosAutomation">The iOS Automation expression to locate the element.</param>
        /// <returns>A By object representing the iOS Automation expression.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the iosAutomation parameter is null or empty.</exception>
        /// <remarks>
        /// When automating an iOS application, Apple's Instruments framework can be used to find elements.
        /// </remarks>
        public static By IosAutomation(string iosAutomation)
        {
            // Check if iosAutomation is null or empty
            if (string.IsNullOrEmpty(iosAutomation))
            {
                const string error = "The 'iosAutomation' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(iosAutomation), message: error);
            }

            // Create a By object for locating elements using iOS Automation expressions
            return new By
            {
                Description = $"By.IosAutomation: {iosAutomation}",
                Using = "-ios uiautomation",
                Value = iosAutomation
            };
        }

        /// <summary>
        /// Creates a By object for locating elements using iOS Class Chain expressions.
        /// </summary>
        /// <param name="classChain">The iOS Class Chain expression to locate the element.</param>
        /// <returns>A By object representing the iOS Class Chain expression.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the classChain parameter is null or empty.</exception>
        public static By IosClassChain(string classChain)
        {
            // Check if classChain is null or empty
            if (string.IsNullOrEmpty(classChain))
            {
                const string error = "The 'classChain' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(classChain), message: error);
            }

            // Create a By object for locating elements using iOS Class Chain expressions
            return new By
            {
                Description = $"By.IosClassChain: {classChain}",
                Using = "-ios class chain",
                Value = classChain
            };
        }

        /// <summary>
        /// Creates a By object for locating elements using iOS Predicate String expressions.
        /// </summary>
        /// <param name="predicateString">The iOS Predicate String expression to locate the element.</param>
        /// <returns>A By object representing the iOS Predicate String expression.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the predicateString parameter is null or empty.</exception>
        public static By IosPredicateString(string predicateString)
        {
            // Check if predicateString is null or empty
            if (string.IsNullOrEmpty(predicateString))
            {
                const string error = "The 'predicateString' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(predicateString), message: error);
            }

            // Create a By object for locating elements using iOS Predicate String expressions
            return new By
            {
                Description = $"By.IosPredicateString: {predicateString}",
                Using = "-ios predicate string",
                Value = predicateString
            };
        }

        /// <summary>
        /// Creates a By object for locating elements by accessibility identifier.
        /// </summary>
        /// <param name="accessibilityId">The accessibility identifier to locate the element.</param>
        /// <returns>A By object representing the accessibility identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the accessibilityId parameter is null or empty.</exception>
        /// <remarks>
        /// Read a unique identifier for a UI element. For XCUITest it is the element's `accessibility-id` attribute.
        /// For Android it is the element's `content-desc` attribute.
        /// </remarks>
        public static By MobileElementAccessibilityId(string accessibilityId)
        {
            // Check if accessibilityId is null or empty
            if (string.IsNullOrEmpty(accessibilityId))
            {
                const string error = "The 'accessibilityId' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(accessibilityId), message: error);
            }

            // Create a By object for locating elements by accessibility identifier using XPath
            return new By
            {
                Description = $"By.MobileAccessibilityId: {accessibilityId}",
                Using = "accessibility id",
                Value = accessibilityId
            };
        }

        /// <summary>
        /// Creates a By object for locating elements by mobile class name.
        /// </summary>
        /// <param name="className">The class name to locate the element.</param>
        /// <returns>A By object representing the mobile class name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the className parameter is null or empty.</exception>
        /// <remarks>
        /// For IOS it is the full name of the XCUI element and begins with XCUIElementType.
        /// For Android it is the full name of the UIAutomator2 class (e.g.: android.widget.TextView).
        /// </remarks>
        public static By MobileElementClassName(string className)
        {
            // Check if className is null or empty
            if (string.IsNullOrEmpty(className))
            {
                const string error = "The 'className' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(className), message: error);
            }

            // Create a By object for locating elements by mobile class name
            return new By
            {
                Description = $"By.MobileClassName: {className}",
                Using = "class name",
                Value = className
            };
        }

        /// <summary>
        /// Creates a By object for locating elements by mobile name.
        /// </summary>
        /// <param name="name">The name to locate the element.</param>
        /// <returns>A By object representing the mobile name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or empty.</exception>
        public static By MobileElementName(string name)
        {
            // Check if name is null or empty
            if (string.IsNullOrEmpty(name))
            {
                const string error = "The 'name' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(name), message: error);
            }

            // Create a By object for locating elements by mobile name using XPath
            return new By
            {
                Description = $"By.MobileElementName: {name}",
                Using = "name",
                Value = name
            };
        }

        /// <summary>
        /// Creates a By object for locating elements by a mobile image specified in base64 format.
        /// </summary>
        /// <param name="imageBase64Value">The base64-encoded image to locate the element.</param>
        /// <returns>A By object representing the mobile image specified in base64 format.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the imageBase64Value parameter is null or empty.</exception>
        /// <remarks>
        /// Locate an element by matching it with a base 64 encoded image file
        /// </remarks>
        public static By MobileElementImage(string imageBase64Value)
        {
            // Check if imageBase64Value is null or empty
            if (string.IsNullOrEmpty(imageBase64Value))
            {
                const string error = "The 'image' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(imageBase64Value), message: error);
            }

            // Create a By object for locating elements by mobile image using XPath
            return new By
            {
                Description = $"By.MobileImage: {imageBase64Value}",
                Using = "-image",
                Value = imageBase64Value
            };
        }

        /// <summary>
        /// Creates a By object for locating elements by resource identifier.
        /// </summary>
        /// <param name="resourceId">The resource identifier to locate the element.</param>
        /// <returns>A By object representing the resource identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the resourceId parameter is null or empty.</exception>
        /// <remarks>
        /// Native element identifier. `resource-id` for android; `name` for iOS.
        /// </remarks>
        public static By MobileElementResourceId(string resourceId)
        {
            // Check if resourceId is null or empty
            if (string.IsNullOrEmpty(resourceId))
            {
                const string error = "The 'resourceId' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(resourceId), message: error);
            }

            // Create a By object for locating elements by resource identifier using XPath
            return new By
            {
                Description = $"By.ResourceId: {resourceId}",
                Using = "id",
                Value = resourceId
            };
        }
        #endregion
    }
}
