using System;
using System.Data;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Use_Wheels.Data;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;

namespace Use_Wheels.Controllers
{
    [Route("admin/cars")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminCarController : ControllerBase
	{
        protected APIResponse _response;
        private ICarRepository _dbCar;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;

        public AdminCarController(ApplicationDbContext db, ICarRepository dbCar, IMapper mapper)
        {
            _db = db;
            _dbCar = dbCar;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        [ResponseCache(Duration = 60)]
        //[ResponseCache(CacheProfileName = "Default30
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllCars()
        {
            IEnumerable<Car> carList = await _dbCar.GetAllAsync(includeProperties: "Rc_Details");
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = carList;
            return Ok(_response);
        }

        [HttpGet("{vehicle_no}", Name = "GetCar")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<APIResponse>> GetCarById(string vehicle_no)
        {
            try
            {
                if (vehicle_no == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var category = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no, includeProperties: "Rc_Details");
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

        [HttpPost]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> AddCar([FromBody] CarDTO carDTO)
        {
            try
            {
                if (await _dbCar.GetAsync(u => u.Vehicle_No.ToLower() == carDTO.Vehicle_No.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Car already Exists!");
                    return BadRequest(ModelState);
                }

                if (carDTO == null)
                {
                    return BadRequest(carDTO);
                }

                Car car = _mapper.Map<Car>(carDTO);
                await _dbCar.CreateAsync(car);

                _response.Result = car;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetCar", new { vehicle_no = car.Vehicle_No }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{vehicle_no}", Name = "DeleteCar")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCar(string vehicle_no)
        {
            try
            {
                if (vehicle_no == null)
                {
                    return BadRequest("Vehicle no. is mandatory");
                }
                var vehicle = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no);
                var rc = await _db.RC.FirstOrDefaultAsync(u => u.Vehicle_No == vehicle_no);
                if (vehicle == null)
                {
                    return NotFound();
                }
                await _dbCar.RemoveAsync(vehicle);
                _db.RC.Remove(rc);
                await _db.SaveChangesAsync();
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

        [HttpPut("{vehicle_no}", Name = "UpdateCar")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCar(string vehicle_no, [FromBody] CarUpdateDTO carUpdateDTO)
        {
            try
            {
                if (carUpdateDTO == null || vehicle_no != carUpdateDTO.Vehicle_No)
                {
                    return BadRequest("Vehicle no. doesn't match");
                }
                Car dbCar = await _dbCar.GetAsync(u => u.Vehicle_No == carUpdateDTO.Vehicle_No);
                if(dbCar == null)
                {
                    return BadRequest("Car doesn't exist");
                }
                Car model = _mapper.Map<Car>(carUpdateDTO);

                model.Created_Date = dbCar.Created_Date;
                await _dbCar.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
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
    }
}

