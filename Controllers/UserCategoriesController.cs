using System;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Data;
using Use_Wheels.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Use_Wheels.Controllers
{
    [Route("user/categories")]
    [ApiController]
    public class UserCategoriesController : ControllerBase
	{
        protected APIResponse _response;
        private ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UserCategoriesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [Authorize(Roles = "customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            IEnumerable<Category> categoryList = await _db.Categories.ToListAsync();
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = categoryList;
            return Ok(_response);
        }
    }
}

