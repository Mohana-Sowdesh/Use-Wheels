namespace Use_Wheels.Utility.IUtilities
{
	public interface IMockAPIUtility
	{
        Task<List<VehicleInfoDTO>> GetMockDataAsync();
    }
}

