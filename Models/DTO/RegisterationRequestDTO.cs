using System;
namespace Use_Wheels.Models.DTO
{
	public class RegisterationRequestDTO
	{
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public DateOnly Dob { get; set; }
        public string Phone_Number { get; set; }
        public string Gender { get; set; }
    }
}

