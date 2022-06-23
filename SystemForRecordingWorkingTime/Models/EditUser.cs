using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class EditUser
    {
        public Int32 Id { get; set; }
        public Int64? Phone { get; set; }
        public String Email { get; set; }
        public EditUser() { }
        public EditUser(User user)
        {
            this.Id = user.Id;
            this.Phone = user.Phone;
            this.Email = user.Email;
        }
    }
}
