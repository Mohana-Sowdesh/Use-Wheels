using System;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Repository.IRepository
{
	public interface ICarRepository : IRepository<Car>
	{
		// Method to update car entity
		Task<Car> UpdateAsync(Car entity);
	}
}

