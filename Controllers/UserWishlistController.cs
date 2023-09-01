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

namespace Use_Wheels.Controllers
{
    [Route("user/wishlist")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class UserWishlistController : ControllerBase
    {
        protected APIResponse _response;
        private ICarRepository _dbCar;
        static Wishlist newWishlist = new Wishlist
        {
            wishlist = new List<Car>()
        };

        public UserWishlistController(ICarRepository dbCar)
        {
            _dbCar = dbCar;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetWishlist()
        {
            Log.Information("List contents: {@Names}", newWishlist.wishlist);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = newWishlist.wishlist.ToList();
            return Ok(_response);
        }

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
                var car = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no);
                if (car == null)
                {
                    return NotFound();
                }

                bool exists = newWishlist.wishlist.Any(x => x.Vehicle_No == car.Vehicle_No);
                if (exists)
                    return BadRequest("Car already present in wishlist");


                Log.Information("Condition result: {@Result}", exists);
                car.Likes = car.Likes + 1;
                newWishlist.wishlist.Add(car);
                await _dbCar.UpdateAsync(car);
                Log.Information("List contents: {@Names}", newWishlist.wishlist);
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

        [HttpDelete("{vehicle_no}", Name = "DeleteElementFromWishList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteElementFromWishList(string vehicle_no)
        {

            if (vehicle_no == null)
            {
                return BadRequest("Vehicle no. is mandatory");
            }
            var vehicle = newWishlist.wishlist.FirstOrDefault(u => u.Vehicle_No == vehicle_no);
                
            if (vehicle == null)
            {
                return NotFound();
            }

            bool deleteResult = newWishlist.wishlist.Remove(vehicle);
            Log.Information("Condition result: {@Result}", deleteResult);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }

    }
}

