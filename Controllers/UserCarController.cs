using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Use_Wheels.Data;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Use_Wheels.Controllers
{
    [Route("user/cars")]
    [ApiController]
    public class UserCarController : ControllerBase
    {
        protected APIResponse _response;
        private ICarRepository _dbCar;
        private readonly IMapper _mapper;

        public UserCarController( ICarRepository dbCar, IMapper mapper)
        {
            _dbCar = dbCar;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [Authorize(Roles = "customer")]
        //[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCars([FromQuery(Name = "categoryId")] int? categoryId,
            [FromQuery] int pageSize = 0, int pageNumber = 1)
        {
            try
            {

                IEnumerable<Car> carList;
                
                if (categoryId == null)
                {
                    carList = await _dbCar.GetAllAsync(u => u.Availability == "available", pageSize: pageSize,
                        pageNumber: pageNumber, includeProperties: "Rc_Details");
                }
                else
                {
                    carList = await _dbCar.GetAllAsync(u => u.Category_Id == categoryId && u.Availability == "available", pageSize: pageSize, pageNumber: pageNumber, includeProperties: "Rc_Details");
                }

                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = carList;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpGet("{vehicle_no}", Name = "GetUserCar")]
        [Authorize(Roles = "customer")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCarById(string vehicle_no)
        {
            try
            {
                if (vehicle_no == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var category = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no && u.Availability == "available", includeProperties: "Rc_Details");
                if (category == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = category;
                _response.StatusCode = HttpStatusCode.OK;
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

