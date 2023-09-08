using System;
using System.Linq;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Repository
{
	public static class WishListRepository
	{
		// A dictionary 'wishlist' which consists username as key and list of cars - 'wishlist' as value
		private static Dictionary<string, List<Car>> wishlist = new Dictionary<string, List<Car>>();

		// Method to check if the username exists as a key in wishlist dictionary
		public static bool IsUserExists(string username)
		{
			return wishlist.ContainsKey(username);
		}

        // Method to get the wishlist of a particular user from wishlist dictionary
        public static List<Car>? GetUserWishlist(string username)
		{
			return wishlist.GetValueOrDefault(username);
		}

        // Method to initialize a new wishlist for a new incoming user in wishlist dictionary
        public static void CreateNewList(string username)
		{
            wishlist[username] = new List<Car>();
        }

		// Method to add a car to user's wishlist
		public static void AddToList(string username, Car car)
		{
			wishlist[username].Add(car);
        }

		// Method to delete a car from user's wishlist
		public static int DeleteFromList(string username, string vehicle_no)
		{
            List<Car> userWishlist = WishListRepository.GetUserWishlist(username);

			if (userWishlist == null)
				throw new BadHttpRequestException("No cars to delete in wishlist", 404);

            var vehicle = userWishlist.Find(u => u.Vehicle_No == vehicle_no);

			if (vehicle == null)
				return -1;
			else
				userWishlist.Remove(vehicle);
			return 1;
        }

		// Method to remove a user from wishlist dictionary
		public static bool DeleteUser(string username)
		{
            return wishlist.Remove(username);
		}
    }
}

