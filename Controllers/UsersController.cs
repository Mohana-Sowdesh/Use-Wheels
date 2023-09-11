using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Use_Wheels.Controllers
{
    [ApiController]
    [Route("/")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        protected APIResponseDTO _response;
        private readonly ILogger<UsersController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(IUserRepository userRepo, ILogger<UsersController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepo = userRepo;
            _response = new();
            _logger = logger;
        }

        /// <summary>
        /// Method to log a user in
        /// </summary>
        /// <param name="LoginRequestDTO"></param>
        /// <returns>APIResponse object with success code on success or error messages on error</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepo.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _logger.LogError(Constants.LoginConstants.INVALID_CREDENTIALS);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(Constants.LoginConstants.INVALID_CREDENTIALS);
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        /// <summary>
        /// Method to register a customer
        /// </summary>
        /// <param name="RegisterationRequestDTO"></param>
        /// <returns>APIResponse object with success code on success or error messages on error</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        { 
            var user = await _userRepo.Register(model);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(Constants.LoginConstants.INVALID_AGE);
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        /// <summary>
        /// Method to log a user out
        /// </summary>
        /// <returns>APIResponse object with success code on success or error message(s) on error</returns>
        [HttpPost("user/logout")]
        [Authorize(Roles = Constants.Roles.CUSTOMER)] // Ensures only authenticated users can log out
        public async Task<ActionResult<APIResponseDTO>> Logout()
        {
            string username = HttpContext.User.Identity.Name;
            LogoutUtility logoutUtility = new LogoutUtility();
            logoutUtility.DeleteUserFromWishlist(username);

            try
            {
                var jwtToken = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault().Split(" ")[1];
                await logoutUtility.InvalidateToken(jwtToken);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = Constants.LogoutConstants.LOGOUT_SUCCESSFUL;
                return Ok(_response);
            }
            catch(Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;
        }
    }
}

