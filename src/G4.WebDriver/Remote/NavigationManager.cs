using G4.WebDriver.Extensions;

using System;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents a navigation manager that simplifies and encapsulates navigation
    /// operations using a WebDriver instance.
    /// </summary>
    public class NavigationManager(IWebDriver driver) : INavigation, IDriverReference
    {
        #region *** Fields     ***
        // Command invoker to send WebDriver commands to the browser.
        private readonly IWebDriverCommandInvoker _invoker = driver.Invoker;
        #endregion

        #region *** Properties ***
        /// <inheritdoc />
        public IWebDriver Driver { get; set; } = driver;

        /// <inheritdoc />
        public string Title
        {
            get
            {
                // Create a new WebDriver command for getting the title.
                var command = _invoker.NewCommand(nameof(WebDriverCommands.GetTitle));

                // Invoke the command and return the title as a string.
                return _invoker.Invoke(command).Value.ToString();
            }
        }

        /// <inheritdoc />
        public string Url
        {
            set => _invoker
                .Invoke(_invoker.NewCommand(nameof(WebDriverCommands.NavigateTo)).SetData(new
                {
                    Url = value
                }));

            get => _invoker
                .Invoke(_invoker.NewCommand(nameof(WebDriverCommands.GetUrl)))
                .Value
                .ToString();
        }
        #endregion

        #region *** Methods    ***
        /// <inheritdoc />
        public void Open(string url)
        {
            Open(url: new Uri(uriString: url));
        }

        /// <inheritdoc />
        public void Open(Uri url)
        {
            // Create an anonymous object with URL data.
            var data = new
            {
                Url = url.AbsoluteUri
            };

            // Create a new WebDriver command for navigating to the URL.
            var command = _invoker
                .NewCommand(nameof(WebDriverCommands.NavigateTo))
                .SetData(data);

            // Invoke the command to perform the navigation.
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public void Redo()
        {
            // Create a new WebDriver command for navigating forward.
            var command = _invoker.NewCommand(nameof(WebDriverCommands.Forward));

            // Invoke the command to perform the forward navigation.
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public void Undo()
        {
            // Create a new WebDriver command for navigating back.
            var command = _invoker.NewCommand(nameof(WebDriverCommands.Back));

            // Invoke the command to perform the back navigation.
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public void Update()
        {
            // Create a new WebDriver command for refreshing the page.
            var command = _invoker.NewCommand(nameof(WebDriverCommands.Refresh));

            // Invoke the command to perform the page refresh.
            _invoker.Invoke(command);
        }
        #endregion
    }
}
