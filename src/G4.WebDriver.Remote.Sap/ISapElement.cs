namespace G4.WebDriver.Remote.Sap
{
    /// <summary>
    /// Represents the element-level SAP GUI Scripting members that the W3C element commands do not cover.
    /// </summary>
    /// <remarks>
    /// The server accepts only the methods and properties on its allow-list for the control family; anything else
    /// fails with a WebDriver exception. A stale element reference fails with a stale-element exception.
    /// </remarks>
    public interface ISapElement
    {
        #region *** Methods      ***
        /// <summary>
        /// Calls an allow-listed SAP GUI Scripting method on the control, such as <c>pressToolbarButton</c> on a grid
        /// view shell or <c>expandNode</c> on a tree shell.
        /// </summary>
        /// <param name="method">The method name.</param>
        /// <param name="arguments">The positional scalar arguments, in call order.</param>
        /// <returns>
        /// The method result: a scalar as is, a collection as an object array, a control as an element, or null when
        /// the method returns nothing.
        /// </returns>
        object InvokeMethod(string method, params object[] arguments);

        /// <summary>
        /// Sets an allow-listed SAP GUI Scripting property on the control, such as <c>selectedRows</c> on a grid view
        /// shell or <c>verticalScrollbar.position</c> on a table control.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="value">The scalar value assigned to the property.</param>
        void SetProperty(string name, object value);
        #endregion
    }
}
