using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public class DayOffAtTheExpenseOfWorkingOutRequest : ReplaceableRequest
    {
        public IEnumerable<WorkingOutDate> WorkingOutDates { get; set; }
        public class WorkingOutDate
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public Int32 Id { get; set; }
            public DateOnly Value { get; set; }
            public Int32 DayOffAtTheExpenseOfWorkingOutRequestId { get; set; }
            public DayOffAtTheExpenseOfWorkingOutRequest DayOffAtTheExpenseOfWorkingOutRequest { get; set; }
        }
    }
}
