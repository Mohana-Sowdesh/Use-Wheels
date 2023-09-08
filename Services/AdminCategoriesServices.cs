using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Services.IServices;

namespace Use_Wheels.Services
{
    public class AdminCategoriesServices : IAdminCategoriesServices
    {
        private ICategoryRepository _dbCategory;

        public AdminCategoriesServices(ICategoryRepository dbCategory)
        {
            _dbCategory = dbCategory;
        }

        public async Task CreateCategory(Category category)
        {
            await _dbCategory.CreateAsync(category);
        }

        public async Task DeleteCategory(Category category)
        {
            await _dbCategory.RemoveAsync(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
           IEnumerable<Category> categoryList = await _dbCategory.GetAllAsync();
           return categoryList;
        }
    }
}

