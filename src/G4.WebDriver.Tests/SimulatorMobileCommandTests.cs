using G4.WebDriver.Exceptions;
using G4.WebDriver.Simulator;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

namespace G4.WebDriver.Tests
{
    [TestClass]
    [TestCategory("SimulatorMobileCommand")]
    [TestCategory("UnitTest")]
    public class SimulatorMobileCommandTests
    {
        // The Appium UiAutomator2 extension command the simulator answers for activity starts.
        private const string StartActivityCommand = "mobile: startActivity";

        [TestMethod(DisplayName = "Verify that the simulator accepts mobile: startActivity with a " +
            "package/activity intent.")]
        #region *** Data Set ***
        [DataRow("com.example.app/com.example.app.MainActivity")]
        [DataRow("com.example.app/.MainActivity")]
        #endregion
        public void StartActivityTest(string intent)
        {
            // Arrange: a simulator session and the options object Appium extension commands take.
            using var driver = new SimulatorDriver(".");
            var options = new Dictionary<string, object> { ["intent"] = intent };

            // Act: send the command the way the SwitchActivity plugin does.
            var result = driver.InvokeScript(StartActivityCommand, options);

            // Assert: the command succeeds with an empty result, like a real device.
            Assert.AreEqual(expected: string.Empty, actual: result);
        }

        [TestMethod(DisplayName = "Verify that the simulator rejects mobile: startActivity without a " +
            "package/activity intent.")]
        #region *** Data Set ***
        [DataRow("com.example.app.MainActivity")]
        [DataRow("/com.example.app.MainActivity")]
        [DataRow("com.example.app/")]
        [DataRow("")]
        #endregion
        public void StartActivityInvalidIntentTest(string intent)
        {
            // Arrange: a simulator session and an intent Android cannot resolve.
            using var driver = new SimulatorDriver(".");
            var options = new Dictionary<string, object> { ["intent"] = intent };

            // Act and Assert: the command fails as the device does, so callers are tested against the real format.
            Assert.Throws<WebDriverException>(() => driver.InvokeScript(StartActivityCommand, options));
        }
    }
}
