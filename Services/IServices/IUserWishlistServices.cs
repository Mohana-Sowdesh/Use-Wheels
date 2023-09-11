namespace Use_Wheels.Services.IServices
{
	public interface IUserWishlistServices
	{
        //Method to get a wishlist of a user
        IEnumerable<Car> GetWishlist(string username);

        //Method to add a car to wishlist of that user
        Task AddToWishlist(string vehicle_no, string username);

        //Method to delete a car from wishlist of a particular user
        void DeleteElementFromWishList(string vehicle_no, string username);
    }
}

