using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using AutoMapper;
using Use_Wheels.Data;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using System.Net;
using System.Web.Http.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using Use_Wheels.Repository;

namespace Use_Wheels.Controllers
{
    [Route("user/cars/order")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class UserOrderController :ControllerBase
	{
        protected APIResponse _response;
        private IOrderRepository _dbOrder;
        private ICarRepository _dbCar;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserOrderController> _logger;

        public UserOrderController(ApplicationDbContext db, IOrderRepository dbOrder, IMapper mapper, ICarRepository dbCar, ILogger<UserOrderController> logger)
        {
            _db = db;
            _dbCar = dbCar;
            _dbOrder = dbOrder;
            _mapper = mapper;
            _logger = logger;
            _response = new();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            Orders order = _mapper.Map<Orders>(orderDTO);

            var userEmail = _db.Users.FirstOrDefault(x => x.Email == orderDTO.Email);
            if (userEmail == null)
            {
                _logger.LogError("Email does not exist");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email does not exist");
                return BadRequest(_response);
            }

            Car car = await _dbCar.GetAsync(u => u.Vehicle_No == orderDTO.Vehicle_No);
            if (car == null)
            {
                return BadRequest("Car doesn't exist");
            }

            car.Availability = "sold";
            await _dbOrder.CreateAsync(order);
            await _dbCar.UpdateAsync(car);

            _response.Result = order;
            _response.StatusCode = HttpStatusCode.Created;
            return Ok("Order placed successfully");
        }
    }
}

