using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Use_Wheels.Models.DTO
{
	public class UserDTO : IdentityUser
    {
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Username can only contain letters and numbers.")]
        public override string UserName { get; set; }

        public string? Role { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z]+")]
        public string First_Name { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z]+")]
        public string Last_Name { get; set; }

        [Required]
        //[RegularExpression("^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/\\d{4}$")]
        public DateOnly Dob { get; set; }

        [Required]
        [RegularExpression("^\\d{10}$")]
        public string Phone_Number { get; set; }

        [Required]
        [RegularExpression("a-zA-Z")]
        public string Gender { get; set; }
    }
}

