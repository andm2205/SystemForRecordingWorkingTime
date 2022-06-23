using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public class DayOffRequest : ReplacebleRequest
    {
        public DayOffRequest() { }
        public  DayOffRequest(CreateDayOffRequest data, User user, ApplicationDbContext dbContext) 
            : base(data, user, dbContext)
        { }

        public override CreateDayOffRequest GetData()
        {
            return new CreateDayOffRequest(this);
        }
    }
}
