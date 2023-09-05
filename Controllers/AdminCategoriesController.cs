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

namespace Use_Wheels.Controllers
{
    [Route("admin/categories")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminCategoriesController : ControllerBase
	{
        protected APIResponse _response;
        private ICategoryRepository _dbCategory;
        private readonly IMapper _mapper;

        public AdminCategoriesController(ICategoryRepository dbCategory, IMapper mapper)
		{
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
        public async Task<ActionResult<APIResponse>> GetAllCategories()
        {
            IEnumerable<Category> categoryList = await _dbCategory.GetAllAsync();
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
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            { 
                if (await _dbCategory.GetAsync(u => u.Category_Names.ToLower() == categoryDTO.Category_Names.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Category already Exists!");
                    return BadRequest(ModelState);
                }

                if (categoryDTO == null)
                {
                    return BadRequest(categoryDTO);
                }

                Category category = _mapper.Map<Category>(categoryDTO);

                await _dbCategory.CreateAsync(category);
                _response.Result = category;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetCategory", new { id = category.Category_Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
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
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var category = await _dbCategory.GetAsync(u => u.Category_Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                await _dbCategory.RemoveAsync(category);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}

