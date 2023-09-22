using System;

namespace Use_Wheels.Models.DTO
{
	public class CarDTO
	{
        [Required]
        [RegularExpression("^[A-Z]{2}\\s\\d{2}\\s[A-Z]{2}\\s\\d{4}$", ErrorMessage = "Please enter a valid vehicle number. Eg: AB 78 BN 8909")]
        public string Vehicle_No { get; set; }

        [Required]
        [RegularExpression("^[1-9]\\d*$", ErrorMessage = "Please enter a valid category ID")]
        public int Category_Id { get; set; }

        public string Description { get; set; }

        [Required]
        [RegularExpression("[1-9]", ErrorMessage = "Pre owner count must be 1 to 9")]
        public int Pre_Owner_Count { get; set; }

        [Required]
        public string Img_URL { get; set; }

        [Required]
        [RegularExpression("^[1-9]\\d*$", ErrorMessage = "Please enter a valid price")]
        public float Price { get; set; }

        public DateTime Created_Date { get; set; } = DateTime.Now;

        public DateTime Updated_Date { get; set; } = DateTime.Now;

        public string? TrialedCar { get; set; }

        [ForeignKey("Rc_Details")]
        public string RC_No { get; set; }
        public RC Rc_Details { get; set; }
    }
}

