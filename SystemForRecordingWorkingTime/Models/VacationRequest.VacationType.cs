using System.ComponentModel.DataAnnotations;

namespace SystemForRecordingWorkingTime.Models
{
    public enum VacationType
    {
        [Display(Name = "BasicPaidLeave")]
        BasicPaidLeave,
        [Display(Name = "LeaveWithoutPay")]
        LeaveWithoutPay,
        [Display(Name = "PregnancyAndMaternityLeave")]
        PregnancyAndMaternityLeave,
        [Display(Name = "ParentalLeave")]
        ParentalLeave,
    }
}
