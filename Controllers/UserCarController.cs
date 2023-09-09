using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace Use_Wheels.Controllers
{
    [Route("user/cars")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class UserCarController : ControllerBase
    {
        protected APIResponseDTO _response;
        private readonly IUserCarServices _service;

        public UserCarController(IUserCarServices service)
        {
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
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponseDTO>> GetCars([FromQuery(Name = "categoryId")] int? categoryId,
            [FromQuery] int pageSize = 0, int pageNumber = 1)
        {
                IEnumerable<Car> carList = await _service.GetCars(categoryId, pageSize, pageNumber);

                // Adds page number and page size info to response headers
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                _response.Result = carList;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

        }

        /// <summary>
        /// Method to get car by id for customer role
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns>APIResponse object consisting the <see cref="Car"/> Car with RC details object on success</returns>
        [HttpGet("{vehicle_no}", Name = "GetUserCar")]
        [ResponseCache(CacheProfileName = "Default30")]
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

