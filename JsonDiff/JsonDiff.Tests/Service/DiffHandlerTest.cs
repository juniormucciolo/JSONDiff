using System;
using System.Linq;
using JsonDiff.Models;
using JsonDiff.Service;
using JsonDiff.Tests.Utils;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace JsonDiff.Tests.Service
{
    [TestFixture]
    public class DiffHandlerTest
    {
        private readonly string jsonId = "1";
        private readonly DiffHandler _diffHandler = new DiffHandler();
        private static readonly MockHelper Mock = new MockHelper();
        private readonly Json _modelHelper = Mock.GetModelHelper();

        [Test]
        public void Should_Be_The_Same_When_Left_And_Right_Side_Are_The_Same()
        {
            // Arrange
            Json jsonSides = new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = _modelHelper.Left,
                Right = _modelHelper.Left
            };

            // Act
            var result = _diffHandler.ProcessDiff(jsonSides);

            // Assert
            Assert.AreEqual(jsonId, result.id);
            Assert.AreEqual("Objects are same", result.message);
            Assert.AreEqual(0, result.differences.Count);
        }

        [Test]
        public void Should_Show_Difference_Between_Left_And_Right_Side()
        {
            // Arrange
            Json jsonSides = new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = _modelHelper.Left,
                Right = _modelHelper.Right
            };

            // Act
            var result = _diffHandler.ProcessDiff(jsonSides);

            // Assert
            Assert.AreEqual(jsonId, result.id);
            Assert.AreEqual("Found 1 differences between jsons", result.message);
            Assert.AreEqual(1, result.differences.Count);
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
                Right = _modelHelper.Right
            };

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => _diffHandler.ProcessDiff(jsonSides));
        }

        [Test]
        public void Should_Get_Exception_When_Right_Side_Is_Null()
        {
            // Arrange
            Json jsonSides = new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = _modelHelper.Left,
                Right = null
            };

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => _diffHandler.ProcessDiff(jsonSides));
        }
    }
}
