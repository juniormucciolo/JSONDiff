using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using JsonDiff.Controllers.v1;
using JsonDiff.Models;
using JsonDiff.Repository;
using Moq;

namespace JsonDiff.Tests.Utils
{
    public class MockHelper
    {
        /// <summary>
        /// Get an instance of DiffController and mock their dependencies using MOQ.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public DiffController GetMockedDiffController(Json json)
        {
            string jsonId = "1";

            Mock<IRepository> mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.GetByIdAsync(jsonId)).Returns(Task.FromResult(json));
            mockRepository.Setup(x => x.AddOrUpdate(It.IsAny<Json>()));
            mockRepository.Setup(x => x.SaveAsync());
            DiffController controller = new DiffController(mockRepository.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            return controller;
        }

        /// <summary>
        /// Get a model with base64 encoded string on JSON sides.
        /// </summary>
        /// <returns></returns>
        public Json GetModelHelper()
        {
            return new Json
            {
                Id = 1,
                JsonId = "1",
                Left = "eyJpZCI6IjUwIn0=",
                Right = "eyJpZCI6IjEwIn0="
            };
        }
    }
}
