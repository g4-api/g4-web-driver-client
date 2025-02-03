using System;
using System.Collections.Generic;

namespace G4.WebDriver.Support.Ui
{
    /// <summary>
    /// Defines the contract for a waitable interface, which provides waiting mechanisms
    /// for WebDriver operations, allowing conditions to be met within a specified timeout.
    /// </summary>
    /// <typeparam name="T">The type of object being waited on.</typeparam>
    public interface IWaitable<T>
    {
        #region *** Properties ***
        /// <summary>
        /// Gets a list of all exception types that are ignored during the wait operation.
        /// </summary>
        /// <value>An <see cref="IList{Type}"/> of exception types to be ignored.</value>
        public IList<Type> IgnoreExceptions { get; }

        /// <summary>
        /// Gets or sets the polling time interval between each WebDriver request while waiting for a condition.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> representing the polling interval.</value>
        public TimeSpan PollingInterval { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of time to wait for a condition to be met before throwing a <see cref="WebDriverTimeoutException"/>.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> representing the timeout duration.</value>
        public TimeSpan Timeout { get; set; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Waits until the provided condition is met or the timeout is reached.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the condition.</typeparam>
        /// <param name="condition">A function that represents the condition to be evaluated.</param>
        /// <returns>The result of the condition if it is met within the timeout period; otherwise, an exception is thrown.</returns>
        public TResult Until<TResult>(Func<T, TResult> condition);
        #endregion
    }
}
