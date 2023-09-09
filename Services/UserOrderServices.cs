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

        public async Task<Orders> CreateOrder(OrderDTO orderDTO)
        {
            var userEmail = _db.Users.FirstOrDefault(x => x.Email == orderDTO.Email);
            if (userEmail == null)
                throw new BadHttpRequestException("Email does not exist", 400);

            Car car = await _dbCar.GetAsync(u => u.Vehicle_No == orderDTO.Vehicle_No && u.Availability == "available");
            if (car == null)
                throw new BadHttpRequestException("Uh-oh, requested car does not exist", 404);

            Orders order = _mapper.Map<Orders>(orderDTO);
            car.Availability = "sold";
            await _dbOrder.CreateAsync(order);
            await _dbCar.UpdateAsync(car);
            return order;
        }
    }
}

