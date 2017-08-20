using System.Linq;
using JsonDiff.Models;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using System.Threading.Tasks;
using System.Web.Http.Results;
using JsonDiff.Tests.Utils;

namespace JsonDiff.Tests.Integrations
{
    [TestFixture]
    public class EndToEndTest
    {
        private static readonly MockHelper Mock = new MockHelper();
        private readonly Json _modelHelper = Mock.GetModelHelper();

        [Test]
        public async Task Should_Get_Differecens_When_Calling_Diff_Request_After_Both_Sides_Are_Stored()
        {
            // Arrange
            var controller = Mock.GetMockedDiffController(new Json() { Id = 1, Left = _modelHelper.Left, Right = _modelHelper.Right, JsonId = _modelHelper.JsonId });
            
            // Act
            await controller.LeftJson(_modelHelper.JsonId, _modelHelper.Left);
            await controller.RightJson(_modelHelper.JsonId, _modelHelper.Right);
            var response = (OkNegotiatedContentResult<JsonResult>) controller.Diff(_modelHelper.JsonId).Result;

            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<JsonResult>>(response);
            Assert.AreEqual("Found 1 differences between jsons", response.Content.message);
            Assert.AreEqual(1, response.Content.differences.Count);
            Assert.AreEqual("value from id property changed from 50 to 10", response.Content.differences.First());
        }
    }
}
