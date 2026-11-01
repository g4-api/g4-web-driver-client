using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Sap.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace G4.WebDriver.Remote.Sap
{
    /// <summary>
    /// Represents a SAP GUI control referenced by a WebDriver element, with the SAP GUI Scripting members that the
    /// W3C element commands do not cover.
    /// </summary>
    /// <remarks>
    /// The element holds only its reference identifier and the owning driver's invoker and session; every command
    /// builds its own request, so elements used one after another or in parallel never share route or payload state.
    /// Child lookups return <see cref="SapElement"/> instances so the SAP members stay available down the tree.
    /// </remarks>
    /// <param name="driver">The driver that owns the session the element belongs to.</param>
    /// <param name="id">The WebDriver element reference identifier.</param>
    public class SapElement(IWebDriver driver, string id) : WebElement(driver, id), ISapElement
    {
        #region *** Constants    ***
        // Names the W3C element-reference key that the server uses when a method returns a control.
        private const string ElementReferenceKey = "element-6066-11e4-a52e-4f735466cecf";
        #endregion

        #region *** Fields       ***
        // Holds the owning driver's invoker, which sends every SAP element command.
        private readonly IWebDriverCommandInvoker _invoker = driver.Invoker;

        // Holds the owning driver's session, whose opaque key fills the session placeholder of each route.
        private readonly SessionIdModel _session = ((IWebDriverSession)driver).Session;
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Finds the first child element that matches the locator and returns it as a <see cref="SapElement"/>.
        /// </summary>
        /// <param name="by">The locator of the child element.</param>
        /// <returns>The matching child element.</returns>
        new public IWebElement FindElement(By by)
        {
            // Locate the child through the standard element command, then wrap it to keep the SAP members.
            var element = base.FindElement(by);

            return new SapElement(Driver, element.Id);
        }

        /// <summary>
        /// Finds every child element that matches the locator and returns them as <see cref="SapElement"/> instances.
        /// </summary>
        /// <param name="by">The locator of the child elements.</param>
        /// <returns>A read-only collection of the matching child elements; empty when nothing matches.</returns>
        new public IEnumerable<IWebElement> FindElements(By by)
        {
            // Locate the children through the standard element command and wrap each one to keep the SAP members.
            var elements = base
                .FindElements(by)
                .Select(element => (IWebElement)new SapElement(Driver, element.Id))
                .ToList();

            return new ReadOnlyCollection<IWebElement>(elements);
        }

        /// <inheritdoc />
        public object InvokeMethod(string method, params object[] arguments)
        {
            // Require a method name before allocating a request the server would reject as invalid.
            ArgumentException.ThrowIfNullOrWhiteSpace(argument: method);

            // Build an invocation-owned command so this element and payload never change the registered template.
            var command = _invoker.NewCommand(commandName: "InvokeSapElementMethod");
            command.Session = _session.OpaqueKey;
            command.Element = Id;
            command.Data = new SapMethodCallModel
            {
                Arguments = arguments ?? [],
                Method = method
            };

            // Call the method on the server; allow-list and stale-reference failures surface as WebDriver exceptions.
            var response = _invoker.Invoke(command);

            // Convert the JSON result so callers receive scalars, arrays, and elements instead of raw JSON.
            return ConvertResult(driver: Driver, value: response.Value);
        }

        /// <inheritdoc />
        public void SetProperty(string name, object value)
        {
            // Require a property name before allocating a request the server would reject as invalid.
            ArgumentException.ThrowIfNullOrWhiteSpace(argument: name);

            // Build an invocation-owned command so this element and payload never change the registered template.
            var command = _invoker.NewCommand(commandName: "SetSapElementProperty");
            command.Session = _session.OpaqueKey;
            command.Element = Id;
            command.Data = new SapPropertyModel
            {
                Name = name,
                Value = value
            };

            // Set the property on the server; allow-list and stale-reference failures surface as WebDriver exceptions.
            _invoker.Invoke(command);
        }

        // Converts a method-call result into plain .NET values for the caller. Strings, numbers, and booleans come
        // back as scalars; arrays become object arrays converted item by item; an element reference becomes a
        // SapElement owned by the same driver; the empty object the server returns for a method without a result
        // becomes null; any other object is returned as its JsonElement. The helper has no side effects.
        private static object ConvertResult(IWebDriver driver, object value)
        {
            // Pass through anything that is not JSON, including the null value of an empty response.
            if (value is not JsonElement element)
            {
                return value;
            }

            // Map each JSON kind to the value the caller works with; null and undefined values become null.
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => ConvertNumber(element),
                JsonValueKind.True or JsonValueKind.False => element.GetBoolean(),
                JsonValueKind.Array => element
                    .EnumerateArray()
                    .Select(item => ConvertResult(driver, value: item))
                    .ToArray(),
                JsonValueKind.Object => ConvertObject(driver, element),
                _ => null
            };

            // Converts a JSON object result: an element reference becomes a SapElement, an empty object (a method
            // without a result) becomes null, and any other object is returned unchanged as its JsonElement.
            static object ConvertObject(IWebDriver driver, JsonElement element)
            {
                // Wrap a control reference so the caller can keep driving the returned control.
                if (element.TryGetProperty(ElementReferenceKey, out var reference))
                {
                    return new SapElement(driver, id: reference.GetString());
                }

                // Report a method without a result as null rather than an empty JSON object.
                var isEmpty = !element.EnumerateObject().Any();

                return isEmpty ? null : element;
            }

            // Converts a JSON number to a boxed long when it is a whole number in range, otherwise to a boxed double.
            // Separate returns keep each boxed type; a conditional expression would widen both branches to double.
            static object ConvertNumber(JsonElement element)
            {
                // Keep whole numbers as long so indexes and counts compare as integers.
                if (element.TryGetInt64(out var number))
                {
                    return number;
                }

                return element.GetDouble();
            }
        }
        #endregion
    }
}
