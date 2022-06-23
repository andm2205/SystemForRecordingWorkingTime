using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SystemForRecordingWorkingTime.Models;

namespace SystemForRecordingWorkingTime
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<StatedDate> StatedDates { get; set; }
        public DbSet<WorkingOutDate> WorkingOutDates { get; set; }
        public DbSet<WorkPlan> WorkPlans { get; set; }
        public DbSet<ProductionCalendar> Calendars { get; set; }
        public DbSet<Day> CalendarDays { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            /*Database.EnsureDeleted();*/
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User[] users = new User[]
            {
                new User
                {
                    Id = -1,
                    Email = "admin@app.ru",
                    Password = "Ca4NCtwT5QAXPNFZ",
                    Role = UserRole.Administrator
                }
#if DEBUG
                ,new User
                {
                    Id = -2,
                    Email = "employee@app.ru",
                    Password = "pnMv6JJLc7UsvEqP",
                    Role = UserRole.Employee
                }
                ,new User
                {
                    Id = -3,
                    Email = "director@app.ru",
                    Password = "L4fptsZVT7snLqbG",
                    Role = UserRole.Director
                }
                ,new User
                {
                    Id = -4,
                    Email = "supervisor@app.ru",
                    Password = "SuGFAd2Kv5JTWW8k",
                    Role = UserRole.Supervisor
                }
#endif
            };

            modelBuilder.Entity<User>().HasData(users);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Request>()
                .HasOne(a => a.ApprovingUser)
                .WithMany(a => a.ApprovingRequests)
                .HasForeignKey(a => a.ApprovingUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReplacebleRequest>()
                .HasOne(a => a.ReplacementUser)
                .WithMany()
                .HasForeignKey(a => a.ReplacementUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkingOutDate>()
                .HasKey(a => new { a.Value, a.DayOffAtTheExpenseOfWorkingOutRequestId });

            modelBuilder.Entity<StatedDate>()
                .HasKey(a => new { a.Value, a.RequestId });

            modelBuilder.Entity<Request>()
                .HasDiscriminator(b => b.Discriminator);
        }
        public DbSet<SystemForRecordingWorkingTime.Models.DayOffAtTheExpenseOfVacationRequest>? DayOffAtTheExpenseOfVacationRequest { get; set; }
        public DbSet<SystemForRecordingWorkingTime.Models.DayOffAtTheExpenseOfWorkingOutRequest>? DayOffAtTheExpenseOfWorkingOutRequest { get; set; }
        public DbSet<SystemForRecordingWorkingTime.Models.DayOffRequest>? DayOffRequest { get; set; }
        public DbSet<SystemForRecordingWorkingTime.Models.RemoteWorkRequest>? RemoteWorkRequest { get; set; }
        public DbSet<SystemForRecordingWorkingTime.Models.VacationRequest>? VacationRequest { get; set; }
    }
}
