using ToysStore.Managers;
using Serilog;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;

namespace ToysStore.Controllers
{
    public class ToysController : ApiController
    {

        private IToysManager _toysManager;
        private ILogger _logger;

        /// <summary>
        /// DependencyInjection for Toys Controller
        /// </summary>
        /// <param name="toysManager"></param>
        /// <param name="logger"></param>
        public ToysController(IToysManager toysManager, ILogger logger)
        {
            _toysManager = toysManager;
            _logger = logger;
        }

        /// <summary>
        /// returns all toys and count of them
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, "You can see all toys", typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, null, typeof(ErrorResponse))]
        [Route("api/get-toys")]
        [HttpGet]
        public IHttpActionResult GetAllToys()
        {
            var result = _toysManager.GetAllToys();
            if (result == null)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(Errors.DATA_BASE_ERROR));
            else
                return Content(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Returns toy with given ID
        /// </summary>
        /// <param name="toyId"></param>
        /// <returns></returns>
        [Route("api/toy-{toyId}")]
        [HttpGet]
        public IHttpActionResult GetToyById([FromUri] int toyId)
        {
            if (toyId <= 0)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(Errors.INVALID_TOY_ID));

            var result = _toysManager.GetToyById(toyId, out Errors errorCode);
            if (result == null)
                return Content(HttpStatusCode.BadRequest, errorCode);
            else
                return Content(HttpStatusCode.OK, result);
        }

    }
}