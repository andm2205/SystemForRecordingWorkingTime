using System.ComponentModel.DataAnnotations;

namespace SystemForRecordingWorkingTime.Models
{
    public enum RequestStatus
    {
        [Display(Name = "New")]
        New,
        [Display(Name = "SentForApproval")]
        SentForApproval,
        [Display(Name = "Agreed")]
        Agreed,
        [Display(Name = "NotAgreed")]
        NotAgreed,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "NotApproved")]
        NotApproved,
        [Display(Name = "Withdrawn")]
        Withdrawn,
        [Display(Name = "Canceled")]
        Canceled
    }
}
