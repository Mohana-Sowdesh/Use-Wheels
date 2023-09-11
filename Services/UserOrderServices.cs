using AutoMapper;

namespace Use_Wheels.Services
{
	public class UserOrderServices : IUserOrderServices
	{
        private readonly ApplicationDbContext _db;
        private IOrderRepository _dbOrder;
        private ICarRepository _dbCar;
        private readonly IMapper _mapper;

        public UserOrderServices(ApplicationDbContext db, IOrderRepository dbOrder, ICarRepository dbCar, IMapper mapper)
		{
            _db = db;
            _dbCar = dbCar;
            _mapper = mapper;
            _dbOrder = dbOrder;
        }

        /// <summary>
        /// Service method to create a new order
        /// </summary>
        /// <param name="orderDTO"><see cref="OrderDTO"/></param>
        /// <returns>List of <see cref="Category"/>categories</returns>
        public async Task<Orders> CreateOrder(OrderDTO orderDTO)
        {
            var userEmail = _db.Users.FirstOrDefault(x => x.Email == orderDTO.Email);
            if (userEmail == null)
                throw new BadHttpRequestException(Constants.OrderConstants.EMAIL_DOES_NOT_EXISTS, Constants.ResponseConstants.BAD_REQUEST);

            Car car = await _dbCar.GetAsync(u => u.Vehicle_No == orderDTO.Vehicle_No && u.Availability == Constants.OrderConstants.AVAILABLE);
            if (car == null)
                throw new BadHttpRequestException(Constants.CarConstants.VEHICLE_NOT_FOUND, Constants.ResponseConstants.NOT_FOUND);

            Orders order = _mapper.Map<Orders>(orderDTO);
            car.Availability = Constants.OrderConstants.SOLD;
            await _dbOrder.CreateAsync(order);
            await _dbCar.UpdateAsync(car);
            return order;
        }
    }
}

