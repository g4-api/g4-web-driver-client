/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/ITargetLocator.cs
 */
using G4.WebDriver.Extensions;

using System.Linq;
using System.Text.Json;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents a target locator for switching between different browser targets (windows, frames, alerts, etc.).
    /// </summary>
    /// <param name="driver">The <see cref="IWebDriver"/> instance to associate with the target locator.</param>
    public class TargetLocator(IWebDriver driver) : ITargetLocator, IDriverReference
    {
        #region *** Properties ***
        /// <inheritdoc />
        public IWebDriver Driver { get; } = driver;
        #endregion

        #region *** Methods    ***
        /// <inheritdoc />
        public IWebElement ActiveElement()
        {
            // Build a command to retrieve the currently active element
            var command = Driver
                .Invoker
                .NewCommand(nameof(WebDriverCommands.GetActiveElement));

            // Invoke the command to get the active element and extract its ID
            var response = Driver.Invoker.Invoke(command);
            var value = (JsonElement)response.Value;
            var element = value.ConvertToDictionary();
            var id = element.First().Value.ToString();

            // Create and return an IWebElement instance representing the active element
            return new WebElement(driver: Driver, id);
        }

        /// <inheritdoc />
        public IUserPrompt Alert()
        {
            // Build a command to retrieve the alert text
            var command = Driver
                .Invoker
                .NewCommand(nameof(WebDriverCommands.GetAlertText));

            // Invoke the command to handle the alert and obtain its text
            Driver.Invoker.Invoke(command);

            // Return an IUserPrompt instance representing the alert dialog
            return new UserPrompt(Driver);
        }

        /// <inheritdoc />
        public IWebDriver NewWindow(string type)
        {
            // Create a new command to open a new window
            var command = Driver
                .Invoker
                .NewCommand(nameof(WebDriverCommands.NewWindow));

            // Set the data for the command
            command.Data = new
            {
                Type = type
            };

            // Invoke the command to open the new window
            Driver.Invoker.Invoke(command);

            // Return the current driver instance for method chaining
            return Driver;
        }

        /// <inheritdoc />
        public IWebDriver Frame(int id)
        {
            // Create a new command for switching to a frame
            var command = Driver
                .Invoker
                .NewCommand(nameof(WebDriverCommands.SwitchToFrame));

            // Set the data for the command
            command.Data = new
            {
                Id = id
            };

            // Invoke the command
            Driver.Invoker.Invoke(command);

            // Return the current driver instance for method chaining
            return Driver;
        }

        /// <inheritdoc />
        public IWebDriver Frame(IWebElement element)
        {
            // Create a new command for switching to a frame
            var command = Driver
                .Invoker
                .NewCommand(nameof(WebDriverCommands.SwitchToFrame));

            // Set the data for the command
            command.Data = new
            {
                Id = element.ConvertToDictionary()
            };

            // Invoke the command
            Driver.Invoker.Invoke(command);

            // Return the current driver instance for method chaining
            return Driver;
        }

        /// <inheritdoc />
        public IWebDriver ParentFrame()
        {
            // Create a new command for switching to the parent frame
            var command = Driver
                .Invoker
                .NewCommand(nameof(WebDriverCommands.SwitchToParentFrame));

            // Invoke the command
            Driver.Invoker.Invoke(command);

            // Return the current driver instance for method chaining
            return Driver;
        }

        /// <inheritdoc />
        public IWebDriver Window(string handle)
        {
            // Build a command to switch to the target window
            var command = Driver
                .Invoker
                .NewCommand(nameof(WebDriverCommands.SwitchToWindow));

            // Set the command data to specify the window handle
            command.Data = new
            {
                Handle = handle
            };

            // Invoke the command to switch to the specified window
            Driver.Invoker.Invoke(command);

            // Return the IWebDriver instance after switching to the window
            return Driver;
        }
        #endregion
    }
}
