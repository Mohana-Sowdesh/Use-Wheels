using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Use_Wheels.Controllers
{
    [Route("user/wishlist")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class UserWishlistController : ControllerBase
    {
        protected APIResponseDTO _response;
        private readonly IUserWishlistServices _service;

        public UserWishlistController(IUserWishlistServices service)
        {
            _service = service;
            _response = new();
        }

        /// <summary>
        /// Controller method to get a wishlist of a user
        /// </summary>
        /// <returns>APIResponse object with wishlist of that particular user as result</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponseDTO>> GetWishlist()
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
        public async Task<ActionResult<APIResponseDTO>> AddToWishlist(string vehicle_no)
        {
            string username = HttpContext.User.Identity.Name;
            await _service.AddToWishlist(vehicle_no, username);

            Log.Information("List contents: {@Names}", WishListRepository.GetUserWishlist(username));
            _response.Result = "Car added to wish-list successfully!!";
            _response.StatusCode = HttpStatusCode.Created;
            return Ok(_response);
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
        public async Task<ActionResult<APIResponseDTO>> DeleteElementFromWishList(string vehicle_no)
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

