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
    [Authorize(Roles = "customer")]
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

        /// <summary>
        /// Method to get categories for customer role
        /// </summary>
        /// <returns>APIResponse object consisting the <see cref="Car"/> Car with RC details object on success</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName = "Default30")]
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

