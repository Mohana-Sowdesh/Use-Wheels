using System;
using System.Web.Http.ModelBinding;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Services.IServices;
using static StackExchange.Redis.Role;

namespace Use_Wheels.Services
{
    public class AdminCategoriesServices : IAdminCategoriesServices
    {
        private ICategoryRepository _dbCategory;
        private readonly IMapper _mapper;

        public AdminCategoriesServices(ICategoryRepository dbCategory, IMapper mapper)
        {
            _mapper = mapper;
            _dbCategory = dbCategory;
        }

        public async Task<Category> CreateCategory(CategoryDTO categoryDTO)
        {
            if (await _dbCategory.GetAsync(u => u.Category_Names.ToLower() == categoryDTO.Category_Names.ToLower()) != null)
                throw new BadHttpRequestException("Category already exists!!", 400);

            Category category = _mapper.Map<Category>(categoryDTO);
            await _dbCategory.CreateAsync(category);
            return category;
        }

        public async Task DeleteCategory(int id)
        {
            if (id <= 0)
                throw new BadHttpRequestException("ID cannot be lesser than or equal to 0", 400);

            Category category = await _dbCategory.GetAsync(u => u.Category_Id == id);

            if (category == null)
                throw new BadHttpRequestException("Requested category is not found", 404);

            await _dbCategory.RemoveAsync(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
           IEnumerable<Category> categoryList = await _dbCategory.GetAllAsync();
           return categoryList;
        }
    }
}

