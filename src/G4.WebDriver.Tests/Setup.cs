/*
 * CHANGE LOG - keep only last 5 threads
 * 
 * RESOURCES
 */
using G4.WebDriver.Tests.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace G4.WebDriver.Tests
{
    /// <summary>
    /// Provides setup and teardown functionality for the test assembly.
    /// This class contains methods for initializing and cleaning up resources used in the tests.
    /// </summary>
    [TestClass]
    public class Setup
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Setup"/> class from being created.
        /// </summary>
        public Setup()
        { }

        /// <summary>
        /// Initializes the test assembly before running the tests.
        /// This method is executed once before any tests in the assembly are run.
        /// </summary>
        /// <param name="context">The TestContext object that provides information about the current test run.</param>
        [AssemblyInitialize]
        public static void OneTimeSetup(TestContext context)
        {
            // Start local static files server
            WebServer.RemoveWebHost(); // Clear any existing web host
            WebServer.NewWebHost();    // Create a new web host
            WebServer.StartWebHost();  // Start the web host

            // Start BrowserStack local agent if the grid endpoint contains "browserstack"
            var remoteEndpoint = context.Properties.TryGetValue("Grid.Endpoint", out var endpoint) ? endpoint.ToString() : string.Empty;
            var isLocal = context.Properties.TryGetValue("Integration.Local", out var local) ? local.ToString() : "true";
            if (remoteEndpoint.Contains("browserstack", StringComparison.OrdinalIgnoreCase) && isLocal == "false")
            {
                // Start the BrowserStack local agent process
                var info = new ProcessStartInfo
                {
                    FileName = Path.Combine(Directory.GetCurrentDirectory(), "Binaries", "BrowserStackLocal.exe"),
                    Arguments = $"--force --key {context.Properties["BrowserStack.Password"]}"
                };
                Process.Start(info);
            }

            // Wait for the process to start (5 seconds)
            Thread.Sleep(TimeSpan.FromSeconds(5));
            context.WriteLine("New test server created successfully.");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // Add any assembly cleanup logic here.
        }
    }
}
