using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Use_Wheels.Models.DTO
{
	public class CarDTO
	{
        public string Vehicle_No { get; set; }

        public int Category_Id { get; set; }

        public string Description { get; set; }

        public int Pre_Owner_Count { get; set; }

        public string Img_URL { get; set; }

        public float Price { get; set; }

    }
}

