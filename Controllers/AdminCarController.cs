using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Use_Wheels.Controllers
{
    [Route("admin/cars")]
    [ApiController]
    [Authorize(Roles = Constants.Roles.ADMIN)]
    public class AdminCarController : ControllerBase
	{
        protected APIResponseDTO _response;
        private readonly IAdminCarServices _service;
        private readonly IMemoryCache _cache;
        private const string AllCarsCacheKey = "AllCars";

        public AdminCarController(IAdminCarServices service, IMemoryCache cache)
        {
            _cache = cache;
            _service = service;
            _response = new();
        }

        /// <summary>
        /// Method to gets all cars for admin role
        /// </summary>
        /// <param></param>
        /// <returns>APIResponse object consisting list of <see cref="Car"/>Car with RC details</returns>
        [HttpGet]
        [ResponseCache(CacheProfileName = Constants.Configurations.CACHE_PROFILE_NAME)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponseDTO>> GetAllCars()
        {
            IEnumerable<Car> carList = await _service.GetAllCars();

            if (!_cache.TryGetValue(AllCarsCacheKey, out IEnumerable<Car> cars))
            {
                // Add data to cache with a cache key and a short duration
                _cache.Set(AllCarsCacheKey, carList, TimeSpan.FromSeconds(30));
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = carList;
            return Ok(_response);
        }


        /// <summary>
        /// Method to gets particular car by ID for admin role
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns>APIResponse object consisting the <see cref="Car"/> Car with RC details object on success</returns>
        [HttpGet("{vehicle_no}", Name = "GetCar")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponseDTO>> GetCarById(string vehicle_no)
        {
            var car = await _service.GetCarById(vehicle_no);

            _response.Result = car;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }


        /// <summary>
        /// Method to gets particular car by ID for admin role
        /// </summary>
        /// <param name="carDTO">CarDTO Object</param>
        /// <returns>APIResponse object consisting the car object on success</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponseDTO>> AddCar([FromBody] CarDTO carDTO)
        {
            Car car = await _service.AddCar(carDTO);

            _cache.Remove(AllCarsCacheKey);
            _cache.Remove(UserCarController.AllUserCarsCacheKey);

            _response.Result = car;
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetCar", new { vehicle_no = car.Vehicle_No }, _response);
        }

        /// <summary>
        /// Method to delete a particular car by vehicle_no for admin role
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns></returns>
        [HttpDelete("{vehicle_no}", Name = "DeleteCar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponseDTO>> DeleteCar(string vehicle_no)
        {
                await _service.DeleteCar(vehicle_no);

                // Cache invalidation
                _cache.Remove(AllCarsCacheKey);
                _cache.Remove(UserCarController.AllUserCarsCacheKey);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
        }

        /// <summary>
        /// Method to update a particular car by vehicle_no for admin role
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <param name="carUpdateDTO">CarUpdateDTO object</param>
        /// <returns>APIResponse with no content on success or error list on exception</returns>
        [HttpPut("{vehicle_no}", Name = "UpdateCar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponseDTO>> UpdateCar(string vehicle_no, [FromBody] CarUpdateDTO carUpdateDTO)
        {
            try
            {   
                await _service.UpdateCar(vehicle_no, carUpdateDTO);

                // Cache invalidation
                _cache.Remove(AllCarsCacheKey);
                _cache.Remove(UserCarController.AllUserCarsCacheKey);

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

