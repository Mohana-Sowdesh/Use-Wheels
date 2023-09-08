using System;
using System.ComponentModel.DataAnnotations;

namespace Use_Wheels.Models.DTO
{
	public class RegisterationRequestDTO
	{
        [RegularExpression("^[A-Za-z0-9_]+$", ErrorMessage = "Username can only contain letters and numbers.")]
        public required string Username { get; set; }

        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=[\\]{}|;:',.<>?])(?=.*\\d).{6,}$", ErrorMessage = "Password must contain atleast 6 characters and must contain atleast 1 uppercase letter, 1 lowercase letter, 1 special character & 1 digit")]
        public required string Password { get; set; }

        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address")]
        public required string Email { get; set; }

        [RegularExpression("[a-zA-Z]+", ErrorMessage = "First name must contain only alphabets")]
        public required string First_Name { get; set; }

        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Last name must contain only alphabets")]
        public required string Last_Name { get; set; }

        //[RegularExpression("^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/\\d{4}$", ErrorMessage = "Please enter a valid date")]
        public required DateOnly Dob { get; set; }

        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid phone number")]
        public required string Phone_Number { get; set; }

        [RegularExpression("^(male|female|Male|Female)$", ErrorMessage = "Gender should be male or female")]
        public required string Gender { get; set; }
    }
}

