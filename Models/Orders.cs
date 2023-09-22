namespace Use_Wheels.Models.DTO
{
	public class Orders
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Order_ID { get; set; }

		[ForeignKey("Email")]
		public string Email { get; set; }
		public User User { get; set; }

		[ForeignKey("Vehicle ID")]
		public string Vehicle_No { get; set; }

		public float Gross_Price { get; set; }

		public float Net_Price { get; set; }

		public string Payment_Type { get; set; }
    }
}

