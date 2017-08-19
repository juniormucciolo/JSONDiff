using JsonDiff.Models;
using JsonDiff.Repository;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using Moq;

namespace JsonDiff.Tests.Repository
{
    [TestFixture]
    public class RepositoryTest
    {
        readonly Mock<IRepository> mockRepository = new Mock<IRepository>();
        private readonly string leftSideJsonEncoded = "eyJpZCI6IjUwIn0=";
        private readonly string rightSideJsonEncoded = "eyJpZCI6IjEwIn0=";
        private readonly string jsonId = "1";

        [Test]
        public void Should_Read_Left_Side_Json()
        {
            // Arrange
            mockRepository.Setup(x => x.GetById(jsonId)).Returns(new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = leftSideJsonEncoded,
                Right = rightSideJsonEncoded
            });

            // Act
            var result = mockRepository.Object.GetById(jsonId);

            // Assert
            Assert.AreEqual(leftSideJsonEncoded, result.Left);
        }

        [Test]
        public void Should_Read_Right_Side_Json()
        {
            // Arrange
            mockRepository.Setup(x => x.GetById(jsonId)).Returns(new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = leftSideJsonEncoded,
                Right = rightSideJsonEncoded
            });

            // Act
            var result = mockRepository.Object.GetById(jsonId);

            // Assert
            Assert.AreEqual(rightSideJsonEncoded, result.Right);
        }

        [Test]
        public void Should_Add_Or_Update_Right_Side_Json()
        {
            // Arrange
            mockRepository.Setup(x => x.AddOrUpdate(It.IsAny<Json>())).Returns(true);

            var json = new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = null,
                Right = leftSideJsonEncoded
            };

            // Act
            var response = mockRepository.Object.AddOrUpdate(json);

            // Assert
            Assert.IsTrue(response);
        }

        [Test]
        public void Should_Add_Or_Update_Left_Side_Json()
        {
            // Arrange
            mockRepository.Setup(x => x.AddOrUpdate(It.IsAny<Json>())).Returns(true);

            var json = new Json()
            {
                Id = 1,
                JsonId = jsonId,
                Left = leftSideJsonEncoded,
                Right = null
            };

            // Act
            var response = mockRepository.Object.AddOrUpdate(json);

            // Assert
            Assert.IsTrue(response);
        }
    }
}
