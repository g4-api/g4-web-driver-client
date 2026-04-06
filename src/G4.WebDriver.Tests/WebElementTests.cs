/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESOURCES
 */
using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Edge;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;

namespace G4.WebDriver.Tests
{
    [TestClass]
    public class WebElementTests : TestBase<EdgeDriver>
    {
        #region *** Element: Find       ***
        [TestMethod]
        [DataRow("ElementFind.html", "ButtonInput")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementByIdTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.Custom.Id(value));

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(element?.Id));
        });

        [TestMethod]
        [DataRow("ElementFind.html", "ContainerName")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementByNameTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.Custom.Name(value));

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(element?.Id));
        });

        [TestMethod]
        [DataRow("ElementFind.html", "container_class")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementByClassNameTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.Custom.PartialClassName(value));

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(element?.Id));
        });

        [TestMethod]
        [DataRow("ElementFind.html", "div")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementByTagNameTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.TagName(value));

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(element?.Id));
        });

        [TestMethod]
        [DataRow("ElementFind.html", "#container")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementByCssTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.CssSelector(value));

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(element?.Id));
        });

        [TestMethod]
        [DataRow("ElementFind.html", "//div[@id='container']")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementByXpathTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.Xpath(value));

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(element?.Id));
        });

        [TestMethod]
        [DataRow("ElementFind.html", "Lorem ipsum dolor")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementByLinkTextTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.LinkText(value));

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(element?.Id));
        });

        [TestMethod]
        [DataRow("ElementFind.html", "Lorem ipsum")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementByLinkTextNegativeTest(string url, string value)
        {
            Assert.Throws<NoSuchElementException>(
                action: () => Invoke(TestContext, url, (driver) => driver.FindElement(By.LinkText(value))),
                message: $"Expected to throw {nameof(NoSuchElementException)} when no element is found with the specified link text."
            );
        }

        [TestMethod]
        [DataRow("ElementFind.html", "Lorem ipsum dolor")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementByPartialLinkTextTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.PartialLinkText(value));

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(element?.Id));
        });

        [TestMethod]
        [DataRow("ElementFind.html", "q")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementNoSuchElementTest(string url, string value)
        {
            Assert.Throws<NoSuchElementException>(
                action: () => Invoke(TestContext, url, (driver) => driver.FindElement(By.Custom.Name(value))),
                message: $"Expected to throw {nameof(NoSuchElementException)} when no element is found with the specified criteria."
            );
        }
        #endregion

        #region *** Element: Properties ***
        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "TextInput")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetTagNameTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.Custom.Id(value));

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(element?.Id));
            Assert.IsTrue(element.TagName.Equals("INPUT", StringComparison.OrdinalIgnoreCase));
        });

        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "TextDiv")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetTextTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var elements = driver.FindElement(By.Custom.Id(value));

            // assert
            Assert.IsGreaterThan(0, elements.Text.Length);
        });

        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "CheckboxEnabled", true)]
        [DataRow("ElementPropertiesAndAttributes.html", "CheckboxDisabled", false)]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void ElementEnabledTest(string url, string value, bool expected) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var elements = driver.FindElement(By.Custom.Id(value));

            // assert
            Assert.AreEqual(expected, elements.Enabled);
        });

        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "CheckboxEnabled", true)]
        [DataRow("ElementPropertiesAndAttributes.html", "CheckboxDisabled", false)]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void ElementSelectedTest(string url, string value, bool expected) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var elements = driver.FindElement(By.Custom.Id(value));

            // assert
            Assert.AreEqual(expected, elements.Selected);
        });

        [TestMethod]
        [DataRow("ElementTextInput.html", "TextAreaEnabled")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void ElementRectTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.Custom.Id(value));
            var rect = element.Rect;

            // assert
            Assert.IsGreaterThan(0, (double)rect.Height);
            Assert.IsGreaterThan(0, (double)rect.Width);
            Assert.IsGreaterThan(0, (double)rect.X);
            Assert.IsGreaterThan(0, (double)rect.Y);
        });

        [TestMethod]
        [DataRow("ElementTextInput.html", "TextAreaEnabled")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void ElementDisplayedTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.Custom.Id(value));

            // assert
            Assert.IsTrue(element.Displayed);
        });

        [TestMethod]
        [DataRow("ElementTextInput.html", "InputHidden")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void ElementDisplayedNegativeTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var element = driver.FindElement(By.Custom.Id(value));

            // assert
            Assert.IsFalse(element.Displayed);
        });
        #endregion

        #region *** Element: Actions    ***
        [TestMethod]
        [DataRow("ElementTextInput.html", "InputEnabledWithText")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void ClearElementTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var element = driver.FindElement(By.Custom.Id(value));

            // invoke
            element.Clear();
            var actual = element.GetProperty("value");

            // assert
            Assert.IsTrue(string.IsNullOrEmpty(actual));
        });

        [TestMethod]
        [DataRow("ElementTextInput.html", "UrlDivText")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void ClearElementExceptionTest(string url, string value)
        {
            Assert.Throws<InvalidElementStateException>(
                action: () => Invoke(TestContext, url, (driver) => driver.FindElement(By.Custom.Id(value)).Clear()),
                message: $"Expected to throw {nameof(InvalidElementStateException)} when trying to clear an element that cannot be cleared."
            );
        }

        [TestMethod]
        [DataRow("ElementInteractions.html", "ClickButton")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void ClickElementTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            driver.FindElement(By.Custom.Id(value)).Click();

            // setup
            var element = driver.FindElement(By.Custom.Id("ClickButtonOutcome"));
            var actual = element.GetAttribute("value");

            // assert
            Assert.IsTrue(actual.Equals("click on element", StringComparison.OrdinalIgnoreCase));
        });

        [TestMethod]
        [DataRow("ElementFind.html")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementFromElementTest(string url) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var container = driver.FindElement(By.Custom.Id("container"));

            // assert
            Assert.IsGreaterThan(0, container.Id.Length);

            // invoke
            var element = container.FindElement(By.Xpath(".//input"));

            // assert
            Assert.IsFalse(element.Id.Equals(container.Id, StringComparison.OrdinalIgnoreCase));
        });

        [TestMethod]
        [DataRow("ElementFind.html")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void FindElementsFromElementTest(string url) => Invoke(TestContext, url, (driver) =>
        {
            // invoke
            var container = driver.FindElement(By.Custom.Id("container"));

            // assert
            Assert.IsGreaterThan(0, container.Id.Length);

            // invoke
            var elements = container.FindElements(By.Xpath(".//input"));

            // assert
            Assert.IsGreaterThan(1, elements.Count());
        });

        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "TextInput")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetAttributeTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var element = driver.FindElement(By.Custom.Id(value));

            // actual
            var attribute = element.GetAttribute("id");

            // assert
            Assert.AreEqual(value, attribute);
        });

        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "TextInput")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetAttributeNullTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var element = driver.FindElement(By.Custom.Id(value));

            // actual
            var attribute = element.GetAttribute("no such attribute");

            // assert
            Assert.IsNull(attribute);
        });

        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "TextInput")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetCssValueTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var element = driver.FindElement(By.Custom.Id(value));

            // actual
            var propertyValue = element.GetCssValue("font-weight");

            // assert
            Assert.AreEqual("400", propertyValue);
        });

        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "TextInput")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetCssValueEmptyTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var element = driver.FindElement(By.Custom.Id(value));

            // actual
            var propertyValue = element.GetCssValue("no such property");

            // assert
            Assert.AreEqual(string.Empty, propertyValue);
        });

        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "TextInput")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetPropertyTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var element = driver.FindElement(By.Custom.Id(value));

            // actual
            var propertyValue = element.GetProperty("type");

            // assert
            Assert.AreEqual("text", propertyValue);
        });

        [TestMethod]
        [DataRow("ElementPropertiesAndAttributes.html", "TextInput")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetPropertyNullTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var element = driver.FindElement(By.Custom.Id(value));

            // actual
            var propertyValue = element.GetProperty("no such property");

            // assert
            Assert.IsNull(propertyValue);
        });

        [TestMethod]
        [DataRow("ElementTextInput.html", "InputEnabled")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void SendKeysTest(string url, string value) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var element = driver.FindElement(By.Custom.Id(value));

            // assert
            var propertyValue = element.GetProperty("value");
            Assert.IsTrue(string.IsNullOrEmpty(propertyValue));

            // actual
            element.SendKeys("Lorem ipsum dolor");
            propertyValue = element.GetProperty("value");

            // assert
            Assert.AreEqual("Lorem ipsum dolor", propertyValue);
        });

        [TestMethod]
        [DataRow("ElementShadowRoot.html")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetShadowRootTest(string url) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var root = driver.FindElement(By.Custom.Id("ShadowComponent")).GetShadowRoot();
            var actual = root.FindElement(By.CssSelector("#TextInput")).GetProperty("title");

            // assert
            Assert.AreEqual("Lorem ipsum dolor", actual);
        });

        [TestMethod]
        [DataRow("ElementShadowRoot.html")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void GetScreenshotTest(string url) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var actual = driver.FindElement(By.Custom.Id("ShadowComponent")).GetScreenshot();

            // assert
            Assert.IsNotEmpty(actual.Bytes);
            Assert.IsGreaterThan(0, actual.ToString().Length);

            // save
            actual.Save("image");
            var file = new FileInfo("image.png");

            // assert
            Assert.IsTrue(file.Exists && file.Length > 0);
        });

        [TestMethod]
        [DataRow("ElementInteractions.html")]
        [TestCategory("Sanity"), TestCategory("Integration")]
        public void ElementEqualsTest(string url) => Invoke(TestContext, url, (driver) =>
        {
            // setup
            var element1 = driver.FindElement(By.Custom.Id("ClickButtonOutcome"));
            var element2 = driver.FindElement(By.Custom.Id("ClickButtonOutcome"));
            var element3 = driver.FindElement(By.Custom.Id("ClickButton"));

            // assert
            Assert.IsFalse(element3.Equals(element2));
            Assert.IsFalse(element3.Equals(element1));
            Assert.IsTrue(element1.Equals(element2));
        });
        #endregion
    }
}
