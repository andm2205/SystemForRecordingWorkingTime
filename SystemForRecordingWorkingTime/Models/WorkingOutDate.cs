using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [Table("WorkingOutDates")]
    public class WorkingOutDate : Models.Date
    {
        [Key, Column(Order = 1)]
        public Int32 DayOffAtTheExpenseOfWorkingOutRequestId { get; set; }
        public DayOffAtTheExpenseOfWorkingOutRequest DayOffAtTheExpenseOfWorkingOutRequest { get; set; }
    }
}
