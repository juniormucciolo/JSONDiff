using System.Net.Http;
using System.Web.Http;
using JsonDiff.Controllers.v1;
using JsonDiff.Models;
using JsonDiff.Repository;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace JsonDiff.Tests.Controllers
{
    [TestFixture]
    public class DiffControllerTest
    {
        private readonly string leftSideJsonEncoded = "eyJpZCI6IjUwIn0=";
        private readonly string rightSideJsonEncoded = "eyJpZCI6IjEwIn0=";
        private readonly string jsonId = "1";
        private readonly string wrongEncodedString = "GwUePB=DhIWwK=123=;'@h76dlZKM";
        private readonly string invalidJsonFormat = "ew0KICAiX2lkIjogIjU5OTRjNTc5ZWIyNWY0NzYyYjNkMzhkNiINCiAgImluZGV4IjogW10sDQp9";

        [Test]
        public async Task Should_Get_Bad_Request_When_Put_Left_Json_Invalid_Format()
        {
            // Arrange
            var controller = DiffController(new Json());
            // Act
            var response = await controller.LeftJson(jsonId, invalidJsonFormat);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Get_Bad_Request_When_Put_Right_Json_Invalid_Format()
        {
            // Arrange
            var controller = DiffController(new Json());
            // Act
            var response = await controller.RightJson(jsonId, invalidJsonFormat);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Put_Left_Json_Encoded_64()
        {
            // Arrange
            var controller = DiffController(new Json());
            // Act
            var response = (OkNegotiatedContentResult<ResponseBase>) await controller.LeftJson(jsonId, leftSideJsonEncoded);
            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<ResponseBase>>(response);
            Assert.AreEqual("Left json stored sucessfully.", response.Content.Message);
            Assert.AreEqual(true, response.Content.Success);
        }

        [Test]
        public async Task Should_Put_Right_Json_Encoded_64()
        {
            // Arrange
            var controller = DiffController(new Json());
            // Act
            var response = (OkNegotiatedContentResult<ResponseBase>) await controller.RightJson(jsonId, rightSideJsonEncoded);
            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<ResponseBase>>(response);
            Assert.AreEqual("Right json stored sucessfully.", response.Content.Message);
            Assert.AreEqual(true, response.Content.Success);
        }

        [Test]
        public async Task Should_Not_Be_Able_To_Put_Right_Uncoded_Json()
        {
            // Arrange
            var controller = DiffController(new Json());
            // Act
            var response = await controller.RightJson(jsonId, wrongEncodedString);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Not_Be_Able_To_Put_Left_Uncoded_Json()
        {
            // Arrange
            var controller = DiffController(new Json());
            // Act
            var response = await controller.LeftJson(jsonId, wrongEncodedString);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Get_Bad_Request_When_Empty_Left_Json()
        {
            // Arrange
            var controller = DiffController(new Json() { Id = 1, Left = null, Right = rightSideJsonEncoded, JsonId = jsonId });
            // Act
            var response = await controller.LeftJson(jsonId, null);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Get_Bad_Request_When_Empty_Right_Json()
        {
            // Arrange
            var controller = DiffController(new Json() { Id = 1, Left = leftSideJsonEncoded, Right = null, JsonId = jsonId });
            // Act
            var response = await controller.RightJson(jsonId, null);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Get_Accepted_Request_When_Both_Side_Are_Stored_Correctly()
        {
            // Arrange
            var controller = DiffController(new Json() { Id = 1, Left = leftSideJsonEncoded, Right = rightSideJsonEncoded, JsonId = jsonId });
            // Act
            var response = await controller.Diff(jsonId);
            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<JsonResult>>(response);
        }

        [Test]
        public async Task Should_Get_Accepted_Request_When_Json_Side_Are_Differently()
        {
            // Arrange
            var controller = DiffController(new Json() { Id = 1, Left = leftSideJsonEncoded, Right = rightSideJsonEncoded, JsonId = jsonId });
            // Act
            var response = await controller.Diff(jsonId);
            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<JsonResult>>(response);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public async Task Should_Get_Bad_Request_When_Id_Is_Invalid(string id)
        {
            // Arrange
            var controller = DiffController(new Json());
            // Act
            var response = (BadRequestErrorMessageResult) await controller.Diff(id);
            // Assert
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(response);
            Assert.AreEqual("Id should not be empty or null.", response.Message);
        }

        protected DiffController DiffController(Json json)
        {
            Mock<IRepository> mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.GetById(jsonId)).Returns(json);
            mockRepository.Setup(x => x.AddOrUpdate(It.IsAny<Json>()));
            mockRepository.Setup(x => x.SaveAsync());
            DiffController controller = new DiffController(mockRepository.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            return controller;
        }
    }
}
