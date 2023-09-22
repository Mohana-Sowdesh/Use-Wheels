using System.Linq.Expressions;

namespace Use_Wheels.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // Method to get all data
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1);

        // Method to get a particular element from database using ID
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);

        // Method to create a new row
        Task CreateAsync(T entity);

        // Method to delete a row from DB
        Task RemoveAsync(T entity);

        // Method to save the changes done in DB
        Task SaveAsync();
    }
}

