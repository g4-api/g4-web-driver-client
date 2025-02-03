using G4.WebDriver.Remote;

namespace G4.WebDriver.Tests.Components
{
    public class TestWebDriverService : WebDriverService
    {
        public TestWebDriverService(string binariesPath, string executableName)
            : base(binariesPath, executableName)
        { }

        public TestWebDriverService(string binariesPath, string executableName, int port)
            : base(binariesPath, executableName, port)
        { }

        public TestWebDriverService(string binariesPath, string executableName, int port, string downloadUrl)
            : base(binariesPath, executableName, string.Empty, port, downloadUrl)
        { }
    }
}
