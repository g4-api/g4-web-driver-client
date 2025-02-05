using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides a mechanism to send commands to the remote end.
    /// This abstract class implements the basic functionality required to interact with a WebDriver session.
    /// </summary>
    public abstract class WebDriverBase : IWebDriver, IWebDriverSession, ITakesScreenshot
    {
        #region *** Fields       ***
        // Initialize an options manager instance
        private IOptions _options;

        // Initialize a navigation manager instance
        private INavigation _navigation;
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverBase"/> class with the specified command invoker and session model.
        /// </summary>
        /// <param name="invoker">The command invoker used to send commands to the WebDriver remote end.</param>
        /// <param name="session">The session model containing the session configuration, including capabilities and authentication details.</param>
        protected WebDriverBase(IWebDriverCommandInvoker invoker, SessionModel session)
        {
            // Assign the provided invoker to the Invoker property.
            Invoker = invoker;

            // Assign the session capabilities to the Capabilities property.
            Capabilities = session.Capabilities;

            // Local function to add custom commands to the invoker.
            void AddCustomCommands()
            {
                // Retrieve custom commands defined by the derived class.
                var commands = GetCustomCommands();

                // Add each custom command to the invoker.
                foreach (var (Name, Command) in commands)
                {
                    Invoker?.AddCommand(Name, Command);
                }
            }

            // Add the custom commands to the invoker.
            AddCustomCommands();

            // If a new session is not required, exit the constructor early.
            if (!session.StartNewSession)
            {
                return;
            }

            // Start the WebDriver service if not already running.
            Invoker.WebDriverService?.StartService();

            // Create a new session by invoking the NewSession command.
            var response = invoker.Invoke(commandName: nameof(WebDriverCommands.NewSession), data: session);

            // Set the session ID model based on the response.
            Session = new SessionIdModel(response.Session);

            // Assign the newly created session to the invoker.
            Invoker.Session = Session;
        }
        #endregion

        #region *** Properties   ***
        /// <inheritdoc />
        public CapabilitiesModel Capabilities { get; }

        /// <inheritdoc />
        public IWebDriverCommandInvoker Invoker { get; set; }

        /// <inheritdoc />
        public string PageSource => Invoker
            .Invoke(Invoker.NewCommand(nameof(WebDriverCommands.GetPageSource)))
            .Value
            .ToString();

        /// <inheritdoc />
        public SessionIdModel Session { get; set; }

        /// <inheritdoc />
        public string WindowHandle => Invoker
            .Invoke(Invoker.NewCommand(nameof(WebDriverCommands.GetWindowHandle)))
            .Value
            .ToString();

        /// <inheritdoc />
        public ReadOnlyCollection<string> WindowHandles
        {
            get
            {
                // Invoke the WebDriver command to get the current window handle
                var value = Invoker
                    .Invoke(Invoker.NewCommand(nameof(WebDriverCommands.GetWindowHandles)))
                    .Value
                    .ToString();

                // Deserialize the JSON string received from WebDriver into a list of strings
                var list = JsonSerializer.Deserialize<IList<string>>(value);

                // Create a read-only collection of window handles and return it
                return new ReadOnlyCollection<string>(list);
            }
        }
        #endregion

        #region *** Methods      ***
        /// <inheritdoc />
        public void Close() => Invoker.Invoke(nameof(WebDriverCommands.CloseWindow));

        /// <inheritdoc />
        public IWebElement FindElement(By by)
        {
            // Send a command to the WebDriver to find an element using the specified locator strategy and value.
            var response = Invoker.Invoke(nameof(WebDriverCommands.FindElement), new
            {
                by.Using,  // The method used to locate the element (e.g., CSS selector, XPath).
                by.Value   // The actual value of the locator (e.g., ".button-class", "//input[@id='submit']").
            });

            // Extract the JSON response from the WebDriver command.
            var value = (JsonElement)response.Value;

            // Convert the JSON response to a dictionary to access the element's details.
            var element = value.ConvertToDictionary();

            // Get the element ID from the dictionary, which uniquely identifies the element within the current session.
            var id = element.First().Value.ToString();

            // Return a new WebElement instance, initializing it with the WebDriver instance and the extracted element ID.
            return new WebElement(driver: this, id);
        }

        /// <inheritdoc />
        public IEnumerable<IWebElement> FindElements(By by)
        {
            // Send a command to the WebDriver to find all elements that match the specified locator strategy and value.
            var response = Invoker.Invoke(nameof(WebDriverCommands.FindElements), new
            {
                by.Using,  // The method used to locate the elements (e.g., CSS selector, XPath).
                by.Value   // The actual value of the locator (e.g., ".button-class", "//input[@id='submit']").
            });

            // Parse the JSON response to an array of elements.
            var value = ((JsonElement)response.Value).EnumerateArray().ToArray();

            // Convert each element in the array to a dictionary to access its details, 
            // and create a WebElement instance for each found element.
            var elements = value
                .Select(i => i.ConvertToDictionary())
                .Select(i => new WebElement(driver: this, id: i.First().Value.ToString()) as IWebElement)
                .ToList();

            // Return a read-only collection of the found WebElement instances.
            return new ReadOnlyCollection<IWebElement>(elements);
        }

        /// <inheritdoc />
        public ScreenshotModel GetScreenshot()
        {
            // Invoke the GetScreenshot WebDriver command
            var response = Invoker.Invoke(nameof(WebDriverCommands.GetScreenshot));

            // Extract the Base64 representation of the screenshot from the response
            var value = (JsonElement)response.Value;
            var base64 = value.GetString();

            // Create and return a ScreenshotModel with the Base64 representation
            return new ScreenshotModel(base64);
        }

        /// <inheritdoc />
        public object InvokeAsyncScript(string script, params object[] args)
        {
            return InvokeAsyncScript<object>(script, args);
        }

        /// <inheritdoc />
        public T InvokeAsyncScript<T>(string script, params object[] args)
        {
            // Prepare arguments, converting IWebElement arguments to dictionaries
            var arguments = args
                .Select(arg => arg is IWebElement elementArg ? elementArg.ConvertToDictionary() : arg)
                .ToArray();

            // Create data object for the script execution command
            var data = new
            {
                Script = script,
                Args = arguments
            };

            // Create and invoke the InvokeScript command
            var command = WebDriverCommands
                .InvokeScriptAsync
                .SetSession(Session.OpaqueKey)
                .SetData(data);

            // Invoke the command using WebDriverInvoker and return the result
            return Invoker.Invoke<T>(command);
        }

        /// <inheritdoc />
        public object InvokeScript(string script, params object[] args)
        {
            return InvokeScript<object>(script, args);
        }

        /// <inheritdoc />
        public object InvokeScript(PinnedScriptModel script, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T InvokeScript<T>(string script, params object[] args)
        {
            // Prepare arguments, converting IWebElement arguments to dictionaries
            var arguments = args
                .Select(arg => arg is IWebElement elementArg ? elementArg.ConvertToDictionary() : arg)
                .ToArray();

            // Create data object for the script execution command
            var data = new
            {
                Script = script,
                Args = arguments
            };

            // Create and invoke the InvokeScript command
            var command = WebDriverCommands
                .InvokeScript
                .SetSession(Session.OpaqueKey)
                .SetData(data);

            // Invoke the command using WebDriverInvoker and return the result
            return Invoker.Invoke<T>(command);
        }

        /// <inheritdoc />
        public IOptions Manage() => _options ??= new OptionsManager(driver: this);

        /// <inheritdoc />
        public INavigation Navigate() => _navigation ??= new NavigationManager(driver: this);

        /// <inheritdoc />
        public void Quit()
        {
            try
            {
                // Attempt to invoke the 'DeleteSession' command to terminate the WebDriver session
                Invoker.Invoke(nameof(WebDriverCommands.DeleteSession));
            }
            catch (InvalidSessionIdException)
            {
                // Ignore the exception if the session ID is invalid
                // This exception typically occurs if the session has already been terminated or is not valid
            }
        }

        /// <inheritdoc />
        public ITargetLocator SwitchTo() => new TargetLocator(driver: this);

        /// <inheritdoc />
        public void Dispose()
        {
            // Call the Dispose method with the parameter set to true to release resources
            Dispose(true);

            // Suppress finalization to prevent the finalizer from running
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the WebDriver instance, releasing associated resources.
        /// </summary>
        /// <param name="disposing">True if called from the <see cref="Dispose"/> method, false if called from the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Finalizer cleanup: No need to invoke WebDriverCommands.DeleteSession or dispose WebDriverService
            if (!disposing)
            {
                return;
            }

            // Dispose resources when called from the Dispose method
            try
            {
                Invoker.Invoke(nameof(WebDriverCommands.DeleteSession));
            }
            catch (InvalidSessionIdException)
            {
                // Ignore the exception if the session ID is invalid
            }

            // Dispose the WebDriverService if available
            Invoker.WebDriverService?.Dispose();
        }

        /// <summary>
        /// Provides custom WebDriver commands that are specific to this implementation.
        /// </summary>
        /// <returns>
        /// An enumerable collection of custom WebDriver commands represented as a tuple containing
        /// the command name and its corresponding <see cref="WebDriverCommandModel"/>.
        /// </returns>
        protected virtual IEnumerable<(string Name, WebDriverCommandModel Command)> GetCustomCommands()
        {
            return [];
        }
        #endregion
    }
}
