using G4.WebDriver.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace G4.WebDriver.Remote.Uia
{
    public class UiaElement(IWebDriver driver, string id) : WebElement(driver, id), IUser32Element
    {
        #region *** Fields     ***
        // Get the WebDriverCommandInvoker instance from the WebDriver instance
        private readonly IWebDriverCommandInvoker _invoker = driver.Invoker;

        // Get the session instance from the WebDriver instance
        private readonly SessionIdModel _session = ((IWebDriverSession)driver).Session;
        #endregion

        #region *** Methods    ***
        /// <inheritdoc />
        new public IWebElement FindElement(By by)
        {
            // Find the element using the base class method
            var webElement = base.FindElement(by);

            // Return a new UiaElement instance with the found element ID
            return new UiaElement(Driver, webElement.Id);
        }

        /// <inheritdoc />
        new public IEnumerable<IWebElement> FindElements(By by)
        {
            // Find the elements using the base class method
            var elements = base
                .FindElements(by)
                .Select(i => (IWebElement)new UiaElement(Driver, i.Id))
                .ToList();

            // Return a read-only collection of the found elements
            return new ReadOnlyCollection<IWebElement>(elements);
        }

        /// <inheritdoc />
        new public string GetAttribute(string attributeName)
        {
            // Prepare the WebDriver command for retrieving the value of an attribute for the current WebElement
            var command = _invoker.Commands["GetUser32Attribute"];
            command.Session = _session.OpaqueKey;
            command.Element = Id;

            // Replace the placeholder in the route with the actual attribute name
            command.Route = command.Route.Replace($"$[{nameof(attributeName)}]", attributeName);

            // Invoke the GetElementAttribute WebDriver command
            var response = _invoker.Invoke(command);

            // Extract and return the attribute value, or null if the value is null
            return response.Value == null
                ? null
                : $"{response.Value}";
        }

        /// <inheritdoc />
        public void MoveToElement()
        {
            // Call the overload with a default MousePositionInputModel.
            MoveToElement(new MousePositionInputModel());
        }

        /// <inheritdoc />
        public void MoveToElement(MousePositionInputModel positionData)
        {
            // Prepare the WebDriver command for moving the mouse pointer to the current WebElement
            var command = _invoker.Commands["MoveUser32MouseToElement"];
            command.Session = _session.OpaqueKey;
            command.Element = Id;
            command.Data = positionData;

            // Invoke the MoveUser32MouseToElement WebDriver command
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public void SendClick()
        {
            // Prepare the WebDriver command for sending a native click command to the current WebElement
            var command = _invoker.Commands["SendUser32ClickToElement"];
            command.Session = _session.OpaqueKey;
            command.Element = Id;

            // Invoke the SendUser32ClickToElement WebDriver command
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public void SendDoubleClick()
        {
            // Prepare the WebDriver command for sending a native double-click command to the current WebElement
            var command = _invoker.Commands["SendUser32DoubleClickToElement"];
            command.Session = _session.OpaqueKey;
            command.Element = Id;

            // Invoke the SendUser32DoubleClickToElement WebDriver command
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public void SetFocus()
        {
            // Prepare the WebDriver command for setting focus on the current WebElement
            var command = _invoker.Commands["SetUser32Focus"];
            command.Session = _session.OpaqueKey;
            command.Element = Id;

            // Invoke the SetUser32Focus WebDriver command
            _invoker.Invoke(command);
        }
        #endregion
    }
}
