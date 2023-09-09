using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Use_Wheels.Controllers
{
    [Route("user/categories")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class UserCategoriesController : ControllerBase
	{
        protected APIResponseDTO _response;
        private IUserCategoriesServices _service;

        public UserCategoriesController(IUserCategoriesServices service)
        {
            _service = service;
            _response = new();
        }

        /// <summary>
        /// Method to get categories for customer role
        /// </summary>
        /// <returns>APIResponse object consisting the <see cref="Car"/> Car with RC details object on success</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<APIResponseDTO>> GetCategories()
        {
            IEnumerable<Category> categoryList = await _service.GetCategories();

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = categoryList;
            return Ok(_response);
        }
    }
}

