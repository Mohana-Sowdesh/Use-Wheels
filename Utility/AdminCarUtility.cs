using System;

namespace Use_Wheels.Utility
{
	public class AdminCarUtility
	{
		public int isVehicleNoSame(CarDTO car)
		{
			if (car.Vehicle_No == car.Rc_Details.Vehicle_No)
				return 1;
			return -1;
		}
	}
}

