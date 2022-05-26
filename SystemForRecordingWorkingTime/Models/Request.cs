using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public abstract class Request
    {
        static public List<Type> MappedInheritorTypes
        {
            get => new List<Type>()
            {
                typeof(DayOffRequest),
                typeof(DayOffAtTheExpenseOfVacationRequest),
                typeof(DayOffAtTheExpenseOfWorkingOutRequest),
                typeof(VacationRequest)
            };
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public RequestStatus RequestStatusValue { get; set; }
        [ForeignKey("ApplicantUser")]
        public Int32 ApplicantUserId { get; set; }
        public User ApplicantUser { get; set; }
        /*[Column(TypeName = "date")]*/
        public DateOnly CreateDate { get; set; }
        /*[Column(TypeName = "date")]*/
        public DateOnly SubmissionDate { get; set; }
        public class StatedDate
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public Int32 Id { get; set; }
            /*[Column(TypeName = "date")]*/
            public DateOnly Value { get; set; }
            public Int32 RequestId { get; set; }
            public Request Request { get; set; }
        }
        public ICollection<StatedDate> StatedDates { get; set; }
        [ForeignKey("ApprovingUser")]
        public Int32 ApprovingUserId { get; set; }
        public User ApprovingUser { get; set; }
        public string Comment { get; set; }
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
}
