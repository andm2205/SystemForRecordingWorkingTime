using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public struct MonthYear
    {
        public MonthYear() { }
        public MonthYear(int month, int year) 
        { 
            Month = month;
            Year = year;
        }
        public Int32 Month { get; set; } = DateTime.Now.Month;
        public Int32 Year { get; set; } = DateTime.Now.Year;
    }
}
