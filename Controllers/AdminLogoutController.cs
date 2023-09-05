using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;

namespace Use_Wheels.Controllers
{
    [Route("/admin")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminLogoutController : ControllerBase
	{
        protected APIResponse _response;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminLogoutController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _response = new();
        }

        /// <summary>
        /// Method to logout an admin role
        /// </summary>
        /// <returns>APIResponse object with success message as result</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault().Split(" ")[1];
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.StringSetAndGetAsync(jwtToken, new RedisValue("1"));

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = "Logout successful";
            return Ok(_response);
        }
    }
}

