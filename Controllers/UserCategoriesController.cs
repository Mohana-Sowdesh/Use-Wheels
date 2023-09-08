using System;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Data;
using Use_Wheels.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Services.IServices;

namespace Use_Wheels.Controllers
{
    [Route("user/categories")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class UserCategoriesController : ControllerBase
	{
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private IUserCategoriesServices _service;

        public UserCategoriesController(IUserCategoriesServices service, IMapper mapper)
        {
            _service = service;
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
            IEnumerable<Category> categoryList = await _service.GetCategories();

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = categoryList;
            return Ok(_response);
        }
    }
}

