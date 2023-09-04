using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public AdminLogoutController()
        {
            _response = new();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = "Logout successful";
            return Ok(_response);
        }
    }
}

