using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Use_Wheels.Controllers
{
    [Route("admin/categories")]
    [ApiController]
    [Authorize(Roles = Constants.Roles.ADMIN)]
    public class AdminCategoriesController : ControllerBase
	{
        protected APIResponseDTO _response;
        private readonly IAdminCategoriesServices _service;
        private const string AllCategoriesCacheKey = "AllCategories";
        private readonly IMemoryCache _cache;

        public AdminCategoriesController(IAdminCategoriesServices service, IMemoryCache cache)
		{
            _cache = cache;
            _service = service;
            _response = new();
        }

        /// <summary>
        /// Method to get all categories for admin role
        /// </summary>
        /// <returns>APIResponse with list of <see cref="Category"/>categories</returns>
        [HttpGet(Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName = Constants.Configurations.CACHE_PROFILE_NAME)]
        public async Task<ActionResult<APIResponseDTO>> GetAllCategories()
        {
            IEnumerable<Category> categoryList = await _service.GetAllCategories();

            if (!_cache.TryGetValue(AllCategoriesCacheKey, out IEnumerable<Category> categories))
            {
                // Add data to cache with a cache key and a short duration
                _cache.Set(AllCategoriesCacheKey, categoryList, TimeSpan.FromSeconds(30));
            }

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

            // Cache invalidation
            _cache.Remove(AllCategoriesCacheKey);
            _cache.Remove(UserCategoriesController.AllUserCategoriesCacheKey);

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

            // Cache invalidation
            _cache.Remove(AllCategoriesCacheKey);
            _cache.Remove(UserCategoriesController.AllUserCategoriesCacheKey);

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
            
        }
    }
}

