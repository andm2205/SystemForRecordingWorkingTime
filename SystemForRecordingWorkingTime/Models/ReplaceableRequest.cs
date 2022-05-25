using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public abstract class ReplaceableRequest : Request
    {
        public Int32 ReplacementUserId { get; set; }
        public User ReplacementUser { get; set; }
    }
}
