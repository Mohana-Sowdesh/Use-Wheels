using System;
using Microsoft.Extensions.Caching.Memory;

namespace Use_Wheels.Utility
{
	public class MockAPIUtility : IMockAPIUtility
	{
		private readonly IMemoryCache _memoryCache;
		private const string MOCK_API_DATA_KEY = "Missing and trialed cars";

		public MockAPIUtility(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

        // Method to get blacklisted cars mock API data
        public async Task<List<VehicleInfoDTO>> GetMockDataAsync()
		{
            var data = _memoryCache.Get<List<VehicleInfoDTO>>(MOCK_API_DATA_KEY);
            _memoryCache.Remove(MOCK_API_DATA_KEY);
            if (data == null)
            {
                var client = new HttpClient();
                data = await client.GetFromJsonAsync<List<VehicleInfoDTO>>(Constants.Utility.MOCK_API_URL) ?? new();
                _memoryCache.Set(MOCK_API_DATA_KEY, data, TimeSpan.FromSeconds(Constants.Utility.MISSING_TRIALED_CARS_CACHE_EXPIRATION));
            }
            return data;
        }

    }
}

