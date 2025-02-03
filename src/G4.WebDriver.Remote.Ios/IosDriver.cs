using G4.WebDriver.Models;
using G4.WebDriver.Remote.Appium;

namespace G4.WebDriver.Remote.Ios
{
    public class IosDriver(IWebDriverCommandInvoker invoker, SessionModel session) : AppiumDriverBase(invoker, session);
}
