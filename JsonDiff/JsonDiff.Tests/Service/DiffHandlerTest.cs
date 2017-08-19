using System;
using System.Linq;
using JsonDiff.Models;
using JsonDiff.Service;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace JsonDiff.Tests.Service
{
    [TestFixture]
    public class DiffHandlerTest
    {
        private readonly string leftSideJsonEncoded = "eyJpZCI6IjUwIn0=";
        private readonly string rightSideJsonEncoded = "eyJpZCI6IjEwIn0=";
        private readonly string jsonId = "1";
        private readonly DiffHandler diffHandler = new DiffHandler();

        [Test]
        public void Should_Be_The_Same_When_Left_And_Right_Side_Are_The_Same()
        {
            // Arrange
            Json jsonSides = new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = leftSideJsonEncoded,
                Right = leftSideJsonEncoded
            };

            // Act
            var result = diffHandler.ProcessDiff(jsonSides);

            // Assert
            Assert.AreEqual(jsonId, result.id);
            Assert.AreEqual("Found 0 differences between jsons", result.message);
            Assert.AreEqual("Objects are same", result.differences.First());
        }

        [Test]
        public void Should_Show_Difference_Between_Left_And_Right_Side()
        {
            // Arrange
            Json jsonSides = new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = leftSideJsonEncoded,
                Right = rightSideJsonEncoded
            };

            // Act
            var result = diffHandler.ProcessDiff(jsonSides);

            // Assert
            Assert.AreEqual(jsonId, result.id);
            Assert.AreEqual("Found 1 differences between jsons", result.message);
            Assert.AreEqual("value from id property changed from 50 to 10", result.differences.First());
        }

        [Test]
        public void Should_Get_Exception_When_Left_Side_Is_Null()
        {
            // Arrange
            Json jsonSides = new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = null,
                Right = rightSideJsonEncoded
            };

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => diffHandler.ProcessDiff(jsonSides));
        }

        [Test]
        public void Should_Get_Exception_When_Right_Side_Is_Null()
        {
            // Arrange
            Json jsonSides = new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = leftSideJsonEncoded,
                Right = null
            };

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => diffHandler.ProcessDiff(jsonSides));
        }
    }
}
