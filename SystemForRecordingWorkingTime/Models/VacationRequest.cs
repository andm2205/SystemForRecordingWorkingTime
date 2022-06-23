using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public partial class VacationRequest : ReplacebleRequest
    {
        public VacationRequest() { }
        public VacationRequest
            (CreateVacationRequest data, User user, ApplicationDbContext dbContext) 
            : base(data, user, dbContext)
        {
            this.VacationTypeValue = data.VacationTypeValue;
            this.Movable = data.Movable;
        }
        public VacationType VacationTypeValue { get; set; }
        public Boolean? Movable { get; set; }
        public override CreateVacationRequest GetData()
        {
            return new CreateVacationRequest(this);
        }
    }
}
