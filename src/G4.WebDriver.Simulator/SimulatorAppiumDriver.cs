using G4.WebDriver.Models;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Appium;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Represents a simulator implementation of the Appium driver, extending the <see cref="AppiumDriverBase"/>.
    /// </summary>
    internal class SimulatorAppiumDriver(IWebDriverCommandInvoker invoker, SessionModel session)
        : AppiumDriverBase(invoker, session);
}
