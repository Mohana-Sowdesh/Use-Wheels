using System;
using System.Data;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Use_Wheels.Data;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Services.IServices;

namespace Use_Wheels.Controllers
{
    [Route("admin/categories")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminCategoriesController : ControllerBase
	{
        protected APIResponseDTO _response;
        private ICategoryRepository _dbCategory;
        private readonly IMapper _mapper;
        private readonly IAdminCategoriesServices _service;

        public AdminCategoriesController(ICategoryRepository dbCategory, IMapper mapper, IAdminCategoriesServices service)
		{
            _service = service;
            _dbCategory = dbCategory;
            _mapper = mapper;
            _response = new();
        }

        /// <summary>
        /// Method to get all categories for admin role
        /// </summary>
        /// <returns>APIResponse with list of <see cref="Category"/>categories</returns>
        [HttpGet(Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<APIResponseDTO>> GetAllCategories()
        {
            IEnumerable<Category> categoryList = await _service.GetAllCategories();

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = categoryList;
            return Ok(_response);
        }


        /// <summary>
        /// Method to create a category for admin role
        /// </summary>
        /// <param name="categoryDTO">CategoryDTO object</param>
        /// <returns>APIResponse with created <see cref="Category"/>category as result on success</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponseDTO>> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            Category category = await _service.CreateCategory(categoryDTO);

            _response.Result = category;
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetCategory", new { id = category.Category_Id }, _response);
        }

        /// <summary>
        /// Method to delete a category for admin role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponseDTO>> DeleteCategory(int id)
        {
            await _service.DeleteCategory(id);

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
            
        }
    }
}

