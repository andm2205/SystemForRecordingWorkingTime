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
        public UInt16 DaysWorked => 
            Convert.ToUInt16(
                DateTime.Now.DayOfYear - 
                Requests.Aggregate(
                    0, (prev, next) => prev + next.StatedDates.Where(
                        date => date.Year == DateTime.Now.Year).Count()));
        public UInt16 UnusedVacationDays => 
            Convert.ToUInt16(
                20 - Requests.Where(
                    request => ((VacationRequest)request).VacationType == VacationType.BasicPaidLeave).Aggregate(
                    0, (prev, next) => prev + next.StatedDates.Where(
                        date => date.Year == DateTime.Now.Year).Count()));

        public UInt16 AllUnusedVacationDays =>
            Convert.ToUInt16(Requests
                .Where(request => ((VacationRequest)request).VacationType == VacationType.BasicPaidLeave)
                .GroupBy(request => request.)

        //20 – {количество дней по утвержденным заявкам на отпуск типа «Основной оплачиваемый отпуск» в текущем году};
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
    class ParentalLeaveRequest : Request
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
    public enum RequestType
    {
        [Display(Name = "Vacation")]
        BasicPaidLeave,
        [Display(Name = "DayOffAtTheExpenseOfVacation")]
        DayOffAtTheExpenseOfVacation,
        [Display(Name = "DayOffAtTheExpenseOfWorkingOut")]
        DayOffAtTheExpenseOfWorkingOut,
        [Display(Name = "DayOff")]
        DayOff,
        [Display(Name = "RemoteWork")]
        RemoteWork
    }
    public enum VacationType
    {
        [Display(Name = "BasicPaidLeave")]
        BasicPaidLeave,
        [Display(Name = "SentForApproval")]
        LeaveWithoutPay,
        [Display(Name = "PregnancyAndMaternityLeave")]
        PregnancyAndMaternityLeave,
        [Display(Name = "ParentalLeave")]
        NotAgreed,
    }
}
