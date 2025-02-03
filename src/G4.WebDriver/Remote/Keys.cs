/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Keys.cs
 */
using G4.WebDriver.Attributes;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Representations of keys able to be pressed that are not text keys for sending to the browser.
    /// </summary>
    public static class Keys
    {
        #region *** Fields  ***
        // Dictionary that caches key names and their string representations.
        private static readonly Dictionary<string, string> _cache = NewCache();

        /// <summary>
        /// Represents the NUL keystroke.
        /// </summary>
        [KeyCode(keyName: nameof(Null), keyCode: 0xE000)]
        public static readonly string Null = ConvertKeyCode(0xE000);

        /// <summary>
        /// Represents the Cancel keystroke.
        /// </summary>
        [KeyCode(keyName: nameof(Cancel), keyCode: 0xE001)]
        public static readonly string Cancel = ConvertKeyCode(0xE001);

        /// <summary>
        /// Represents the Help keystroke.
        /// </summary>
        [KeyCode(keyName: nameof(Help), keyCode: 0xE002)]
        public static readonly string Help = ConvertKeyCode(0xE002);

        /// <summary>
        /// Represents the Backspace key.
        /// </summary>
        [KeyCode(keyName: nameof(Backspace), keyCode: 0xE003)]
        public static readonly string Backspace = ConvertKeyCode(0xE003);

        /// <summary>
        /// Represents the Tab key.
        /// </summary>
        [KeyCode(keyName: nameof(Tab), keyCode: 0xE004)]
        public static readonly string Tab = ConvertKeyCode(0xE004);

        /// <summary>
        /// Represents the Clear keystroke.
        /// </summary>
        [KeyCode(keyName: nameof(Clear), keyCode: 0xE005)]
        public static readonly string Clear = ConvertKeyCode(0xE005);

        /// <summary>
        /// Represents the Return key.
        /// </summary>
        [KeyCode(keyName: nameof(Return), keyCode: 0xE006)]
        public static readonly string Return = ConvertKeyCode(0xE006);

        /// <summary>
        /// Represents the Enter key.
        /// </summary>
        [KeyCode(keyName: nameof(Enter), keyCode: 0xE007)]
        public static readonly string Enter = ConvertKeyCode(0xE007);

        /// <summary>
        /// Represents the Shift key.
        /// </summary>
        [KeyCode(keyName: nameof(Shift), keyCode: 0xE008)]
        public static readonly string Shift = ConvertKeyCode(0xE008);

        /// <summary>
        /// Represents the Shift key.
        /// </summary>
        [KeyCode(keyName: nameof(LeftShift), keyCode: 0xE008)]
        public static readonly string LeftShift = ConvertKeyCode(0xE008);

        /// <summary>
        /// Represents the Control key.
        /// </summary>
        [KeyCode(keyName: nameof(Control), keyCode: 0xE009)]
        public static readonly string Control = ConvertKeyCode(0xE009);

        /// <summary>
        /// Represents the Control key.
        /// </summary>
        [KeyCode(keyName: nameof(LeftControl), keyCode: 0xE009)]
        public static readonly string LeftControl = ConvertKeyCode(0xE009);

        /// <summary>
        /// Represents the Alt key.
        /// </summary>
        [KeyCode(keyName: nameof(Alt), keyCode: 0xE00A)]
        public static readonly string Alt = ConvertKeyCode(0xE00A);

        /// <summary>
        /// Represents the Alt key.
        /// </summary>
        [KeyCode(keyName: nameof(LeftAlt), keyCode: 0xE00A)]
        public static readonly string LeftAlt = ConvertKeyCode(0xE00A);

        /// <summary>
        /// Represents the Pause key.
        /// </summary>
        [KeyCode(keyName: nameof(Pause), keyCode: 0xE00B)]
        public static readonly string Pause = ConvertKeyCode(0xE00B);

        /// <summary>
        /// Represents the Escape key.
        /// </summary>
        [KeyCode(keyName: nameof(Escape), keyCode: 0xE00C)]
        public static readonly string Escape = ConvertKeyCode(0xE00C);

        /// <summary>
        /// Represents the SpaceBar key.
        /// </summary>
        [KeyCode(keyName: nameof(Space), keyCode: 0xE00D)]
        public static readonly string Space = ConvertKeyCode(0xE00D);

        /// <summary>
        /// Represents the Page Up key.
        /// </summary>
        [KeyCode(keyName: nameof(PageUp), keyCode: 0xE00E)]
        public static readonly string PageUp = ConvertKeyCode(0xE00E);

        /// <summary>
        /// Represents the Page Down key.
        /// </summary>
        [KeyCode(keyName: nameof(PageDown), keyCode: 0xE00F)]
        public static readonly string PageDown = ConvertKeyCode(0xE00F);

        /// <summary>
        /// Represents the End key.
        /// </summary>
        [KeyCode(keyName: nameof(End), keyCode: 0xE010)]
        public static readonly string End = ConvertKeyCode(0xE010);

        /// <summary>
        /// Represents the Home key.
        /// </summary>
        [KeyCode(keyName: nameof(Home), keyCode: 0xE011)]
        public static readonly string Home = ConvertKeyCode(0xE011);

        /// <summary>
        /// Represents the left arrow key.
        /// </summary>
        [KeyCode(keyName: nameof(Left), keyCode: 0xE012)]
        public static readonly string Left = ConvertKeyCode(0xE012);

        /// <summary>
        /// Represents the left arrow key.
        /// </summary>
        [KeyCode(keyName: nameof(ArrowLeft), keyCode: 0xE012)]
        public static readonly string ArrowLeft = ConvertKeyCode(0xE012);

        /// <summary>
        /// Represents the up arrow key.
        /// </summary>
        [KeyCode(keyName: nameof(Up), keyCode: 0xE013)]
        public static readonly string Up = ConvertKeyCode(0xE013);

        /// <summary>
        /// Represents the up arrow key.
        /// </summary>
        [KeyCode(keyName: nameof(ArrowUp), keyCode: 0xE013)]
        public static readonly string ArrowUp = ConvertKeyCode(0xE013);

        /// <summary>
        /// Represents the right arrow key.
        /// </summary>
        [KeyCode(keyName: nameof(Right), keyCode: 0xE014)]
        public static readonly string Right = ConvertKeyCode(0xE014);

        /// <summary>
        /// Represents the right arrow key.
        /// </summary>
        [KeyCode(keyName: nameof(ArrowRight), keyCode: 0xE014)]
        public static readonly string ArrowRight = ConvertKeyCode(0xE014);

        /// <summary>
        /// Represents the down arrow key.
        /// </summary>
        [KeyCode(keyName: nameof(Down), keyCode: 0xE015)]
        public static readonly string Down = ConvertKeyCode(0xE015);

        /// <summary>
        /// Represents the down arrow key.
        /// </summary>
        [KeyCode(keyName: nameof(ArrowDown), keyCode: 0xE015)]
        public static readonly string ArrowDown = ConvertKeyCode(0xE015);

        /// <summary>
        /// Represents the Insert key.
        /// </summary>
        [KeyCode(keyName: nameof(Insert), keyCode: 0xE016)]
        public static readonly string Insert = ConvertKeyCode(0xE016);

        /// <summary>
        /// Represents the Delete key.
        /// </summary>
        [KeyCode(keyName: nameof(Delete), keyCode: 0xE017)]
        public static readonly string Delete = ConvertKeyCode(0xE017);

        /// <summary>
        /// Represents the semi-colon key.
        /// </summary>
        [KeyCode(keyName: nameof(Semicolon), keyCode: 0xE018)]
        public static readonly string Semicolon = ConvertKeyCode(0xE018);

        /// <summary>
        /// Represents the equal sign key.
        /// </summary>
        [KeyCode(keyName: nameof(Equal), keyCode: 0xE019)]
        public static readonly string Equal = ConvertKeyCode(0xE019);

        /// <summary>
        /// Represents the number pad 0 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad0), keyCode: 0xE01A)]
        public static readonly string NumberPad0 = ConvertKeyCode(0xE01A);

        /// <summary>
        /// Represents the number pad 1 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad1), keyCode: 0xE01B)]
        public static readonly string NumberPad1 = ConvertKeyCode(0xE01B);

        /// <summary>
        /// Represents the number pad 2 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad2), keyCode: 0xE01C)]
        public static readonly string NumberPad2 = ConvertKeyCode(0xE01C);

        /// <summary>
        /// Represents the number pad 3 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad3), keyCode: 0xE01D)]
        public static readonly string NumberPad3 = ConvertKeyCode(0xE01D);

        /// <summary>
        /// Represents the number pad 4 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad4), keyCode: 0xE01E)]
        public static readonly string NumberPad4 = ConvertKeyCode(0xE01E);

        /// <summary>
        /// Represents the number pad 5 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad5), keyCode: 0xE01F)]
        public static readonly string NumberPad5 = ConvertKeyCode(0xE01F);

        /// <summary>
        /// Represents the number pad 6 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad6), keyCode: 0xE020)]
        public static readonly string NumberPad6 = ConvertKeyCode(0xE020);

        /// <summary>
        /// Represents the number pad 7 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad7), keyCode: 0xE021)]
        public static readonly string NumberPad7 = ConvertKeyCode(0xE021);

        /// <summary>
        /// Represents the number pad 8 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad8), keyCode: 0xE022)]
        public static readonly string NumberPad8 = ConvertKeyCode(0xE022);

        /// <summary>
        /// Represents the number pad 9 key.
        /// </summary>
        [KeyCode(keyName: nameof(NumberPad9), keyCode: 0xE023)]
        public static readonly string NumberPad9 = ConvertKeyCode(0xE023);

        /// <summary>
        /// Represents the number pad multiplication key.
        /// </summary>
        [KeyCode(keyName: nameof(Multiply), keyCode: 0xE024)]
        public static readonly string Multiply = ConvertKeyCode(0xE024);

        /// <summary>
        /// Represents the number pad addition key.
        /// </summary>
        [KeyCode(keyName: nameof(Add), keyCode: 0xE025)]
        public static readonly string Add = ConvertKeyCode(0xE025);

        /// <summary>
        /// Represents the number pad thousands separator key.
        /// </summary>
        [KeyCode(keyName: nameof(Separator), keyCode: 0xE026)]
        public static readonly string Separator = ConvertKeyCode(0xE026);

        /// <summary>
        /// Represents the number pad subtraction key.
        /// </summary>
        [KeyCode(keyName: nameof(Subtract), keyCode: 0xE027)]
        public static readonly string Subtract = ConvertKeyCode(0xE027);

        /// <summary>
        /// Represents the number pad decimal separator key.
        /// </summary>
        [KeyCode(keyName: nameof(Decimal), keyCode: 0xE028)]
        public static readonly string Decimal = ConvertKeyCode(0xE028);

        /// <summary>
        /// Represents the number pad division key.
        /// </summary>
        [KeyCode(keyName: nameof(Divide), keyCode: 0xE029)]
        public static readonly string Divide = ConvertKeyCode(0xE029);

        /// <summary>
        /// Represents the function key F1.
        /// </summary>
        [KeyCode(keyName: nameof(F1), keyCode: 0xE031)]
        public static readonly string F1 = ConvertKeyCode(0xE031);

        /// <summary>
        /// Represents the function key F2.
        /// </summary>
        [KeyCode(keyName: nameof(F2), keyCode: 0xE032)]
        public static readonly string F2 = ConvertKeyCode(0xE032);

        /// <summary>
        /// Represents the function key F3.
        /// </summary>
        [KeyCode(keyName: nameof(F3), keyCode: 0xE033)]
        public static readonly string F3 = ConvertKeyCode(0xE033);

        /// <summary>
        /// Represents the function key F4.
        /// </summary>
        [KeyCode(keyName: nameof(F4), keyCode: 0xE034)]
        public static readonly string F4 = ConvertKeyCode(0xE034);

        /// <summary>
        /// Represents the function key F5.
        /// </summary>
        [KeyCode(keyName: nameof(F5), keyCode: 0xE035)]
        public static readonly string F5 = ConvertKeyCode(0xE035);

        /// <summary>
        /// Represents the function key F6.
        /// </summary>
        [KeyCode(keyName: nameof(F6), keyCode: 0xE036)]
        public static readonly string F6 = ConvertKeyCode(0xE036);

        /// <summary>
        /// Represents the function key F7.
        /// </summary>
        [KeyCode(keyName: nameof(F7), keyCode: 0xE037)]
        public static readonly string F7 = ConvertKeyCode(0xE037);

        /// <summary>
        /// Represents the function key F8.
        /// </summary>
        [KeyCode(keyName: nameof(F8), keyCode: 0xE038)]
        public static readonly string F8 = ConvertKeyCode(0xE038);

        /// <summary>
        /// Represents the function key F9.
        /// </summary>
        [KeyCode(keyName: nameof(F9), keyCode: 0xE039)]
        public static readonly string F9 = ConvertKeyCode(0xE039);

        /// <summary>
        /// Represents the function key F10.
        /// </summary>
        [KeyCode(keyName: nameof(F10), keyCode: 0xE03A)]
        public static readonly string F10 = ConvertKeyCode(0xE03A);

        /// <summary>
        /// Represents the function key F11.
        /// </summary>
        [KeyCode(keyName: nameof(F11), keyCode: 0xE03B)]
        public static readonly string F11 = ConvertKeyCode(0xE03B);

        /// <summary>
        /// Represents the function key F12.
        /// </summary>
        [KeyCode(keyName: nameof(F12), keyCode: 0xE03C)]
        public static readonly string F12 = ConvertKeyCode(0xE03C);

        /// <summary>
        /// Represents the function key META.
        /// </summary>
        [KeyCode(keyName: nameof(Meta), keyCode: 0xE03D)]
        public static readonly string Meta = ConvertKeyCode(0xE03D);

        /// <summary>
        /// Represents the function key COMMAND.
        /// </summary>
        [KeyCode(keyName: nameof(Command), keyCode: 0xE03D)]
        public static readonly string Command = ConvertKeyCode(0xE03D);

        /// <summary>
        /// Represents the Zenkaku/Hankaku key.
        /// </summary>
        [KeyCode(keyName: nameof(ZenkakuHankaku), keyCode: 0xE040)]
        public static readonly string ZenkakuHankaku = ConvertKeyCode(0xE040);
        #endregion

        #region *** Methods ***
        /// <summary>
        /// Gets the string representation of a keyboard key based on its key code.
        /// </summary>
        /// <param name="keyCode">The key code of the keyboard key.</param>
        /// <returns>The string representation of the keyboard key.</returns>
        public static string Get(int keyCode) => ConvertKeyCode(keyCode);

        /// <summary>
        /// Gets the string representation of a keyboard key based on its key name.
        /// </summary>
        /// <param name="keyName">The name of the keyboard key.</param>
        /// <returns>The string representation of the keyboard key, or null if not found.</returns>
        public static string Get(string keyName) => _cache.TryGetValue(keyName, out string value) ? value : null;

        // Creates a new cache for mapping key names to their string representations.
        private static Dictionary<string, string> NewCache()
        {
            // Create a new dictionary for caching key names and their string representations (case-insensitive)
            var cache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Get all public static fields from the Keys class
            var fields = typeof(Keys).GetFields(BindingFlags.Public | BindingFlags.Static);

            // If there are no fields, return the empty cache
            if (fields == null || fields.Length == 0)
            {
                return cache;
            }

            // Populate the cache with key names and their string representations
            foreach (var field in fields)
            {
                // Assign the string representation of the key to its corresponding key name in the cache
                var attribute = field.GetCustomAttribute<KeyCodeAttribute>();
                cache[attribute.KeyName] = ConvertKeyCode(attribute.KeyCode);
            }

            // Return the populated cache
            return cache;
        }

        // Converts a key code to its string representation.
        private static string ConvertKeyCode(int keyCode)
        {
            // Use the CultureInfo.InvariantCulture for consistent character conversions
            var culture = CultureInfo.InvariantCulture;

            // Convert the provided key code to its corresponding character and return as string
            return Convert.ToString(Convert.ToChar(keyCode, culture), culture);
        }
        #endregion
    }
}
