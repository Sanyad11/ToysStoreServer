using ToysStore.Entities;
using ToysStore.Managers;
using Serilog;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;

namespace ToysStore.Controllers
{
    public class BacketController : ApiController
    {

        private IBacketManager _backetManager;
        private ILogger _logger;

        /// <summary>
        /// Dependency Injections for Backet Controller
        /// </summary>
        /// <param name="backetManager"></param>
        /// <param name="logger"></param>
        public BacketController(IBacketManager backetManager, ILogger logger)
        {
            _backetManager = backetManager;
            _logger = logger;
        }

        /// <summary>
        /// Validated datas and added new record to data base or updated existing
        /// </summary>
        /// <param name="backetData"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, "Toy successfully added to backet")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Operation failed", typeof(ErrorResponse))]
        [Route("api/backet-add")]
        [HttpPost]
        public IHttpActionResult UpdateBacket([FromBody] BacketData backetData)
        {
            var validating = this.ValidatingBacketData(backetData.UserId, backetData.ToyId);
            if (validating != Errors.OK)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(validating));

            var result = _backetManager.UpdateBacket(backetData);

            if (result == Errors.OK)
                return Content(HttpStatusCode.OK, "Success");

            return Content(HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Validated datas and removed existing record from data base
        /// </summary>
        /// <param name="backetData"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, "Toy successfully removed from backet")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Operation failed", typeof(ErrorResponse))]
        [Route("api/backet-remove-{userId}-{toyId}")]
        [HttpDelete]
        public IHttpActionResult RemoveFromBacket([FromUri] int userId, [FromUri] int toyId)
        {
            var validating = this.ValidatingBacketData(userId, toyId);
            if (validating != Errors.OK)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(validating));

            var result = _backetManager.RemoveFromBacket(userId, toyId);

            if (result == Errors.OK)
                return Content(HttpStatusCode.OK, "Success");

            return Content(HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Returns all toys in backet by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, "You can see all toys in your backet")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Operation failed", typeof(ErrorResponse))]
        [Route("api/backet-get-{userId}")]
        [HttpGet]
        public IHttpActionResult GetBucketList([FromUri] int userId)
        {
            var validating = this.ValidatingBacketData(userId);
            if (validating != Errors.OK)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(validating));

            var result = _backetManager.GetBucketList(userId);
            if (result == null)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(Errors.DATA_BASE_ERROR));
            else
                return Content(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Remove all rows with current userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, "You can see all toys in your backet")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Operation failed", typeof(ErrorResponse))]
        [Route("api/backet-buy-{userId}")]
        [HttpDelete]
        public IHttpActionResult BuyAllToysFromBacket([FromUri] int userId)
        {
            var validating = this.ValidatingBacketData(userId);
            if (validating != Errors.OK)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(validating));

            var result = _backetManager.BuyAllToysFromBacket(userId);
            if (result != Errors.OK)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(Errors.DATA_BASE_ERROR));
            else
                return Content(HttpStatusCode.OK, "Success");
        }

        private Errors ValidatingBacketData(int userId)
        {
            if (userId <= 0)
                return Errors.INVALID_USER_ID;           

            return Errors.OK;
        }

        private Errors ValidatingBacketData(int userId, int toyId)
        {
            if (toyId <= 0)
                return Errors.INVALID_TOY_ID;
            
            return ValidatingBacketData(userId);
        }

    }
}