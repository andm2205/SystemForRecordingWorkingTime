using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class CreateRemoteWorkRequest : CreateRequest
    {
        public CreateRemoteWorkRequest() { }
        public String WorkPlans { get; set; }

        public override RemoteWorkRequest GetRequest(User user, ApplicationDbContext dbContext)
        {
            return new RemoteWorkRequest(this, user, dbContext);
        }
        public override void DeleteRequest(User user, ApplicationDbContext dbContext, int requestId)
        {
            dbContext.WorkPlans.RemoveRange(dbContext.WorkPlans.Where(a => a.RemoteWorkRequestId == requestId));
            base.DeleteRequest(user, dbContext, requestId);
        }
        public override void AddLinkedRequestData
            (User user, ApplicationDbContext dbContext, Int32 requestId)
        {
            base.AddLinkedRequestData(user, dbContext, requestId);
            dbContext.WorkPlans.AddRange(
                WorkPlans
                .Split('\n')
                .Select(a => new WorkPlan
                {
                    Value = a,
                    RemoteWorkRequestId = requestId
                }));
        }
        public CreateRemoteWorkRequest(RemoteWorkRequest request) : base(request)
        {
            this.WorkPlans = String.Join('\n', request.WorkPlans.Select(a => a.Value));
        }
    }
}
