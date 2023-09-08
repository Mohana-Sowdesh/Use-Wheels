using System;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Services.IServices;

namespace Use_Wheels.Services
{
	public class UserCategoriesServices : IUserCategoriesServices
	{
        private ICategoryRepository _dbCategory;

        public UserCategoriesServices(ICategoryRepository dbCategory)
		{
            _dbCategory = dbCategory;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            IEnumerable<Category> categoryList = await _dbCategory.GetAllAsync();
            return categoryList;
        }
    }
}

