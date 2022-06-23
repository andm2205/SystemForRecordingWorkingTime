using System.ComponentModel.DataAnnotations.Schema;

namespace SystemForRecordingWorkingTime.Models
{
    [NotMapped]
    public class AgreeRequest
    {
        public Int32? RequestId { get; set; }
        public Boolean Movable { get; set; } = false;
        public Boolean Replaceble { get; set; } = false;
        public String Comment { get; set; }
        public AgreeRequest() { }
        public AgreeRequest(Request request)
        {
            this.RequestId = request.Id;
            this.Movable = request is VacationRequest && ((VacationRequest)request).Movable.GetValueOrDefault(false);
            this.Replaceble = request is ReplacebleRequest;
            this.Comment = request.Comment;
        }
        public static readonly RequestStatus[] RequestStatusValues = new RequestStatus[] {RequestStatus.Agreed, RequestStatus.NotAgreed};
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
        public IEnumerable<DateOnly> StatedDates
        {
                get => String.IsNullOrEmpty(statedDatesList)
                ? null
                : StatedDatesList
                .Split(',')
                .Select(a => DateOnly.Parse(a));
        }
        private String statedDatesList;
        public String StatedDatesList
        {
            get => statedDatesList;
            set
            {
                if (Movable)
                    statedDatesList = value;
                else throw new Exception("request is not movable");
            }
        }
        private String replacementUserEmail;
        public String ReplacementUserEmail 
        {
            get => replacementUserEmail;
            set
            {
                if (Replaceble)
                    replacementUserEmail = value;
                else throw new Exception("request is not replaceble");
            }
        }
    }
}
