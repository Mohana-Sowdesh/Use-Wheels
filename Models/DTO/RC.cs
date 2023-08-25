using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Use_Wheels.Models.DTO
{
	public class RC
	{
		[Key]
        [RegularExpression("0-9{6}")]
        public string RC_No { get; set; }

		[Required]
		[ForeignKey("Vehicle No")]
		public string Vehicle_No;

		[Required]
		public DateOnly Date_Of_Reg { get; set; }

		[Required]
		public DateOnly Reg_Valid_Upto { get; set; }

		[Required]
        [RegularExpression("a-zA-Z\\s")]
        public string Owner_Name { get; set; }

        [Required]
        [RegularExpression("a-zA-Z0-9\\s")]
        public string Owner_Address { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z-]+$")]
        public string Board_Type { get; set; }

        [Required]
        public DateOnly FC_Validity { get; set; }

		[Required]
        [RegularExpression("^[a-zA-Z\\s]+$")]
        public string Insurance_Type { get; set; }

		[Required]
        [RegularExpression("^[a-zA-Z0-9]+$")]
        public string Car_Model { get; set; }

        [Required]
        [RegularExpression("^(19\\d{2}|2\\d{3})$")]
        public int Manufactured_Year { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z\\s]+$")]
        public string Fuel_Type { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z\\s]+$")]
        public string Colour { get; set; }

        public DateTime Created_Date { get; set; }

        public DateTime Updated_Date { get; set; }
    }
}

