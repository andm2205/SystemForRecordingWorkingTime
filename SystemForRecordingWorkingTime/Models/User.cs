using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public String? Surname { get; set; }
        public String? Name { get; set; }
        public String? Patronymic { get; set; }
        public UserRole Role { get; set; }
        public String? JobTitle { get; set; }
        public Int64? Phone { get; set; }
        public String Email { get; set; }
        [DataType(DataType.Password)]
        public String? Password { get; set; }
        [InverseProperty("ApplicantUser")]
        public ICollection<Request> ApplicantRequests { get; set; } = new List<Request>();
        [InverseProperty("ApprovingUser")]
        public ICollection<Request> ApprovingRequests { get; set; } = new List<Request>();
        public Int32 DaysWorked => DateTime.Now.DayOfYear - ApplicantRequests
            .Where(request => request.ApprovingUserId == this.Id)
            .Aggregate(0, (sum, request) => sum +
                request.StatedDates
                .Where(date => date.Value.Year == DateTime.Now.Year)
                .Count());
        public Int32 UnusedVacationDays => 20 - ApplicantRequests
            .Where(request => request.ApprovingUserId == this.Id)
            .Where(
                request =>
                typeof(VacationRequest) == request.GetType()
                && ((VacationRequest)request).VacationTypeValue == VacationType.BasicPaidLeave
                && request.RequestStatus == RequestStatus.Approved)
            .SelectMany(request => request.StatedDates)
            .Where(date => date.Value.Year == DateTime.Now.Year)
            .Count();

        public Int32 AllUnusedVacationDays => ApplicantRequests
            .Where(request => request.ApprovingUserId == this.Id)
            .Where(
                request =>
                typeof(VacationRequest) == request.GetType()
                && ((VacationRequest)request).VacationTypeValue == VacationType.BasicPaidLeave
                && request.RequestStatus == RequestStatus.Approved)
            .SelectMany(request => request.StatedDates)
            .Where(date => date.Value.Year != DateTime.Now.Year)
            .GroupBy(date => date.Value.Year)
            .Aggregate(
                0, (sum, dates) => sum + (20 - dates
                .Count()));
        public Int32 DaysOff=> ApplicantRequests
            .Where(request => request.ApprovingUserId == this.Id)
            .Where(
                request =>
                typeof(DayOffRequest) == request.GetType()
                && request.RequestStatus == RequestStatus.Approved)
            .Count();

        public Dictionary<RequestStatus, Int32> RequestCountByStatus => ApplicantRequests
            .Where(request => request.ApprovingUserId == this.Id)
            .GroupBy(request => request.RequestStatus)
            .Select(requests => new {Key = requests.Key, RequestsCount = requests.Count() })
            .ToDictionary(x => x.Key, x => x.RequestsCount);
    }
}
