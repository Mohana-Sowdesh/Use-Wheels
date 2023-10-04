using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Use_Wheels.Services
{
	public class OrderServices : IOrderServices
	{
        private readonly ApplicationDbContext _db;
        private IOrderRepository _dbOrder;
        private ICarRepository _dbCar;
        private readonly IMockAPIUtility _apiUtility;
        private readonly IMapper _mapper;
        AdminCarUtility adminCarUtility = new AdminCarUtility();
        private readonly UserManager<User> _userManager;

        public OrderServices(ApplicationDbContext db, IOrderRepository dbOrder, ICarRepository dbCar, IMapper mapper, IMockAPIUtility apiUtility, UserManager<User> userManager)
		{
            _db = db;
            _dbCar = dbCar;
            _mapper = mapper;
            _dbOrder = dbOrder;
            _apiUtility = apiUtility;
            _userManager = userManager;
        }

        /// <summary>
        /// Service method to create a new order
        /// </summary>
        /// <param name="orderDTO"><see cref="OrderDTO"/></param>
        /// <returns>List of <see cref="Category"/>categories</returns>
        public async Task<Orders> CreateOrder(OrderDTO orderDTO)
        {
            var userEmail = await _userManager.FindByEmailAsync(orderDTO.Email);
            if (userEmail == null)
                throw new BadHttpRequestException(Constants.OrderConstants.EMAIL_DOES_NOT_EXISTS, Constants.ResponseConstants.BAD_REQUEST);

            Car car = await _dbCar.GetAsync(u => u.Vehicle_No == orderDTO.Vehicle_No && u.Availability == true);
            if (car == null)
                throw new BadHttpRequestException(Constants.CarConstants.VEHICLE_NOT_FOUND, Constants.ResponseConstants.NOT_FOUND);

            Orders order = _mapper.Map<Orders>(orderDTO);
            order.Gross_Price = car.Price;
            car.Availability = false;

            List<VehicleInfoDTO> blackedCars = await _apiUtility.GetMockDataAsync();
            int mockApiDataCheckResult = adminCarUtility.CheckIfVehicleIsIllegal(blackedCars, orderDTO.Vehicle_No);

            // Providing 18% discount if the ordered car has undergone a legal trial
            if (mockApiDataCheckResult == 2)
                order.Gross_Price = car.Price - (18 * car.Price / 100);

            await _dbOrder.CreateAsync(order);
            await _dbCar.UpdateAsync(car);
            return order;
        }
    }
}

