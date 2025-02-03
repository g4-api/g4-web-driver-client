using System.Text.Json.Serialization;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// Represents the base class for all action models.
    /// </summary>
    [JsonDerivedType(derivedType: typeof(KeyboardActionModel), typeDiscriminator: "KeyboardAction")]
    [JsonDerivedType(derivedType: typeof(MouseActionModel), typeDiscriminator: "MouseAction")]
    [JsonDerivedType(derivedType: typeof(NullActionModel), typeDiscriminator: "NullAction")]
    [JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
    public abstract class ActionModelBase
    {
        /// <summary>
        /// Gets or sets the duration of the null action.
        /// </summary>
        public int? Duration { get; set; }
    }
}
