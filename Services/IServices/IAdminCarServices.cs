using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services.IServices
{
	public interface IAdminCarServices
	{
        Task<IEnumerable<Car>> GetAllCars();
        Task<Car> GetCarById(string vehicle_no);
        Task<Car> AddCar(CarDTO carDTO);
        Task DeleteCar(string vehicle_no);
        Task UpdateCar(string vehicle_no, CarUpdateDTO carUpdateDTO);
    }
}

