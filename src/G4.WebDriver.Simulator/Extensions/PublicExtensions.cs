/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/By.cs
 */
using G4.WebDriver.Models;

using System.Reflection;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.ComponentModel;
using G4.WebDriver.Simulator;
using System.Collections.Generic;

namespace G4.WebDriver.Extensions
{
    /// <summary>
    /// Provides extension methods for various utility operations used in the G4 WebDriver.
    /// </summary>
    public static class PublicExtensions
    {
        /// <summary>
        /// Adds capabilities to the given <see cref="SimulatorDriver"/> and returns a new driver with the updated capabilities.
        /// </summary>
        /// <param name="driver">The original <see cref="SimulatorDriver"/> instance.</param>
        /// <param name="capabilities">A dictionary of capabilities to add to the driver.</param>
        /// <returns>A new <see cref="SimulatorDriver"/> instance with the added capabilities, or <c>null</c> if the "NullDriver" capability is set to <c>true</c>.
        /// </returns>
        public static SimulatorDriver AddCapabilities(this SimulatorDriver driver, IDictionary<string, object> capabilities)
        {
            // Check if the "NullDriver" capability is set to true; if so, return null
            if (capabilities.TryGetValue(key: "NullDriver", out object value) && (bool)value)
            {
                return null;
            }

            // Create a new session model
            var sessionModel = new SessionModel();

            // Add the provided capabilities to the session model's AlwaysMatch capabilities
            foreach (var item in capabilities)
            {
                sessionModel.Capabilities.AlwaysMatch[item.Key] = item.Value;
            }

            // Add the original driver's AlwaysMatch capabilities to the session model
            foreach (var item in driver.Capabilities.AlwaysMatch)
            {
                sessionModel.Capabilities.AlwaysMatch[item.Key] = item.Value;
            }

            // Return a new SimulatorDriver instance with the updated session model
            return new SimulatorDriver(driver.DriverBinaries, sessionModel);
        }

