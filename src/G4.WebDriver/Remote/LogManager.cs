using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System.Collections.Generic;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Manages log retrieval operations for WebDriver. Implements the <see cref="ILogManager"/> interface.
    /// </summary>
    /// <param name="driver">The WebDriver instance used to retrieve logs.</param>
    public class LogManager(IWebDriver driver) : ILogManager
    {
        /// <inheritdoc />
        public IEnumerable<LogEntryModel> GetLog(string type)
        {
            // Create a new WebDriver command for retrieving logs.
            WebDriverCommandModel command = driver.Invoker.NewCommand(nameof(WebDriverCommands.GetLog));

            // Set the log type to be retrieved.
            command.Data = new Dictionary<string, object>
            {
                ["sessionId"] = driver.GetSession().OpaqueKey,
                ["type"] = type
            };

            // Invoke the command and return the retrieved log entries.
            return driver.Invoker.Invoke<LogEntryModel[]>(command);
        }
    }
}
