using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace G4.WebDriver.Remote.Sap.Models
{
    /// <summary>
    /// Represents the request body that calls an allow-listed SAP GUI Scripting method on a control.
    /// </summary>
    /// <remarks>
    /// This is the client copy of the server's method-call contract; the server rejects methods that are not on its
    /// allow-list for the control family.
    /// </remarks>
    public class SapMethodCallModel
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the positional scalar arguments passed to the method, in call order.
        /// </summary>
        /// <remarks>
        /// An empty list calls the method without arguments.
        /// </remarks>
        public IList<object> Arguments { get; set; } = [];

        /// <summary>
        /// Gets or sets the SAP GUI Scripting method name, such as <c>pressToolbarButton</c> or <c>expandNode</c>.
        /// </summary>
        [Required]
        public string Method { get; set; }
        #endregion
    }

    /// <summary>
    /// Represents the request body that sets an allow-listed SAP GUI Scripting property on a control.
    /// </summary>
    /// <remarks>
    /// This is the client copy of the server's property contract; the server rejects properties that are not on its
    /// allow-list for the control family.
    /// </remarks>
    public class SapPropertyModel
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the property name, such as <c>selectedRows</c> or <c>verticalScrollbar.position</c>.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the scalar value assigned to the property.
        /// </summary>
        public object Value { get; set; }
        #endregion
    }

    /// <summary>
    /// Represents the main window's status bar of the attached SAP GUI session.
    /// </summary>
    /// <remarks>
    /// The model holds the message fields only; it carries no screen or business data beyond the status message.
    /// </remarks>
    public class SapStatusBarModel
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets a value indicating whether SAP GUI shows the message as a pop-up instead of in the status bar.
        /// </summary>
        public bool MessageAsPopup { get; set; }

        /// <summary>
        /// Gets or sets the message class (ID) of the status message.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the message number within the message class.
        /// </summary>
        public string MessageNumber { get; set; }

        /// <summary>
        /// Gets or sets the message type, such as <c>S</c> (success), <c>W</c> (warning), or <c>E</c> (error).
        /// </summary>
        /// <remarks>
        /// Empty when the status bar holds no message.
        /// </remarks>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the message text shown in the status bar.
        /// </summary>
        public string Text { get; set; }
        #endregion
    }

    /// <summary>
    /// Represents the request body that sends a SAP virtual key to a window of the attached session.
    /// </summary>
    public class SapVirtualKeyModel
    {
        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the SAP virtual key code, from 0 to 99, such as 0 for Enter or 3 for Back.
        /// </summary>
        [Range(0, 99)]
        public int VirtualKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the window that receives the key, such as <c>wnd[1]</c>.
        /// </summary>
        /// <remarks>
        /// Null sends the key to the session's current window.
        /// </remarks>
        public string Window { get; set; }
        #endregion
    }
}
