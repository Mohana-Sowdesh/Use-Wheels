using System;
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
    }
}

