using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public class DayOffAtTheExpenseOfWorkingOutRequest : ReplacebleRequest
    {
        public DayOffAtTheExpenseOfWorkingOutRequest() { }
        public DayOffAtTheExpenseOfWorkingOutRequest
            (CreateDayOffAtTheExpenseOfWorkingOutRequest data, User user, ApplicationDbContext dbContext) 
            : base(data, user, dbContext)
        { }
        public IEnumerable<WorkingOutDate> WorkingOutDates { get; set; }

        public override CreateDayOffAtTheExpenseOfWorkingOutRequest GetData()
        {
            return new CreateDayOffAtTheExpenseOfWorkingOutRequest(this);
        }
    }
}
