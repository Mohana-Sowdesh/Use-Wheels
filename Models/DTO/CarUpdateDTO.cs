using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Use_Wheels.Models.DTO
{
	public class CarUpdateDTO
	{
        [Required]
        [RegularExpression("^[A-Z]{2}\\s\\d{2}\\s[A-Z]{2}\\s\\d{4}$")]
        public string Vehicle_No { get; set; }

        [Required]
        [RegularExpression("^[1-9]\\d*$")]
        public int Category_Id { get; set; }

        public string Description { get; set; }

        public bool Availability { get; set; }

        [Required]
        [RegularExpression("[1-9]")]
        public int Pre_Owner_Count { get; set; }

        [Required]
        public string Img_URL { get; set; }

        [Required]
        [RegularExpression("^[1-9]\\d*$")]
        public float Price { get; set; }

        public DateTime Updated_Date { get; set; } = DateTime.Now;

        [ForeignKey("Rc_Details")]
        public string RC_No { get; set; }
        public RC Rc_Details { get; set; }
    }
}

