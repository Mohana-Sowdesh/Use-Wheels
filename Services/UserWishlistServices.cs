namespace Use_Wheels.Services
{
	public class UserWishlistServices : IUserWishlistServices
    {
        private ICarRepository _dbCar;

        public UserWishlistServices(ICarRepository dbCar)
		{
            _dbCar = dbCar;
		}

        /// <summary>
        /// Service method to add a car to wishlist of that user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="car"><see cref="Car"/></param>
        /// <returns></returns>
        public async Task AddToWishlist(string vehicle_no, string username)
        {
            Car car = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no, includeProperties: "Rc_Details");
            if (car == null || car.Availability == "sold")
                throw new BadHttpRequestException("Uh-oh, the requested car cannot be found", 400);

            bool isUserInWishlist = WishListRepository.IsUserExists(username);
            List<Car> userCars;

            if (!isUserInWishlist)
                WishListRepository.CreateNewList(username);

            bool exists = WishListRepository.GetUserWishlist(username).Any(x => x.Vehicle_No == car.Vehicle_No);

            if (exists)
                throw new BadHttpRequestException("Car already present in wishlist", 400);
            else
            {
                WishListRepository.AddToList(username, car);
                car.Likes = car.Likes + 1;
                await _dbCar.UpdateAsync(car);
            }
        }

        /// <summary>
        /// Service method to delete a car from wishlist of a particular user
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task DeleteElementFromWishList(string vehicle_no, string username)
        {
            if (vehicle_no == null)
                throw new BadHttpRequestException("Vehicle no. is mandatory", 400);

            int deleteResult = WishListRepository.DeleteFromList(username, vehicle_no);

            if (deleteResult == -1)
                throw new BadHttpRequestException("Vehicle not found", 404);

            Log.Information("Condition result: {@Result}", deleteResult);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Service method to get a wishlist of a user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Wishlist of that particular user</returns>
        public IEnumerable<Car> GetWishlist(string username)
        {
            IEnumerable<Car> userWishlist = WishListRepository.GetUserWishlist(username);
            return userWishlist;
        }
    }
}

