using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.WebPages;
using JsonDiff.Models;
using JsonDiff.Repository;
using JsonDiff.Service;

namespace JsonDiff.Controllers.v1
{
    /// <summary>
    /// Diff Controler.
    /// </summary>
    [RoutePrefix("v1")]
    public class DiffController : ApiController
    {
        private readonly IRepository _repository;
        private readonly EncodeHandler _encoder;
        private readonly DiffHandler _diff;

        /// <summary>
        /// Unit constructor.
        /// </summary>
        /// <param name="repository"></param>
        public DiffController(IRepository repository)
        {
            this._repository = repository;
            this._encoder = new EncodeHandler();
            this._diff = new DiffHandler();
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DiffController()
        {
            this._repository = new Repository.Repository();
            this._encoder = new EncodeHandler();
            this._diff = new DiffHandler();
        }

        /// <summary>
        /// The property {id} is required and it will be the identifier to track the JSON. The JSON must be encoded in base64 binary.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="json"></param>
        /// <returns>HttpResponseMessage</returns>
        [Route("{id}/right")]
        [HttpPut]
        public async Task<IHttpActionResult> RightJson(string id, [FromBody]string json)
        {
            try
            {
                _encoder.DeserializeJson(json);
                await _repository.SaveJsonAsync(id, json, Side.Right);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok( new ResponseBase() { Message = $"{Side.Right} json stored sucessfully.", Success = true });
        }

        /// <summary>
        /// The property {id} is required and it will be the identifier to track the JSON. The JSON must be encoded in base64 binary.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="json"></param>
        /// <returns>HttpResponseMessage</returns>
        [Route("{id}/left")]
        [HttpPut]
        public async Task<IHttpActionResult> LeftJson(string id, [FromBody]string json)
        {
            try
            {
                _encoder.DeserializeJson(json);
                await _repository.SaveJsonAsync(id, json, Side.Left);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(new ResponseBase() { Message = $"{Side.Left} json stored sucessfully.", Success = true });
        }

        /// <summary>
        /// The property {id} is required and it will be the identifier to track the JSON.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JSON Response with differences found in two JSON previosly sent as right and left side.</returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Diff(string id)
        {
            if (id == null || id.Trim().IsEmpty())
            {
                return BadRequest("Id should not be empty or null.");
            }

            var jsonById = _repository.GetById(id);

            if (jsonById.Left == null || jsonById.Right == null)
            {
                return BadRequest("Left and Right side are required to peform diff.");
            }

            try
            {
                var response = _diff.ProcessDiff(jsonById);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