        /// <summary>
        /// Retrieves the first method in the specified type that has a <see cref="DescriptionAttribute"/> matching the given description.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to search for methods.</param>
        /// <param name="actual">The value to match against the method's <see cref="DescriptionAttribute"/> description.</param>
        /// <returns>
        /// A <see cref="MethodInfo"/> instance representing the first matching method if found; otherwise, <c>null</c>.
        /// </returns>
        public static MethodInfo GetMethodByDescription(this Type type, string actual)
        {
            // Use default binding flags: instance, non-public, and static members
            return GetMethodByDescription(
                type: type,
                actual: actual,
                flags: BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
        }

        /// <summary>
        /// Retrieves the first method in the specified type that has a <see cref="DescriptionAttribute"/> matching the given description.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to search for methods.</param>
        /// <param name="actual">The value to match against the method's <see cref="DescriptionAttribute"/> description.</param>
        /// <param name="flags">The <see cref="BindingFlags"/> to control the search behavior.</param>
        /// <returns>
        /// A <see cref="MethodInfo"/> instance representing the first matching method if found; otherwise, <c>null</c>.
        /// </returns>
        public static MethodInfo GetMethodByDescription(this Type type, string actual, BindingFlags flags)
        {
            // Use default regex options: ignore case and single line
            return GetMethodByDescription(
                type: type,
                actual: actual,
                flags: flags,
                comparison: RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// Retrieves the first method in the specified type that has a <see cref="DescriptionAttribute"/> matching the given description.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to search for methods.</param>
        /// <param name="actual">The value to match against the method's <see cref="DescriptionAttribute"/> description.</param>
        /// <param name="flags">The <see cref="BindingFlags"/> to control the search behavior.</param>
        /// <param name="comparison">The <see cref="RegexOptions"/> to control the comparison behavior.</param>
        /// <returns>
        /// A <see cref="MethodInfo"/> instance representing the first matching method if found; otherwise, <c>null</c>.
        /// </returns>
        public static MethodInfo GetMethodByDescription(this Type type, string actual, BindingFlags flags, RegexOptions comparison)
        {
            // Get all methods from the type that have a DescriptionAttribute
            var methodsWithDescription = type.GetMethods(flags)
                .Where(method => method.GetCustomAttribute<DescriptionAttribute>() != null);

            // Search for the first method where the DescriptionAttribute's description matches the provided value
            return methodsWithDescription.FirstOrDefault(method =>
            {
                // Get the description from the DescriptionAttribute
                var description = method.GetCustomAttribute<DescriptionAttribute>().Description;

                // Use Regex to match the actual value with the description, based on the provided comparison options
                return Regex.IsMatch(input: actual, pattern: description, options: comparison);
            });
        }

        /// <summary>
        /// Gets elements with <option> tag.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Option(this By _) => By.Custom.Id(SimulatorLocators.Option);

        /// <summary>
        /// Gets a mechanism to find a valid 'select' element (positive).
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By SelectElement(this By _) => By.Custom.Id(SimulatorLocators.SelectElement);

        /// <summary>
        /// Gets a mechanism to find a valid 'select' element (positive) with no options.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By SelectElementNoOptions(this By _) => By.Custom.Id(SimulatorLocators.SelectElement);

        /// <summary>
        /// Gets a mechanism to find a valid mock element (positive).
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Positive(this By _) => By.Custom.Id(SimulatorLocators.Positive);

        /// <summary>
        /// Gets a mechanism to find an invalid mock element (negative).
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Negative(this By _) => By.Custom.Id(SimulatorLocators.Negative);

        /// <summary>
        /// Gets a mechanism to find mock element which will throw <see cref="NoSuchElementException"/>
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By None(this By _) => By.Custom.Id(SimulatorLocators.None);

        /// <summary>
        /// Gets a mechanism to find mock element which will throw <see cref="StaleElementReferenceException"/>
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Stale(this By _) => By.Custom.Id(SimulatorLocators.Stale);

        /// <summary>
        /// Gets a mechanism to find mock element which will return null reference.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Null(this By _) => By.Custom.Id(SimulatorLocators.Null);

        /// <summary>
        /// Gets a mechanism to find mock element which will throw <see cref="WebDriverException"/>
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Exception(this By _) => By.Custom.Id(SimulatorLocators.Exception);

        /// <summary>
        /// Gets a mechanism to find a valid mock element (positive) with 90% success rate.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By RandomPositive(this By _) => By.Custom.Id(SimulatorLocators.RandomPositive);

        /// <summary>
        /// Gets a mechanism to find an invalid mock element (negative) with 90% success rate.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By RandomNegative(this By _) => By.Custom.Id(SimulatorLocators.RandomNegative);

        /// <summary>
        /// Gets a mechanism to find an exists mock element with 90% success rate.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By RandomExists(this By _) => By.Custom.Id(SimulatorLocators.RandomExists);

        /// <summary>
        /// Gets a mechanism to find an exists mock element with 10% success rate.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By RandomNotExists(this By _) => By.Custom.Id(SimulatorLocators.RandomNotExists);

        /// <summary>
        /// Gets a mechanism to find a random element with 90% chance of getting null result.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By RandomNull(this By _) => By.Custom.Id(SimulatorLocators.RandomNull);

        /// <summary>
        /// Gets a mechanism to find a random element with 90% chance of getting <see cref="NoSuchElementException"/>.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By RandomNoSuchElement(this By _) => By.Custom.Id(SimulatorLocators.RandomNoSuchElement);

        /// <summary>
        /// Gets a mechanism to find a random element with 90% chance of getting <see cref="StaleElementReferenceException"/>.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By RandomStale(this By _) => By.Custom.Id(SimulatorLocators.RandomStale);

        /// <summary>
        /// Gets a mechanism to find the current focused element.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Focused(this By _) => By.Custom.Id(SimulatorLocators.Focused);

        /// <summary>
        /// Gets a mechanism to find [input type="file"] element.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By InputFile(this By _) => By.Custom.Id(SimulatorLocators.File);

        /// <summary>
        /// Gets a mechanism to find [body] element.
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Body(this By _) => By.Custom.Id(SimulatorLocators.File);

        /// <summary>
        /// Gets a mechanism to find mock element which will throw <see cref="InvalidElementStateException"/>
        /// </summary>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By InvalidState(this By _) => By.Custom.Id(SimulatorLocators.InvalidElementState);
    }
}
