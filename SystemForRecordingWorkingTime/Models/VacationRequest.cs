using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public class VacationRequest : ReplaceableRequest
    {
        public VacationType VacationTypeValue { get; set; }
        public Boolean Movable { get; set; }
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
}
