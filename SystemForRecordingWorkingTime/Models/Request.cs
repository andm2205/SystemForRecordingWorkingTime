using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public abstract class Request
    {
        static public IEnumerable<MappedInheritorTypesEnum> MappedInheritorTypesList
        {
            get => Enum.GetValues(typeof(MappedInheritorTypesEnum)).Cast<MappedInheritorTypesEnum>();
        }
        public enum MappedInheritorTypesEnum
        {
            DayOffRequest = 0,
            DayOffAtTheExpenseOfVacationRequest,
            DayOffAtTheExpenseOfWorkingOutRequest,
            VacationRequest,
            RemoteWorkRequest
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public RequestStatus RequestStatus { get; set; }
        [ForeignKey("ApplicantUser")]
        public Int32? ApplicantUserId { get; set; }
        public User? ApplicantUser { get; set; }
        public DateOnly CreateDate { get; set; }
        public DateOnly? SubmissionDate { get; set; }
        public IEnumerable<StatedDate> StatedDates { get; set; }
        [ForeignKey("ApprovingUser")]
        public Int32? ApprovingUserId { get; set; }
        public User? ApprovingUser { get; set; }
        public String? Comment { get; set; }
        public String Discriminator { get; set; }
        public Request() { }
        public Request(CreateRequest data, User user, ApplicationDbContext dbContext)
        {
            this.RequestStatus = RequestStatus.New;
            this.ApplicantUserId = user.Id;
            this.CreateDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            this.ApprovingUserId = dbContext.Users.Single(a => a.Email == data.ApprovingUserEmail).Id;
            this.Comment = data.Comment;
        }
        public abstract CreateRequest GetData();
    }
}
