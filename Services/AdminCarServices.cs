using System;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Web.Http.ModelBinding;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Use_Wheels.Models.DTO;
using Use_Wheels.Utility;

namespace Use_Wheels.Services
{
	public class AdminCarServices : IAdminCarServices
	{
        private readonly ApplicationDbContext _db;
        private ICarRepository _dbCar;
        AdminCarUtility adminCarUtility = new AdminCarUtility();
        private readonly IMapper _mapper;

        public AdminCarServices(ApplicationDbContext db, ICarRepository dbCar, IMapper mapper)
		{
            _db = db;
            _dbCar = dbCar;
            _mapper = mapper;
        }

        /// <summary>
        /// Service method to add new car to DB
        /// </summary>
        /// <param name="carDTO">CarDTO Object</param>
        /// <returns>Car object</returns>
        public async Task<Car> AddCar(CarDTO carDTO)
        {
            if (await _dbCar.GetAsync(u => u.Vehicle_No.ToLower() == carDTO.Vehicle_No.ToLower()) != null)
                throw new BadHttpRequestException("Car already exists!!", 400);

            if (carDTO == null)
                throw new BadHttpRequestException("Request cannot be null", 400);

            int result = adminCarUtility.isVehicleNoSame(carDTO);
            if (result == -1)
                throw new BadHttpRequestException("Vehicle no. in the requests doesn't match", 400);

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
            if (vehicle_no == null)
                throw new BadHttpRequestException("Vehicle number is mandatory", 400);

            var vehicle = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no);
            var rc = await _db.RC.FirstOrDefaultAsync(u => u.Vehicle_No == vehicle_no);

            if (vehicle == null)
                throw new BadHttpRequestException("Vehicle not found", 404);

            await _dbCar.RemoveAsync(vehicle);
            _db.RC.Remove(rc);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Service method to delete car from DB
        /// </summary>
        /// <returns>List of Car object</returns>
        public async Task<IEnumerable<Car>> GetAllCars()
        {
            IEnumerable<Car> carList = await _dbCar.GetAllAsync(includeProperties: "Rc_Details");
            return carList;
        }

        /// <summary>
        /// Service method to get car by ID from DB
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <returns>Car object</returns>
        public async Task<Car> GetCarById(string vehicle_no)
        {
            if (vehicle_no == null)
                throw new BadHttpRequestException("Vehicle number cannot be null", 400);

            var car = await _dbCar.GetAsync(u => u.Vehicle_No == vehicle_no, includeProperties: "Rc_Details");

            if (car == null)
                throw new BadHttpRequestException("Uh-oh, the requested vehicle is not found", 404); ;
            return car;
        }

        /// <summary>
        /// Service method to update car 
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <param name="carUpdateDTO">CarUpdateDTO object</param>
        /// <returns></returns>
        public async Task UpdateCar(string vehicle_no, CarUpdateDTO carUpdateDTO)
        {
            if (carUpdateDTO == null || vehicle_no != carUpdateDTO.Vehicle_No)
                throw new BadHttpRequestException("Vehicle no. doesn't match", 400);

            Car dbCar = await _dbCar.GetAsync(u => u.Vehicle_No == carUpdateDTO.Vehicle_No, false);

            if (dbCar == null)
                throw new BadHttpRequestException("Uh-oh, the requested vehicle is not found", 404);

            Car model = _mapper.Map<Car>(carUpdateDTO);

            model.Created_Date = dbCar.Created_Date;

            await _dbCar.UpdateAsync(model);
        }
    }
}

