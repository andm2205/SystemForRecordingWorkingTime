using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [Table("StatedDates")]
    public class StatedDate : Models.Date
    {
        [Key, Column(Order = 1)]
        public Int32 RequestId { get; set; }
        public Request Request { get; set; }
    }
}
