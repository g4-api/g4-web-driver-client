using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Appium;

using System;

namespace G4.WebDriver.Remote.Android
{
    public class AndroidDriver : AppiumDriverBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using default Android options.
        /// </summary>
        public AndroidDriver()
            : this(new AndroidOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using the specified Android options.
        /// </summary>
        /// <param name="options">The <see cref="AndroidOptions"/> to be used for this driver.</param>
        public AndroidDriver(AndroidOptions options)
            : this(options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using the specified session model.
        /// </summary>
        /// <param name="session">The <see cref="SessionModel"/> to be used for this driver.</param>
        public AndroidDriver(SessionModel session)
            : this(new AndroidWebDriverService(), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using the specified driver service.
        /// </summary>
        /// <param name="driverService">The <see cref="AndroidWebDriverService"/> to be used for this driver.</param>
        public AndroidDriver(AndroidWebDriverService driverService)
            : this(driverService, new AndroidOptions(), TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using the specified binaries path.
        /// </summary>
        /// <param name="binariesPath">The path to the binaries to be used for this driver.</param>
        public AndroidDriver(string binariesPath)
            : this(binariesPath, new AndroidOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using the specified binaries path and Android options.
        /// </summary>
        /// <param name="binariesPath">The path to the binaries to be used for this driver.</param>
        /// <param name="options">The <see cref="AndroidOptions"/> to be used for this driver.</param>
        public AndroidDriver(string binariesPath, AndroidOptions options)
            : this(binariesPath, options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using the specified binaries path and session model.
        /// </summary>
        /// <param name="binariesPath">The path to the binaries to be used for this driver.</param>
        /// <param name="session">The <see cref="SessionModel"/> to be used for this driver.</param>
        public AndroidDriver(string binariesPath, SessionModel session)
            : this(new AndroidWebDriverService(binariesPath), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using the specified driver service, Android options, and timeout.
        /// </summary>
        /// <param name="driverService">The <see cref="AndroidWebDriverService"/> to be used for this driver.</param>
        /// <param name="options">The <see cref="AndroidOptions"/> to be used for this driver.</param>
        /// <param name="timeout">The timeout period for the driver commands.</param>
        public AndroidDriver(AndroidWebDriverService driverService, AndroidOptions options, TimeSpan timeout)
            : this(driverService, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <inheritdoc />
        public AndroidDriver(AndroidWebDriverService driverService, SessionModel session, TimeSpan timeout)
            : base(new WebDriverCommandInvoker(driverService, timeout), session.Capabilities.NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using a remote server address and default Android options.
        /// </summary>
        /// <param name="remoteServerAddress">The URI of the remote WebDriver server address.</param>
        public AndroidDriver(Uri remoteServerAddress)
            : this(remoteServerAddress, new AndroidOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using a remote server address and specified Android options.
        /// </summary>
        /// <param name="remoteServerAddress">The URI of the remote WebDriver server address.</param>
        /// <param name="options">The <see cref="AndroidOptions"/> to be used for this driver.</param>
        public AndroidDriver(Uri remoteServerAddress, AndroidOptions options)
            : this(remoteServerAddress, options, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using a remote server address, specified Android options, and a timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The URI of the remote WebDriver server address.</param>
        /// <param name="options">The <see cref="AndroidOptions"/> to be used for this driver.</param>
        /// <param name="timeout">The timeout period for the driver commands.</param>
        public AndroidDriver(Uri remoteServerAddress, AndroidOptions options, TimeSpan timeout)
            : this(remoteServerAddress, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDriver"/> class using a remote server address, session model, and a timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The URI of the remote WebDriver server address.</param>
        /// <param name="session">The <see cref="SessionModel"/> to be used for this driver.</param>
        /// <param name="timeout">The timeout period for the driver commands.</param>
        public AndroidDriver(Uri remoteServerAddress, SessionModel session, TimeSpan timeout)
            : this(new WebDriverCommandInvoker(remoteServerAddress, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <inheritdoc />
        public AndroidDriver(IWebDriverCommandInvoker invoker, SessionModel session)
            : base(invoker, session)
        {
            // Additional initialization logic can be added here if needed.
        }
        #endregion
    }
}
