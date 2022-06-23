using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public abstract class CreateReplacebleRequest : CreateRequest
    {
        public CreateReplacebleRequest() { }
        public String ReplacementUserEmail { get; set; }
        public override abstract ReplacebleRequest GetRequest(User user, ApplicationDbContext dbContext);
        public CreateReplacebleRequest(ReplacebleRequest request) : base(request)
        {
            this.ReplacementUserEmail = request.ReplacementUser.Email;
        }
    }
}
