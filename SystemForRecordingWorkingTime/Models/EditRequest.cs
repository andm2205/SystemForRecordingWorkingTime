using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class EditRequest
    {
        public Int32 RequestId { get; set; }
        public CreateRequest Data { get; set; }
    }
}
