using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public class WorkPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public String Value { get; set; }
        public Int32 RemoteWorkRequestId { get; set; }
        public RemoteWorkRequest RemoteWorkRequest { get; set; }
    }
}
