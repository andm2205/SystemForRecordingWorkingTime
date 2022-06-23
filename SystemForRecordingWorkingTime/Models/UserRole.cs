using System.ComponentModel.DataAnnotations;

namespace SystemForRecordingWorkingTime.Models
{
    public enum UserRole
    {
        [Display(Name = "Employee")]
        Employee,
        [Display(Name = "Director")]
        Director,
        [Display(Name = "Supervisor")]
        Supervisor,
        [Display(Name = "Administrator")]
        Administrator
    }
}
