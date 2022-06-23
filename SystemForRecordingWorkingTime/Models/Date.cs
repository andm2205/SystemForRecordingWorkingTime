using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public abstract class Date
    {
        [Key, Column(Order = 0)]
        public DateOnly Value { get; set; }
    }
}
