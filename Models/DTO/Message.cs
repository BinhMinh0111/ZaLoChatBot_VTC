namespace ZaloOA_v2.Models.ServiceModels
{
    public class Message
    {
        public string MessageId { get; set; } = null!;
        public long? UserId { get; set; }
        public long? NoticeId { get; set; }
    }
}
