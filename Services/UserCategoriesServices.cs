namespace Use_Wheels.Services
{
	public class UserCategoriesServices : IUserCategoriesServices
	{
        private ICategoryRepository _dbCategory;

        public UserCategoriesServices(ICategoryRepository dbCategory)
		{
            _dbCategory = dbCategory;
        }

        /// <summary>
        /// Service method to get all categories from DB
        /// </summary>
        /// <returns>List of <see cref="Category"/>categories</returns>
        public async Task<IEnumerable<Category>> GetCategories()
        {
            IEnumerable<Category> categoryList = await _dbCategory.GetAllAsync();
            return categoryList;
        }
    }
}

