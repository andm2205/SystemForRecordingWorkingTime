using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public class ProductionCalendar
    {
        public ProductionCalendar() { }
        public ProductionCalendar(CreateProductionCalendar data) 
        {
            this.Status = ProductionCalendarStatus.Created;
            this.Month = data.Month;
            this.Year = data.Year;
            this.Days = data.Days;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public ProductionCalendarStatus Status { get; set; }
        public Int32 Month { get; set; }
        public Int32 Year { get; set; }
        public IList<Day> Days { get; set; }

    }
    public class CreateProductionCalendar
    {
        public CreateProductionCalendar() { }
        public CreateProductionCalendar(Int32 month, Int32 year)
        {
            Month = month;
            Year = year;
            Days = 
                Enumerable
                .Range(1, DateTime.DaysInMonth(year, month))
                .Select(day => new Day(new DateOnly(year, month, day)))
                .ToList();
        }
        public Int32 Month { get; set; }
        public Int32 Year { get; set; }
        public IList<Day> Days { get; set; }
    }
    public class Day
    {
        public static DayOfWeek[] Weekends = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
        public static DayOfWeek[] Weekdays = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        public static Boolean IsWeekend(DateOnly date)
        {
            return Weekends.Contains(date.DayOfWeek);
        }
        public static Boolean IsWeekday(DateOnly date)
        {
            return Weekdays.Contains(date.DayOfWeek);
        }
        public static DayType GetDayType(DateOnly date)
        {
            return IsWeekend(date) ? DayType.Weekend : DayType.Weekday;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Int32 Id { get; set; }
        public Int32 ProductionCalendarId { get; set; }
        public ProductionCalendar ProductionCalendar { get; set; }
        public DateOnly Date { get; set; }
        [NotMapped]
        public String DateString
        {
            get => Date.ToString("yyyy-MM-dd");
            set => Date = DateOnly.Parse(value);
        }
        public DayType Type { get; set; }
        public Day() { }
        public Day(DateOnly date)
        {
            this.Date = date;
            this.Type = GetDayType(date);
        }
    }
    public enum DayType
    {
        Weekday,
        Weekend,
        Holiday
    }
    public enum ProductionCalendarStatus
    {
        Created,
        Activated
    }
}
