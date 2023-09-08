using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services.IServices
{
    public interface IUserCarServices
    {
        Task<IEnumerable<Car>> GetCars(int? categoryId, int pageSize = 0, int pageNumber = 1);
        Task<Car> GetCarById(string vehicle_no);
    }
}

