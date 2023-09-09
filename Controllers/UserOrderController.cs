using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;


namespace Use_Wheels.Controllers
{
    [Route("user/cars/order")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class UserOrderController :ControllerBase
	{
        protected APIResponseDTO _response;
        private readonly IUserOrderServices _service;

        public UserOrderController(IUserOrderServices service)
        { 
            _service = service;
            _response = new();
        }

        /// <summary>
        /// Method to create an order for customer role
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <returns>APIResponse object consisting the <see cref="OrderDTO"/> order object</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponseDTO>> CreateOrder([FromBody] OrderDTO orderDTO)
        { 
            Orders order = await _service.CreateOrder(orderDTO);

            _response.Result = order;
            _response.StatusCode = HttpStatusCode.Created;
            return Ok("Order placed successfully");
        }
    }
}

