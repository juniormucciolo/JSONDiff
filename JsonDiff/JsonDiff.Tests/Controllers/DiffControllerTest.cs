using JsonDiff.Models;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using System.Threading.Tasks;
using System.Web.Http.Results;
using JsonDiff.Tests.Utils;

namespace JsonDiff.Tests.Controllers
{
    [TestFixture]
    public class DiffControllerTest
    {
        private readonly string wrongEncodedString = "GwUePB=DhIWwK=123=;'@h76dlZKM";
        private readonly string invalidJsonFormat = "ew0KICAiX2lkIjogIjU5OTRjNTc5ZWIyNWY0NzYyYjNkMzhkNiINCiAgImluZGV4IjogW10sDQp9";
        private static readonly MockHelper Mock = new MockHelper();
        private readonly Json _modelHelper = Mock.GetModelHelper();

        [Test]
        public async Task Should_Get_Bad_Request_When_Put_Left_Json_Invalid_Format()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json());
            // Act
            var response = await controller.LeftJson(_modelHelper.JsonId, invalidJsonFormat);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Get_Bad_Request_When_Put_Right_Json_Invalid_Format()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json());
            // Act
            var response = await controller.RightJson(_modelHelper.JsonId, invalidJsonFormat);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Put_Left_Json_Encoded_64()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json());
            // Act
            var response = (OkNegotiatedContentResult<ResponseBase>) await controller.LeftJson(_modelHelper.JsonId, _modelHelper.Left);
            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<ResponseBase>>(response);
            Assert.AreEqual("Left json stored sucessfully.", response.Content.Message);
            Assert.AreEqual(true, response.Content.Success);
        }

        [Test]
        public async Task Should_Put_Right_Json_Encoded_64()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json());
            // Act
            var response = (OkNegotiatedContentResult<ResponseBase>) await controller.RightJson(_modelHelper.JsonId, _modelHelper.Right);
            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<ResponseBase>>(response);
            Assert.AreEqual("Right json stored sucessfully.", response.Content.Message);
            Assert.AreEqual(true, response.Content.Success);
        }

        [Test]
        public async Task Should_Not_Be_Able_To_Put_Right_Uncoded_Json()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json());
            // Act
            var response = await controller.RightJson(_modelHelper.JsonId, wrongEncodedString);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Not_Be_Able_To_Put_Left_Uncoded_Json()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json());
            // Act
            var response = await controller.LeftJson(_modelHelper.JsonId, wrongEncodedString);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Get_Bad_Request_When_Empty_Left_Json()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json() { Id = 1, Left = null, Right = _modelHelper.Right, JsonId = _modelHelper.JsonId });
            // Act
            var response = await controller.LeftJson(_modelHelper.JsonId, null);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Get_Bad_Request_When_Empty_Right_Json()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json() { Id = 1, Left = _modelHelper.Left, Right = null, JsonId = _modelHelper.JsonId });
            // Act
            var response = await controller.RightJson(_modelHelper.JsonId, null);
            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task Should_Get_Accepted_Request_When_Both_Side_Are_Stored_Correctly()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json() { Id = 1, Left = _modelHelper.Left, Right = _modelHelper.Right, JsonId = _modelHelper.JsonId });
            // Act
            var response = await controller.Diff(_modelHelper.JsonId);
            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<JsonResult>>(response);
        }

        [Test]
        public async Task Should_Get_Accepted_Request_When_Json_Side_Are_Differently()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json() { Id = 1, Left = _modelHelper.Left, Right = _modelHelper.Right, JsonId = _modelHelper.JsonId });
            // Act
            var response = await controller.Diff(_modelHelper.JsonId);
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
            var controller = Mock.GetMockedDiffController(new Json());
            // Act
            var response = (BadRequestErrorMessageResult) await controller.Diff(id);
            // Assert
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(response);
            Assert.AreEqual("Id should not be empty or null.", response.Message);
        }
    }
}
