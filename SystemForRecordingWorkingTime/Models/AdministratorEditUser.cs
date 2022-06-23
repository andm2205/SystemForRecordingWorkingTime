using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class AdministratorEditUser
    {
        public AdministratorEditUser() { }
        public AdministratorEditUser(User user) 
        { 
            this.Id = user.Id;
            this.Surname = user.Surname;
            this.Name = user.Name;
            this.Patronymic = user.Patronymic;
            this.Role = user.Role;
            this.JobTitle = user.JobTitle;
            this.Phone = user.Phone;
            this.Email = user.Email;
        }
        public Int32 Id { get; set; }
        public String? Surname { get; set; }
        public String? Name { get; set; }
        public String? Patronymic { get; set; }
        public UserRole Role { get; set; }
        public String? JobTitle { get; set; }
        public Int64? Phone { get; set; }
        public String Email { get; set; }

    }
}
