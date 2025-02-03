/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/WebDriverResult.cs
 */
using System.Runtime.Serialization;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Specifies return values for actions in the driver.
    /// </summary>
    [DataContract]
    public static class WebDriverStatusCodes
    {
        /// <summary>
        /// The action was successful.
        /// </summary>
        [DataMember] public const int Success = 0;

        /// <summary>
        /// The index specified for the action was out of the acceptable range.
        /// </summary>
        [DataMember] public const int IndexOutOfBounds = 1;

        /// <summary>
        /// No collection was specified.
        /// </summary>
        [DataMember] public const int NoCollection = 2;

        /// <summary>
        /// No string was specified.
        /// </summary>
        [DataMember] public const int NoString = 3;

        /// <summary>
        /// No string length was specified.
        /// </summary>
        [DataMember] public const int NoStringLength = 4;

        /// <summary>
        /// No string wrapper was specified.
        /// </summary>
        [DataMember] public const int NoStringWrapper = 5;

        /// <summary>
        /// No driver matching the criteria exists.
        /// </summary>
        [DataMember] public const int NoSuchDriver = 6;

        /// <summary>
        /// No element matching the criteria exists.
        /// </summary>
        [DataMember] public const int NoSuchElement = 7;

        /// <summary>
        /// No frame matching the criteria exists.
        /// </summary>
        [DataMember] public const int NoSuchFrame = 8;

        /// <summary>
        /// The functionality is not supported.
        /// </summary>
        [DataMember] public const int UnknownCommand = 9;

        /// <summary>
        /// The specified element is no longer valid.
        /// </summary>
        [DataMember] public const int ObsoleteElement = 10;

        /// <summary>
        /// The specified element is not displayed.
        /// </summary>
        [DataMember] public const int ElementNotDisplayed = 11;

        /// <summary>
        /// The specified element is not enabled.
        /// </summary>
        [DataMember] public const int InvalidElementState = 12;

        /// <summary>
        /// An unhandled error occurred.
        /// </summary>
        [DataMember] public const int UnhandledError = 13;

        /// <summary>
        /// An error occurred; but it was expected.
        /// </summary>
        [DataMember] public const int ExpectedError = 14;

        /// <summary>
        /// The specified element is not selected.
        /// </summary>
        [DataMember] public const int ElementNotSelectable = 15;

        /// <summary>
        /// No document matching the criteria exists.
        /// </summary>
        [DataMember] public const int NoSuchDocument = 16;

        /// <summary>
        /// An unexpected JavaScript error occurred.
        /// </summary>
        [DataMember] public const int UnexpectedJavaScriptError = 17;

        /// <summary>
        /// No result is available from the JavaScript execution.
        /// </summary>
        [DataMember] public const int NoScriptResult = 18;

        /// <summary>
        /// The result from the JavaScript execution is not recognized.
        /// </summary>
        [DataMember] public const int XPathLookupError = 19;

        /// <summary>
        /// No collection matching the criteria exists.
        /// </summary>
        [DataMember] public const int NoSuchCollection = 20;

        /// <summary>
        /// A timeout occurred.
        /// </summary>
        [DataMember] public const int Timeout = 21;

        /// <summary>
        /// A null pointer was received.
        /// </summary>
        [DataMember] public const int NullPointer = 22;

        /// <summary>
        /// No window matching the criteria exists.
        /// </summary>
        [DataMember] public const int NoSuchWindow = 23;

        /// <summary>
        /// An illegal attempt was made to set a cookie under a different domain than the current page.
        /// </summary>
        [DataMember] public const int InvalidCookieDomain = 24;

        /// <summary>
        /// A request to set a cookie's value could not be satisfied.
        /// </summary>
        [DataMember] public const int UnableToSetCookie = 25;

        /// <summary>
        /// An alert was found open unexpectedly.
        /// </summary>
        [DataMember] public const int UnexpectedAlertOpen = 26;

        /// <summary>
        /// A request was made to switch to an alert; but no alert is currently open.
        /// </summary>
        [DataMember] public const int NoAlertPresent = 27;

        /// <summary>
        /// An asynchronous JavaScript execution timed out.
        /// </summary>
        [DataMember] public const int AsyncScriptTimeout = 28;

        /// <summary>
        /// The coordinates of the element are invalid.
        /// </summary>
        [DataMember] public const int InvalidElementCoordinates = 29;

        /// <summary>
        /// The selector used (CSS/XPath) was invalid.
        /// </summary>
        [DataMember] public const int InvalidSelector = 32;

        /// <summary>
        /// A session was not created by the driver
        /// </summary>
        [DataMember] public const int SessionNotCreated = 33;

        /// <summary>
        /// The requested move was outside the active view port
        /// </summary>
        [DataMember] public const int MoveTargetOutOfBounds = 34;

        /// <summary>
        /// The XPath selector was invalid.
        /// </summary>
        [DataMember] public const int InvalidXPathSelector = 51;

        /// <summary>
        /// An insecure SSl certificate was specified.
        /// </summary>
        [DataMember] public const int InsecureCertificate = 59;

        /// <summary>
        /// The element was not intractable
        /// </summary>
        [DataMember] public const int ElementNotInteractable = 60;

        /// <summary>
        /// An invalid argument was passed to the command.
        /// </summary>
        [DataMember] public const int InvalidArgument = 61;

        /// <summary>
        /// No cookie was found matching the name requested.
        /// </summary>
        [DataMember] public const int NoSuchCookie = 62;

        /// <summary>
        /// The driver was unable to capture the screen.
        /// </summary>
        [DataMember] public const int UnableToCaptureScreen = 63;

        /// <summary>
        /// The click on the element was intercepted by a different element.
        /// </summary>
        [DataMember] public const int ElementClickIntercepted = 64;

        /// <summary>
        /// The element does not have a shadow root.
        /// </summary>
        [DataMember] public const int NoSuchShadowRoot = 65;
    }
}
