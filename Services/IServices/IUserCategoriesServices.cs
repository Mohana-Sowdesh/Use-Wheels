using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services.IServices
{
	public interface IUserCategoriesServices
	{
        // Method to get all categories from DB
        Task<IEnumerable<Category>> GetCategories();
    }
}

