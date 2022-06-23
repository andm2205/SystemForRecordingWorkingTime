using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class WorkingCalendar
    {
        public WorkingCalendar(Request[] requests, Int32 userId)
        {
            Requests = requests;
            UserId = userId;
        }
        public Int32 UserId { get; set; }
        public Request[] Requests { get; set; }
        public UInt16 Year { get; set; } = Convert.ToUInt16(DateTime.Today.Year);
        public Byte Month { get; set; } = Convert.ToByte(DateTime.Today.Month);

    }
    public class WorkingCalendarFilter
    {
        public String UserEmail { get; set; }
        public UInt16 Year { get; set; } = Convert.ToUInt16(DateTime.Today.Year);
        public Byte Month { get; set; } = Convert.ToByte(DateTime.Today.Month);
        public Request.MappedInheritorTypesEnum[] RequestTypes { get; set; } = new Request.MappedInheritorTypesEnum[0];
        public RequestStatus[] RequestStatuses { get; set; } = new RequestStatus[0];
    }
}
