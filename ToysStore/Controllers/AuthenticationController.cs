using ToysStore.DataContracts;
using ToysStore.Managers;
using Serilog;
using Swashbuckle.Swagger.Annotations;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace ToysStore.Controllers
{
    public class AuthenticationController : ApiController
    {
        private IAuthenticationManager _authenticationManager;
        private ILogger _logger;

        /// <summary>
        /// DependencyInjection for Authentication Controller
        /// </summary>
        /// <param name="authenticationManager"></param>
        /// <param name="logger"></param>
        public AuthenticationController(IAuthenticationManager authenticationManager, ILogger logger)
        {
            _authenticationManager = authenticationManager;
            _logger = logger;
        }

        /// <summary>
        /// Adding new users
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, "User added", typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Exception with adding new user", typeof(ErrorResponse))]
        [HttpPut]
        [Route("api/register-user")]
        public IHttpActionResult AddUser([FromBody] RegisterUserRequest registerRequest)
        {
            if (registerRequest.Login == null ||
                registerRequest.Password == null ||
                registerRequest.Email == null)
            {
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(Errors.EMPTY_ONE_OR_MORE_FIELD));
            }

            if (registerRequest.Login.Count() <= 6)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(Errors.INCORRECT_LOGIN_LENGHT));
                        
            if (registerRequest.Password.Count() < 6 || registerRequest.Password.Count() > 20)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(Errors.INCORRECT_PASSWORD_LENGHT));

            int indexOfDog = registerRequest.Email.LastIndexOf('@');

            if (indexOfDog == -1 || registerRequest.Email.Count() < 5 || indexOfDog >= registerRequest.Email.LastIndexOf('.'))
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(Errors.INCORRECT_EMAIL));

            var result = _authenticationManager.AddUser(registerRequest);

            if (result == Errors.OK)
                return Content(HttpStatusCode.OK, "User has been added");
            return Content(HttpStatusCode.BadRequest, new ErrorResponse(result));
            
        }

        /// <summary>
        /// login api validate user login and password
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, "validation passed", typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "validation not passed", typeof(ErrorResponse))]
        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login([FromBody] LoginRequest login)
        {
            var response = _authenticationManager.Login(login);
            if (response != Errors.OK)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(response));
            return Content(HttpStatusCode.OK, "Success");
        }

        /// <summary>
        /// log out user
        /// </summary>
        /// <returns>
        /// </returns>
        [SwaggerResponse(HttpStatusCode.OK, "Loged out", typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "System error", typeof(ErrorResponse))]
        [HttpDelete]
        [Route("api/v1/logout")]
        public IHttpActionResult LogOut()
        {
            var response = _authenticationManager.LogOut();
            if (response != Errors.OK)
                return Content(HttpStatusCode.BadRequest, new ErrorResponse(response));
            return Content(HttpStatusCode.OK, "Success");
        }
    }
}

        