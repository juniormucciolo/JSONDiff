using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;
using JsonDiff.Models;
using JsonDiff.Repository;
using JsonDiff.Service;

namespace JsonDiff.Controllers
{
    [RoutePrefix("v1")]
    public class DiffController : ApiController
    {
        private readonly IRepository repository;
        private readonly EncodeHandler encoder;
        private readonly DiffHandler diff;

        /// <summary>
        /// Unit constructor.
        /// </summary>
        /// <param name="repository"></param>
        public DiffController(IRepository repository)
        {
            this.repository = repository;
            this.encoder = new EncodeHandler();
            this.diff = new DiffHandler();
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DiffController()
        {
            this.repository = new Repository.Repository();
            this.encoder = new EncodeHandler();
            this.diff = new DiffHandler();
        }

        /// <summary>
        /// The property {id} is required and it will be the identifier to track the JSON. The JSON must be encoded in base64 binary.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="json"></param>
        /// <returns>HttpResponseMessage</returns>
        [Route("{id}/right")]
        [HttpPut]
        public HttpResponseMessage RightJson(string id, [FromBody]string json)
        {
            try
            {
                encoder.DeserializeJson(json);
                encoder.Decode(json);
                repository.SaveJson(id, json, Side.Right);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Error message: {e.Message}");
            }

            return Request.CreateResponse(HttpStatusCode.Accepted, $"{Side.Right} json stored sucessfully.");
        }

        /// <summary>
        /// The property {id} is required and it will be the identifier to track the JSON. The JSON must be encoded in base64 binary.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="json"></param>
        /// <returns>HttpResponseMessage</returns>
        [Route("{id}/left")]
        [HttpPut]
        public HttpResponseMessage LeftJson(string id, [FromBody]string json)
        {
            try
            {
                encoder.DeserializeJson(json);
                encoder.Decode(json);
                repository.SaveJson(id, json, Side.Left);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Error message: {e.Message}");
            }

            return Request.CreateResponse(HttpStatusCode.Accepted, $"{Side.Left} json stored sucessfully.");
        }

        /// <summary>
        /// The property {id} is required and it will be the identifier to track the JSON.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JSON Response with differences found in two JSON previosly sent as right and left side.</returns>
        [Route("{id}")]
        [HttpGet]
        public HttpResponseMessage Diff(string id)
        {
            if (id == null || id.Trim().IsEmpty())
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error message: Id should not be empty or null");
            }

            var jsonById = repository.GetById(id);

            if (jsonById.Left == null || jsonById.Right == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error message: Left and Right side are required to peform diff");
            }

            try
            {
                var response = diff.ProcessDiff(jsonById);
                return Request.CreateResponse(HttpStatusCode.Accepted, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Error message: {e.Message}");
            }
        }
    }
}
