using System.ComponentModel.DataAnnotations;

namespace SystemForRecordingWorkingTime.Models
{
    public class User
    {
        public String Surname { get; set; }
        public String Name { get; set; }
        public String Patronymic { get; set; }
        public String Role { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public readonly List<Request> Requests = new();
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

        public IEnumerable<Int32> GetRequestsCount(User user)
        {
            return Requests
                .Where(request => request.ApprovingUser == user)
                .GroupBy(request => request.RequestStatus)
                .Select(requests => requests.Count());
        }
    }
    public enum UserRole
    {
        Employee,
        Director,
        Supervisor,
        Administrator
    }
    public class Request
    {
        public RequestStatus RequestStatus { get; set; }
        public User Applicant { get; set; }
        public DateOnly CreateDate { get; set; }
        public DateOnly SubmissionDate { get; set; }
        public readonly List<DateOnly> StatedDates = new();
        public User ApprovingUser { get; set; }
        public string Comment { get; set; }
    }
    class VacationRequest : Request
    {
        public VacationType VacationType { get; set; }
        public User ReplacementEmployee { get; set; }
        public Boolean Movable { get; set; }
    }
    class DayOffAtTheExpenseOfVacationRequest : Request
    {
        User ReplacementEmployee { get; set; }
    }

    class DayOffAtTheExpenseOfWorkingOutRequest : Request
    {
        User ReplacementEmployee { get; set; }
        public DateOnly[] WorkingOutDates { get; set; }
    }

    class DayOffRequest : Request
    {
        User ReplacementEmployee { get; set; }
    }

    class RemoteWorkRequest : Request
    {
        string[] WorkPlans { get; set; }
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
