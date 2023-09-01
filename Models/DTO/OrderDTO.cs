using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Use_Wheels.Models.DTO
{
	public class OrderDTO
	{
        [Required]
        public string Email { get; set; }

        [Required]
        public string Vehicle_No { get; set; }

        [Required]
        public float Net_Price { get; set; }

        [Required]
        public string Payment_Type { get; set; }
    }
}

