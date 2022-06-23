using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public class DayOffAtTheExpenseOfVacationRequest : ReplacebleRequest
    {
        public DayOffAtTheExpenseOfVacationRequest() { }
        public DayOffAtTheExpenseOfVacationRequest(CreateReplacebleRequest data, User user,ApplicationDbContext dbContext) 
            : base(data, user, dbContext)
            { }
        public override CreateDayOffAtTheExpenseOfVacationRequest GetData()
        {
            return new CreateDayOffAtTheExpenseOfVacationRequest(this);
        }
    }
}
