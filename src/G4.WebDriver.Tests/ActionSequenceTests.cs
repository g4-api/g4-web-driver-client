using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;
using G4.WebDriver.Remote.Chrome;
using G4.WebDriver.Remote.Interactions;
using G4.WebDriver.Simulator;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace G4.WebDriver.Tests
{
    [TestClass]
    [TestCategory("ActionSequence")]
    public class ActionSequenceTests : TestBase<ChromeDriver>
    {
        // Serializes actions the way a request does (camel case names), so the serialization test reads the
        // same member names the driver receives.
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [TestMethod(DisplayName = "Verify that AddAction places a keyboard action on the keyboard source and " +
            "holds the pointer source with a pause on the same tick.")]
        [TestCategory("UnitTest")]
        public void AddActionKeyboardModelTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);
            var action = new KeyboardActionModel { Type = KeyboardActionModel.ActionTypes.KeyDown, Value = "a" };

            // Act: add the raw keyboard action through the low-level entry point.
            sequence.AddAction(action);

            // Assert: the keyboard source owns the action and the pointer source waits for that tick.
            Assert.AreSame(action, sequence._keySource.Actions.Single());
            Assert.IsInstanceOfType<NullActionModel>(sequence._mouseSource.Actions.Single());
        }

        [TestMethod(DisplayName = "Verify that AddAction places a pointer action on the pointer source and " +
            "holds the keyboard source with a pause on the same tick.")]
        [TestCategory("UnitTest")]
        public void AddActionMouseModelTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);
            var action = new MouseActionModel { Duration = 100, Type = MouseActionModel.ActionTypes.PointerMove };

            // Act: add the raw pointer action through the low-level entry point.
            sequence.AddAction(action);

            // Assert: the pointer source owns the action and the keyboard source waits for that tick.
            Assert.AreSame(action, sequence._mouseSource.Actions.Single());
            Assert.IsInstanceOfType<NullActionModel>(sequence._keySource.Actions.Single());
        }

        [TestMethod(DisplayName = "Verify that AddAction throws an ArgumentNullException for a null action.")]
        [TestCategory("UnitTest")]
        public void AddActionNullArgumentTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: describe the call with a missing action; the assertion below runs it.
            var act = () => sequence.AddAction(action: null);

            // Assert: the missing action is rejected before any source is changed.
            Assert.ThrowsExactly<ArgumentNullException>(act);
            Assert.IsEmpty(sequence._mouseSource.Actions);
        }

        [TestMethod(DisplayName = "Verify that AddAction places a pause on both the keyboard and the pointer " +
            "sources.")]
        [TestCategory("UnitTest")]
        public void AddActionPauseModelTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);
            var action = new NullActionModel { Duration = 50, Type = NullActionModel.ActionTypes.Pause };

            // Act: add the raw pause through the low-level entry point.
            sequence.AddAction(action);

            // Assert: both sources wait for the same pause, so neither runs ahead of the other.
            Assert.AreSame(action, sequence._keySource.Actions.Single());
            Assert.AreSame(action, sequence._mouseSource.Actions.Single());
        }

        [TestMethod(DisplayName = "Verify that AddAction throws an ArgumentException for an action type that " +
            "has no input source.")]
        [TestCategory("UnitTest")]
        public void AddActionUnsupportedModelTest()
        {
            // Arrange: a model type the sequence does not know stands in for a future action type.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: describe the call with the unknown model; the assertion below runs it.
            var act = () => sequence.AddAction(new UnsupportedActionModel());

            // Assert: the unknown model is rejected and named in the message.
            var exception = Assert.ThrowsExactly<ArgumentException>(act);
            Assert.Contains(nameof(UnsupportedActionModel), exception.Message);
        }

        [TestMethod(DisplayName = "Verify that AddPointerDown without a button presses the left button.")]
        [TestCategory("UnitTest")]
        public void AddPointerDownLeftButtonTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: add a press without naming a button.
            sequence.AddPointerDown();

            // Assert: the press uses the left button and has no release after it.
            var action = (MouseActionModel)sequence._mouseSource.Actions.Single();
            Assert.AreEqual(MouseActionModel.ActionTypes.PointerDown, action.Type);
            Assert.AreEqual(MouseActionModel.MouseButtons.Left, action.Button);
        }

        [TestMethod(DisplayName = "Verify that AddPointerDown throws an ArgumentOutOfRangeException for a " +
            "negative button.")]
        [TestCategory("UnitTest")]
        public void AddPointerDownNegativeButtonTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: describe the press of a negative button; the assertion below runs it.
            var act = () => sequence.AddPointerDown(button: -1);

            // Assert: the negative button is rejected before the pointer source is changed.
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(act);
            Assert.IsEmpty(sequence._mouseSource.Actions);
        }

        [TestMethod(DisplayName = "Verify that AddPointerDown adds one press of the given button, without a " +
            "release, and holds the keyboard source for that tick.")]
        #region *** Data Set ***
        [DataRow(MouseActionModel.MouseButtons.Left)]
        [DataRow(MouseActionModel.MouseButtons.Middle)]
        [DataRow(MouseActionModel.MouseButtons.Right)]
        [DataRow(MouseActionModel.MouseButtons.Back)]
        [DataRow(MouseActionModel.MouseButtons.Forward)]
        #endregion
        [TestCategory("UnitTest")]
        public void AddPointerDownTest(int button)
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: add a press of the button under test.
            sequence.AddPointerDown(button);

            // Assert: the pointer source holds exactly one press of that button.
            var action = (MouseActionModel)sequence._mouseSource.Actions.Single();
            Assert.AreEqual(MouseActionModel.ActionTypes.PointerDown, action.Type);
            Assert.AreEqual(button, action.Button);

            // Assert: the keyboard source waits for the same tick.
            Assert.IsInstanceOfType<NullActionModel>(sequence._keySource.Actions.Single());
        }

        [TestMethod(DisplayName = "Verify that AddPointerUp without a button releases the left button.")]
        [TestCategory("UnitTest")]
        public void AddPointerUpLeftButtonTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: add a release without naming a button.
            sequence.AddPointerUp();

            // Assert: the release uses the left button and has no press before it.
            var action = (MouseActionModel)sequence._mouseSource.Actions.Single();
            Assert.AreEqual(MouseActionModel.ActionTypes.PointerUp, action.Type);
            Assert.AreEqual(MouseActionModel.MouseButtons.Left, action.Button);
        }

        [TestMethod(DisplayName = "Verify that AddPointerUp throws an ArgumentOutOfRangeException for a " +
            "negative button.")]
        [TestCategory("UnitTest")]
        public void AddPointerUpNegativeButtonTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: describe the release of a negative button; the assertion below runs it.
            var act = () => sequence.AddPointerUp(button: -1);

            // Assert: the negative button is rejected before the pointer source is changed.
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(act);
            Assert.IsEmpty(sequence._mouseSource.Actions);
        }

        [TestMethod(DisplayName = "Verify that AddPointerUp adds one release of the given button and holds " +
            "the keyboard source for that tick.")]
        #region *** Data Set ***
        [DataRow(MouseActionModel.MouseButtons.Left)]
        [DataRow(MouseActionModel.MouseButtons.Middle)]
        [DataRow(MouseActionModel.MouseButtons.Right)]
        [DataRow(MouseActionModel.MouseButtons.Back)]
        [DataRow(MouseActionModel.MouseButtons.Forward)]
        #endregion
        [TestCategory("UnitTest")]
        public void AddPointerUpTest(int button)
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: add a release of the button under test.
            sequence.AddPointerUp(button);

            // Assert: the pointer source holds exactly one release of that button.
            var action = (MouseActionModel)sequence._mouseSource.Actions.Single();
            Assert.AreEqual(MouseActionModel.ActionTypes.PointerUp, action.Type);
            Assert.AreEqual(button, action.Button);

            // Assert: the keyboard source waits for the same tick.
            Assert.IsInstanceOfType<NullActionModel>(sequence._keySource.Actions.Single());
        }

        [TestMethod(DisplayName = "Verify that AddScroll with an element uses the element reference as the " +
            "scroll origin.")]
        [TestCategory("UnitTest")]
        public void AddScrollElementOriginTest()
        {
            // Arrange: a simulator element provides an element id without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);
            var element = new SimulatorElement(driver);

            // Act: add a scroll that starts from the element.
            sequence.AddScroll(element, deltaX: 0, deltaY: 120);

            // Assert: the wheel source holds a scroll whose origin is the reference of that element.
            var action = (WheelActionModel)GetWheelSource(sequence).Actions.Single();
            var origin = (IDictionary<string, object>)action.Origin;
            Assert.AreEqual(120, action.DeltaY);
            Assert.AreEqual(element.Id, origin.Values.Single());
        }

        [TestMethod(DisplayName = "Verify that AddScroll throws an ArgumentNullException for a null element.")]
        [TestCategory("UnitTest")]
        public void AddScrollNullElementTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: describe the scroll with a missing element; the assertion below runs it.
            var act = () => sequence.AddScroll(element: null, deltaX: 0, deltaY: 120);

            // Assert: the missing element is rejected and no wheel source joins the request.
            Assert.ThrowsExactly<ArgumentNullException>(act);
            Assert.HasCount(2, sequence.GetInputSources());
        }

        [TestMethod(DisplayName = "Verify that AddScroll runs the scroll after the actions added before it " +
            "and before the actions added after it.")]
        [TestCategory("UnitTest")]
        public void AddScrollTickAlignmentTest()
        {
            // Arrange: two pointer actions come first, so the scroll must wait for two ticks.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);
            sequence.AddPointerDown();
            sequence.AddPointerUp();

            // Act: add the scroll, then one keyboard action that must run after it.
            sequence.AddScroll(deltaX: 0, deltaY: 120);
            sequence.AddKeyDown("a");

            // Assert: the wheel source waits two ticks and scrolls on the third.
            var wheelActions = GetWheelSource(sequence).Actions;
            Assert.HasCount(3, wheelActions);
            Assert.IsInstanceOfType<NullActionModel>(wheelActions[0]);
            Assert.IsInstanceOfType<NullActionModel>(wheelActions[1]);
            Assert.IsInstanceOfType<WheelActionModel>(wheelActions[2]);

            // Assert: the pointer and keyboard sources wait on the scroll tick and run the key on the fourth.
            Assert.HasCount(4, sequence._mouseSource.Actions);
            Assert.HasCount(4, sequence._keySource.Actions);
            Assert.IsInstanceOfType<NullActionModel>(sequence._mouseSource.Actions[2]);
            Assert.IsInstanceOfType<NullActionModel>(sequence._keySource.Actions[2]);
            Assert.IsInstanceOfType<KeyboardActionModel>(sequence._keySource.Actions[3]);
        }

        [TestMethod(DisplayName = "Verify that AddScroll without an element scrolls from the viewport origin " +
            "with the given amounts and a numeric duration.")]
        [TestCategory("UnitTest")]
        public void AddScrollViewportOriginTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);

            // Act: add a scroll without naming an element.
            sequence.AddScroll(deltaX: 30, deltaY: 120);

            // Assert: the wheel source holds one scroll from the viewport with both amounts.
            var action = (WheelActionModel)GetWheelSource(sequence).Actions.Single();
            Assert.AreEqual(WheelActionModel.ActionTypes.Scroll, action.Type);
            Assert.AreEqual(ActionOrigins.Viewport, action.Origin);
            Assert.AreEqual(30, action.DeltaX);
            Assert.AreEqual(120, action.DeltaY);

            // Assert: the duration is a number, because the driver rejects a scroll with a null duration.
            Assert.AreEqual(0, action.Duration);
        }

        [TestMethod(DisplayName = "Verify that ClearActions empties every input source, including the wheel " +
            "source.")]
        [TestCategory("UnitTest")]
        public void ClearActionsTest()
        {
            // Arrange: a sequence with a pointer action and a scroll has actions on all three sources.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);
            sequence.AddPointerDown();
            sequence.AddScroll(deltaX: 0, deltaY: 120);

            // Act: release the input state and reset the sequence.
            sequence.ClearActions();

            // Assert: no source keeps an action and the wheel source leaves the request again.
            Assert.IsEmpty(sequence._mouseSource.Actions);
            Assert.IsEmpty(sequence._keySource.Actions);
            Assert.HasCount(2, sequence.GetInputSources());
        }

        [TestMethod(DisplayName = "Verify that the default constructor creates a mouse pointer source with " +
            "the original source id.")]
        [TestCategory("UnitTest")]
        public void ConstructorDefaultPointerTypeTest()
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");

            // Act: create the sequence the way existing callers do.
            var sequence = new ActionSequence(driver);

            // Assert: the pointer source is the same mouse source existing requests always carried.
            Assert.AreEqual(PointerTypes.Mouse, sequence._mouseSource.Parameters["pointerType"]);
            Assert.AreEqual("default mouse", sequence._mouseSource.Id);
        }

        [TestMethod(DisplayName = "Verify that the pointer source declares the pointer type the sequence was " +
            "created with.")]
        #region *** Data Set ***
        [DataRow(PointerTypes.Mouse)]
        [DataRow(PointerTypes.Pen)]
        [DataRow(PointerTypes.Touch)]
        #endregion
        [TestCategory("UnitTest")]
        public void ConstructorPointerTypeTest(string pointerType)
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");

            // Act: create the sequence with the pointer type under test.
            var sequence = new ActionSequence(driver, pointerType);

            // Assert: the pointer source tells the driver which device its actions imitate.
            Assert.AreEqual(InputSourceTypes.Pointer, sequence._mouseSource.Type);
            Assert.AreEqual(pointerType, sequence._mouseSource.Parameters["pointerType"]);
            Assert.AreEqual($"default {pointerType}", sequence._mouseSource.Id);
        }

        [TestMethod(DisplayName = "Verify that the constructor throws an ArgumentException for an unknown " +
            "pointer type.")]
        #region *** Data Set ***
        [DataRow("finger")]
        [DataRow("Mouse")]
        [DataRow("")]
        [DataRow(null)]
        #endregion
        [TestCategory("UnitTest")]
        public void ConstructorUnknownPointerTypeTest(string pointerType)
        {
            // Arrange: a simulator driver gives the sequence a driver without opening a browser.
            using var driver = new SimulatorDriver(".");

            // Act: describe the creation with the unknown type; the assertion below runs it.
            var act = () => new ActionSequence(driver, pointerType);

            // Assert: the unknown type fails at creation, not later as a driver error.
            Assert.ThrowsExactly<ArgumentException>(act);
        }

        [TestMethod(DisplayName = "Verify that GetInputSources returns the pointer and keyboard sources only " +
            "when no scroll was added.")]
        [TestCategory("UnitTest")]
        public void GetInputSourcesWithoutScrollTest()
        {
            // Arrange: a sequence with pointer and keyboard actions only.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);
            sequence.AddPointerDown();
            sequence.AddKeyDown("a");

            // Act: read the sources the request carries.
            var sources = sequence.GetInputSources().ToArray();

            // Assert: the request keeps its original shape, pointer source first and no wheel source.
            Assert.HasCount(2, sources);
            Assert.AreSame(sequence._mouseSource, sources[0]);
            Assert.AreSame(sequence._keySource, sources[1]);
        }

        [TestMethod(DisplayName = "Verify that GetInputSources adds the wheel source after a scroll was added.")]
        [TestCategory("UnitTest")]
        public void GetInputSourcesWithScrollTest()
        {
            // Arrange: a sequence that scrolls once.
            using var driver = new SimulatorDriver(".");
            var sequence = new ActionSequence(driver);
            sequence.AddScroll(deltaX: 0, deltaY: 120);

            // Act: read the sources the request carries.
            var sources = sequence.GetInputSources().ToArray();

            // Assert: the wheel source joins the request as the third source.
            Assert.HasCount(3, sources);
            Assert.AreEqual(InputSourceTypes.Wheel, sources[2].Type);
        }

        [TestMethod(DisplayName = "Verify that a text key added through AddAction is typed into the focused " +
            "element.")]
        #region *** Data Set ***
        [DataRow("ElementInteractions.html")]
        #endregion
        [TestCategory("Integration")]
        public void InvokeKeyActionsTest(string url)
            => Invoke(TestContext, options: new ChromeOptions(), url, (driver) =>
        {
            // Arrange: focus the text input with a click, so the key lands in it.
            var element = driver.FindElement(By.Custom.Id("SendKeys"));
            var sequence = new ActionSequence(driver).AddClick(element);

            // Act: press and release one key through raw keyboard actions, then send the sequence.
            sequence.AddAction(new KeyboardActionModel { Type = KeyboardActionModel.ActionTypes.KeyDown, Value = "a" });
            sequence.AddAction(new KeyboardActionModel { Type = KeyboardActionModel.ActionTypes.KeyUp, Value = "a" });
            sequence.Invoke();

            // Assert: the input received the key.
            Assert.AreEqual("a", element.GetProperty("value"));
        });

        [TestMethod(DisplayName = "Verify that a pointer down followed by a pointer up of the left button " +
            "is a click on the element under the pointer.")]
        #region *** Data Set ***
        [DataRow("ElementInteractions.html")]
        #endregion
        [TestCategory("Integration")]
        public void InvokePointerDownUpLeftButtonTest(string url)
            => Invoke(TestContext, options: new ChromeOptions(), url, (driver) =>
        {
            // Arrange: put the pointer on the button the page reports clicks for.
            var element = driver.FindElement(By.Custom.Id("ClickButton"));
            var sequence = new ActionSequence(driver).AddMoveMouseCursor(element);

            // Act: press and release the left button as two separate actions, then send the sequence.
            sequence.AddPointerDown();
            sequence.AddPointerUp();
            sequence.Invoke();

            // Assert: the page saw a click on the button.
            var actual = driver.FindElement(By.Custom.Id("ClickButtonOutcome")).GetAttribute("value");
            Assert.AreEqual("click on element", actual);
        });

        [TestMethod(DisplayName = "Verify that a pointer down followed by a pointer up of the right button " +
            "opens the context menu of the element under the pointer.")]
        #region *** Data Set ***
        [DataRow("ElementInteractions.html")]
        #endregion
        [TestCategory("Integration")]
        public void InvokePointerDownUpRightButtonTest(string url)
            => Invoke(TestContext, options: new ChromeOptions(), url, (driver) =>
        {
            // Arrange: put the pointer on the button the page reports context clicks for.
            var element = driver.FindElement(By.Custom.Id("ClickButton"));
            var sequence = new ActionSequence(driver).AddMoveMouseCursor(element);

            // Act: press and release the right button as two separate actions, then send the sequence.
            sequence.AddPointerDown(button: MouseActionModel.MouseButtons.Right);
            sequence.AddPointerUp(button: MouseActionModel.MouseButtons.Right);
            sequence.Invoke();

            // Assert: the page saw a context click on the button.
            var actual = driver.FindElement(By.Custom.Id("ClickButtonOutcome")).GetAttribute("value");
            Assert.AreEqual("context on element", actual);
        });

        [TestMethod(DisplayName = "Verify that a scroll with an element origin scrolls that element and not " +
            "the page.")]
        #region *** Data Set ***
        [DataRow("Scrolls.html")]
        #endregion
        [TestCategory("Integration")]
        public void InvokeScrollElementTest(string url)
            => Invoke(TestContext, options: new ChromeOptions(), url, (driver) =>
        {
            // Arrange: the text area has more lines than rows, so it scrolls on its own.
            var element = driver.FindElement(By.Custom.Id("TextAreaEnabled"));

            // Act: turn the wheel over the text area and send the sequence.
            new ActionSequence(driver).AddScroll(element, deltaX: 0, deltaY: 60).Invoke();

            // Assert: the text area moved and the page stayed where it was.
            var elementTop = WaitForScroll(driver, id: "ElementScrollTopOutcome");
            var pageTop = driver.FindElement(By.Custom.Id("PageScrollTopOutcome")).GetAttribute("value");
            Assert.IsGreaterThan(0, elementTop);
            Assert.AreEqual("0", pageTop);
        });

        [TestMethod(DisplayName = "Verify that a scroll from the viewport origin scrolls the page down.")]
        #region *** Data Set ***
        [DataRow("Scrolls.html")]
        #endregion
        [TestCategory("Integration")]
        public void InvokeScrollViewportTest(string url)
            => Invoke(TestContext, options: new ChromeOptions(), url, (driver) =>
        {
            // Act: turn the wheel down from the viewport origin and send the sequence.
            new ActionSequence(driver).AddScroll(deltaX: 0, deltaY: 300).Invoke();

            // Assert: the page reports a vertical scroll position below the top.
            var pageTop = WaitForScroll(driver, id: "PageScrollTopOutcome");
            Assert.IsGreaterThan(0, pageTop);
        });

        [TestMethod(DisplayName = "Verify that a list of mixed actions keeps the scroll members when it is " +
            "serialized for a request.")]
        [TestCategory("UnitTest")]
        public void WheelActionModelSerializationTest()
        {
            // Arrange: a scroll held as its base type, the way an input source holds its actions.
            IList<ActionModelBase> actions = [new WheelActionModel { DeltaX = 30, DeltaY = 120 }];

            // Act: serialize the list with the naming a request uses.
            var json = JsonSerializer.Serialize(actions, JsonOptions);

            // Assert: the scroll members reach the driver; they are lost when the model is not a known derived type.
            Assert.Contains("\"type\":\"scroll\"", json);
            Assert.Contains("\"deltaX\":30", json);
            Assert.Contains("\"deltaY\":120", json);
            Assert.Contains("\"origin\":\"viewport\"", json);
        }

        // Gets the wheel source of the sequence, or fails the test when the sequence does not send one. The
        // wheel source has no public field, so a test reads it from the sources the request carries.
        private static InputSourceModel GetWheelSource(ActionSequence sequence)
        {
            // Read the wheel source from the request sources; Single fails when it is missing.
            return sequence.GetInputSources().Single(i => i.Type == InputSourceTypes.Wheel);
        }

        // Waits until the page reports a scroll position above zero in the given outcome input and returns
        // it. The browser applies a wheel scroll shortly after the driver answers, so the outcome is polled
        // for up to five seconds; a position that stays at zero is returned as zero and fails the assertion.
        private static double WaitForScroll(ChromeDriver driver, string id)
        {
            // Poll the outcome input, because the scroll event reaches the page after the request returns.
            var timeout = DateTime.UtcNow.AddSeconds(5);
            var position = 0d;

            while (position <= 0 && DateTime.UtcNow < timeout)
            {
                var value = driver.FindElement(By.Custom.Id(id)).GetAttribute("value");
                _ = double.TryParse(value, out position);
            }

            return position;
        }

        // Stands in for an action type the sequence does not know. It has no input source, which is what the
        // unsupported-model test needs.
        private sealed class UnsupportedActionModel : ActionModelBase
        {
        }
    }
}
