using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class CreateVacationRequest : CreateReplacebleRequest
    {
        public VacationType VacationTypeValue { get; set; }
        public Boolean Movable { get; set; }
        public override VacationRequest GetRequest(User user, ApplicationDbContext dbContext)
        {
            return new VacationRequest(this, user, dbContext);
        }
        public CreateVacationRequest(VacationRequest request) : base(request)
        {
            this.VacationTypeValue = request.VacationTypeValue;
            this.Movable = request.Movable.Value;
        }
    }
}
