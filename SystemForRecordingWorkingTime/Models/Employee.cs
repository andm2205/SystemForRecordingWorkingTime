using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public UInt32 Id { get; set; }
        public String Surname { get; set; }
        public String Name { get; set; }
        public String Patronymic { get; set; }
        public UserRole Role { get; set; }
        public UInt64 Phone { get; set; }
        public String Email { get; set; }
        [InverseProperty("ApplicantUser")]
        public ICollection<Request> Requests { get; set; }
        public Int32 DaysWorked => DateTime.Now.DayOfYear - Requests
            .Aggregate(0, (sum, request) => sum +
                request.StatedDates
                .Where(date => date.Year == DateTime.Now.Year)
                .Count());
        public Int32 UnusedVacationDays => 20 - Requests
            .Where(
                request => 
                typeof(VacationRequest) == request.GetType()
                && ((VacationRequest)request).VacationType == VacationType.BasicPaidLeave 
                && request.RequestStatus == RequestStatus.Approved)
            .SelectMany(request => request.StatedDates)
            .Where(date => date.Year == DateTime.Now.Year)
            .Count();

        public Int32 AllUnusedVacationDays => Requests
            .Where(
                request =>
                typeof(VacationRequest) == request.GetType()
                && ((VacationRequest)request).VacationType == VacationType.BasicPaidLeave
                && request.RequestStatus == RequestStatus.Approved)
            .SelectMany(request => request.StatedDates)
            .Where(date => date.Year != DateTime.Now.Year)
            .GroupBy(date => date.Year)
            .Aggregate(
                0, (sum, dates) => sum + (20 - dates
                .Count()));

        public Int32 DaysOff => Requests
            .Where(
                request =>
                typeof(DayOffRequest) == request.GetType()
                && request.RequestStatus == RequestStatus.Approved)
            .Count();

        public Dictionary<RequestStatus, Int32> GetRequestsCount(User user)
        {
            return Requests
                .Where(request => request.ApprovingUser == user)
                .GroupBy(request => request.RequestStatus)
                .Select(requests => new {Key = requests.Key, RequestsCount = requests.Count() })
                .ToDictionary(x => x.Key, x => x.RequestsCount);
        }
    }
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
    public abstract class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public UInt32 Id { get; set; }
        public RequestStatus RequestStatus { get; set; }
        [ForeignKey("ApplicantUser")]
        public UInt32 ApplicantUserId { get; set; }
        public User ApplicantUser { get; set; }
        [Column(TypeName = "date")]
        public DateOnly CreateDate { get; set; }
        [Column(TypeName = "date")]
        public DateOnly SubmissionDate { get; set; }
        public List<DateTime> StatedDates { get; set; }
        [ForeignKey("ApprovingUser")]
        public UInt32 ApprovingUserId { get; set; }
        public User ApprovingUser { get; set; }
        public string Comment { get; set; }
    }
    public class VacationRequest : Request
    {
        public VacationType VacationType { get; set; }
        public UInt32 ReplacementUserId { get; set; }
        public User ReplacementUser { get; set; }
        public Boolean Movable { get; set; }
    }
    public class DayOffAtTheExpenseOfVacationRequest : Request
    {
        public UInt32 ReplacementUserId { get; set; }
        public User ReplacementUser { get; set; }
    }

    public class DayOffAtTheExpenseOfWorkingOutRequest : Request
    {
        public UInt32 ReplacementUserId { get; set; }
        public User ReplacementUser { get; set; }
/*        public List<DateOnly> WorkingOutDates { get; set; }
*/    }

    public class DayOffRequest : Request
    {
        public UInt32 ReplacementUserId { get; set; }
        public User ReplacementUser { get; set; }
    }

    public class RemoteWorkRequest : Request
    {
        public  List<String> WorkPlans { get; set; }
    }

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
