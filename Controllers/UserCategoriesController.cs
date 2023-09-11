using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Use_Wheels.Controllers
{
    [Route("user/categories")]
    [ApiController]
    [Authorize(Roles = Constants.Roles.CUSTOMER)]
    public class UserCategoriesController : ControllerBase
	{
        protected APIResponseDTO _response;
        private IUserCategoriesServices _service;
        public const string AllUserCategoriesCacheKey = "AllUserCategories";
        private readonly IMemoryCache _cache;

        public UserCategoriesController(IUserCategoriesServices service, IMemoryCache cache)
        {
            _cache = cache;
            _service = service;
            _response = new();
        }

        /// <summary>
        /// Method to get categories for customer role
        /// </summary>
        /// <returns>APIResponse object consisting the <see cref="Car"/> Car with RC details object on success</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName = Constants.Configurations.CACHE_PROFILE_NAME)]
        public async Task<ActionResult<APIResponseDTO>> GetCategories()
        {
            IEnumerable<Category> categoryList = await _service.GetCategories();

            if (!_cache.TryGetValue(AllUserCategoriesCacheKey, out IEnumerable<Category> usercategories))
            {
                // Add data to cache with a cache key and a short duration
                _cache.Set(AllUserCategoriesCacheKey, categoryList, TimeSpan.FromSeconds(30));
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = categoryList;
            return Ok(_response);
        }
    }
}

