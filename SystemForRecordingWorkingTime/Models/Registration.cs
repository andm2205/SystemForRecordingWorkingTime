using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class Registration
    {
        public String Email { get; set; }
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }
    }
}
