using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Use_Wheels.Data;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository.IRepository;

namespace Use_Wheels.Repository
{
	public class CarRepository : Repository<Car>, ICarRepository
	{
        private readonly ApplicationDbContext _db;

        public CarRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        // Method to update a car entity
        public async Task<Car> UpdateAsync(Car entity)
        {
            entity.Updated_Date = DateTime.Now;
            _db.Cars.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

