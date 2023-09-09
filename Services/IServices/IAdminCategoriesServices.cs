using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Services.IServices
{
	public interface IAdminCategoriesServices
	{
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> CreateCategory(CategoryDTO categoryDTO);
        Task DeleteCategory(int id);
    }
}

