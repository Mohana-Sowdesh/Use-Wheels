using StackExchange.Redis;

namespace Use_Wheels.Utility
{
	public class LogoutUtility
	{
        // Method to invalidate token
        public async Task InvalidateToken(string jwtToken)
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.StringSetAndGetAsync(jwtToken, new RedisValue("1"), new TimeSpan(24, 0, 0));
        }

        // Method to delete the user data from wishlist dictionary
        public void DeleteUserFromWishlist(string username)
        {
            bool isUserExists = WishListRepository.IsUserExists(username);

            if (isUserExists)
                WishListRepository.DeleteUser(username);
        }
    }
}

