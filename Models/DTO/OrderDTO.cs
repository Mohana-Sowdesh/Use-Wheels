using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Use_Wheels.Models.DTO
{
	public class OrderDTO
	{
        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^[A-Z]{2}\\s\\d{2}\\s[A-Z]{2}\\s\\d{4}$", ErrorMessage = "Please enter a valid vehicle number. Eg: AB 78 BN 8909")]
        public string Vehicle_No { get; set; }

        [Required]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Please enter a valid payment type")]
        public string Payment_Type { get; set; }
    }
}

