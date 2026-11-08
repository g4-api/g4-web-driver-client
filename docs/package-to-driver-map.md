# Package to driver map

This page shows which NuGet package provides which driver class, and which G4 driver name (the `driver` value in `driverParameters`) selects it. G4 driver names are defined in the `G4.Abstraction.WebDriver` package. The full list is in the [G4 drivers table](../../g4-abstraction/docs/drivers.md).

## Packages with a G4 driver name

| Package | Driver class | G4 driver name | Needs |
| --- | --- | --- | --- |
| `G4.WebDriver.Remote.Chrome` | `ChromeDriver` | `ChromeDriver` | `chromedriver` or a grid |
| `G4.WebDriver.Remote.Edge` | `EdgeDriver` | `MicrosoftEdgeDriver` | `msedgedriver` or a grid |
| `G4.WebDriver.Remote.Firefox` | `FirefoxDriver` | `FirefoxDriver` | `geckodriver` or a grid |
| `G4.WebDriver.Remote.Android` | `AndroidDriver` | `AndroidDriver` | An Appium server |
| `G4.WebDriver.Remote.Uia` | `UiaDriver` | `UiaDriver` | The UIA driver server |
| `G4.WebDriver.Remote.Sap` | `SapDriver` | `SapDriver` | The SAP driver server |
| `G4.WebDriver.Simulator` | `SimulatorDriver` | `SimulatorDriver` and `NoDriver` | Nothing. It is a mock. |

Source: the `[G4DriverPlugin]` attributes in `g4-abstraction/src/G4.Abstraction.WebDriver/Drivers/*Plugin.cs`, and the driver classes in `src/G4.WebDriver.Remote.*`.

## Packages without a G4 driver name

| Package | What it provides | Why it has no name |
| --- | --- | --- |
| `G4.WebDriver` | The core: `IWebDriver`, `RemoteWebDriver`, `By`, the models and the command invoker. | It is the base that every other package builds on. |
| `G4.WebDriver.Remote.Chromium` | `ChromiumDriverBase` and `ChromiumOptionsBase`, the shared base for Chrome, Edge and Opera. | It is a base package. |
| `G4.WebDriver.Remote.Opera` | `OperaDriver`. | `g4-abstraction` has no Opera plugin, so G4 cannot select it by name. |
| `G4.WebDriver.Remote.Appium` | `AppiumDriverBase`, `AppiumOptionsBase` and the keyboard and geolocation members. | It is a base package. |
| `G4.WebDriver.Remote.Ios` | `IosDriver` and `IosOptions` over the Appium base. | `g4-abstraction` has no iOS plugin. |
| `G4.WebDriver.Remote.MacOs` | Nothing yet. The project has no source files. | `g4-abstraction` has no macOS plugin. |

Source: the `class` declarations in `src/G4.WebDriver.Remote.*`; `ChromeDriver`, `EdgeDriver` and `OperaDriver` all derive from `ChromiumDriverBase`, and `IosDriver` derives from `AppiumDriverBase`.

## Choose a package

| You want to drive | Install | Then |
| --- | --- | --- |
| A browser from C# | `G4.WebDriver.Remote.Chrome`, `Edge` or `Firefox` | Create the driver class directly. |
| A browser from a G4 automation | Nothing from this repo | Set `driver` in `driverParameters`. `G4.Abstraction.WebDriver` brings in the packages it needs. |
| A Windows desktop app | `G4.WebDriver.Remote.Uia` | Start the UIA driver server and create `UiaDriver`. |
| SAP GUI | `G4.WebDriver.Remote.Sap` | Start the SAP driver server and create `SapDriver`. |
| An Android or iOS device | `G4.WebDriver.Remote.Android` or `Ios` | Start Appium and create the driver. |
| A test without a real app | `G4.WebDriver.Simulator` | Create `SimulatorDriver`. |

## Related

- [Core package README](packages-readme/G4.WebDriver.md)
- Repository README
