using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class CreateUser
    {
        public Int32 Id { get; set; }
        public String Surname { get; set; }
        public String Name { get; set; }
        public String Patronymic { get; set; }
        public UserRole Role { get; set; }
        public Int64 Phone { get; set; }
        public String Email { get; set; }
    }
}
