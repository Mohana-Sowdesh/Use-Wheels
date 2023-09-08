using System;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using static Azure.Core.HttpHeader;
using Use_Wheels.Repository;
using Use_Wheels.Services.IServices;

namespace Use_Wheels.Controllers
{
    [Route("user/wishlist")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class UserWishlistController : ControllerBase
    {
        protected APIResponse _response;
        private ICarRepository _dbCar;
        private readonly IUserWishlistServices _service;

        public UserWishlistController(ICarRepository dbCar, IUserWishlistServices service)
        {
            _service = service;
            _dbCar = dbCar;
            _response = new();
        }

        /// <summary>
        /// Controller method to get a wishlist of a user
        /// </summary>
        /// <returns>APIResponse object with wishlist of that particular user as result</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetWishlist()
        {
            string username = HttpContext.User.Identity.Name;
            IEnumerable<Car> userWishlist = _service.GetWishlist(username);
            Log.Information("List contents: {@Names}", WishListRepository.GetUserWishlist(username));

            if (userWishlist == null)
            {
                Log.Error("No cars present in wishlist");
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "No cars present in wishlist";
            }
            else
            {
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = userWishlist;
            }
            return Ok(_response);
        }

        /// <summary>
        /// Controller method to add a car to wishlist of that user
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns>APIResponse object with success message on successful addition else error message</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> AddToWishlist(string vehicle_no)
        {
            try
            {
                if (vehicle_no == null)
                {
                    return BadRequest();
                }
                var car = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no, includeProperties: "Rc_Details");
                if (car == null || car.Availability == "sold")
                {
                    return NotFound();
                }
                string username = HttpContext.User.Identity.Name;
                bool isUserInWishlist = WishListRepository.IsUserExists(username);
                List<Car> userCars;

                if (!isUserInWishlist)
                    WishListRepository.CreateNewList(username);

                bool exists = WishListRepository.GetUserWishlist(username).Any(x => x.Vehicle_No == car.Vehicle_No);

                if (exists)
                {
                    Log.Information("Condition result: {@Result}", exists);
                    return BadRequest("Car already present in wishlist");
                }
                else
                {
                    _service.AddToWishlist(username, car);
                }

                Log.Information("List contents: {@Names}", WishListRepository.GetUserWishlist(username));
                _response.Result = "Car added to wish-list successfully!!";
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        /// <summary>
        /// Controller method to delete a car from wishlist of a particular user
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns>APIResponse object with success message on successful deletion else not found</returns>
        [HttpDelete("{vehicle_no}", Name = "DeleteElementFromWishList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteElementFromWishList(string vehicle_no)
        {
            string username = HttpContext.User.Identity.Name;
            _service.DeleteElementFromWishList(vehicle_no, username);
            
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            _response.Result = "Car deleted successfully from wishlist!!";
            return Ok(_response);
        }

    }
}

