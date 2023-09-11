using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Use_Wheels.Controllers
{
    [Route("user/cars")]
    [ApiController]
    [Authorize(Roles = Constants.Roles.CUSTOMER)]
    public class UserCarController : ControllerBase
    {
        protected APIResponseDTO _response;
        private readonly IUserCarServices _service;
        private readonly IMemoryCache _cache;
        public const string AllUserCarsCacheKey = "AllUserCars";

        public UserCarController(IUserCarServices service, IMemoryCache cache)
        {
            _cache = cache;
            _service = service;
            _response = new();
        }

        /// <summary>
        /// Method to get cars for customer role
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>APIResponse object with the requested list of <see cref="Car"/>cars</returns>
        [HttpGet]
        [ResponseCache(CacheProfileName = Constants.Configurations.CACHE_PROFILE_NAME)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponseDTO>> GetCars([FromQuery(Name = "categoryId")] int? categoryId,
            [FromQuery] int pageSize = 0, int pageNumber = 1)
        {
                IEnumerable<Car> carList = await _service.GetCars(categoryId, pageSize, pageNumber);

                if (!_cache.TryGetValue(AllUserCarsCacheKey, out IEnumerable<Car> usercars))
                {
                    // Add data to cache with a cache key and a short duration
                    _cache.Set(AllUserCarsCacheKey, carList, TimeSpan.FromSeconds(30));
                }

                // Adds page number and page size info to response headers
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                Response.Headers.Add(Constants.Pagination.PAGINATION_KEY, JsonSerializer.Serialize(pagination));

                if (carList.Count() == 0)
                {
                    _response.Result = Constants.CarConstants.NO_CARS_PRESENT;
                    _response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    _response.Result = carList;
                    _response.StatusCode = HttpStatusCode.OK;
                }
                return Ok(_response);
        }

        /// <summary>
        /// Method to get car by id for customer role
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns>APIResponse object consisting the <see cref="Car"/> Car with RC details object on success</returns>
        [HttpGet("{vehicle_no}", Name = "GetUserCar")]
        [ResponseCache(CacheProfileName = Constants.Configurations.CACHE_PROFILE_NAME)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponseDTO>> GetCarById(string vehicle_no)
        {
            Car car = await _service.GetCarById(vehicle_no);
                
            _response.Result = car;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}

