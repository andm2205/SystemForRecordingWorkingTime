using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class RequestTypeChooser
    {
        public Int32 TypeIndex { get; set; }
    }
}
