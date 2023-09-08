using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Services.IServices;

namespace Use_Wheels.Services
{
	public class UserOrderServices : IUserOrderServices
	{
        private IOrderRepository _dbOrder;
        private ICarRepository _dbCar;

        public UserOrderServices(IOrderRepository dbOrder, ICarRepository dbCar)
		{
            _dbCar = dbCar;
            _dbOrder = dbOrder;
        }

        public async Task CreateOrder(Car car, Orders order)
		{
            car.Availability = "sold";
            await _dbOrder.CreateAsync(order);
            await _dbCar.UpdateAsync(car);
        }
    }
}

