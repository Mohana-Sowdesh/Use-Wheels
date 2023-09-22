using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Use_Wheels.Controllers
{
    [Route("/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
	{
        protected APIResponseDTO _response;
        private readonly ICategoriesServices _service;
        private readonly IMemoryCache _cache;

        public CategoriesController(ICategoriesServices service)
		{
            _service = service;
            _response = new();
        }

        /// <summary>
        /// Method to get all categories for admin role
        /// </summary>
        /// <returns>APIResponse with list of <see cref="Category"/>categories</returns>
        [HttpGet(Name = "GetCategory")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName = Constants.Configurations.CACHE_PROFILE_NAME)]
        public async Task<ActionResult<APIResponseDTO>> GetAllCategories()
        {
            IEnumerable<Category> categoryList = await _service.GetAllCategories();

            _response.Result = categoryList;
            return Ok(_response);
        }


        /// <summary>
        /// Method to create a category for admin role
        /// </summary>
        /// <param name="categoryDTO">CategoryDTO object</param>
        /// <returns>APIResponse with created <see cref="Category"/>category as result on success</returns>
        [HttpPost]
        [Authorize(Roles = Constants.Roles.ADMIN)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponseDTO>> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            Category category = await _service.CreateCategory(categoryDTO);

            _response.Result = category;
            return CreatedAtRoute("GetCategory", new { id = category.Category_Id }, _response);
        }

        /// <summary>
        /// Method to delete a category for admin role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [Authorize(Roles = Constants.Roles.ADMIN)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponseDTO>> DeleteCategory(int id)
        {
            await _service.DeleteCategory(id);
            return NoContent();
        }
    }
}

