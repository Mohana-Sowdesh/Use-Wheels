using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services
{
	public class CarServices : ICarServices
	{
        private readonly ApplicationDbContext _db;
        private ICarRepository _dbCar;
        private readonly IMapper _mapper;
        private readonly IMockAPIUtility _apiUtility;
        AdminCarUtility adminCarUtility = new AdminCarUtility();

        public CarServices(ApplicationDbContext db, ICarRepository dbCar, IMapper mapper, IMockAPIUtility apiUtility)
		{
            _db = db;
            _dbCar = dbCar;
            _mapper = mapper;
            _apiUtility = apiUtility;
        }

        /// <summary>
        /// Service method to add new car to DB
        /// </summary>
        /// <param name="carDTO">CarDTO Object</param>
        /// <returns>Car object</returns>
        public async Task<Car> AddCar(CarDTO carDTO, string username)
        {
            if (await _dbCar.GetAsync(u => u.Vehicle_No.ToLower() == carDTO.Vehicle_No.ToLower()) != null)
                throw new BadHttpRequestException(Constants.CarConstants.CAR_ALREADY_EXISTS, Constants.ResponseConstants.BAD_REQUEST);

            if (carDTO == null)
                throw new BadHttpRequestException(Constants.CarConstants.DTO_NULL, Constants.ResponseConstants.BAD_REQUEST);

            int result = adminCarUtility.isVehicleNoSame(carDTO);
            if (result == -1)
                throw new BadHttpRequestException(Constants.CarConstants.VEHICLE_NUM_NOT_MATCH, Constants.ResponseConstants.BAD_REQUEST);

            List<VehicleInfoDTO> blackedCars = await _apiUtility.GetMockDataAsync();
            int mockApiDataCheckResult = adminCarUtility.CheckIfVehicleIsIllegal(blackedCars, carDTO.Vehicle_No);

            if (mockApiDataCheckResult == 1)
            {
                User user = _db.Users.FirstOrDefault(u => u.UserName == username);
                user.isBlacked = true;
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                throw new BadHttpRequestException(Constants.CarConstants.MISSING_VEHICLE_ERROR, Constants.ResponseConstants.BAD_REQUEST);
            }

            Car car = _mapper.Map<Car>(carDTO);
            await _dbCar.CreateAsync(car);
            return car;
        }

        /// <summary>
        /// Service method to delete car from DB
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns></returns>
        public async Task DeleteCar(string vehicle_no)
        {
            int validationResult = adminCarUtility.isVehicleNoValid(vehicle_no);

            if (validationResult == 0)
                throw new BadHttpRequestException(Constants.CarConstants.INVALID_VEHICLE_NUM, Constants.ResponseConstants.BAD_REQUEST);

            var vehicle = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no);
            var rc = await _db.RC.FirstOrDefaultAsync(u => u.Vehicle_No == vehicle_no);

            if (vehicle == null)
                throw new BadHttpRequestException(Constants.CarConstants.VEHICLE_NOT_FOUND, Constants.ResponseConstants.NOT_FOUND);

            await _dbCar.RemoveAsync(vehicle);
            _db.RC.Remove(rc);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Service method to delete car from DB
        /// </summary>
        /// <returns>List of Car object</returns>
        public async Task<IEnumerable<CarDTO>> GetAllCars(string role)
        {
            IEnumerable<Car> carList;
            if (role == "admin")
                carList = await _dbCar.GetAllAsync(includeProperties: "Rc_Details");
            else
                carList = await _dbCar.GetAllAsync(u => u.Availability == true, includeProperties: "Rc_Details");

            List<VehicleInfoDTO> blackedCars = await _apiUtility.GetMockDataAsync();
            IEnumerable<CarDTO> carDTOList = _mapper.Map<IEnumerable<CarDTO>>(carList);

            foreach (CarDTO car in carDTOList)
            {
                int mockApiDataCheckResult = adminCarUtility.CheckIfVehicleIsIllegal(blackedCars, car.Vehicle_No);

                if (mockApiDataCheckResult == 2)
                    car.TrialedCar = Constants.CarConstants.CAR_TRIALED_WARNING_MSG;
            }

            return carDTOList;
        }

        /// <summary>
        /// Service method to get car by ID from DB
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns>Car object</returns>
        public async Task<CarDTO> GetCarById(string vehicle_no, string role)
        {
            int validationResult = adminCarUtility.isVehicleNoValid(vehicle_no);

            if (validationResult == 0)
                throw new BadHttpRequestException(Constants.CarConstants.INVALID_VEHICLE_NUM, Constants.ResponseConstants.BAD_REQUEST);

            Car car;

            if (role == "admin")
                car = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no, includeProperties: "Rc_Details");
            else
                car = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no && u.Availability == true, includeProperties: "Rc_Details");

            if (car == null)
                throw new BadHttpRequestException(Constants.CarConstants.VEHICLE_NOT_FOUND, Constants.ResponseConstants.NOT_FOUND); ;

            List<VehicleInfoDTO> blackedCars = await _apiUtility.GetMockDataAsync();
            int mockApiDataCheckResult = adminCarUtility.CheckIfVehicleIsIllegal(blackedCars, vehicle_no);

            CarDTO carDTO = _mapper.Map<CarDTO>(car);
            if (mockApiDataCheckResult == 2)
                carDTO.TrialedCar = Constants.CarConstants.CAR_TRIALED_WARNING_MSG;

            return carDTO;
        }

        /// <summary>
        /// Service method to update car 
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <param name="carUpdateDTO">CarUpdateDTO object</param>
        /// <returns></returns>
        public async Task UpdateCar(string vehicle_no, CarUpdateDTO carUpdateDTO)
        {
            int validationResult = adminCarUtility.isVehicleNoValid(vehicle_no);

            if (validationResult == 0)
                throw new BadHttpRequestException(Constants.CarConstants.INVALID_VEHICLE_NUM, Constants.ResponseConstants.BAD_REQUEST);

            if (vehicle_no != carUpdateDTO.Vehicle_No)
                throw new BadHttpRequestException(Constants.CarConstants.VEHICLE_NUM_NOT_MATCH, Constants.ResponseConstants.BAD_REQUEST);

            Car dbCar = await _dbCar.GetAsync(u => u.Vehicle_No == carUpdateDTO.Vehicle_No, false);

            if (dbCar == null)
                throw new BadHttpRequestException(Constants.CarConstants.VEHICLE_NOT_FOUND, Constants.ResponseConstants.NOT_FOUND);

            Car model = _mapper.Map<Car>(carUpdateDTO);

            model.Created_Date = dbCar.Created_Date;

            await _dbCar.UpdateAsync(model);
        }
    }
}

