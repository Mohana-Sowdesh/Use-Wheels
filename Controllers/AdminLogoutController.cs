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
    [Authorize(Roles = Constants.Roles.ADMIN)]
    public class AdminLogoutController : ControllerBase
	{
        protected APIResponseDTO _response;
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
        public async Task<ActionResult<APIResponseDTO>> Logout()
        {
            try
            {
                string jwtToken = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault().Split(" ")[1];

                LogoutUtility logoutUtility = new LogoutUtility();
                await logoutUtility.InvalidateToken(jwtToken);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = Constants.LogoutConstants.LOGOUT_SUCCESSFUL;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;
        }
    }
}

