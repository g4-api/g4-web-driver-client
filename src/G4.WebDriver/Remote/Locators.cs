using System.Runtime.Serialization;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides constant values representing different types of locators for locating elements on a web page.
    /// </summary>
    [DataContract]
    public static class Locators
    {
        /// <summary>
        /// Represents the CSS selector locator type.
        /// </summary>
        [DataMember]
        public const string CssSelector = "CssSelector";

        /// <summary>
        /// Represents the Link Text locator type.
        /// </summary>
        [DataMember]
        public const string LinkText = "LinkText";

        /// <summary>
        /// Represents the Partial Link Text locator type.
        /// </summary>
        [DataMember]
        public const string PartialLinkText = "PartialLinkText";

        /// <summary>
        /// Represents the Tag Name locator type.
        /// </summary>
        [DataMember]
        public const string TagName = "TagName";

        /// <summary>
        /// Represents the XPath locator type.
        /// </summary>
        [DataMember]
        public const string Xpath = "Xpath";
    }
}
