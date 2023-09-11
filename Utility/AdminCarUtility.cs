using System;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace Use_Wheels.Utility
{
	public class AdminCarUtility
	{
		// Method to check if both the vehicle numbers are same in the incoming request
		public int isVehicleNoSame(CarDTO car)
		{
			if (car.Vehicle_No == car.Rc_Details.Vehicle_No)
				return 1;
			return -1;
		}

		// Method to check if the request vehicle_no is valid or not
		public int isVehicleNoValid(string vehicle_no)
		{
            string pattern = "^[A-Z]{2}\\s\\d{2}\\s[A-Z]{2}\\s\\d{4}$";
            Match match = Regex.Match(vehicle_no, pattern);

			if (match.Success)
				return 1;
			else
				return 0;
        }
	}
}

