using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Use_Wheels.Models.DTO
{
	public class Car
	{
        [Key]
        [RegularExpression("^[A-Z]{2}\\s\\d{2}\\s[A-Z]{2}\\s\\d{4}$")]
        public string Vehicle_No { get; set; }

        [Required]
        [RegularExpression("0-9")]
        [ForeignKey("Category")]
        public int Category_Id { get; set; }
        //Navigation property
        public Category Category { get; set; }

        [Required]
        public string Description { get; set; }

        public string Availability { get; set; } = "available";

        [Required]
        public int Pre_Owner_Count { get; set; }

        [Required]
        public string Img_URL { get; set; }

        [Required]
        public float Price { get; set; }

        public int Likes { get; set; } = 0;

        public DateTime Created_Date { get; set; }

        public DateTime Updated_Date { get; set; }
    }
}

