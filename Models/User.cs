using System;
using Microsoft.AspNetCore.Identity;

namespace Use_Wheels.Models
{
	public class User : IdentityUser
    {
        public string? Role { get; set; } = "customer";

        [Required]
        [RegularExpression("[a-zA-Z]+")]
        public string First_Name { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z]+")]
        public string Last_Name { get; set; }

        [Required]
        public DateOnly Dob { get; set; }

        [Required]
        [RegularExpression("^\\d{10}$")]
        public string Phone_Number { get; set; }

        [Required]
        [RegularExpression("a-zA-Z")]
        public string Gender { get; set; }
    }
}

