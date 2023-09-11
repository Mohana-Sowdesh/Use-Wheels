using System;
namespace Use_Wheels.Utility
{
	public static class Constants
	{
		public static class Configurations
		{
			public const string CACHE_PROFILE_NAME = "Default30";
			public const string JWT_SECRET_CONFIGURATION_KEY = "ApiSettings:Secret";
			public const string SQL_CONFIGURATION_KEY = "SQLConnection";
			public const string REDIS_CONNECTION_KEY = "localhost:6379";
			public const string TOKEN_PROVIDER_NAME = "Demo";
		}

		public static class Swagger
		{
			public const string JWT_SECURITY_DESCRIPTION = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.";
        }

		public static class Authorization
		{
			public const string INVALID_SCOPE_AUTHORIZATION_FAILED = "Authorization failed - Invalid Scope";
			public const string AUTHORIZATION_FAILED = "Authorization failed";
			public const string UNAUTHORIZED = "Unauthorized";
			public const string JWT_BLACKLISTED_TOKEN_VALUE = "1";
			public const string JWT_TOKEN_ISSUER = "JwtToken:Issuer";
			public const string JWT_TOKEN_AUDIENCE = "JwtToken:Audience";
        }

		public static class Roles
		{
			public const string ADMIN = "admin";
			public const string CUSTOMER = "customer";
		}

		public static class Pagination
		{
			public const string PAGINATION_KEY = "X-Pagination";
		}

		public static class LogoutConstants
		{
			public const string LOGOUT_SUCCESSFUL = "Logout successful";

        }

		public static class OrderConstants
		{
			public const string ORDER_SUCCESSFUL = "Order placed successfully";
			public const string EMAIL_DOES_NOT_EXISTS = "Email does not exist";
			public const string AVAILABLE = "available";
			public const string SOLD = "sold";
        }

		public static class LoginConstants
		{
			public const string INVALID_CREDENTIALS = "Credentials are incorrect";
			public const string INVALID_AGE = "Error while registering - User must above or equal to 18 years of age";
        }

		public static class WishlistConstants
		{
			public const string NO_CARS_PRESENT = "No cars present in wishlist";
			public const string ADD_CAR_SUCCESS = "Car added to wish-list successfully!!";
			public const string DELETE_CAR_SUCCESS = "Car deleted successfully from wishlist!!";
			public const string CAR_ALREADY_PRESENT = "Car already present in wishlist";
			public const string VEHICLE_NOT_FOUND = "Requested car not found";
        }

		public static class CarConstants
		{
			public const string CAR_ALREADY_EXISTS = "Car already exists!!";
			public const string DTO_NULL = "Request cannot be null";
			public const string VEHICLE_NUM_NOT_MATCH = "Vehicle no. in the requests doesn't match";
			public const string VEHICLE_NOT_FOUND = "Uh-oh, the requested vehicle is not found";
			public const string NO_CARS_PRESENT = "Sorry!! No cars are present!!";
			public const string INVALID_VEHICLE_NUM = "Please enter a valid vehicle number";
        }

		public static class CategoryConstants
		{
			public const string CATEGORY_ALREADY_EXISTS = "Category already exists!!";
			public const string ID_VALIDATION = "ID cannot be lesser than or equal to 0";
			public const string CATEGORY_NOT_FOUND = "Requested category is not found";
        }

        public static class ResponseConstants
		{
			public const int BAD_REQUEST = 400;
			public const int NOT_FOUND = 404;
			public const int UNAUTHORIZED = 401;
		}

    }
}

