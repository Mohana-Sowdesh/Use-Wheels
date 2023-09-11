using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services.IServices
{
    public interface IUserCarServices
    {
        // Method to get all the cars from DB
        Task<IEnumerable<Car>> GetCars(int? categoryId, int pageSize = 0, int pageNumber = 1);

        // Method to get a particular car by ID from DB
        Task<Car> GetCarById(string vehicle_no);
    }
}

