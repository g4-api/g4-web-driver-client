namespace G4.WebDriver.Tests.Framework
{
    //[TestClass]
    //public abstract class TestConfigurations
    //{
    //    /// <summary>
    //    /// Initializes the test assembly before running the tests.
    //    /// This method is executed once before any tests in the assembly are run.
    //    /// </summary>
    //    /// <param name="context">The TestContext object that provides information about the current test run.</param>
    //    [AssemblyInitialize]
    //    public static void OneTimeSetup(TestContext context)
    //    {
    //        // Start local static files server
    //        WebServer.RemoveWebHost(); // Clear any existing web host
    //        WebServer.NewWebHost();    // Create a new web host
    //        WebServer.StartWebHost();  // Start the web host

    //        // Start BrowserStack local agent if the grid endpoint contains "browserstack"
    //        var remoteEndpoint = context.Properties.Contains(key: "Grid.Endpoint")
    //            ? $"{context.Properties["Grid.Endpoint"]}"
    //            : string.Empty;

    //        var isLocal = $"{context.Properties["Integration.Local"]}".Equals("true", StringComparison.OrdinalIgnoreCase);

    //        if (remoteEndpoint.Contains("browserstack", StringComparison.OrdinalIgnoreCase) && !isLocal)
    //        {
    //            // Start the BrowserStack local agent process
    //            var info = new ProcessStartInfo
    //            {
    //                FileName = Path.Combine(Directory.GetCurrentDirectory(), "Binaries", "BrowserStackLocal.exe"),
    //                Arguments = $"--force --key {context.Properties["BrowserStack.Password"]}"
    //            };
    //            Process.Start(info);
    //        }

    //        // Wait for the process to start (5 seconds)
    //        Thread.Sleep(TimeSpan.FromSeconds(5));
    //        context.WriteLine("New test server created successfully.");
    //    }
    //}
}
