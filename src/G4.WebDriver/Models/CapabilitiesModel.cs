using System;
using System.Collections.Generic;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// The capabilities which will bare to communicate the features supported
    /// by a given implementation.
    /// </summary>
    public class CapabilitiesModel
    {
        #region *** Constructors ***
        /// <summary>
        /// Initialize a new instance of CapabilitiesModel object.
        /// </summary>
        public CapabilitiesModel()
        {
            AlwaysMatch = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets a collection of capabilities that must all be matched.
        /// </summary>
        public IDictionary<string, object> AlwaysMatch { get; set; }
        #endregion
    }
}
