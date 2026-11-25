using G4.WebDriver.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text.Json;

namespace G4.WebDriver.Tests
{
    [TestClass]
    [TestCategory("MousePositionInputModel")]
    [TestCategory("UnitTest")]
    public class MousePositionInputModelTests
    {
        // Serializes the model the way a request does (camel case names), so the test reads the members the driver
        // receives.
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [TestMethod(DisplayName = "Verify that an offset the caller does not set is zero for MiddleCenter and one " +
            "for an edge or corner alignment.")]
        #region *** Data Set ***
        [DataRow("MiddleCenter", 0)]
        [DataRow("middlecenter", 0)]
        [DataRow("TopLeft", 1)]
        [DataRow("BottomRight", 1)]
        [DataRow("MiddleLeft", 1)]
        #endregion
        public void DefaultOffsetTest(string alignment, int expected)
        {
            // Arrange: a position that names only its alignment.
            var position = new MousePositionInputModel { Alignment = alignment };

            // Act: read both offsets, which resolve the alignment default because neither was set.
            var offsetX = position.OffsetX;
            var offsetY = position.OffsetY;

            // Assert: the default follows the alignment, so the center lands exactly on the center.
            Assert.AreEqual(expected, offsetX);
            Assert.AreEqual(expected, offsetY);
        }

        [TestMethod(DisplayName = "Verify that the default position is the top-left corner nudged one pixel inside.")]
        public void DefaultPositionTest()
        {
            // Arrange and Act: a position with no member set.
            var position = new MousePositionInputModel();

            // Assert: the defaults are unchanged for callers that set nothing.
            Assert.AreEqual("TopLeft", position.Alignment);
            Assert.AreEqual(1, position.OffsetX);
            Assert.AreEqual(1, position.OffsetY);
        }

        [TestMethod(DisplayName = "Verify that an offset the caller sets is kept for every alignment, zero included.")]
        #region *** Data Set ***
        [DataRow("MiddleCenter", 12, -3)]
        [DataRow("TopLeft", 0, 0)]
        [DataRow("BottomCenter", -5, 8)]
        #endregion
        public void ExplicitOffsetTest(string alignment, int offsetX, int offsetY)
        {
            // Arrange: a position with both offsets set by the caller.
            var position = new MousePositionInputModel
            {
                Alignment = alignment,
                OffsetX = offsetX,
                OffsetY = offsetY
            };

            // Act: serialize the position the way a request does.
            var json = JsonSerializer.Serialize(position, JsonOptions);
            using var document = JsonDocument.Parse(json);

            // Assert: the driver receives exactly the offsets the caller set.
            Assert.AreEqual(offsetX, document.RootElement.GetProperty("offsetX").GetInt32());
            Assert.AreEqual(offsetY, document.RootElement.GetProperty("offsetY").GetInt32());
        }

        [TestMethod(DisplayName = "Verify that a MiddleCenter request without offsets sends zero offsets.")]
        public void MiddleCenterRequestTest()
        {
            // Arrange: a center position that leaves the offsets to the default.
            var position = new MousePositionInputModel { Alignment = "MiddleCenter" };

            // Act: serialize the position the way a request does.
            var json = JsonSerializer.Serialize(position, JsonOptions);
            using var document = JsonDocument.Parse(json);

            // Assert: the request carries zero offsets, so the driver lands on the center itself.
            Assert.AreEqual(0, document.RootElement.GetProperty("offsetX").GetInt32());
            Assert.AreEqual(0, document.RootElement.GetProperty("offsetY").GetInt32());
        }
    }
}
