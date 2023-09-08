using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Use_Wheels.Models.DTO;
using Use_Wheels.Repository;
using Use_Wheels.Repository.IRepository;
using Use_Wheels.Services.IServices;

namespace Use_Wheels.Services
{
	public class UserWishlistServices : IUserWishlistServices
    {
        private ICarRepository _dbCar;

        public UserWishlistServices(ICarRepository dbCar)
		{
            _dbCar = dbCar;
		}

        /// <summary>
        /// Service method to add a car to wishlist of that user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="car"><see cref="Car"/></param>
        /// <returns></returns>
        public async Task AddToWishlist(string username, Car car)
        {
            WishListRepository.AddToList(username, car);
            car.Likes = car.Likes + 1;
            await _dbCar.UpdateAsync(car);
        }

        /// <summary>
        /// Service method to delete a car from wishlist of a particular user
        /// </summary>
        /// <param name="vehicle_no"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task DeleteElementFromWishList(string vehicle_no, string username)
        {
            if (vehicle_no == null)
                throw new BadHttpRequestException("Vehicle no. is mandatory", 400);

            int deleteResult = WishListRepository.DeleteFromList(username, vehicle_no);

            if (deleteResult == -1)
                throw new BadHttpRequestException("Vehicle not found", 404);

            Log.Information("Condition result: {@Result}", deleteResult);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Service method to get a wishlist of a user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Wishlist of that particular user</returns>
        public IEnumerable<Car> GetWishlist(string username)
        {
            IEnumerable<Car> userWishlist = WishListRepository.GetUserWishlist(username);
            return userWishlist;
        }
    }
}

