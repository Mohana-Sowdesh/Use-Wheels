namespace Use_Wheels.Services
{
	public class UserWishlistServices : IUserWishlistServices
    {
        private ICarRepository _dbCar;
        AdminCarUtility adminCarUtility = new AdminCarUtility();

        public UserWishlistServices(ICarRepository dbCar)
		{
            _dbCar = dbCar;
		}

        /// <summary>
        /// Service method to add a car to wishlist of that user
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task AddToWishlist(string vehicle_no, string username)
        {
            Car car = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no, includeProperties: "Rc_Details");
            if (car == null || car.Availability == false)
                throw new BadHttpRequestException(Constants.CarConstants.VEHICLE_NOT_FOUND, Constants.ResponseConstants.BAD_REQUEST);

            bool isUserInWishlist = WishListRepository.IsUserExists(username);
            List<Car> userCars;

            if (!isUserInWishlist)
                WishListRepository.CreateNewList(username);

            bool exists = WishListRepository.GetUserWishlist(username).Any(x => x.Vehicle_No == car.Vehicle_No);

            if (exists)
                throw new BadHttpRequestException(Constants.WishlistConstants.CAR_ALREADY_PRESENT, Constants.ResponseConstants.BAD_REQUEST);
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
        public void DeleteElementFromWishList(string vehicle_no, string username)
        {
            int validationResult = adminCarUtility.isVehicleNoValid(vehicle_no);

            if (validationResult == 0)
                throw new BadHttpRequestException(Constants.CarConstants.INVALID_VEHICLE_NUM, Constants.ResponseConstants.BAD_REQUEST);

            int deleteResult = WishListRepository.DeleteFromList(username, vehicle_no);

            if (deleteResult == -1)
                throw new BadHttpRequestException(Constants.WishlistConstants.VEHICLE_NOT_FOUND, Constants.ResponseConstants.BAD_REQUEST);

            Log.Information("Condition result: {@Result}", deleteResult);
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

