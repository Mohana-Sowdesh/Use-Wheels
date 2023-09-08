using System;

namespace Use_Wheels.Models.DTO
{
	public class RC
	{
		[Key]
        [RegularExpression("[0-9]{6}", ErrorMessage = "RC number must be of 6 digits")]
        public string RC_No { get; set; }

        [RegularExpression("^[A-Z]{2}\\s\\d{2}\\s[A-Z]{2}\\s\\d{4}$", ErrorMessage = "Please enter a valid vehicle number. Eg: AB 78 BN 8909")]
        public required string Vehicle_No { get; set; }
        
        [Required]
        //[RegularExpression("^\\d{4}$-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Please enter a valid date")]
        public DateOnly Date_Of_Reg { get; set; }

		[Required]
        //[RegularExpression("^\\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Please enter a valid date")]
        public DateOnly Reg_Valid_Upto { get; set; }

		[Required]
        [RegularExpression("^[a-zA-Z]+\\s*$", ErrorMessage = "Owner name must contain only letters")]
        public string Owner_Name { get; set; }

        [Required]
        //[RegularExpression("^[a-zA-Z]+\\s*$", ErrorMessage = "Address must contain only letters")]
        public string Owner_Address { get; set; }

        [Required]
        //[RegularExpression("^[A-Za-z]+\\s?-?[A-Za-z]*$", ErrorMessage = "Please enter a valid board type")]
        public string Board_Type { get; set; }

        [Required]
        //[RegularExpression("^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Please enter a valid date")]
        public DateOnly FC_Validity { get; set; }

		[Required]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Please enter a valid insurance type")]
        public string Insurance_Type { get; set; }

		[Required]
        [RegularExpression("^[A-Za-z\\s]+?[0-9]*$", ErrorMessage = "Please enter a valid car model")]
        public string Car_Model { get; set; }

        [Required]
        [RegularExpression("^(19\\d{2}|2\\d{3})$", ErrorMessage = "Please enter a valid manufactured year. Manufactured year must not be before 1900")]
        public int Manufactured_Year { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Please enter a valid fuel type")]
        public string Fuel_Type { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Please enter a valid colour")]
        public string Colour { get; set; }

        public DateTime Created_Date { get; set; } = DateTime.Now;

        public DateTime Updated_Date { get; set; } = DateTime.Now;
    }
}

