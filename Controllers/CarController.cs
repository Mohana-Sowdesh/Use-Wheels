using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Use_Wheels.Controllers
{
    [Route("/cars")]
    [ApiController]
    public class CarController : ControllerBase
	{
        protected APIResponseDTO _response;
        private readonly ICarServices _service;
        private readonly IMemoryCache _cache;
        private const string AllCarsCacheKey = "AllCars";

        public CarController(ICarServices service, IMemoryCache cache)
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
        [Authorize]
        [ResponseCache(CacheProfileName = Constants.Configurations.CACHE_PROFILE_NAME)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponseDTO>> GetAllCars()
        {
            string role = HttpContext.User.Identities.First().Claims.ElementAt(1).Value;
            IEnumerable<CarDTO> carList = await _service.GetAllCars(role);

            if (!_cache.TryGetValue(AllCarsCacheKey, out IEnumerable<Car> cars))
            {
                // Add data to cache with a cache key and a short duration
                _cache.Set(AllCarsCacheKey, carList, TimeSpan.FromSeconds(30));
            }

            _response.Result = carList;
            return Ok(_response);
        }


        /// <summary>
        /// Method to gets particular car by ID for admin role
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns>APIResponse object consisting the <see cref="Car"/> Car with RC details object on success</returns>
        [HttpGet("{vehicle_no}", Name = "GetCar")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponseDTO>> GetCarById(string vehicle_no)
        {
            string role = HttpContext.User.Identities.First().Claims.ElementAt(1).Value;
            var car = await _service.GetCarById(vehicle_no, role);

            _response.Result = car;
            return Ok(_response);
        }


        /// <summary>
        /// Method to gets particular car by ID for admin role
        /// </summary>
        /// <param name="carDTO">CarDTO Object</param>
        /// <returns>APIResponse object consisting the car object on success</returns>
        [HttpPost]
        [Authorize(Roles = Constants.Roles.SELLER)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponseDTO>> AddCar([FromBody] CarDTO carDTO)
        {
            string username = HttpContext.User.Identity.Name;
            Car car = await _service.AddCar(carDTO, username);

            _cache.Remove(AllCarsCacheKey);

            _response.Result = car;
            return CreatedAtRoute("GetCar", new { vehicle_no = car.Vehicle_No }, _response);
        }

        /// <summary>
        /// Method to delete a particular car by vehicle_no for admin role
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns></returns>
        [HttpDelete("{vehicle_no}", Name = "DeleteCar")]
        [Authorize(Roles = Constants.Roles.ADMIN)]
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
            return NoContent();
        }

        /// <summary>
        /// Method to update a particular car by vehicle_no for admin role
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <param name="carUpdateDTO">CarUpdateDTO object</param>
        /// <returns>APIResponse with no content on success or error list on exception</returns>
        [HttpPut("{vehicle_no}", Name = "UpdateCar")]
        [Authorize(Roles = "seller, admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponseDTO>> UpdateCar(string vehicle_no, [FromBody] CarUpdateDTO carUpdateDTO)
        {
            await _service.UpdateCar(vehicle_no, carUpdateDTO);

            // Cache invalidation
            _cache.Remove(AllCarsCacheKey);

            return NoContent();
        }
    }
}

