using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public class RemoteWorkRequest : Request
    {
        public RemoteWorkRequest() { }
        public RemoteWorkRequest(CreateRemoteWorkRequest data, User user, ApplicationDbContext dbContext) 
            : base(data, user, dbContext)
        { }
        public IEnumerable<WorkPlan> WorkPlans { get; set; }

        public override CreateRemoteWorkRequest GetData()
        {
            return new CreateRemoteWorkRequest(this);
        }
    }
}
