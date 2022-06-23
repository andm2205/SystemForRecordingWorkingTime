using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class CreateDayOffAtTheExpenseOfVacationRequest : CreateReplacebleRequest
    {
        public override DayOffAtTheExpenseOfVacationRequest GetRequest(User user, ApplicationDbContext dbContext)
        {
            return new DayOffAtTheExpenseOfVacationRequest(this, user, dbContext);
        }
        public CreateDayOffAtTheExpenseOfVacationRequest(DayOffAtTheExpenseOfVacationRequest request)
            : base(request) { }
    }
}
