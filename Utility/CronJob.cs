using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Use_Wheels.Utility
{
	public class CronJob : IJob
	{
        private ApplicationDbContext _db;
        public CronJob(ApplicationDbContext db)
        {
            _db = db;
        }

        // Method to run CRON job for
        public async Task Execute(IJobExecutionContext context)
        {
            LogoutUtility logoutUtility = new LogoutUtility();
            Log.Information("Job executed at " + DateTime.Now);
            IQueryable<User> userQuery = _db.Set<User>();
            var allUsers = await userQuery.ToListAsync();
            foreach (var user in allUsers)
            {
                if(user.Last_Login + TimeSpan.FromHours(24) < DateTime.Now)
                    logoutUtility.DeleteUserFromWishlist(user.UserName);
            }
        }
    }
}

