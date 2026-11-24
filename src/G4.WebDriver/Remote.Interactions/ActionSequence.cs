using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace G4.WebDriver.Remote.Interactions
{
    /// <summary>
    /// Represents a sequence of actions to be performed using WebDriver interactions.
    /// </summary>
    public class ActionSequence
    {
        #region *** Constants     ***
        // Lists the pointer types a pointer source accepts. The constructor checks against it, so an unknown
        // type fails when the sequence is created and not later as a driver error.
        private static readonly HashSet<string> SupportedPointerTypes =
        [
            PointerTypes.Mouse,
            PointerTypes.Pen,
            PointerTypes.Touch
        ];
        #endregion

        #region *** Fields        ***
        public readonly InputSourceModel _keySource;   // Private field to store the keyboard input source
        public readonly InputSourceModel _mouseSource; // Private field to store the mouse input source
        private readonly IWebDriver _driver;           // Private field to store the WebDriver instance

        // Holds the wheel actions of this sequence. It stays empty until the first scroll is added and is sent
        // only when it holds an action, so a sequence without scrolling still sends the two original sources.
        private readonly InputSourceModel _wheelSource;
        #endregion

        #region *** Constructors  ***
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSequence"/> class with a mouse pointer.
        /// </summary>
        /// <param name="driver">The <see cref="IWebDriver"/> instance to use for actions.</param>
        public ActionSequence(IWebDriver driver)
            : this(driver, pointerType: PointerTypes.Mouse)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSequence"/> class with a pointer of the given type.
        /// </summary>
        /// <remarks>
        /// The pointer type is fixed for the lifetime of the sequence and decides which events the application
        /// receives for the pointer actions (mouse, pen or touch events). The sequence owns one pointer source,
        /// one keyboard source and one wheel source; it does not model several pointers at once.
        /// </remarks>
        /// <param name="driver">The <see cref="IWebDriver"/> instance to use for actions.</param>
        /// <param name="pointerType">The pointer type, one of the <see cref="PointerTypes"/> values.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="pointerType"/> is not a known pointer type.
        /// </exception>
        public ActionSequence(IWebDriver driver, string pointerType)
        {
            // Reject an unknown pointer type before any source is built, so a typo fails here with the list
            // of accepted values and not as an "invalid argument" answer from the driver.
            if (!SupportedPointerTypes.Contains(pointerType))
            {
                var message = $"The pointer type '{pointerType}' is not supported. " +
                    $"Supported types: {string.Join(", ", SupportedPointerTypes)}.";
                throw new ArgumentException(message, nameof(pointerType));
            }

            // Store the driver instance
            _driver = driver;

            // Initialize the keyboard input source
            _keySource = new InputSourceModel
            {
                Type = InputSourceTypes.Key,
                Id = "default keyboard",
                Actions = []
            };

            // Initialize the pointer input source; the source id carries the pointer type, which keeps the
            // original "default mouse" id for a mouse pointer.
            _mouseSource = new InputSourceModel
            {
                Type = InputSourceTypes.Pointer,
                Id = $"default {pointerType}",
                Parameters = new Dictionary<string, object>
                {
                    ["pointerType"] = pointerType
                },
                Actions = []
            };

            // Initialize the wheel input source; it is created up front and joins the request only after a
            // scroll is added.
            _wheelSource = new InputSourceModel
            {
                Type = InputSourceTypes.Wheel,
                Id = "default wheel",
                Actions = []
            };
        }
        #endregion

        #region *** Methods       ***
        /// <summary>
        /// Adds one raw action to the sequence and routes it to the input source that owns its type.
        /// </summary>
        /// <remarks>
        /// This is the low-level entry point: the caller builds the action model and the sequence only places
        /// it. A <see cref="KeyboardActionModel"/> goes to the keyboard source, a <see cref="MouseActionModel"/>
        /// to the pointer source and a <see cref="WheelActionModel"/> to the wheel source; a
        /// <see cref="NullActionModel"/> (a pause) holds the keyboard and pointer sources together. Every action
        /// takes a tick of its own, so the driver runs the actions in the order they were added. Nothing is sent
        /// until <see cref="Invoke"/> is called. The driver reads the duration of a pointer move and of a pause,
        /// so set <see cref="ActionModelBase.Duration"/> on those models; a missing duration is sent as null.
        /// </remarks>
        /// <param name="action">The action to add.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type of <paramref name="action"/> has no input source.
        /// </exception>
        public ActionSequence AddAction(ActionModelBase action)
        {
            // Require the action before its type selects the input source that receives it.
            ArgumentNullException.ThrowIfNull(argument: action);

            // Wrap the single action once, because the source helpers take collections of actions.
            var actions = new[] { action };

            // Route the action to the source that owns its type. Every branch leaves the keyboard and pointer
            // sources with the same number of ticks, which is what keeps the actions in the order they were added.
            switch (action)
            {
                case KeyboardActionModel:
                    AddKeyboardActions(keySource: _keySource, mouseSource: _mouseSource, actions);
                    break;
                case MouseActionModel:
                    AddMouseActions(mouseSource: _mouseSource, keySource: _keySource, actions);
                    break;
                case WheelActionModel:
                    AddWheelAction(sequence: this, action);
                    break;
                case NullActionModel:
                    // A pause belongs to no single device, so it holds both sources for the same tick.
                    _keySource.Actions.Add(action);
                    _mouseSource.Actions.Add(action);
                    break;
                default:
                    // Report the unknown model by name, because no input source can carry it to the driver.
                    var message = $"The action type '{action.GetType().Name}' has no input source in this sequence.";
                    throw new ArgumentException(message, nameof(action));
            }

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a click action at the current mouse cursor position.
        /// </summary>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddClick()
        {
            // Get the left click action
            var mouseActions = MouseActions.NewLeftClickAction();

            // Add the left click action to the sequence
            AddMouseActions(_mouseSource, _keySource, mouseActions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a click action at the specified element.
        /// </summary>
        /// <param name="element">The element to click on.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
        public ActionSequence AddClick(IWebElement element)
        {
            // Throw an ArgumentNullException with a descriptive message
            if (element == null)
            {
                throw new ArgumentNullException(
                    nameof(element), "The 'element' parameter cannot be null.");
            }

            // Get the move and left click actions
            var moveActions = new[] { MouseActions.NewMoveAction(element) };
            var clickActions = MouseActions.NewLeftClickAction();

            // Add the move and left click actions to the sequence
            AddMouseActions(_mouseSource, _keySource, moveActions, clickActions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a click action at the specified coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate.</param>
        /// <param name="y">The Y-coordinate.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddClick(int x, int y)
        {
            // Get the move and left click actions
            var moveAction = new[] { MouseActions.NewMoveAction(x, y) };
            var clickAction = MouseActions.NewLeftClickAction();

            // Add the move and left click actions to the sequence
            AddMouseActions(_mouseSource, _keySource, moveAction, clickAction);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a double-click action to the sequence.
        /// </summary>
        /// <returns>The current <see cref="ActionSequence"/> instance for chaining.</returns>
        public ActionSequence AddContextClick()
        {
            // Get the right-click action
            var mouseActions = MouseActions.NewRightClickAction();

            // Add the right-click action to the sequence
            AddMouseActions(_mouseSource, _keySource, mouseActions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a double-click action to the sequence with the specified element.
        /// </summary>
        /// <param name="element">The element on which to perform the double-click action.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance for chaining.</returns>
        public ActionSequence AddContextClick(IWebElement element)
        {
            // Throw an ArgumentNullException with a descriptive message
            if (element == null)
            {
                throw new ArgumentNullException(
                    nameof(element), "The 'element' parameter cannot be null.");
            }

            // Get the move and right-click actions
            var moveActions = new[] { MouseActions.NewMoveAction(element) };
            var clickActions = MouseActions.NewRightClickAction();

            // Add the move and right-click actions to the sequence
            AddMouseActions(_mouseSource, _keySource, moveActions, clickActions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a double-click action to the sequence at the specified coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate.</param>
        /// <param name="y">The Y-coordinate.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance for chaining.</returns>
        public ActionSequence AddContextClick(int x, int y)
        {
            // Get the move and right-click actions
            var moveAction = new[] { MouseActions.NewMoveAction(x, y) };
            var clickAction = MouseActions.NewRightClickAction();

            // Add the move and right-click actions to the sequence
            AddMouseActions(_mouseSource, _keySource, moveAction, clickAction);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a double-click action to the sequence.
        /// </summary>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddDoubleClick()
        {
            // Get the mouse double-click action
            var clickAction = MouseActions.NewLeftClickAction();

            // Add the double-click action to the sequence
            AddMouseActions(_mouseSource, _keySource, clickAction, clickAction);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a double-click action at the specified element.
        /// </summary>
        /// <param name="element">The <see cref="IWebElement"/> to double-click.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddDoubleClick(IWebElement element)
        {
            // Throw an ArgumentNullException with a descriptive message
            if (element == null)
            {
                throw new ArgumentNullException(
                    nameof(element), "The 'element' parameter cannot be null.");
            }

            // Get the move action to the element and mouse double-click action
            var moveActions = new[] { MouseActions.NewMoveAction(element) };
            var clickActions = MouseActions.NewLeftClickAction();

            // Add the move action and double-click action to the sequence
            AddMouseActions(_mouseSource, _keySource, moveActions, clickActions, clickActions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a double-click action at the specified coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate.</param>
        /// <param name="y">The Y-coordinate.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddDoubleClick(int x, int y)
        {
            // Get the move action to the coordinates and mouse double-click action
            var moveAction = new[] { MouseActions.NewMoveAction(x, y) };
            var clickAction = MouseActions.NewLeftClickAction();

            // Add the move action and double-click action to the sequence
            AddMouseActions(_mouseSource, _keySource, moveAction, clickAction, clickAction);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a key down action for the specified key.
        /// </summary>
        /// <param name="key">The key to press down.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddKeyDown(string key)
        {
            // Throw an ArgumentNullException with a descriptive message
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(
                    nameof(key), "The 'key' parameter cannot be null or empty.");
            }

            // Create a keyboard action for key down
            var actions = new[]
            {
                KeyboardActions.NewKeyDownAction(key)
            };

            // Add the keyboard action to the sequence
            AddKeyboardActions(_keySource, _mouseSource, actions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a key down action to the action sequence for a specific WebElement.
        /// </summary>
        /// <param name="element">The WebElement to target for the key down action.</param>
        /// <param name="key">The key to press down.</param>
        /// <returns>The updated ActionSequence instance for method chaining.</returns>
        public ActionSequence AddKeyDown(IWebElement element, string key)
        {
            // Throw an ArgumentNullException with a descriptive message
            if (element == null)
            {
                throw new ArgumentNullException(
                    nameof(element), "The 'element' parameter cannot be null.");
            }

            // Throw an ArgumentNullException with a descriptive message
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(
                    nameof(key), "The 'key' parameter cannot be null or empty.");
            }

            // Get the move, left-click, and key down actions
            var moveActions = new[] { MouseActions.NewMoveAction(element) };
            var clickActions = MouseActions.NewLeftClickAction();
            var keyActions = new[] { KeyboardActions.NewKeyDownAction(key) };

            // Add the move and left-click actions to the sequence
            AddMouseActions(_mouseSource, _keySource, moveActions, clickActions);

            // Add the key down action to the sequence
            AddKeyboardActions(_keySource, _mouseSource, keyActions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a key up action for the specified key.
        /// </summary>
        /// <param name="key">The key to release.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddKeyUp(string key)
        {
            // Throw an ArgumentNullException with a descriptive message
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(
                    nameof(key), "The 'key' parameter cannot be null or empty.");
            }

            // Create a keyboard action for key up
            var action = new KeyboardActionModel
            {
                Type = KeyboardActionModel.ActionTypes.KeyUp,
                Value = key
            };
            var actions = new[] { action };

            // Add the keyboard action to the sequence
            AddKeyboardActions(_keySource, _mouseSource, actions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a key up action to the action sequence for a specific WebElement.
        /// </summary>
        /// <param name="element">The WebElement to target for the key up action.</param>
        /// <param name="key">The key to release (key up).</param>
        /// <returns>The updated ActionSequence instance for method chaining.</returns>
        public ActionSequence AddKeyUp(IWebElement element, string key)
        {
            // Throw an ArgumentNullException with a descriptive message
            if (element == null)
            {
                throw new ArgumentNullException(
                    nameof(element), "The 'element' parameter cannot be null.");
            }

            // Throw an ArgumentNullException with a descriptive message
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(
                    nameof(key), "The 'key' parameter cannot be null or empty.");
            }

            // Get the move, left-click, and key up actions
            var moveActions = new[] { MouseActions.NewMoveAction(element) };
            var clickActions = MouseActions.NewLeftClickAction();
            var keyActions = new[] { KeyboardActions.NewKeyUpAction(key) };

            // Add the move and left-click actions to the sequence
            AddMouseActions(_mouseSource, _keySource, moveActions, clickActions);

            // Add the key up action to the sequence
            AddKeyboardActions(_keySource, _mouseSource, keyActions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a key press action to the action sequence, representing both key down and key up actions.
        /// </summary>
        /// <param name="key">The key to press.</param>
        /// <returns>The updated ActionSequence instance for method chaining.</returns>
        public ActionSequence AddKeyPress(string key)
        {
            // Throw an ArgumentNullException with a descriptive message
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(
                    nameof(key), "The 'key' parameter cannot be null or empty.");
            }

            // Create an array of actions representing key down and key up actions for the specified key
            var actions = new[]
            {
                KeyboardActions.NewKeyDownAction(key),
                KeyboardActions.NewKeyUpAction(key)
            };

            // Add the key press actions to the sequence
            AddKeyboardActions(_keySource, _mouseSource, actions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a key press action associated with a specific WebElement, representing both key down and key up actions.
        /// </summary>
        /// <param name="element">The WebElement associated with the key press.</param>
        /// <param name="key">The key to press.</param>
        /// <returns>The updated ActionSequence instance for method chaining.</returns>
        public ActionSequence AddKeyPress(IWebElement element, string key)
        {
            // Throw an ArgumentNullException with a descriptive message
            if (element == null)
            {
                throw new ArgumentNullException(
                    nameof(element), "The 'element' parameter cannot be null.");
            }

            // Throw an ArgumentNullException with a descriptive message
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(
                    nameof(key), "The 'key' parameter cannot be null or empty.");
            }

            // Create an array of actions representing key down and key up actions for the specified key
            var keyActions = new[]
            {
                KeyboardActions.NewKeyDownAction(key),
                KeyboardActions.NewKeyUpAction(key)
            };

            // Create mouse move and click actions for the specified element
            var moveActions = new[] { MouseActions.NewMoveAction(element) };
            var clickActions = MouseActions.NewLeftClickAction();

            // Add the mouse move and click actions to the sequence
            AddMouseActions(_mouseSource, _keySource, moveActions, clickActions);

            // Add the key press actions to the sequence
            AddKeyboardActions(_keySource, _mouseSource, keyActions);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a move mouse cursor action to the specified coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate to move the mouse cursor to.</param>
        /// <param name="y">The Y-coordinate to move the mouse cursor to.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance for method chaining.</returns>
        public ActionSequence AddMoveMouseCursor(int x, int y)
        {
            // Call the overloaded method with a default origin of "pointer"
            return AddMoveMouseCursor(x, y, "pointer");
        }

        /// <summary>
        /// Adds a move mouse cursor action to the specified coordinates, with a specified origin.
        /// </summary>
        /// <param name="x">The X-coordinate to move the mouse cursor to.</param>
        /// <param name="y">The Y-coordinate to move the mouse cursor to.</param>
        /// <param name="origin">The origin reference for the move action, e.g., "pointer".</param>
        /// <returns>The current <see cref="ActionSequence"/> instance for method chaining.</returns>
        public ActionSequence AddMoveMouseCursor(int x, int y, string origin)
        {
            // Create a new move mouse action with the specified coordinates
            var mouseAction = MouseActions.NewMoveAction(x, y);

            // Set the origin of the mouse action
            mouseAction.Origin = origin;

            // Create an array containing the move action
            var mouseActions = new[] { mouseAction };

            // Add the mouse action to the action sequence using the provided sources
            AddMouseActions(_mouseSource, _keySource, mouseActions);

            // Return the current ActionSequence instance to allow method chaining
            return this;
        }

        /// <summary>
        /// Adds a move mouse cursor action to the specified element.
        /// </summary>
        /// <param name="element">The <see cref="IWebElement"/> to move the cursor to.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddMoveMouseCursor(IWebElement element)
        {
            // Get the move action to the specified element
            var mouseAction = new[] { MouseActions.NewMoveAction(element) };

            // Add the move action to the sequence
            AddMouseActions(_mouseSource, _keySource, mouseAction);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a pause action to the action sequence, pausing the sequence for the specified duration.
        /// </summary>
        /// <param name="duration">The duration of the pause in milliseconds.</param>
        /// <returns>The updated ActionSequence instance for method chaining.</returns>
        public ActionSequence AddPauseAction(int duration)
        {
            // Create a pause action with the specified duration
            var action = new NullActionModel
            {
                Duration = duration,
                Type = NullActionModel.ActionTypes.Pause
            };

            // Add the pause action to both key and mouse action sources
            _keySource.Actions.Add(action);
            _mouseSource.Actions.Add(action);

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Adds a press of the left pointer button at the current pointer position, without releasing it.
        /// </summary>
        /// <remarks>
        /// The button stays pressed after the sequence is sent, until a pointer up action or
        /// <see cref="ClearActions"/> releases it.
        /// </remarks>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddPointerDown()
        {
            // Press the left button, the primary button of every pointer type.
            return AddPointerDown(button: MouseActionModel.MouseButtons.Left);
        }

        /// <summary>
        /// Adds a press of the given pointer button at the current pointer position, without releasing it.
        /// </summary>
        /// <remarks>
        /// The press and the release are separate actions, which is what makes drag, press-and-hold and
        /// modifier-style gestures possible. The button stays pressed after the sequence is sent, until a
        /// pointer up action for the same button or <see cref="ClearActions"/> releases it.
        /// </remarks>
        /// <param name="button">
        /// The button to press, one of the <see cref="MouseActionModel.MouseButtons"/> values.
        /// </param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="button"/> is negative.</exception>
        public ActionSequence AddPointerDown(int button)
        {
            // Reject a negative button here, because the driver accepts only an unsigned button number.
            ArgumentOutOfRangeException.ThrowIfNegative(value: button);

            // Describe the press alone; the release is a separate action the caller adds when the gesture ends.
            var action = new MouseActionModel
            {
                Button = button,
                Type = MouseActionModel.ActionTypes.PointerDown
            };

            // Place the press on its own tick of the pointer source.
            return AddAction(action);
        }

        /// <summary>
        /// Adds a release of the left pointer button at the current pointer position.
        /// </summary>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddPointerUp()
        {
            // Release the left button, the primary button of every pointer type.
            return AddPointerUp(button: MouseActionModel.MouseButtons.Left);
        }

        /// <summary>
        /// Adds a release of the given pointer button at the current pointer position.
        /// </summary>
        /// <remarks>
        /// Releasing a button that is not pressed is not an error; the driver ignores it. The release can be
        /// sent in a later sequence than the press, because the driver keeps the input state between requests.
        /// </remarks>
        /// <param name="button">
        /// The button to release, one of the <see cref="MouseActionModel.MouseButtons"/> values.
        /// </param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="button"/> is negative.</exception>
        public ActionSequence AddPointerUp(int button)
        {
            // Reject a negative button here, because the driver accepts only an unsigned button number.
            ArgumentOutOfRangeException.ThrowIfNegative(value: button);

            // Describe the release alone, so it can end a press that an earlier action or sequence started.
            var action = new MouseActionModel
            {
                Button = button,
                Type = MouseActionModel.ActionTypes.PointerUp
            };

            // Place the release on its own tick of the pointer source.
            return AddAction(action);
        }

        /// <summary>
        /// Adds a wheel scroll that starts at the top-left corner of the viewport.
        /// </summary>
        /// <remarks>
        /// The driver scrolls the scrollable container under the start point, which for this overload is
        /// usually the page itself. Use the element overload to scroll a container inside the page.
        /// </remarks>
        /// <param name="deltaX">The horizontal scroll amount in pixels; a positive value scrolls right.</param>
        /// <param name="deltaY">The vertical scroll amount in pixels; a positive value scrolls down.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        public ActionSequence AddScroll(int deltaX, int deltaY)
        {
            // Describe a scroll from the viewport origin; the model's default origin is the viewport.
            var action = new WheelActionModel
            {
                DeltaX = deltaX,
                DeltaY = deltaY
            };

            // Place the scroll on its own tick of the wheel source.
            return AddAction(action);
        }

        /// <summary>
        /// Adds a wheel scroll that starts at the center of the given element.
        /// </summary>
        /// <remarks>
        /// The driver scrolls the nearest scrollable container under the element's center, so this overload
        /// scrolls a list, a text area or another container without moving the pointer first. The driver
        /// reports an error when the element's center is outside the viewport.
        /// </remarks>
        /// <param name="element">The element whose center is the scroll start point.</param>
        /// <param name="deltaX">The horizontal scroll amount in pixels; a positive value scrolls right.</param>
        /// <param name="deltaY">The vertical scroll amount in pixels; a positive value scrolls down.</param>
        /// <returns>The current <see cref="ActionSequence"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
        public ActionSequence AddScroll(IWebElement element, int deltaX, int deltaY)
        {
            // Require the element before its reference becomes the scroll origin.
            ArgumentNullException.ThrowIfNull(argument: element);

            // Describe a scroll that starts from the element, using the element reference the driver expects.
            var action = new WheelActionModel
            {
                DeltaX = deltaX,
                DeltaY = deltaY,
                Origin = element.ConvertToDictionary()
            };

            // Place the scroll on its own tick of the wheel source.
            return AddAction(action);
        }

        /// <summary>
        /// The Release Actions command is used to release all the keys and pointer buttons that are currently depressed.
        /// This causes events to be fired as if the state was released by an explicit series of actions.
        /// It also clears all the internal state of the virtual devices.
        /// </summary>
        /// <returns>The updated ActionSequence instance for method chaining.</returns>
        public ActionSequence ClearActions()
        {
            try
            {
                // Get the invoker from the WebDriver instance
                var invoker = _driver.Invoker;

                // Mount the command for delete actions
                var command = invoker.NewCommand(nameof(WebDriverCommands.DeleteActions));

                // Invoke the command
                invoker.Invoke(command);
            }
            catch (Exception e)
            {
                // If an exception occurs during the invocation, rethrow the base exception
                throw e.GetBaseException();
            }
            finally
            {
                // Clear actions from both key and mouse action sources
                _keySource.Actions.Clear();
                _mouseSource.Actions.Clear();

                // Clear the wheel source with them, so a reused sequence starts with every source empty.
                _wheelSource.Actions.Clear();
            }

            // Return the current instance for chaining
            return this;
        }

        /// <summary>
        /// Gets the input sources this sequence sends when it is invoked, in the order they are sent.
        /// </summary>
        /// <remarks>
        /// The pointer and keyboard sources are always included. The wheel source is included only when it
        /// holds an action, so a driver that does not know wheel sources never receives one from a sequence
        /// that does not scroll. The returned models are the live sources of the sequence, not copies.
        /// </remarks>
        /// <returns>The input sources that <see cref="Invoke"/> sends.</returns>
        public IEnumerable<InputSourceModel> GetInputSources()
        {
            // Start with the two sources every sequence sends, in the order the request always used.
            var sources = new List<InputSourceModel> { _mouseSource, _keySource };

            // Add the wheel source only when a scroll was added, which keeps the request of a sequence
            // without scrolling identical to the one sent before wheel support existed.
            if (_wheelSource.Actions.Count > 0)
            {
                sources.Add(_wheelSource);
            }

            return sources;
        }

        /// <summary>
        /// Invokes the WebDriver actions by sending the mouse and key actions to the driver.
        /// </summary>
        public void Invoke()
        {
            // Create a data object containing the mouse and key sources, and the wheel source when it has actions.
            var data = new
            {
                Actions = GetInputSources().ToArray()
            };

            // Invoke the WebDriver commands and pass the action data.
            _driver.Invoker.Invoke(nameof(WebDriverCommands.Actions), data);
        }

        // Adds mouse actions and corresponding pauses to input sources.
        private static void AddMouseActions(
            InputSourceModel mouseSource,
            InputSourceModel keySource,
            params IEnumerable<ActionModelBase>[] mouseActions)
        {
            // Combine all mouse actions into a single sequence of actions.
            var actions = mouseActions.SelectMany(i => i);

            // Create a null action representing a pause.
            var pause = new NullActionModel
            {
                Duration = 0,
                Type = NullActionModel.ActionTypes.Pause
            };

            // Add each mouse action to the mouse input source
            // and add a corresponding pause to the key input source.
            foreach (var action in actions)
            {
                mouseSource.Actions.Add(action);
                keySource.Actions.Add(pause);
            }
        }

        // Adds keyboard actions and corresponding pauses to input sources.
        private static void AddKeyboardActions(
            InputSourceModel keySource,
            InputSourceModel mouseSource,
            params IEnumerable<ActionModelBase>[] keyboardActions)
        {
            // Combine all keyboard actions into a single sequence of actions.
            var actions = keyboardActions.SelectMany(i => i);

            // Create a null action representing a pause.
            var pause = new NullActionModel
            {
                Duration = 0,
                Type = NullActionModel.ActionTypes.Pause
            };

            // Add each keyboard action to the key input source
            // and add a corresponding pause to the mouse input source.
            foreach (var action in actions)
            {
                keySource.Actions.Add(action);
                mouseSource.Actions.Add(pause);
            }
        }

        // Adds one wheel action on a tick of its own and holds the pointer and keyboard sources for that tick.
        // The wheel source is not padded while pointer and keyboard actions are added, so it can be shorter
        // than the two other sources; this helper first brings it up to the current tick. It mutates the three
        // sources of the given sequence and sends nothing.
        private static void AddWheelAction(ActionSequence sequence, ActionModelBase action)
        {
            // Create a null action representing a pause.
            var pause = new NullActionModel
            {
                Duration = 0,
                Type = NullActionModel.ActionTypes.Pause
            };

            // Pad the wheel source up to the tick the other sources reached, so the scroll runs after every
            // action that was added before it and not at the start of the sequence.
            for (var i = sequence._wheelSource.Actions.Count; i < sequence._mouseSource.Actions.Count; i++)
            {
                sequence._wheelSource.Actions.Add(pause);
            }

            // Add the wheel action and hold the two other sources with a pause, so the next pointer or
            // keyboard action runs after the scroll.
            sequence._wheelSource.Actions.Add(action);
            sequence._mouseSource.Actions.Add(pause);
            sequence._keySource.Actions.Add(pause);
        }
        #endregion

        #region *** Nested Types  ***
        /// <summary>
        /// Helper class for creating mouse action models.
        /// </summary>
        private static class MouseActions
        {
            /// <summary>
            /// Creates a new mouse move action model to move to a specific element.
            /// </summary>
            /// <param name="element">The WebElement to move the mouse to.</param>
            /// <returns>A new instance of a mouse move action model.</returns>
            public static MouseActionModel NewMoveAction(IWebElement element)
            {
                // Convert the element to a dictionary for origin data
                var origin = element.ConvertToDictionary();

                // Create a mouse move action model to move to the element
                return new MouseActionModel
                {
                    Duration = 250,                                 // Duration in milliseconds for the movement
                    Origin = origin,                                // Origin data for the movement
                    Type = MouseActionModel.ActionTypes.PointerMove // Type of mouse action
                };
            }

            /// <summary>
            /// Creates a new mouse move action model to move to a specific position.
            /// </summary>
            /// <param name="x">The X-coordinate to move to.</param>
            /// <param name="y">The Y-coordinate to move to.</param>
            /// <returns>A new instance of a mouse move action model.</returns>
            public static MouseActionModel NewMoveAction(int x, int y)
            {
                // Create a mouse move action model to move to the specified position
                return new MouseActionModel
                {
                    Duration = 250,                                  // Duration in milliseconds for the movement
                    Type = MouseActionModel.ActionTypes.PointerMove, // Type of mouse action
                    X = x,                                           // Target X-coordinate
                    Y = y                                            // Target Y-coordinate
                };
            }

            /// <summary>
            /// Creates a sequence of mouse actions for a left click (pointer down and up).
            /// </summary>
            /// <returns>A sequence of mouse actions for left click.</returns>
            public static MouseActionModel[] NewLeftClickAction() =>
            [
                // Perform a pointer down action (left-click)
                new MouseActionModel
                {
                    Button = MouseActionModel.MouseButtons.Left,
                    Type = MouseActionModel.ActionTypes.PointerDown
                },

                // Perform a pointer up action (release left-click)
                new MouseActionModel
                {
                    Button = MouseActionModel.MouseButtons.Left,
                    Type = MouseActionModel.ActionTypes.PointerUp
                }
            ];

            /// <summary>
            /// Creates a sequence of mouse actions for a right click (pointer down and up).
            /// </summary>
            /// <returns>A sequence of mouse actions for right click.</returns>
            public static MouseActionModel[] NewRightClickAction() =>
            [
                // Perform a pointer down action (right-click)
                new MouseActionModel
                {
                    Button = MouseActionModel.MouseButtons.Right,
                    Type = MouseActionModel.ActionTypes.PointerDown
                },
                // Perform a pointer up action (release right-click)
                new MouseActionModel
                {
                    Button = MouseActionModel.MouseButtons.Right,
                    Type = MouseActionModel.ActionTypes.PointerUp
                }
            ];
        }

        /// <summary>
        /// Helper class for creating keyboard action models.
        /// </summary>
        private static class KeyboardActions
        {
            /// <summary>
            /// Creates a new keyboard key down action model.
            /// </summary>
            /// <param name="key">The key to press down.</param>
            /// <returns>A new instance of a keyboard key down action model.</returns>
            public static KeyboardActionModel NewKeyDownAction(string key)
            {
                // Create a new KeyboardActionModel for key down action.
                return new KeyboardActionModel
                {
                    Type = KeyboardActionModel.ActionTypes.KeyDown,
                    Value = key
                };
            }

            /// <summary>
            /// Creates a new keyboard key up action model.
            /// </summary>
            /// <param name="key">The key to release (key up).</param>
            /// <returns>A new instance of a keyboard key up action model.</returns>
            public static KeyboardActionModel NewKeyUpAction(string key)
            {
                // Create a new KeyboardActionModel for key up action.
                return new KeyboardActionModel
                {
                    Type = KeyboardActionModel.ActionTypes.KeyUp,
                    Value = key
                };
            }
        }
        #endregion
    }
}
