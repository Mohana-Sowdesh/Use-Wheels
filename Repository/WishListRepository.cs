using System;
using System.Linq;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Repository
{
	public static class WishListRepository
	{
		public static Dictionary<string, List<Car>> wishlist = new Dictionary<string, List<Car>>();

		public static bool IsUserExists(string username)
		{
			return wishlist.ContainsKey(username);
		}

		public static List<Car>? GetUserWishlist(string username)
		{
			return wishlist.GetValueOrDefault(username);
		}

		public static void CreateNewList(string username)
		{
            wishlist[username] = new List<Car>();
        }

		public static void AddToList(string username, Car car)
		{
			wishlist[username].Add(car);
        }

		public static int DeleteFromList(string username, string vehicle_no)
		{
            List<Car> userWishlist = WishListRepository.GetUserWishlist(username);
            var vehicle = userWishlist.FirstOrDefault(u => u.Vehicle_No == vehicle_no);

			if (vehicle == null)
				return -1;
			else
				userWishlist.Remove(vehicle);
			return 1;
        }

		public static bool DeleteUser(string username)
		{
            return wishlist.Remove(username);
		}
    }
}

