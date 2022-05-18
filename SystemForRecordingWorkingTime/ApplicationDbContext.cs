using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
        }
        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    d => DateOnly.FromDateTime(d))
            { }
        }
    }
}
