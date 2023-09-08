using System;
namespace Use_Wheels.Utility
{
	public class UserRegisterUtility
	{
        private readonly ApplicationDbContext _db;

        public UserRegisterUtility(ApplicationDbContext db)
        {
            _db = db;
        }

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

