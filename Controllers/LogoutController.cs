using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;

namespace Use_Wheels.Controllers
{
    [ApiController]
    [Route("/admin")]
    public class LogoutController : ControllerBase
	{
        protected APIResponse _response;
        public LogoutController()
        {
            _response = new();
        }

        [HttpPost("logout")]
        [Authorize] 
        public IActionResult Logout()
        {
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = "Logout successful";
            return Ok(_response);
        }
    }
}

