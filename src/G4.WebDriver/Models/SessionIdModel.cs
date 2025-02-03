using System;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Provides a mechanism for maintaining a session of a WebDriverServer.
    /// </summary>
    /// <param name="opaqueKey">The key of the session in use.</param>
    public class SessionIdModel(string opaqueKey)
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the SessionIdModel class.
        /// </summary>
        public SessionIdModel()
            : this($"{Guid.NewGuid()}")
        { }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the key of the session in use.
        /// </summary>
        public string OpaqueKey { get; set; } = opaqueKey;
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => OpaqueKey;
        #endregion
    }
}
