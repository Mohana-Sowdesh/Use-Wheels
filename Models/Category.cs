using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Use_Wheels.Models.DTO
{
	public class Category
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Category_Id { get; set; }

        [Required]
        [RegularExpression("a-zA-Z\\s")]
        public string Category_Names { get; set; }
    }
}

