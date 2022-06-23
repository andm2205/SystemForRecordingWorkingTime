using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class CreateDayOffRequest : CreateReplacebleRequest
    {
        public CreateDayOffRequest() { }
        public override DayOffRequest GetRequest(User user, ApplicationDbContext dbContext)
        {
            return new DayOffRequest(this, user, dbContext);
        }
        public CreateDayOffRequest(DayOffRequest request)
            : base(request) { }
    }
}
