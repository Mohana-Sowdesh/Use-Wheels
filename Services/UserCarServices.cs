namespace Use_Wheels.Services
{
	public class UserCarServices : IUserCarServices
	{
        private ICarRepository _dbCar;
        AdminCarUtility adminCarUtility = new AdminCarUtility();

        public UserCarServices(ICarRepository dbCar)
		{
            _dbCar = dbCar;
        }

        /// <summary>
        /// Service method to get a particular car by ID from DB
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns><see cref="Car"/></returns>
        public async Task<Car> GetCarById(string vehicle_no)
        {
            int validationResult = adminCarUtility.isVehicleNoValid(vehicle_no);

            if (validationResult == 0)
                throw new BadHttpRequestException(Constants.CarConstants.INVALID_VEHICLE_NUM, Constants.ResponseConstants.BAD_REQUEST);

            Car car = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no && u.Availability == "available", includeProperties: "Rc_Details");

            if (car == null)
                throw new BadHttpRequestException(Constants.CarConstants.VEHICLE_NOT_FOUND, Constants.ResponseConstants.NOT_FOUND);

            return car;
        }

        /// <summary>
        /// Service method to get all cars from DB
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>List of <see cref="Car"/>cars</returns>
        public async Task<IEnumerable<Car>> GetCars(int? categoryId, int pageSize = 0, int pageNumber = 1)
        { 
            IEnumerable<Car> carList;

            if (categoryId <= 0)
                throw new BadHttpRequestException(Constants.CategoryConstants.ID_VALIDATION, Constants.ResponseConstants.BAD_REQUEST);

            if (categoryId == null)
            {
                carList = await _dbCar.GetAllAsync(u => u.Availability == "available", pageSize: pageSize,
                    pageNumber: pageNumber, includeProperties: "Rc_Details");
            }
            else
                carList = await _dbCar.GetAllAsync(u => u.Category_Id == categoryId && u.Availability == "available", pageSize: pageSize, pageNumber: pageNumber, includeProperties: "Rc_Details");

            return carList;
        }
    }
}

