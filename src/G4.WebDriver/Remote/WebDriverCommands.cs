using G4.WebDriver.Attributes;
using G4.WebDriver.Models;

using System.Net.Http;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// A container for WebDriver commands.
    /// </summary>
    [WebDriverCommandsContainer(nameof(WebDriverCommands))]
    public static class WebDriverCommands
    {
        #region *** Driver : Delete ***
        /// <summary>
        /// WebDriver command to close the currently focused browser window.
        /// This command uses the DELETE method to send the window close request to the WebDriver endpoint.
        /// The route specifies the path for the command in the WebDriver endpoint and requires
        /// the session parameter to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(CloseWindow))]
        public static WebDriverCommandModel CloseWindow => new()
        {
            Method = HttpMethod.Delete,
            Route = "/session/$[session]/window"
        };

        /// <summary>
        /// WebDriver command to delete all input actions in the current input sequence.
        /// This command uses the DELETE method to remove the recorded input actions for the current session.
        /// The route specifies the path for the command in the WebDriver endpoint and requires
        /// the session parameter to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(DeleteActions))]
        public static WebDriverCommandModel DeleteActions => new()
        {
            Method = HttpMethod.Delete,
            Route = "/session/$[session]/actions"
        };

        /// <summary>
        /// Gets the command to delete all cookies from the browser's cookie store.
        /// </summary>
        [WebDriverCommand(nameof(DeleteCookies))]
        public static WebDriverCommandModel DeleteCookies => new()
        {
            Method = HttpMethod.Delete,
            Route = "/session/$[session]/cookie"
        };

        /// <summary>
        /// Gets the command to delete a named cookie from the browser's cookie store.
        /// </summary>
        [WebDriverCommand(nameof(DeleteNamedCookie))]
        public static WebDriverCommandModel DeleteNamedCookie => new()
        {
            Method = HttpMethod.Delete,
            Route = "/session/$[session]/cookie/$[name]"
        };

        /// <summary>
        /// WebDriver command to delete the current browser session.
        /// This command uses the DELETE method to terminate the current browser session.
        /// The route specifies the path for the command in the WebDriver endpoint and requires
        /// the session parameter to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(DeleteSession))]
        public static WebDriverCommandModel DeleteSession => new()
        {
            Method = HttpMethod.Delete,
            Route = "/session/$[session]"
        };
        #endregion

        #region *** Driver : Get    ***
        /// <summary>
        /// WebDriver command to retrieve the text content of an alert dialog.
        /// This command uses the GET method to fetch the text from the alert dialog associated with the current browser session.
        /// The route specifies the path for the command in the WebDriver endpoint and requires
        /// the session parameter to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(GetAlertText))]
        public static WebDriverCommandModel GetAlertText => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/alert/text"
        };

        /// <summary>
        /// WebDriver command to retrieve the currently active element in the current browser session.
        /// This command uses the GET method to fetch information about the currently focused element.
        /// The route specifies the path for the command in the WebDriver endpoint and requires
        /// the session parameter to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(GetActiveElement))]
        public static WebDriverCommandModel GetActiveElement => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/active"
        };

        /// <summary>
        /// WebDriver command to retrieve all cookies of the current browser session.
        /// This command uses the GET method to fetch information about cookies associated with the current session.
        /// The route specifies the path for the command in the WebDriver endpoint and requires
        /// the session parameter to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(GetCookies))]
        public static WebDriverCommandModel GetCookies => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/cookie"
        };

        /// <summary>
        /// Defines the WebDriver command to retrieve log entries of a specific type from the WebDriver session.
        /// </summary>
        [WebDriverCommand(nameof(GetLog))]
        public static WebDriverCommandModel GetLog => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/log"
        };

        /// <summary>
        /// Defines the WebDriver command to retrieve the available log types from the WebDriver session.
        /// </summary>
        [WebDriverCommand(nameof(GetLogTypes))]
        public static WebDriverCommandModel GetLogTypes => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/log/types"
        };

        /// <summary>
        /// Gets the command to retrieve a named cookie from the browser's cookie store.
        /// </summary>
        [WebDriverCommand(nameof(GetNamedCookie))]
        public static WebDriverCommandModel GetNamedCookie => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/cookie/$[name]"
        };

        /// <summary>
        /// WebDriver command to retrieve the source code of the current page.
        /// This command uses the GET method to fetch the page source.
        /// The route specifies the path for the command in the WebDriver endpoint and requires
        /// the session parameter to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(GetPageSource))]
        public static WebDriverCommandModel GetPageSource => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/source"
        };

        /// <summary>
        /// WebDriver command to retrieve the status of the WebDriver server.
        /// This command uses the GET method to fetch the server status.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(GetStatus))]
        public static WebDriverCommandModel GetStatus => new()
        {
            Method = HttpMethod.Get,
            Route = "/status"
        };

        /// <summary>
        /// WebDriver command to retrieve the current timeouts of the WebDriver session.
        /// This command uses the GET method to fetch the session timeouts.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including the session ID placeholder.
        /// </summary>
        [WebDriverCommand(nameof(GetTimeouts))]
        public static WebDriverCommandModel GetTimeouts => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/timeouts"
        };

        /// <summary>
        /// WebDriver command to retrieve the title of the current page in the WebDriver session.
        /// This command uses the GET method to fetch the page title.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including the session ID placeholder.
        /// </summary>
        [WebDriverCommand(nameof(GetTitle))]
        public static WebDriverCommandModel GetTitle => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/title"
        };

        /// <summary>
        /// WebDriver command to retrieve the current URL of the browser session.
        /// This command uses the GET method to fetch the current URL.
        /// The route specifies the path for the command in the WebDriver endpoint and requires
        /// the session parameter to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(GetUrl))]
        public static WebDriverCommandModel GetUrl => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/url"
        };

        /// <summary>
        /// WebDriver command to retrieve the handle of the current window in the WebDriver session.
        /// This command uses the GET method to fetch the window handle.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including the session ID placeholder.
        /// </summary>
        [WebDriverCommand(nameof(GetWindowHandle))]
        public static WebDriverCommandModel GetWindowHandle => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/window"
        };

        /// <summary>
        /// WebDriver command to retrieve the handles of all currently open windows in the WebDriver session.
        /// This command uses the GET method to fetch the window handles.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including the session ID placeholder.
        /// </summary>
        [WebDriverCommand(nameof(GetWindowHandles))]
        public static WebDriverCommandModel GetWindowHandles => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/window/handles"
        };

        /// <summary>
        /// WebDriver command to retrieve the current position and size of the browser window in the WebDriver session.
        /// This command uses the GET method to fetch the window rectangle.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including the session ID placeholder.
        /// </summary>
        [WebDriverCommand(nameof(GetWindowRect))]
        public static WebDriverCommandModel GetWindowRect => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/window/rect"
        };
        #endregion

        #region *** Driver : Post   ***
        /// <summary>
        /// WebDriver command to perform a sequence of pointer and keyboard actions.
        /// This command uses the POST method to initiate a sequence of actions in the WebDriver session.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including the session ID placeholder.
        /// </summary>
        [WebDriverCommand(nameof(Actions))]
        public static WebDriverCommandModel Actions => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/actions"
        };

        /// <summary>
        /// Gets the command to add a cookie to the browser's cookie store.
        /// </summary>
        [WebDriverCommand(nameof(AddCookie))]
        public static WebDriverCommandModel AddCookie => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/cookie"
        };

        /// <summary>
        /// Represents a WebDriver command model for accepting an alert dialog.
        /// </summary>
        [WebDriverCommand(nameof(AlertAccept))]
        public static WebDriverCommandModel AlertAccept => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/alert/accept",
        };

        /// <summary>
        /// WebDriver command to dismiss the currently displayed alert.
        /// This command uses the POST method to dismiss the alert in the WebDriver session.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including the session ID placeholder.
        /// </summary>
        [WebDriverCommand(nameof(AlertDismiss))]
        public static WebDriverCommandModel AlertDismiss => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/alert/dismiss",
        };

        /// <summary>
        /// WebDriver command to navigate back in the browser history.
        /// This command uses the POST method to trigger the browser to navigate back.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including the session ID placeholder.
        /// </summary>
        [WebDriverCommand(nameof(Back))]
        public static WebDriverCommandModel Back => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/back",
        };

        /// <summary>
        /// WebDriver command to clear the content of an element.
        /// This command uses the POST method to trigger the browser to clear the content of the specified element.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including the session and element ID placeholders.
        /// </summary>
        [WebDriverCommand(nameof(ClearElement))]
        public static WebDriverCommandModel ClearElement => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/element/$[element]/clear",
        };

        /// <summary>
        /// WebDriver command to simulate a click action on a specified element within the browser.
        /// This command uses the POST method to send the click action request to the WebDriver endpoint.
        /// The route specifies the path for the command in the WebDriver endpoint and requires
        /// the session and element parameters to identify the specific browser session and the target element.
        /// </summary>
        [WebDriverCommand(nameof(ClickElement))]
        public static WebDriverCommandModel ClickElement => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/element/$[element]/click",
        };

        /// <summary>
        /// WebDriver command to execute a synchronous script on the browser.
        /// This command allows the execution of custom JavaScript code within the browser session.
        /// It uses the POST method to send the script to the WebDriver endpoint for synchronous execution.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// The command requires the session parameter to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(InvokeScript))]
        public static WebDriverCommandModel InvokeScript => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/execute/sync",
        };

        /// <summary>
        /// WebDriver command to execute an asynchronous JavaScript script on the browser.
        /// This command allows the execution of custom JavaScript code asynchronously within the browser session.
        /// It uses the POST method to send the script to the WebDriver endpoint for asynchronous execution.
        /// The route specifies the path for the command in the WebDriver endpoint and requires the session parameter 
        /// to identify the specific browser session.
        /// </summary>
        [WebDriverCommand(nameof(InvokeScriptAsync))]
        public static WebDriverCommandModel InvokeScriptAsync => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/execute/async",
        };

        /// <summary>
        /// WebDriver command to find an element.
        /// This command uses the POST method to locate an element based on the provided criteria.
        /// The route specifies the path for the command in the WebDriver endpoint, including the session placeholder.
        /// </summary>
        [WebDriverCommand(nameof(FindElement))]
        public static WebDriverCommandModel FindElement => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/element"
        };

        /// <summary>
        /// WebDriver command to find elements within a shadow root.
        /// This command is used to locate and interact with elements that exist within the shadow DOM (Document Object Model).
        /// The command requires the session and shadow parameters to identify the specific shadow root and its elements.
        /// The method used is POST, and the route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(FindElementFromShadowRoot))]
        public static WebDriverCommandModel FindElementFromShadowRoot => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/shadow/$[shadow]/element"
        };

        /// <summary>
        /// WebDriver command to find multiple elements.
        /// This command uses the POST method to locate elements based on the provided criteria.
        /// The route specifies the path for the command in the WebDriver endpoint, including the session placeholder.
        /// </summary>
        [WebDriverCommand(nameof(FindElements))]
        public static WebDriverCommandModel FindElements => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/elements"
        };

        /// <summary>
        /// WebDriver command to find multiple elements within a shadow root.
        /// This command uses the POST method to locate elements based on the provided criteria within a specified shadow root.
        /// The route specifies the path for the command in the WebDriver endpoint, including the session and shadow placeholders.
        /// </summary>
        [WebDriverCommand(nameof(FindElementsFromShadowRoot))]
        public static WebDriverCommandModel FindElementsFromShadowRoot => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/shadow/$[shadow]/elements"
        };

        /// <summary>
        /// WebDriver command to navigate forward in the browser history.
        /// This command uses the POST method to navigate forward within the current session.
        /// The route specifies the path for the command in the WebDriver endpoint, including the session placeholder.
        /// </summary>
        [WebDriverCommand(nameof(Forward))]
        public static WebDriverCommandModel Forward => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/forward"
        };

        /// <summary>
        /// WebDriver command to maximize the browser window to full screen.
        /// This command uses the POST method to set the window to full screen within the current session.
        /// The route specifies the path for the command in the WebDriver endpoint, including the session placeholder.
        /// </summary>
        [WebDriverCommand(nameof(FullScreenWindow))]
        public static WebDriverCommandModel FullScreenWindow => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/window/fullscreen"
        };

        /// <summary>
        /// WebDriver command to capture a screenshot of the current browser window.
        /// This command uses the GET method to obtain a screenshot within the current session.
        /// The route specifies the path for the command in the WebDriver endpoint, including the session placeholder.
        /// </summary>
        [WebDriverCommand(nameof(GetScreenshot))]
        public static WebDriverCommandModel GetScreenshot => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/screenshot"
        };

        /// <summary>
        /// WebDriver command to maximize the current browser window.
        /// This command uses the POST method to maximize the window within the current session.
        /// The route specifies the path for the command in the WebDriver endpoint, including the session placeholder.
        /// </summary>
        [WebDriverCommand(nameof(MaximizeWindow))]
        public static WebDriverCommandModel MaximizeWindow => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/window/maximize"
        };

        /// <summary>
        /// WebDriver command to minimize the current browser window.
        /// This command uses the POST method to minimize the window within the current session.
        /// The route specifies the path for the command in the WebDriver endpoint, including the session placeholder.
        /// </summary>
        [WebDriverCommand(nameof(MinimizeWindow))]
        public static WebDriverCommandModel MinimizeWindow => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/window/minimize"
        };

        /// <summary>
        /// WebDriver command to navigate to a specified URL.
        /// This command uses the POST method to navigate to the provided URL within the current session.
        /// The route specifies the path for the command in the WebDriver endpoint, including the session placeholder.
        /// </summary>
        [WebDriverCommand(nameof(NavigateTo))]
        public static WebDriverCommandModel NavigateTo => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/url"
        };

        /// <summary>
        /// WebDriver command to create a new session.
        /// This command uses the POST method to initiate a new session.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(NewSession))]
        public static WebDriverCommandModel NewSession => new()
        {
            Method = HttpMethod.Post,
            Route = "/session"
        };

        /// <summary>
        /// WebDriver command to open a new browser window within the current session.
        /// This command uses the POST method to create a new window.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(NewWindow))]
        public static WebDriverCommandModel NewWindow => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/window/new"
        };

        /// <summary>
        /// WebDriver command to refresh the current browser window.
        /// This command uses the POST method to trigger a refresh.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(Refresh))]
        public static WebDriverCommandModel Refresh => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/refresh"
        };

        /// <summary>
        /// WebDriver command model for sending text to an alert dialog.
        /// </summary>
        [WebDriverCommand(nameof(SendAlertText))]
        public static WebDriverCommandModel SendAlertText => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/alert/text",
        };

        /// <summary>
        /// WebDriver command to set timeouts for various operations.
        /// This command uses the POST method to set timeouts.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(SetTimeouts))]
        public static WebDriverCommandModel SetTimeouts => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/timeouts"
        };

        /// <summary>
        /// WebDriver command to set the size and position of the browser window.
        /// This command uses the POST method to set the window rectangle.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(SetWindowRect))]
        public static WebDriverCommandModel SetWindowRect => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/window/rect"
        };

        /// <summary>
        /// WebDriver command to switch to a specific frame within the current session.
        /// This command uses the POST method to switch to the specified frame.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(SwitchToFrame))]
        public static WebDriverCommandModel SwitchToFrame => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/frame"
        };

        /// <summary>
        /// WebDriver command to switch back to the parent frame within the current session.
        /// This command uses the POST method to switch back to the parent frame.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(SwitchToParentFrame))]
        public static WebDriverCommandModel SwitchToParentFrame => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/frame/parent"
        };

        /// <summary>
        /// WebDriver command to switch to a specific window within the current session.
        /// This command uses the POST method to switch to a window.
        /// The route specifies the path for the command in the WebDriver endpoint.
        /// </summary>
        [WebDriverCommand(nameof(SwitchToWindow))]
        public static WebDriverCommandModel SwitchToWindow => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/window"
        };
        #endregion

        #region *** Element: Get    ***
        /// <summary>
        /// WebDriver command to retrieve the value of a specified attribute of an element.
        /// This command uses the GET method to get the element attribute.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session, element, and attribute name.
        /// </summary>
        [WebDriverCommand(nameof(GetElementAttribute))]
        public static WebDriverCommandModel GetElementAttribute => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/attribute/$[attributeName]"
        };

        /// <summary>
        /// WebDriver command to retrieve the computed label of an element.
        /// This command uses the GET method to get the computed label of the element.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel GetElementComputedLabel => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/computedlabel"
        };

        /// <summary>
        /// WebDriver command to retrieve the computed role of an element.
        /// This command uses the GET method to get the computed role of the element.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel GetElementComputedRole => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/computedrole"
        };

        /// <summary>
        /// WebDriver command to retrieve the computed CSS value of an element's property.
        /// This command uses the GET method to get the computed CSS value of the specified property for the element.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session, element, and property name.
        /// </summary>
        public static WebDriverCommandModel GetElementCssValue => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/css/$[propertyName]"
        };

        /// <summary>
        /// WebDriver command to check if an element is enabled.
        /// This command uses the GET method to check the enabled status of the element.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel GetElementEnabled => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/enabled"
        };

        /// <summary>
        /// WebDriver command to get the value of a specified property of an element.
        /// This command uses the GET method to retrieve the property value.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session, element, and property name.
        /// </summary>
        public static WebDriverCommandModel GetElementProperty => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/property/$[propertyName]"
        };

        /// <summary>
        /// WebDriver command to check if an element is selected.
        /// This command uses the GET method to retrieve the selection status.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel GetElementSelected => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/selected"
        };

        /// <summary>
        /// WebDriver command to retrieve the shadow root of an element.
        /// This command uses the GET method to get the shadow root.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel GetElementShadowRoot => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/shadow"
        };

        /// <summary>
        /// WebDriver command to retrieve the rectangle properties of an element.
        /// This command uses the GET method to get the element's rectangle properties.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel GetElementRect => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/rect"
        };

        /// <summary>
        /// WebDriver command to retrieve the tag name of an element.
        /// This command uses the GET method to get the element's tag name.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel GetElementTagName => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/name"
        };

        /// <summary>
        /// WebDriver command to retrieve the text content of an element.
        /// This command uses the GET method to get the element's text.
        /// The route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel GetElementText => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/text"
        };
        #endregion

        #region *** Element: Post   ***
        /// <summary>
        /// WebDriver command to find an element within another element.
        /// This command uses the POST method to perform the search,
        /// and the route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel FindElementFromElement => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/element/$[element]/element"
        };

        /// <summary>
        /// WebDriver command to find multiple elements within another element.
        /// This command uses the POST method to perform the search,
        /// and the route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel FindElementsFromElement => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/element/$[element]/elements"
        };

        /// <summary>
        /// WebDriver command to simulate typing keys into an element.
        /// This command uses the POST method to send the keys,
        /// and the route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel SendKeys => new()
        {
            Method = HttpMethod.Post,
            Route = "/session/$[session]/element/$[element]/value"
        };

        /// <summary>
        /// WebDriver command to capture a screenshot of a specific element.
        /// This command uses the GET method to request the element screenshot,
        /// and the route specifies the path for the command in the WebDriver endpoint,
        /// including placeholders for the session and element.
        /// </summary>
        public static WebDriverCommandModel GetElementScreenshot => new()
        {
            Method = HttpMethod.Get,
            Route = "/session/$[session]/element/$[element]/screenshot"
        };
        #endregion
    }
}
