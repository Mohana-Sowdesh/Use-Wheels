using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Use_Wheels.Models.DTO
{
	public class Orders
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Order_ID { get; set; }

		[ForeignKey("User ID")]
		public int User_ID { get; set; }
		public UserDTO User;

		[ForeignKey("Vehicle ID")]
		public string Vehicle_No { get; set; }

		public float Net_Price { get; set; }

		public string Payment_Type { get; set; }
    }
}

