using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Appium;
using G4.WebDriver.Remote.Appium.Models;
using G4.WebDriver.Remote.Interactions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Defines the interface through which the user controls the browser.
    /// </summary>
    public partial class SimulatorDriver :
        IActionsInvoker,
        IWebDriverSession,
        ITakesScreenshot,
        IWebDriver,
        IGeolocation,
        IMobileDeviceKeyboard
    {
        #region *** Fields       ***
        [GeneratedRegex("(?<=top:(\\s+)?)\\d+|\\d+(?=\\))")]
        private static partial Regex GetTopPattern();

        [GeneratedRegex("(?<=left:(\\s+)?)\\d+|(?<=\\()\\d+")]
        private static partial Regex GetLeftPattern();

        // members: state
        private GeolocationModel _geolocationModel;
        private IOptions _options;
        private int _leftPosition;
        private int _topPosition;
        private ReadOnlyCollection<string> _windowHandles;
        private readonly INavigation _navigation;
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the SimulatorDriver class.
        /// </summary>
        public SimulatorDriver()
            : this(".")
        { }

        /// <summary>
        /// Initializes a new instance of the SimulatorDriver class.
        /// </summary>
        public SimulatorDriver(SessionModel sessionModel)
            : this(".", sessionModel)
        { }

        /// <summary>
        /// Initializes a new instance of the SimulatorDriver class.
        /// </summary>
        /// <param name="driverBinaries">The full path to the directory containing driver executables.</param>
        public SimulatorDriver(string driverBinaries)
            : this(driverBinaries, new SessionModel())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorDriver"/> class.
        /// </summary>
        /// <param name="driverBinaries">The full path to the directory containing driver executables.</param>
        /// <param name="sessionModel">A collection containing the desired capabilities of this <see cref="SimulatorDriver"/>.</param>
        public SimulatorDriver(string driverBinaries, SessionModel sessionModel)
        {
            // setup
            SetChildWindows(sessionModel.Capabilities.AlwaysMatch);

            // state
            WindowHandle = _windowHandles[0];
            Session = new SessionIdModel($"simulator-{Guid.NewGuid()}");
            DriverBinaries = driverBinaries;
            Capabilities = new SimulatorOptions().ConvertToCapabilities();
            Invoker = new SimulatorCommandInvoker();

            foreach (var capability in sessionModel.Capabilities.AlwaysMatch)
            {
                Capabilities.AlwaysMatch[capability.Key] = capability.Value;
            }

            _navigation = new SimulatorNavigation(this);
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets a value indicating whether the on-screen keyboard is currently visible.
        /// </summary>
        /// <remarks>This property checks the device capabilities to determine if the keyboard is shown.</remarks>
        public bool IsOnScreenKeyboardVisible
        {
            get
            {
                // Check if the "IsKeyboardShown" capability is available in the device capabilities.
                var isCapability = Capabilities
                    .AlwaysMatch
                    .TryGetValue("IsKeyboardShown", out object keyboardShownOut);

                // Return true if the capability is available and the keyboard is shown, otherwise, return false.
                return isCapability && keyboardShownOut is bool keyboardShown && keyboardShown;
            }
        }

        /// <summary>
        /// Gets or sets the URL the browser is currently displaying.
        /// </summary>
        public string Url
        {
            get => Navigate().Url;
            set => Navigate().Open(value);
        }

        /// <summary>
        /// Gets the source of the page last loaded by the browser.
        /// </summary>
        public string PageSource { get; set; } = new StringBuilder()
            .Append("<html>")
            .Append("   <head>".Trim())
            .Append("       <title>mock page source title</title>")
            .Append("   </head>".Trim())
            .Append("   <body class=\"mockClass\">".Trim())
            .Append("       <div id=\"mockDiv\">mock div text</div>".Trim())
            .Append("       <positive>mock div text 1</positive>".Trim())
            .Append("       <positive>mock div text 2</positive>".Trim())
            .Append("       <positive>mock div text 3</positive>".Trim())
            .Append("   </body>".Trim())
            .Append("</html>")
            .ToString();

        /// <summary>
        /// Gets the current window handle, which is an opaque handle to this window that
        /// uniquely identifies it within this driver instance.
        /// </summary>
        public string WindowHandle { get; set; }

        /// <summary>
        /// Get the current driver binaries location.
        /// </summary>
        public string DriverBinaries { get; }

        /// <summary>
        /// Gets the window handles of open browser windows.
        /// </summary>
        public ReadOnlyCollection<string> WindowHandles => GetWindowHandles();

        /// <summary>
        /// Gets the session ID of the WebDriverServer session.
        /// </summary>
        public SessionIdModel Session { get; set; }

        /// <summary>
        /// Gets a value indicating whether this object is a valid action executor.
        /// </summary>
        public bool IsInvoker { get; } = true;

        /// <summary>
        /// Gets or set an implementation of IWebDriverCapabilities that is used to
        /// communicate the features supported by a given driver implementation.
        /// </summary>
        public CapabilitiesModel Capabilities { get; set; }

        /// <summary>
        /// Gets or sets a list of valid locator values. When passing these values with any locator,
        /// a positive element will be returned even not element method found, otherwise will
        /// throw <see cref="NoSuchElementException"/>.
        /// </summary>
        public IEnumerable<string> LocatorsWhiteList { get; set; } =
        [
            "//div",
            "//span",
            "//p"
        ];

        /// <summary>
        /// Gets or sets an implementation of IWebDriverCommandInvoker that provides a way to send commands to the remote end.
        /// </summary>
        public IWebDriverCommandInvoker Invoker { get; }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Close the current window, quitting the browser if it is the last window currently open.
        /// </summary>
        public void Close()
        {
            // exception conditions
            var isKey = Capabilities.AlwaysMatch.ContainsKey(SimulatorCapabilities.ThrowOnClose);
            var isException = isKey && (bool)Capabilities.AlwaysMatch[SimulatorCapabilities.ThrowOnClose];
            if (isException)
            {
                throw new WebDriverException();
            }

            // exit conditions
            if (_windowHandles.Count == 0)
            {
                return;
            }

            // remove current windows
            var windowHandles = _windowHandles.ToList();
            windowHandles.Remove(WindowHandle);
            _windowHandles = new ReadOnlyCollection<string>(windowHandles);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        /// <param name="disposing">True to perform cleanup associated tasks.</param>
        protected virtual void Dispose(bool disposing)
        {
            _windowHandles = new ReadOnlyCollection<string>([]);
        }

        /// <summary>
        /// Executes JavaScript asynchronously in the context of the currently selected frame
        /// or window.
        /// </summary>
        /// <param name="script">The JavaScript code to execute.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>The value returned by the script.</returns>
        public object InvokeAsyncScript(string script, params object[] args) => InvokeScript(args, script);

        /// <summary>
        /// Executes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">The JavaScript code to execute.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>The value returned by the script.</returns>
        public object InvokeScript(string script, params object[] args)
        {
            return InvokeScript(args, script);
        }

        public T InvokeScript<T>(string script, params object[] args)
        {
            throw new NotImplementedException();
        }

        public object InvokeScript(PinnedScriptModel script, params object[] args)
        {
            throw new NotImplementedException();
        }

        // Provides methods for invoking scripts with WebDriver interactions.
        private object InvokeScript(IEnumerable<object> args, string script)
        {
            // Check for null arguments and throw exception if any are found
            if (args.Any() && args.Any(i => i == null))
            {
                throw new WebDriverException("One or more script arguments are null.");
            }

            // Specify RegexOptions for the regular expression matching
            const RegexOptions RegexOptions = RegexOptions.Singleline | RegexOptions.IgnoreCase;

            // Retrieve all methods within the current class
            var instanceMethods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic) ?? [];
            var staticMethods = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic) ?? [];
            var methods = instanceMethods.Concat(staticMethods)
                .Where(i => i.GetCustomAttribute<DescriptionAttribute>() != null);

            // Find a method whose description matches the script pattern
            var method = methods.FirstOrDefault(i =>
                Regex.IsMatch(script, i.GetCustomAttribute<DescriptionAttribute>().Description, RegexOptions));

            // If no matching method is found, throw an exception
            if (method == default)
            {
                throw new WebDriverException("No matching script method found.");
            }

            try
            {
                // Get the arguments for the script method
                var arguments = GetScriptMethodArguments(method, script, args);

                // Create an instance if method is not static, otherwise use null
                var instance = method.IsStatic ? null : this;

                // Invoke the method with the provided arguments
                return method.Invoke(instance, arguments);
            }
            catch (Exception e)
            {
                // Throw the base exception of the caught exception
                throw e.GetBaseException();
            }
        }

        // Gets the arguments to be passed to a script method based on the method's parameters and provided arguments.
        private static object[] GetScriptMethodArguments(MethodInfo method, string script, params object[] args)
        {
            // Get the parameters of the script method.
            var parameters = method.GetParameters();

            // Initialize the arguments array with an empty array.
            var arguments = Array.Empty<object>();

            // Check if the script method has a single parameter of type string.
            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
            {
                arguments = [script];
            }

            // Check if the script method has a single parameter of type object[].
            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(object[]))
            {
                arguments = args;
            }

            // Check if the script method has two parameters, one of type string and the other of type object[].
            if (parameters.Length == 2
                && parameters[0].ParameterType == typeof(string)
                && parameters[1].ParameterType == typeof(object[]))
            {
                arguments = [script, args];
            }

            // Return the array of objects representing the arguments for the script method.
            return arguments;
        }

        /// <summary>
        /// Finds the first OpenQA.Selenium.IWebElement using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>The first matching OpenQA.Selenium.IWebElement on the current context.</returns>
        public IWebElement FindElement(By by) => SimulatorElement.GetElement(this, by);

        /// <summary>
        /// Finds all OpenQA.Selenium.IWebElement within the current context using the given
        /// mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns> A collections of all <see cref="IWebElement"/> matching the current criteria, or an empty list if nothing matches.</returns>
        public IEnumerable<IWebElement> FindElements(By by)
        {
            try
            {
                return SimulatorElement.GetElements(this, by);
            }
            catch (Exception e) when (e is NoSuchElementException)
            {
                return new ReadOnlyCollection<IWebElement>([]);
            }
        }

        /// <summary>
        /// Instructs the driver to change its settings.
        /// </summary>
        /// <returns> An <see cref="IOptions"/> object allowing the user to change the settings of the driver.</returns>
        public IOptions Manage() => _options ??= new SimulatorOptionsManager(new RectModel
        {
            X = _leftPosition,
            Y = _topPosition
        });

        /// <summary>
        /// Instructs the driver to navigate the browser to another location.
        /// </summary>
        /// <returns>An <see cref="INavigation" /> object allowing the user to access the browser's history and to navigate to a given URL.</returns>
        public INavigation Navigate() => _navigation;

        /// <summary>
        /// Quits this driver, closing every associated window.
        /// </summary>
        public void Quit()
        {
            var isKey = Capabilities.AlwaysMatch.ContainsKey(SimulatorCapabilities.ThrowOnClose);
            var isException = isKey && (bool)Capabilities.AlwaysMatch[SimulatorCapabilities.ThrowOnClose];
            if (isException)
            {
                throw new WebDriverException();
            }
            _windowHandles = new ReadOnlyCollection<string>([]);
        }

        /// <summary>
        /// Instructs the driver to send future commands to a different frame or window.
        /// </summary>
        /// <returns>An <see cref="ITargetLocator" /> object which can be used to select a frame or window.</returns>
        public ITargetLocator SwitchTo() => new SimulatorTargetLocator(this);

        /// <summary>
        /// Performs the specified list of actions with this action executor.
        /// </summary>
        /// <param name="sequence">The list of action sequences to perform.</param>
        public void InvokeActions(IEnumerable<ActionSequence> sequence)
        {
            // mock method - should not do anything
        }

        /// <summary>
        /// Resets the input state of the action executor.
        /// </summary>
        public void ResetInputState()
        {
            // mock method - should not do anything
        }

        /// <inheritdoc />
        public ScreenshotModel GetScreenshot() => new SimulatorScreenshot().GetScreenshot();

        /// <inheritdoc />
        public GeolocationModel GetGeolocation()
        {
            // Create a random geolocation if not set
            var randomGeolocation = new GeolocationModel
            {
                Altitude = new Random().NextDouble(),
                Latitude = new Random().NextDouble(),
                Longitude = new Random().NextDouble(),
            };

            // Return the current geolocation or the random one if not set
            return _geolocationModel ?? randomGeolocation;
        }

        /// <inheritdoc />
        public void SetGeolocation(GeolocationModel geolocationModel)
        {
            // Set the geolocation to the specified value
            _geolocationModel = geolocationModel;
        }

        /// <inheritdoc />
        public void HideKeyboard()
        {
            // This method is used in a simulator for keyboard hiding simulation.
            // No actual action is performed.
        }

        /// <inheritdoc />
        public void HideKeyboard(string strategy)
        {
            // This method is used in a simulator for keyboard hiding simulation.
            // No actual action is performed.
        }

        /// <inheritdoc />
        public void HideKeyboard(string strategy, string keyName)
        {
            // This method is used in a simulator for keyboard hiding simulation.
            // No actual action is performed.
        }

        /// <inheritdoc />
        public void HideKeyboard(HideKeyboardModel model)
        {
            // This method is used in a simulator for keyboard hiding simulation.
            // No actual action is performed.
        }

        /// <inheritdoc />
        public void ShowKeyboard()
        {
            // This method is used in a simulator for keyboard hiding simulation.
            // No actual action is performed.
        }

        // sets child windows based on provided capabilities
        private void SetChildWindows(IDictionary<string, object> capabilities)
        {
            // normalize number of child windows
            int windows = 0;
            if (capabilities.TryGetValueCaseInsensitive(SimulatorCapabilities.ChildWindows, out object capabilityOut))
            {
                _ = int.TryParse($"{capabilityOut}", out windows);
            }

            // setup window handles
            var windowHandles = new List<string> { $"window-{Guid.NewGuid()}" };

            // add handles for capabilities
            for (int i = 0; i < windows; i++)
            {
                windowHandles.Add($"window-{Guid.NewGuid()}");
            }

            // return a new collection with all handles
            _windowHandles = new ReadOnlyCollection<string>(windowHandles);
        }

        // Gets the window handles, throwing a WebDriverException if the "ExceptionOnHandles" capability is set.
        private ReadOnlyCollection<string> GetWindowHandles()
        {
            // Check if the "ExceptionOnHandles" capability is set in AlwaysMatch.
            // Throw a WebDriverException if the capability is set.
            if (Capabilities?.AlwaysMatch?.ContainsKey("ExceptionOnHandles") == true)
            {
                throw new WebDriverException();
            }

            // Return the window handles if the capability is not set.
            return _windowHandles;
        }

        // Creates a new window handle and sets it as the current window handle.
        internal void NewWindowHandle()
        {
            // Cast the driver to SimulatorDriver (assuming SimulatorDriver is the actual driver type)
            var driver = this;

            // Get the list of existing window handles
            var list = _windowHandles.ToList();

            // Generate a new window handle using Guid.NewGuid()
            var newHandle = $"window-{Guid.NewGuid()}";

            // Add the new window handle to the list
            list.Add(newHandle);

            // Update the list of window handles in the driver
            _windowHandles = new ReadOnlyCollection<string>(list);

            // Set the current window handle to the new one
            driver.WindowHandle = newHandle;
        }

        // Retrieves the outer HTML script result based on the provided script.
        [Description(@"^return arguments\[\d+\].outerHTML;$")]
        private static string GetOuterHtmlScriptResult(string script)
        {
            // Extract the number of elements from the script using regular expression.
            var numberOfElements = Regex.Match(script, @"\d+").Value;
            _ = int.TryParse(numberOfElements, out int numberOfElementsOut);
            numberOfElementsOut = numberOfElementsOut <= 0 ? 2 : numberOfElementsOut;

            // Generate a list of mock elements based on the extracted number.
            var elements = new List<string>();
            for (int i = 0; i < numberOfElementsOut; i++)
            {
                elements.Add("<positive mock-attribute=\"mock attribute value\">mock element nested inner-text</positive>");
            }

            // Construct and return the outer HTML result.
            return "<div mock-attribute=\"mock attribute value\">mock element inner-text" +
                   string.Concat(elements) +
                   "</div>";
        }

        // Retrieves the result of the "scriptMacro" script.
        [Description("scriptMacro")]
        [SuppressMessage("Minor Code Smell", "S3400:Methods should not return constants", Justification = "This method is necessary for script execution.")]
        private static string GetMacroScriptResult() => "some text and number 777";

        // Retrieves a random result for the "readyState" script.
        [Description("readyState")]
        private static string GetRandomReadyStateScriptResult()
        {
            // Generate a random number between 0 and 100.
            var random = new Random().Next(0, 100);

            // Determine the readyState based on the generated random number.
            if (random < 20)
            {
                return "uninitialized";
            }
            if (random > 20 && random < 40)
            {
                return "loading";
            }
            if (random >= 40 && random < 60)
            {
                return "loaded";
            }
            if (random >= 60 && random < 80)
            {
                return "interactive";
            }

            // Default to "complete" if the random number is not within the specified ranges.
            return "complete";
        }

        // Retrieves the result for the script that matches the pattern ".*invalid.*" and throws a WebDriverException.
        [Description(".*invalid.*")]
        private static string GetInvalidExceptionScriptResult() => throw new WebDriverException();

        // Retrieves the result for various scripts, returning an empty string for matching patterns.
        [Description(@"^$|unitTesting|arguments\[\d+\]\.(?!scroll)(?!(.*invalid.*))|^document.forms.*.submit.*.;$")]
        private static string GetEmptyScriptResult() => string.Empty;

        // Retrieves the result for the script starting with "window.scroll".
        [Description(@"^window\.scroll")]
        private string GetScrollScriptResult(string script)
        {
            // Extract the Y and X positions from the script using predefined patterns.
            var y = GetTopPattern().Match(script).Value;
            var x = GetLeftPattern().Match(script).Value;

            // Parse the Y and X positions, updating class fields _topPosition and _leftPosition.
            _ = int.TryParse(y, out _topPosition);
            _ = int.TryParse(x, out _leftPosition);

            // Return an empty string as the result.
            return string.Empty;
        }

        // Retrieves the result for the script returning the scroll position of an element.
        [Description(@"^return arguments\[0]\.scroll(Top|Left);$")]
        private static string GetScrollScriptResult(string script, object[] args)
        {
            // Initialize default values for scroll positions.
            var x = "0";
            var y = "0";

            // Check if arguments are provided and the first argument is of type SimulatorElement.
            if (args?.Length > 0 && args[0].GetType() == typeof(SimulatorElement))
            {
                // Retrieve scroll positions from the SimulatorElement.
                x = ((SimulatorElement)args[0]).GetAttribute("scrollLeft");
                y = ((SimulatorElement)args[0]).GetAttribute("scrollTop");
            }

            // Return the requested scroll position based on the presence of "scrollLeft" in the script.
            return script.Contains("scrollLeft") ? x : y;
        }

        // Retrieves the result for the script starting with "arguments[0].scroll".
        [Description(@"^arguments\[0]\.scroll")]
        private static string GetScrollElementScriptResult(string script, object[] args)
        {
            // Extract the Y and X positions from the script using predefined patterns.
            var y = GetTopPattern().Match(script).Value;
            var x = GetLeftPattern().Match(script).Value;

            // Check if arguments are provided and the first argument is of type SimulatorElement.
            if (args?.Length > 0 && args[0].GetType() == typeof(SimulatorElement))
            {
                // Update the attributes of the SimulatorElement with scroll positions.
                ((SimulatorElement)args[0]).Attributes = new Dictionary<string, string>
                {
                    ["scrollTop"] = y,
                    ["scrollLeft"] = x
                };
            }

            // Return an empty string as the result.
            return string.Empty;
        }

        // Retrieves the result for the script opening a new window with 'about:blank' or '_blank'.
        [Description(@"^window\.open\(('|"")about:blank('|""),(\s+)?('|"")_blank('|"")\);?$|^window\.open\(.*_blank.*\)$")]
        private string GetBlankPageScriptResult(string script)
        {
            // Get the list of existing window handles.
            var list = _windowHandles.ToList();

            // Generate a new window handle using Guid.NewGuid().
            var newHandle = $"window-{Guid.NewGuid()}";

            // Add the new window handle to the list.
            list.Add(newHandle);

            // Update the list of window handles in the driver.
            _windowHandles = new ReadOnlyCollection<string>(list);

            // Return the original script.
            return script;
        }
        #endregion
    }
}
