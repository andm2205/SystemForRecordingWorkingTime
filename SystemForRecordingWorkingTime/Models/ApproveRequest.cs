using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class ApproveRequest
    {
        public ApproveRequest() { }
        public ApproveRequest(Request request)
        {
            this.RequestId = request.Id;
        }
        public Int32 RequestId { get; set; }
        private RequestStatus status = RequestStatus.NotApproved;
        public RequestStatus Status
        {
            get => status;
            set
            {
                if (RequestStatusValues.Contains(value))
                    status = value;
                else throw new ArgumentOutOfRangeException();
            }
        }
        public static readonly RequestStatus[] RequestStatusValues = new RequestStatus[] { RequestStatus.Approved, RequestStatus.NotApproved };
    }
}
