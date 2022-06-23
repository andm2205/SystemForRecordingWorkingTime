using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    public abstract class ReplacebleRequest : Request
    {
        public Int32? ReplacementUserId { get; set; }
        public User? ReplacementUser { get; set; }
        public ReplacebleRequest() { }
        public ReplacebleRequest
            (CreateReplacebleRequest data, User user, ApplicationDbContext dbContext)
            : base(data, user, dbContext)
        {
            this.ReplacementUserId = dbContext.Users.Single(a => a.Email == data.ReplacementUserEmail).Id;
        }
    }
}
