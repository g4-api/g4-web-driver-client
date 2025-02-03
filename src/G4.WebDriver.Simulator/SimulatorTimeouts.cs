using G4.WebDriver.Remote;

using System;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Represents the timeout settings for the simulator's web driver.
    /// </summary>
    public class SimulatorTimeouts : ITimeouts
    {
        /// <inheritdoc />
        public TimeSpan Implicit { get; set; }

        /// <inheritdoc />
        public TimeSpan PageLoad { get; set; }

        /// <inheritdoc />
        public TimeSpan Script { get; set; }
    }
}
