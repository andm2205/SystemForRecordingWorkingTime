using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class WorkingSchedule
    {
        public WorkingSchedule(Request[] requests, User[] users, Int32 month, Int32 year)
        {
            Requests = requests;
            Users = users;
            Month = month;
            Year = year;
        }
        public Request[] Requests { get; set; }
        public Int32 Month { get; set; }
        public Int32 Year { get; set; }
        public User[] Users { get; set; }
    }
    public class WorkingScheduleFilter
    {
        public String[]? UserEmails { get; set; } = null;
        public Int32 Month { get; set; } = DateTime.Now.Month;
        public Int32 Year { get; set; } = DateTime.Now.Year;
        public Request.MappedInheritorTypesEnum[] RequestTypes { get; set; } = Array.Empty<Request.MappedInheritorTypesEnum>();
        public RequestStatus[] RequestStatuses { get; set; } = Array.Empty<RequestStatus>();
    }
}
