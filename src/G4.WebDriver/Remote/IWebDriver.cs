/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/IWebDriver.cs
 */
using System;
using System.Collections.ObjectModel;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Defines the interface through which the user controls the remote end.
    /// </summary>
    public interface IWebDriver : ISearchContext, IDisposable, IJavaScriptInvoker
    {
        #region *** Properties ***
        /// <summary>
        /// Gets or sets an implementation of IWebDriverCommandInvoker that provides a way to send commands to the remote end.
        /// </summary>
        IWebDriverCommandInvoker Invoker { get; }

        /// <summary>
        /// Gets the source of the page last loaded by the application.
        /// </summary>
        string PageSource { get; }

        /// <summary>
        /// Gets the current window handle, which is an opaque handle to this
        /// window that uniquely identifies it within this driver instance.
        /// </summary>
        string WindowHandle { get; }

        /// <summary>
        /// Gets the window handles of the open application windows.
        /// </summary>
        ReadOnlyCollection<string> WindowHandles { get; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Close the current window, quitting the application if it is the last window currently open.
        /// </summary>
        void Close();

        /// <summary>
        /// Quits this driver, closing every associated window.
        /// </summary>
        void Quit();

        /// <summary>
        /// Instructs the driver to change its settings.
        /// </summary>
        /// <returns>An IOptions object allowing the user to change the settings of the driver.</returns>
        IOptions Manage();

        /// <summary>
        /// Instructs the driver to navigate the application to another location.
        /// </summary>
        /// <returns>An INavigation object allowing the user to access the application history and to navigate to a given URL.</returns>
        INavigation Navigate();

        /// <summary>
        /// Instructs the driver to send future commands to a different frame or window.
        /// </summary>
        /// <returns>An ITargetLocator object which can be used to select a frame or window.</returns>
        ITargetLocator SwitchTo();
        #endregion
    }
}
