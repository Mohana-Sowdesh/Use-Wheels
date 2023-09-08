using System;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Use_Wheels.Repository;
using StackExchange.Redis;

namespace Use_Wheels.Controllers
{
    [ApiController]
    [Route("/")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        protected APIResponse _response;
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
                _logger.LogError("Invalid credentials entered");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
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
            int ifUserNameUnique = _userRepo.IsUniqueUser(model.Username, model.Email);
            if (ifUserNameUnique == -1)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }
            else if (ifUserNameUnique == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email already exists");
                return BadRequest(_response);
            }

            bool isAbove18 = _userRepo.IsAbove18(model.Dob);
            if(!isAbove18)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Age must be above 18");
                return BadRequest(_response);
            }

            var user = await _userRepo.Register(model);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering - User must above or equal to 18 years of age");
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
        [Authorize(Roles = "customer")] // Ensures only authenticated users can log out
        public async Task<ActionResult<APIResponse>> Logout()
        {
            string username = HttpContext.User.Identity.Name;
            bool isUserExists =  WishListRepository.IsUserExists(username);

            if (isUserExists)
            {
                WishListRepository.DeleteUser(username);
            }

            try
            {
                var jwtToken = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault().Split(" ")[1];
                var redis = ConnectionMultiplexer.Connect("localhost:6379");
                IDatabase db = redis.GetDatabase();
                await db.StringSetAndGetAsync(jwtToken, new RedisValue("1"), new TimeSpan(24, 0, 0));

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "Logout successful";
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

