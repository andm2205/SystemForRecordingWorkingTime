using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public abstract class CreateRequest
    {

        public CreateRequest() { }
        private DateOnly[] StatedDates { get; set; }
        public String StatedDatesList { get; set; }
        public String ApprovingUserEmail { get; set; }
        public string Comment { get; set; }
        public abstract Request GetRequest
            (User user, ApplicationDbContext dbContext);
        public virtual void DeleteRequest(User user, ApplicationDbContext dbContext, Int32 requestId)
        {
            dbContext.StatedDates.RemoveRange(dbContext.StatedDates.Where(a => a.RequestId == requestId));
        }
        public virtual void AddLinkedRequestData
            (User user, ApplicationDbContext dbContext, Int32 requestId)
        {
            dbContext.StatedDates.AddRange(
                StatedDatesList
                .Split(',')
                .Select(a => new StatedDate()
                {
                    Value = DateOnly.Parse(a),
                    RequestId = requestId
                }));
        }
        public CreateRequest(Request request)
        {
            this.StatedDatesList = String.Join(',', request.StatedDates.Select(a => a.Value.ToString("yyyy-MM-dd")));
            this.ApprovingUserEmail = request.ApprovingUser.Email;
            this.Comment = request.Comment;
        }
    }
}
