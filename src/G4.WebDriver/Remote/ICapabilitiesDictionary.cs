using System.Collections.Generic;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents an interface for converting capabilities to a dictionary format.
    /// </summary>
    public interface ICapabilitiesDictionary
    {
        /// <summary>
        /// Converts the capabilities to a dictionary with string keys and object values.
        /// </summary>
        /// <returns>A dictionary where the keys are strings and the values are objects, representing the capabilities.</returns>
        IDictionary<string, object> ConvertToDictionary();
    }
}
