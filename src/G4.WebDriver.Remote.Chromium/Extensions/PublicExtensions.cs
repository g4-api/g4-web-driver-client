using G4.WebDriver.Remote.Chromium;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace G4.WebDriver.Extensions
{
    /// <summary>
    /// Provides extension methods for the ChromiumDriver class.
    /// </summary>
    public static class PublicExtensions
    {
        /// <summary>
        /// Resolves the debugger address for the current Chromium-based driver instance.
        /// </summary>
        /// <param name="driver">The Chromium driver instance to resolve the debugger address for.</param>
        /// <returns>A <see cref="string"/> representing the debugger address in the format "address:port" if found; otherwise, an empty string.</returns>
        public static string ResolveDebuggerAddress(this ChromiumDriverBase driver)
        {
            // Retrieve the capabilities dictionary from the driver.
            var capabilities = driver.Capabilities.AlwaysMatch;

            // Attempt to resolve the debugger address directly from the capabilities.
            var debuggerAddress = ResolveDebuggerAddress(capabilities);

            // If a debugger address is found, return it.
            if (!string.IsNullOrEmpty(debuggerAddress))
            {
                return debuggerAddress;
            }

            // Extract the command-line arguments from the capabilities.
            var arguments = GetArguments(capabilities);

            // Attempt to resolve the debugger address and port from the command-line arguments.
            var (address, port) = ResolveDebuggerAddress(arguments);

            // If no port is found, return an empty string.
            if (string.IsNullOrEmpty(port))
            {
                return string.Empty;
            }

            // Return the debugger address in the format "address:port".
            return $"{address}:{port}";
        }

        // Resolves the debugger address from the provided WebDriver capabilities dictionary.
        private static string ResolveDebuggerAddress(IDictionary<string, object> capabilities)
        {
            // Find the first key in the capabilities dictionary that matches the pattern for options (e.g., ":chromeOptions").
            var optionsKey = capabilities
                .Keys
                .FirstOrDefault(i => Regex.IsMatch(input: i, pattern: "(?<=:\\w+)Options$"));

            // Check if an options key was found.
            var isOptions = !string.IsNullOrEmpty(optionsKey);

            // If an options key is found, cast the associated value to a dictionary; otherwise, use an empty dictionary.
            var options = isOptions
                ? (Dictionary<string, object>)capabilities[optionsKey]
                : [];

            // Check if the "debuggerAddress" key exists in the options dictionary and retrieve its value if present.
            var isDebuggerAddress = options.TryGetValue("debuggerAddress", out object debuggerAddress);

            // Return the debugger address if found; otherwise, return an empty string.
            return isDebuggerAddress
                ? $"{debuggerAddress}"
                : string.Empty;
        }

        // Resolves the debugger address and port from a list of command-line arguments.
        private static (string Address, string Port) ResolveDebuggerAddress(IEnumerable<string> arguments)
        {
            // Regex pattern to capture the value after '='.
            const string Pattern = @"(?<==)\w+";

            // If the arguments collection is null or empty, return empty strings for both address and port.
            if (arguments?.Any() != true)
            {
                return (string.Empty, string.Empty);
            }

            // Default address if not specified.
            const string DefaultAddress = "--remote-debugging-address=localhost";

            // Find the argument for the remote debugging address; use the default if not found.
            var debuggingAddressArg = arguments.FirstOrDefault(i => i.StartsWith("--remote-debugging-address", StringComparison.OrdinalIgnoreCase)) ?? DefaultAddress;

            // Find the argument for the remote debugging port; use an empty string if not found.
            var debuggingPortArg = arguments.FirstOrDefault(i => i.StartsWith("--remote-debugging-port", StringComparison.OrdinalIgnoreCase)) ?? string.Empty;

            // Extract the value for the debugging address using regex.
            var debuggingAddress = Regex.Match(input: debuggingAddressArg, Pattern)?.Value;

            // Extract the value for the debugging port using regex.
            var debuggingPort = Regex.Match(input: debuggingPortArg, Pattern)?.Value;

            // Return the resolved debugger address and port.
            return (debuggingAddress, debuggingPort);
        }

        // Extracts the list of arguments from the provided capabilities dictionary.
        private static IEnumerable<string> GetArguments(IDictionary<string, object> capabilities)
        {
            // Find the first key in the capabilities dictionary that matches the pattern for options (e.g., ":chromeOptions").
            var optionsKey = capabilities
                .Keys
                .FirstOrDefault(i => Regex.IsMatch(input: i, pattern: "(?<=:\\w+)Options$"));

            // Check if an options key was found.
            var isOptions = !string.IsNullOrEmpty(optionsKey);

            // If an options key is found, cast the associated value to a dictionary; otherwise, use an empty dictionary.
            var options = isOptions
                ? (Dictionary<string, object>)capabilities[optionsKey]
                : [];

            // Check if the "args" key exists in the options dictionary and retrieve its value if present.
            var isArguments = options.TryGetValue("args", out object arguments);

            // Return the list of arguments if found; otherwise, return an empty collection.
            return isArguments
                ? (IEnumerable<string>)arguments
                : [];
        }
    }
}
