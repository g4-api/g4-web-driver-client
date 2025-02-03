using G4.WebDriver.Models;
using G4.WebDriver.Remote;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Interface allowing handling of driver logs.
    /// </summary>
    public class SimulatorLogs : ILogManager
    {
        #region *** Fields       ***
        private readonly IList<LogEntryModel> _logEntries = [];
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the SimulatorLogs class.
        /// </summary>
        public SimulatorLogs()
        {
            LogTypes = new ReadOnlyCollection<string>(
            [
                "Mock"
            ]);
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets the list of available log types for this driver.
        /// </summary>
        public IEnumerable<string> LogTypes { get; }
        #endregion

        #region *** Methods      ***
        /// <inheritdoc />
        public IEnumerable<LogEntryModel> GetLog(string type)
        {
            return new ReadOnlyCollection<LogEntryModel>(_logEntries);
        }
        #endregion
    }
}
