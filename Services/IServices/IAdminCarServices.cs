using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services.IServices
{
	public interface IAdminCarServices
	{
        // Method to get all cars from DB
        Task<IEnumerable<Car>> GetAllCars();

        // Method to get 1 particular car from DB by ID 
        Task<Car> GetCarById(string vehicle_no);

        // Method to add a new car to DB
        Task<Car> AddCar(CarDTO carDTO);

        // Method to delete a car from DB
        Task DeleteCar(string vehicle_no);

        // Method to update a car in DB
        Task UpdateCar(string vehicle_no, CarUpdateDTO carUpdateDTO);
    }
}

