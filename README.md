# G4.WebDriver

[![NuGet](https://img.shields.io/nuget/v/G4.WebDriver?logo=nuget&logoColor=959da5&label=NuGet&labelColor=24292f)](https://www.nuget.org/packages/G4.WebDriver)
[![Build, Test & Release](https://github.com/g4-api/g4-web-driver-client/actions/workflows/release-pipline.yml/badge.svg)](https://github.com/g4-api/g4-web-driver-client/actions/workflows/release-pipline.yml)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://www.apache.org/licenses/LICENSE-2.0)

A from-scratch .NET WebDriver protocol client library for remotely controlling web browsers, mobile devices, and desktop applications. Built as an alternative to Selenium's .NET bindings with zero external runtime dependencies.

## Overview

G4.WebDriver communicates with browser/device driver processes (chromedriver, msedgedriver, geckodriver, Appium) using the W3C WebDriver protocol over HTTP. It provides a standardized `IWebDriver` interface implemented across multiple driver types:

| Driver | Package | Description |
| --- | --- | --- |
| `ChromeDriver` | `G4.WebDriver.Remote.Chrome` | Google Chrome automation |
| `EdgeDriver` | `G4.WebDriver.Remote.Edge` | Microsoft Edge automation |
| `FirefoxDriver` | `G4.WebDriver.Remote.Firefox` | Mozilla Firefox automation |
| `OperaDriver` | `G4.WebDriver.Remote.Opera` | Opera browser automation |
| `AndroidDriver` | `G4.WebDriver.Remote.Android` | Android device automation via Appium |
| `IosDriver` | `G4.WebDriver.Remote.Ios` | iOS device automation via Appium |
| `MacOsDriver` | `G4.WebDriver.Remote.MacOs` | macOS application automation via Appium |
| `UiaDriver` | `G4.WebDriver.Remote.Uia` | Windows desktop UI automation |
| `SapDriver` | `G4.WebDriver.Remote.Sap` | SAP GUI for Windows automation via SAP GUI Scripting |
| `SimulatorDriver` | `G4.WebDriver.Simulator` | Browser-less mock driver for unit testing |
| `RemoteWebDriver` | `G4.WebDriver` | Generic driver for any WebDriver-compatible endpoint |

## Features

- **W3C WebDriver Protocol** -- Full implementation of the standard protocol with 100+ commands
- **Zero External Dependencies** -- The core library uses only the .NET BCL (no third-party NuGet packages)
- **Multi-Browser Support** -- Chrome, Edge, Firefox, and Opera with driver process lifecycle management
- **Mobile Automation** -- Android, iOS, and macOS via Appium protocol
- **Desktop Automation** -- Windows UI Automation (UIA) for native desktop applications
- **SAP GUI Automation** -- SAP GUI for Windows through SAP GUI Scripting
- **Chrome DevTools Protocol** -- Direct CDP access on Chromium-based browsers
- **Simulator** -- Mock driver for unit testing without launching real browsers
- **Event-Driven Architecture** -- Pre/post command invocation hooks for logging, auditing, and extensions
- **.NET 10** -- Targets the latest framework for modern language and runtime features
- **31 Custom Exception Types** -- Matching all W3C WebDriver error codes

## Installation

Install the core package:

```bash
dotnet add package G4.WebDriver
```

Then add the driver package for your target browser:

```bash
dotnet add package G4.WebDriver.Remote.Chrome
dotnet add package G4.WebDriver.Remote.Edge
dotnet add package G4.WebDriver.Remote.Firefox
dotnet add package G4.WebDriver.Remote.Opera
```

For mobile/desktop automation:

```bash
dotnet add package G4.WebDriver.Remote.Appium
dotnet add package G4.WebDriver.Remote.Android
dotnet add package G4.WebDriver.Remote.Ios
dotnet add package G4.WebDriver.Remote.MacOs
dotnet add package G4.WebDriver.Remote.Uia
dotnet add package G4.WebDriver.Remote.Sap
```

For unit testing without a real browser:

```bash
dotnet add package G4.WebDriver.Simulator
```

## Quick Start

```csharp
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Chrome;

// Create a Chrome driver (auto-starts chromedriver)
using var driver = new ChromeDriver();

// Navigate to a page
driver.Navigate().Open("https://example.com");

// Find elements using CSS selectors, XPath, and more
var heading = driver.FindElement(By.CssSelector("h1"));
Console.WriteLine(heading.Text);

var links = driver.FindElements(By.TagName("a"));
Console.WriteLine($"Found {links.Count()} links");

// Interact with elements
var button = driver.FindElement(By.Id("submit"));
button.Click();

// Execute JavaScript
var title = driver.InvokeScript("return document.title");

// Take a screenshot
var screenshot = driver.GetScreenshot();

// Cleanup
driver.Quit();
```

## Supported Platforms

| Platform | Driver Required | Package |
| --- | --- | --- |
| Google Chrome | `chromedriver` | `G4.WebDriver.Remote.Chrome` |
| Microsoft Edge | `msedgedriver` | `G4.WebDriver.Remote.Edge` |
| Mozilla Firefox | `geckodriver` | `G4.WebDriver.Remote.Firefox` |
| Opera | `operadriver` | `G4.WebDriver.Remote.Opera` |
| Android | Appium Server | `G4.WebDriver.Remote.Android` |
| iOS | Appium Server | `G4.WebDriver.Remote.Ios` |
| macOS | Appium Server | `G4.WebDriver.Remote.MacOs` |
| Windows Desktop | UIA WebDriver | `G4.WebDriver.Remote.Uia` |
| SAP GUI for Windows | SAP WebDriver (`Sap.DriverServer.exe`) | `G4.WebDriver.Remote.Sap` |
| Any (Remote) | Any WebDriver endpoint | `G4.WebDriver` |
| None (Mock) | N/A | `G4.WebDriver.Simulator` |

## Browser Drivers

### Chrome

```csharp
using G4.WebDriver.Remote.Chrome;

// Default -- starts a new Chrome session
using var driver = new ChromeDriver();

// With options
var options = new ChromeOptions();
options.Arguments = new[] { "--headless", "--no-sandbox", "--disable-gpu" };
options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
using var driver = new ChromeDriver(options);

// With custom chromedriver path
using var driver = new ChromeDriver(@"C:\drivers");

// With custom service, options, and timeout
var service = new ChromeWebDriverService(@"C:\drivers");
using var driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(30));

// Connect to a remote Chrome instance
using var driver = new ChromeDriver(new Uri("http://localhost:9515"), options);
```

### Edge

```csharp
using G4.WebDriver.Remote.Edge;

// Default
using var driver = new EdgeDriver();

// With options
var options = new EdgeOptions();
options.Arguments = new[] { "--inprivate" };
options.UseWebView = false;
using var driver = new EdgeDriver(options);

// With custom msedgedriver path
using var driver = new EdgeDriver(@"C:\drivers");

// With custom service, options, and timeout
var service = new EdgeWebDriverService(@"C:\drivers");
using var driver = new EdgeDriver(service, options, TimeSpan.FromSeconds(30));

// Connect to a remote Edge instance
using var driver = new EdgeDriver(new Uri("http://localhost:17556"), options);
```

### Firefox

```csharp
using G4.WebDriver.Remote.Firefox;

// Default
using var driver = new FirefoxDriver();

// With options
var options = new FirefoxOptions();
options.AddCapabilities("moz:firefoxOptions", new Dictionary<string, object>
{
    ["args"] = new[] { "-headless" },
    ["binary"] = @"C:\Program Files\Mozilla Firefox\firefox.exe"
});
using var driver = new FirefoxDriver(options);

// With custom geckodriver path
using var driver = new FirefoxDriver(@"C:\drivers");

// With custom service, options, and timeout
var service = new FirefoxWebDriverService(@"C:\drivers");
using var driver = new FirefoxDriver(service, options, TimeSpan.FromSeconds(30));

// Connect to a remote Firefox instance
using var driver = new FirefoxDriver(new Uri("http://localhost:4444"), options);
```

## SAP GUI Driver

The `SapDriver` drives SAP GUI for Windows through the SAP WebDriver server (`Sap.DriverServer.exe`). The
server attaches each WebDriver session to a SAP GUI session that is already open; it never starts SAP GUI or
logs on. SAP GUI Scripting must be enabled on the SAP server and in the SAP GUI options before you start; the
driver does not turn it on.

`SapOptions` chooses the SAP GUI session. Set `SessionPath`, or `ConnectionIndex` and `SessionIndex`, to pick
one directly. `SystemName`, `Client`, and `ConnectionDescription` narrow the candidates instead. With nothing
set, the server attaches to the only open session.

```csharp
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Sap;

// Attach to the first session of the first SAP GUI connection on a running server
var options = new SapOptions
{
    ConnectionIndex = 0,
    SessionIndex = 0
};

using var driver = new SapDriver(new Uri("http://localhost:5556"), options);

// Send Enter (virtual key 0) to the current window, then read the status bar
driver.SendVirtualKey(0);
var statusBar = driver.GetStatusBar();
Console.WriteLine($"{statusBar.MessageType}: {statusBar.Text}");

// Call an allow-listed SAP GUI Scripting method on a control
var grid = (SapElement)driver.FindElement(By.Xpath("//GuiShell[@subType='GridView']"));
grid.InvokeMethod("pressToolbarButton", "&MB_FILTER");

// Read the SAP object model document of the session
var document = driver.GetDocument();
```

**SAP-specific commands:**

| Member | Description |
| --- | --- |
| `SapDriver.SendVirtualKey` | Sends a SAP virtual key, such as 0 for Enter or 3 for Back, to a window |
| `SapDriver.GetStatusBar` | Returns the message type, ID, number, and text of the status bar |
| `SapDriver.GetDocument` | Returns the SAP object model XML of the session or of a located element |
| `SapElement.InvokeMethod` | Calls an allow-listed SAP GUI Scripting method on a control |
| `SapElement.SetProperty` | Sets an allow-listed SAP GUI Scripting property on a control |

The SAP server does not support raw User32 input, so `SapDriver` has no User32 commands.

## Simulator

The `SimulatorDriver` provides a browser-less mock implementation of `IWebDriver` for unit testing. It returns simulated page sources, elements, navigation, and JavaScript execution without launching a real browser.

```csharp
using G4.WebDriver.Models;
using G4.WebDriver.Simulator;

// Create a simulator with default settings
using var driver = new SimulatorDriver();

// Navigate (no-op, but sets internal state)
driver.Navigate().Open("https://example.com");

// Find elements (returns mock elements)
var element = driver.FindElement(By.CssSelector("div.mockClass"));

// Access page source (returns mock HTML)
string html = driver.PageSource;

// Configure behavior via session capabilities
var session = new SessionModel();
session.Capabilities.AlwaysMatch["ChildWindows"] = 1;
session.Capabilities.AlwaysMatch["ExceptionOnHandles"] = true;

using var driver = new SimulatorDriver(session);
```

**Configurable behaviors:**

| Capability | Effect |
| --- | --- |
| `ChildWindows` | Number of child windows to simulate |
| `ExceptionOnHandles` | Throw exception when accessing `WindowHandles` |
| `ThrowOnClose` | Throw exception on `Close()` or `Quit()` |
| `IsKeyboardShown` | Simulates on-screen keyboard visibility |

## RemoteWebDriver

Connect to any WebDriver-compatible endpoint (Selenium Grid, BrowserStack, or a standalone driver server):

```csharp
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Edge;

// Connect to a remote endpoint with Edge options
var options = new EdgeOptions();
using var driver = new RemoteWebDriver("http://localhost:4444", options);

// Navigate and interact
driver.Navigate().Open("https://example.com");
var element = driver.FindElement(By.TagName("h1"));
Console.WriteLine(element.Text);

driver.Quit();
```

## Element Location

The `By` class provides static factory methods for locating elements:

```csharp
// CSS Selector
var el = driver.FindElement(By.CssSelector("#login-form .submit-btn"));

// XPath
var el = driver.FindElement(By.Xpath("//div[@class='card']//a"));

// Tag Name
var headings = driver.FindElements(By.TagName("h1"));

// Link Text
var link = driver.FindElement(By.LinkText("Sign In"));

// Partial Link Text
var link = driver.FindElement(By.PartialLinkText("Sign"));
```

| Method | Strategy |
| --- | --- |
| `By.CssSelector(value)` | CSS selector |
| `By.Xpath(value)` | XPath expression |
| `By.TagName(value)` | HTML tag name |
| `By.LinkText(value)` | Exact link text |
| `By.PartialLinkText(value)` | Partial link text |

## G4 Ecosystem

G4.WebDriver is the browser and device automation foundation layer of the [G4 automation platform](https://github.com/g4-api). It is consumed as a NuGet dependency by other G4 components:

- **G4 Engine** -- The core automation engine that orchestrates rules and actions, using G4.WebDriver for all browser interactions
- **G4 Plugins** -- Extension modules that leverage WebDriver for specific automation tasks
- **G4 Actions** -- Reusable automation actions built on top of the WebDriver API

The event-driven `WebDriverCommandInvoker` architecture allows the G4 engine to hook into command execution for logging, auditing, and rule processing as commands flow through the driver.

## Contributing

Contributions are welcome. To get started:

1. Clone the repository
2. Open `src/G4.sln` in Visual Studio or your preferred IDE
3. Build the solution:

```bash
dotnet restore src/G4.sln
dotnet build src/G4.sln
```

4. Run the tests:

```bash
dotnet test src/G4.sln
```

Tests require browser drivers to be installed on the machine (or a remote Selenium Grid/BrowserStack configured in `test/settings/Default.runsettings`). A local ASP.NET Core test web server is started automatically on port 9002.

Please open an issue or submit a pull request on [GitHub](https://github.com/g4-api/g4-web-driver-client).

## License

This project is licensed under the [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0).
