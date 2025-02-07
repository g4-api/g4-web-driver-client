using G4.WebDriver.Models;
using G4.WebDriver.Remote.Uia;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;

namespace G4.WebDriver.Tests
{
    [TestClass]
    [TestCategory("UiaDriver")]
    [TestCategory("WindowsAppTest")]
    public class UiaDriverTests : TestBase<UiaDriver>
    {
        [TestMethod(displayName: "Verify that a new application driver can be instantiated and perform UI interactions correctly.")]
        public void NewApplicationDriverTest()
        {
            // Retrieve the local Grid endpoint path from the test context properties
            var binariesPath = $"{TestContext.Properties["Grid.Endpoint"]}";

            // Update the local Grid endpoint property with the path to the UiaDriverServer binaries
            TestContext.Properties["Grid.Endpoint"] = Path.Combine(binariesPath, "UiaDriverServer");

            // Configure UiaOptions with the path to the application executable
            var options = new UiaOptions
            {
                App = "E:\\Binaries\\Automation\\SimpleWpfTestApp.exe"
            };

            // Define locators for UI elements using their automation IDs
            var textbox = By.Xpath("//Edit[@AutomationId='InputTextBoxAutomationId']");
            var button = By.Xpath("//Button[@AutomationId='SubmitButtonAutomationId']");
            var label = By.Xpath("//Text[@AutomationId='OutputLabelAutomationId']");

            // Invoke the driver with specified options and an empty URL, passing in an action to interact with the UI elements
            Invoke(
                TestContext,
                options,
                url: string.Empty,
                action: driver =>
                {
                    // Enter text into the input text box
                    driver.FindElement(textbox).SendKeys("Foo");

                    // Click the submit button
                    driver.FindElement(button).Click();

                    // Retrieve the text from the output label
                    var actual = driver.FindElement(label).GetAttribute("Name");

                    // Verify that the output label displays the correct text after the button click
                    Assert.AreEqual("You entered: Foo", actual, $"Output label text mismatch. Expected 'You entered: Foo' but got '{actual}'.");
                }
            );
        }

        [TestMethod(displayName: "Verify that a new UiaDriver can be instantiated and is not ready by default.")]
        public void NewDesktopDriverTest()
        {
            // Retrieve the local Grid endpoint path from the test context properties
            var binariesPath = $"{TestContext.Properties["Grid.Endpoint"]}";

            // Update the local Grid endpoint property with the path to the UiaDriverServer binaries
            TestContext.Properties["Grid.Endpoint"] = Path.Combine(binariesPath, "UiaDriverServer");

            // Invoke the driver with default options and an empty URL, passing in an action to validate the driver state
            Invoke(
                TestContext,
                options: default,
                url: string.Empty,
                action: driver =>
                {
                    // Verify that the WebDriverService is not ready when the driver is instantiated
                    // This confirms that only one Windows instance is supported
                    Assert.IsFalse(driver.Invoker.WebDriverService.Ready, "WebDriverService is reported as ready, but it should be in a not-ready state since only one Windows instance is supported.");

                    // Verify that the driver object is not null, confirming successful instantiation
                    Assert.IsNotNull(driver, "Driver instance is null. Expected a valid driver instance to be created.");
                }
            );
        }
    }
}
