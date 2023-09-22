using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services.IServices
{
	public interface ICategoriesServices
	{
        // Method to get all categories from DB
        Task<IEnumerable<Category>> GetAllCategories();

        // Method to insert a new category into DB
        Task<Category> CreateCategory(CategoryDTO categoryDTO);

        // Method to delete a category from DB
        Task DeleteCategory(int id);
    }
}

