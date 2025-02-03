using System.Collections.Generic;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// This model is used to describe a sequence of keyboard actions as an input source.
    /// It provides properties to define the actions to be performed, an identifier for the source,
    /// and the type of input source (which is typically "key" for keyboard).
    /// </summary>
    public class InputSourceModel
    {
        /// <summary>
        /// Gets or sets the collection of keyboard actions to be performed.
        /// </summary>
        public IList<ActionModelBase> Actions { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the key input source.
        /// </summary>
        public string Id { get; set; } = "default keyboard";

        /// <summary>
        /// Gets or sets the parameters associated with the input source.
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the type of the input source.
        /// </summary>
        public string Type { get; set; } = "key";
    }
}
