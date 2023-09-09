using System;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Utility
{
	public class RegisterUtility
	{
        private readonly ApplicationDbContext _db;

        public RegisterUtility(ApplicationDbContext db)
        {
            _db = db;
        }

        // Method that validates the new registeration request
        public void ValidateNewRegisterationRequest(RegisterationRequestDTO registerationRequestDTO)
        {
            int ifUserNameUnique = IsUniqueUser(registerationRequestDTO.Username, registerationRequestDTO.Email);
            if (ifUserNameUnique == -1)
                throw new BadHttpRequestException("Username already exists", 400);
            else if (ifUserNameUnique == 0)
                throw new BadHttpRequestException("Email already exists", 400);

            bool isAbove18 = IsAbove18(registerationRequestDTO.Dob);
            if (!isAbove18)
                throw new BadHttpRequestException("Age must be above 18", 400);
        }

        // Method to check if new user registeration request's username & email are unique or not
        public int IsUniqueUser(string username, string email)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == username);
            var userEmail = _db.Users.FirstOrDefault(x => x.Email == email);

            if (user != null)
            {
                Log.Error("Requested username already exists");
                return -1;
            }
            else if (userEmail != null)
            {
                Log.Error("Requested email already exists");
                return 0;
            }
            return 1;
        }

        // Method to check if the age of new user is above 18 or not
        public bool IsAbove18(DateOnly Dob)
        {
            int birthYear = Dob.Year;
            int currentYear = DateTime.Today.Year;

            if (currentYear - birthYear < 18)
            {
                Log.Error("User age is not above or equal to 18");
                return false;
            }
            return true;
        }
    }
}

