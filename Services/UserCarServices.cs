using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Services.IServices;

namespace Use_Wheels.Services
{
	public class UserCarServices : IUserCarServices
	{
        private ICarRepository _dbCar;

        public UserCarServices(ICarRepository dbCar)
		{
            _dbCar = dbCar;
        }

        public async Task<Car> GetCarById(string vehicle_no)
        {
            Car car = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no && u.Availability == "available", includeProperties: "Rc_Details");
            return car;
        }

        public async Task<IEnumerable<Car>> GetCars(int? categoryId, int pageSize = 0, int pageNumber = 1)
        {
            IEnumerable<Car> carList;
            if (categoryId == null)
            {
                carList = await _dbCar.GetAllAsync(u => u.Availability == "available", pageSize: pageSize,
                    pageNumber: pageNumber, includeProperties: "Rc_Details");
            }
            else
            {
                carList = await _dbCar.GetAllAsync(u => u.Category_Id == categoryId && u.Availability == "available", pageSize: pageSize, pageNumber: pageNumber, includeProperties: "Rc_Details");
            }
            return carList;
        }
    }
}

