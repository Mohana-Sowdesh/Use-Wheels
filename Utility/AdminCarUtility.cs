using System.Text.Json;
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

        // Method to check if the incoming new vehicle request from the seller is malicious or not.
        public int CheckIfVehicleIsIllegal(List<VehicleInfoDTO> blackedCars, string vehicleNo)
        {
            string vehicleNumWithoutSpace = vehicleNo.Replace(" ", "");
            VehicleInfoDTO foundVehicle = blackedCars.FirstOrDefault(u => u.vehichleNo == vehicleNumWithoutSpace);

            if (foundVehicle == null)
                return 0;
            else if (foundVehicle.isMissing == true)
                return 1;
            else if (foundVehicle.isTrial == true)
                return 2;
            return -1;
        }
    }
}

