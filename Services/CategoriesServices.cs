using AutoMapper;

namespace Use_Wheels.Services
{
    public class CategoriesServices : ICategoriesServices
    {
        private ICategoryRepository _dbCategory;
        private readonly IMapper _mapper;

        public CategoriesServices(ICategoryRepository dbCategory, IMapper mapper)
        {
            _mapper = mapper;
            _dbCategory = dbCategory;
        }

        /// <summary>
        /// Service method to create a new category
        /// </summary>
        /// <param name="categoryDTO"><see cref="CategoryDTO"/></param>
        /// <returns><see cref="Category"/>Category object</returns>
        public async Task<Category> CreateCategory(CategoryDTO categoryDTO)
        {
            if (await _dbCategory.GetAsync(u => u.Category_Names.ToLower() == categoryDTO.Category_Names.ToLower()) != null)
                throw new BadHttpRequestException(Constants.CategoryConstants.CATEGORY_ALREADY_EXISTS, Constants.ResponseConstants.BAD_REQUEST);

            Category category = _mapper.Map<Category>(categoryDTO);
            await _dbCategory.CreateAsync(category);
            return category;
        }

        /// <summary>
        /// Service method to delete a category from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteCategory(int id)
        {
            if (id <= 0)
                throw new BadHttpRequestException(Constants.CategoryConstants.ID_VALIDATION, Constants.ResponseConstants.BAD_REQUEST);

            Category category = await _dbCategory.GetAsync(u => u.Category_Id == id);

            if (category == null)
                throw new BadHttpRequestException(Constants.CategoryConstants.CATEGORY_NOT_FOUND, Constants.ResponseConstants.NOT_FOUND);

            await _dbCategory.RemoveAsync(category);
        }

        /// <summary>
        /// Service method to get all categories from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
           IEnumerable<Category> categoryList = await _dbCategory.GetAllAsync();
           return categoryList;
        }
    }
}

