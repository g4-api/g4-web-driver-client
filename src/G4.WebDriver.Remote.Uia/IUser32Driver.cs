using System;

namespace G4.WebDriver.Remote.Uia
{
    /// <summary>
    /// Represents a web driver that can interact with the user32.dll for native Windows UI automation.
    /// </summary>
    public interface IUser32Driver
    {
        /// <summary>
        /// Sends input scan codes to the WebDriver server.
        /// </summary>
        /// <param name="codes">The scan codes to send.</param>
        public void SendInputs(params string[] codes);

        /// <summary>
        /// Sends input scan codes to the WebDriver server multiple times.
        /// </summary>
        /// <param name="repeat">The number of times to repeat the input.</param>
        /// <param name="codes">The scan codes to send.</param>
        public void SendInputs(int repeat, params string[] codes);

        /// <summary>
        /// Sends text input to the WebDriver server.
        /// </summary>
        /// <param name="text">The text to send.</param>
        public void SendKeys(string text);

        /// <summary>
        /// Sends each character of the specified text as a key input with a delay between each keystroke.
        /// </summary>
        /// <param name="text">The text to be sent as key inputs.</param>
        /// <param name="delay">The delay to wait between sending each key.</param>
        public void SendKeys(string text, TimeSpan delay);

        /// <summary>
        /// Sends text input to the WebDriver server multiple times.
        /// </summary>
        /// <param name="repeat">The number of times to repeat the input.</param>
        /// <param name="text">The text to send.</param>
        public void SendKeys(int repeat, string text);
    }
}
