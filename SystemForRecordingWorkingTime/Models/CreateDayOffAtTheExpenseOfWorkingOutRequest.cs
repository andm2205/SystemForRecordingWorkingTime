using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class CreateDayOffAtTheExpenseOfWorkingOutRequest : CreateReplacebleRequest
    {
        public String WorkingOutDatesList { get; set; }
        public override DayOffAtTheExpenseOfWorkingOutRequest GetRequest
            (User user, ApplicationDbContext dbContext)
        {
            return new DayOffAtTheExpenseOfWorkingOutRequest(this, user, dbContext);
        }
        public override void DeleteRequest(User user, ApplicationDbContext dbContext, int requestId)
        {
            dbContext.WorkingOutDates
                .RemoveRange(
                    dbContext.WorkingOutDates
                    .Where(a => a.DayOffAtTheExpenseOfWorkingOutRequestId == requestId));
            base.DeleteRequest(user, dbContext, requestId);
        }
        public override void AddLinkedRequestData
            (User user, ApplicationDbContext dbContext, Int32 requestId)
        {
            base.AddLinkedRequestData(user, dbContext, requestId);
            dbContext.WorkingOutDates.AddRange(
                StatedDatesList
                .Split(',')
                .Select(a => new WorkingOutDate()
                {
                    Value = DateOnly.Parse(a),
                    DayOffAtTheExpenseOfWorkingOutRequestId = requestId
                }));
        }
        public CreateDayOffAtTheExpenseOfWorkingOutRequest(DayOffAtTheExpenseOfWorkingOutRequest request)
            : base(request)
        {
            this.WorkingOutDatesList = String.Join(',', request.WorkingOutDates.Select(a => a.Value.ToString("yyyy-MM-dd")));
        }
    }
}
