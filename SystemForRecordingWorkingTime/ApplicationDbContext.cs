using Microsoft.EntityFrameworkCore;
using SystemForRecordingWorkingTime.Models;

namespace SystemForRecordingWorkingTime
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<VacationRequest> VacationRequest { get; set; }
        public DbSet<DayOffAtTheExpenseOfVacationRequest> DayOffAtTheExpenseOfVacationRequest { get; set; }
        public DbSet<DayOffAtTheExpenseOfWorkingOutRequest> DayOffAtTheExpenseOfWorkingOutRequest { get; set; }
        public DbSet<DayOffRequest> DayOffRequest { get; set; }
        public DbSet<RemoteWorkRequest> RemoteWorkRequest { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
