using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class RequestTypeChooser
    {
        public Request.MappedInheritorTypesEnum TypeIndex { get; set; }
    }
}
