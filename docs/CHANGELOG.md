# Changelog

All notable changes to the G4.WebDriver packages are listed here. The format follows
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/). Versions match the GitHub release tags and the NuGet
package versions.

## 2026.11.25.21

### Added

- `SimulatorDriver` answers the Appium `mobile: startActivity` command. It accepts a `package/activity` intent and
  rejects any other intent with a `WebDriverException`, as an Android device does, so plugin tests can run activity
  starts against the simulator.

## 2026.11.25.20

### Changed

- `MousePositionInputModel` offsets that the caller does not set now default by alignment: 0 for `MiddleCenter`, so
  a center point lands exactly on the center, and 1 for every other alignment, as before. Offsets the caller sets are
  sent unchanged.
- The README describes shadow roots (`GetShadowRoot`), `ActionSequence` input actions and the UIA mouse position.
- The package READMEs name .NET 10, and the README no longer links to a guide outside the repository.

## 2026.11.24.19

### Added

- `ActionSequence` single input actions: `AddPointerDown`, `AddPointerUp`, `AddScroll`, `AddAction`, the
  `PointerTypes` and `ActionOrigins` constants, and the pen and touch pointer types.

## 2026.11.24.18

### Changed

- The shadow root search test uses a sequence assertion.

## 2026.11.23.17

### Fixed

- Searching inside a shadow root uses the WebDriver shadow-root commands, and `GetShadowRoot` returns closed shadow
  roots as well as open ones.

## 2026.11.08.16

### Changed

- Expanded the driver documentation and the package READMEs.

## 2026.11.04.15

### Fixed

- `GetAttribute` returns markup properties such as `outerHTML`.

## 2026.11.01.14

### Added

- The SAP GUI remote WebDriver (`G4.WebDriver.Remote.Sap`).
