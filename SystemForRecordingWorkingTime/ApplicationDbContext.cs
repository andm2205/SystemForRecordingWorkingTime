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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
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
            modelBuilder.Entity<User>().HasData(new User 
            { 
                Id = -1, 
                Email = "admin@admin.ru", 
                Password = "nHLrqFHC8pr3cz6CdnzT",
                RoleValue = User.Role.Administrator
            });

            modelBuilder.Entity<VacationRequest>();
            modelBuilder.Entity<DayOffAtTheExpenseOfVacationRequest>();
            modelBuilder.Entity<DayOffAtTheExpenseOfWorkingOutRequest>();
            modelBuilder.Entity<DayOffRequest>();

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Request>()
                .HasOne(a => a.ApprovingUser)
                .WithMany(a => a.ApprovingRequests)
                .HasForeignKey(a => a.ApprovingUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReplaceableRequest>()
                .HasOne(a => a.ReplacementUser)
                .WithMany()
                .HasForeignKey(a => a.ReplacementUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<SystemForRecordingWorkingTime.Models.DayOffAtTheExpenseOfVacationRequest>? DayOffAtTheExpenseOfVacationRequest { get; set; }
        public DbSet<SystemForRecordingWorkingTime.Models.DayOffAtTheExpenseOfWorkingOutRequest>? DayOffAtTheExpenseOfWorkingOutRequest { get; set; }
        public DbSet<SystemForRecordingWorkingTime.Models.DayOffRequest>? DayOffRequest { get; set; }
        public DbSet<SystemForRecordingWorkingTime.Models.RemoteWorkRequest>? RemoteWorkRequest { get; set; }
    }
}
