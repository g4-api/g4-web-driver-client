using G4.WebDriver.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace G4.WebDriver.Support.Ui
{
    /// <summary>
    /// Provides a wait mechanism for WebDriver operations, allowing conditions to be met within a specified timeout period.
    /// </summary>
    /// <typeparam name="T">The type of object being waited on.</typeparam>
    public class WebDriverWait<T> : IWaitable<T>
    {
        #region *** Fields       ***
        // The object being waited on.
        private readonly T _input;
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverWait{T}"/> class with a default timeout of 10 seconds.
        /// </summary>
        /// <param name="input">The input object to be used for waiting.</param>
        /// <exception cref="ArgumentNullException">Thrown when the input object is null.</exception>
        public WebDriverWait(T input)
            : this(input, TimeSpan.FromSeconds(10))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverWait{T}"/> class with a specified timeout.
        /// </summary>
        /// <param name="input">The input object to be used for waiting.</param>
        /// <param name="timeout">The maximum amount of time to wait for a condition to be met.</param>
        /// <exception cref="ArgumentNullException">Thrown when the input object is null.</exception>
        public WebDriverWait(T input, TimeSpan timeout)
        {
            // Throw an exception if the input object is null.
            if (EqualityComparer<T>.Default.Equals(input, default))
            {
                // Throw an exception if the input object is null.
                var message = $"Input object cannot be null.";
                throw new ArgumentNullException(nameof(input), message);
            }

            // Set the input object.
            _input = input;

            // Initialize the list of exceptions to ignore during the wait operation.
            IgnoreExceptions = [];

            // Set the default polling interval to 0.5 seconds.
            PollingInterval = TimeSpan.FromSeconds(0.5);

            // Set the timeout duration.
            Timeout = timeout;
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets a list of all exception types that are ignored during the wait operation.
        /// </summary>
        public IList<Type> IgnoreExceptions { get; }

        /// <summary>
        /// Gets or sets the polling time interval between each WebDriver request while waiting for a condition.
        /// </summary>
        public TimeSpan PollingInterval { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of time to wait for a condition to be met before throwing a <see cref="WebDriverTimeoutException"/>.
        /// </summary>
        public TimeSpan Timeout { get; set; }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Waits until the provided condition is met or the timeout is reached.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the condition.</typeparam>
        /// <param name="condition">A function that represents the condition to be evaluated.</param>
        /// <returns>The result of the condition if it is met within the timeout period; otherwise, throws a <see cref="WebDriverTimeoutException"/>.</returns>
        public TResult Until<TResult>(Func<T, TResult> condition)
        {
            // Initialize the result object.
            TResult result = default;

            // Flag to indicate if the condition result is true.
            var isTrue = false;

            // Flag to indicate if the condition has been met.
            var isComplete = false;

            // Check if the result type is a boolean.
            var isType = typeof(TResult) == typeof(bool);

            // Calculate the timeout deadline.
            var timeout = DateTime.UtcNow.Add(Timeout);
            while (!isComplete && DateTime.UtcNow < timeout)
            {
                try
                {
                    // Invoke the condition and evaluate the result.
                    result = InvokeUntil(_input, condition);
                    isTrue = isType && bool.TryParse($"{result}", out bool resultOut) && resultOut;
                    // Complete if the condition is met.
                    isComplete = isTrue || result != null;
                    if (!isComplete)
                    {
                        // Wait for the polling interval before retrying.
                        Thread.Sleep(PollingInterval);
                    }
                }
                catch (Exception e) when (IgnoreExceptions.Any(i => i == e.GetType()))
                {
                    // Ignore exceptions specified in IgnoreExceptions.
                }
            }

            // Throw a WebDriverTimeoutException if the condition is not met within the timeout period.
            return isComplete
                ? result
                : throw new WebDriverTimeoutException("Invoke-Condition = TimedOut", Timeout);
        }

        // Invokes the provided condition function until it returns a valid result or throws an exception if the input is invalid.
        private static TResult InvokeUntil<TResult>(T input, Func<T, TResult> condition)
        {
            // Check if the result type is a boolean.
            var isBoolean = typeof(TResult) == typeof(bool);

            // Check if the result type is an object.
            var isObject = typeof(object).IsAssignableFrom(typeof(TResult));

            if (condition == null)
            {
                // Throw an exception if the condition is null.
                const string message = "Invoke-Condition = (NoCondition)";
                throw new ArgumentNullException(nameof(condition), message);
            }
            if (!isBoolean && !isObject)
            {
                // Throw an exception if the result type is neither boolean nor object.
                const string message = "Invoke-Condition = (BadRequest|ObjectTypeMismatch)";
                throw new ArgumentNullException(nameof(condition), message);
            }

            // Invoke the condition function with the provided input.
            return condition.Invoke(input);
        }
        #endregion
    }
}
