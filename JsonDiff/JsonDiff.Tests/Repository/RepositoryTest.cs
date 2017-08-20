using System.Threading.Tasks;
using JsonDiff.Models;
using JsonDiff.Repository;
using JsonDiff.Tests.Utils;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using Moq;

namespace JsonDiff.Tests.Repository
{
    [TestFixture]
    public class RepositoryTest
    {
        private static readonly MockHelper Mock = new MockHelper();
        private readonly Json _modelHelper = Mock.GetModelHelper();
        readonly Mock<IRepository> _mockRepository = new Mock<IRepository>();

        [Test]
        public void Should_Read_Left_Side_Json()
        {
            // Arrange

            _mockRepository.Setup(x => x.GetByIdAsync(_modelHelper.JsonId)).Returns(Task.FromResult(new Json()
            {
                Id = 1,
                JsonId = _modelHelper.JsonId,
                Left = _modelHelper.Left,
                Right = _modelHelper.Right
            }));

            // Act
            var result = _mockRepository.Object.GetByIdAsync(_modelHelper.JsonId).Result;

            // Assert
            Assert.AreEqual(_modelHelper.Left, result.Left);
        }

        [Test]
        public void Should_Read_Right_Side_Json()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetByIdAsync(_modelHelper.JsonId)).Returns(Task.FromResult(new Json()
            {
                Id = 1,
                JsonId = _modelHelper.JsonId,
                Left = _modelHelper.Left,
                Right = _modelHelper.Right
            }));

            // Act
            var result = _mockRepository.Object.GetByIdAsync(_modelHelper.JsonId).Result;

            // Assert
            Assert.AreEqual(_modelHelper.Right, result.Right);
        }

        [Test]
        public void Should_Add_Or_Update_Right_Side_Json()
        {
            // Arrange
            _mockRepository.Setup(x => x.AddOrUpdate(It.IsAny<Json>())).Returns(true);

            var json = new Json()
            {
                Id = 1,
                JsonId = _modelHelper.JsonId,
                Left = null,
                Right = _modelHelper.Left
            };

            // Act
            var response = _mockRepository.Object.AddOrUpdate(json);

            // Assert
            Assert.IsTrue(response);
        }

        [Test]
        public void Should_Add_Or_Update_Left_Side_Json()
        {
            // Arrange
            _mockRepository.Setup(x => x.AddOrUpdate(It.IsAny<Json>())).Returns(true);

            var json = new Json()
            {
                Id = 1,
                JsonId = _modelHelper.JsonId,
                Left = _modelHelper.Left,
                Right = null
            };

            // Act
            var response = _mockRepository.Object.AddOrUpdate(json);

            // Assert
            Assert.IsTrue(response);
        }
    }
}
