namespace ZaloOA_v2.Models.ServiceModels
{
    public class MessageDTO
    {
        public string MessageId { get; set; } = null!;
        public long? UserId { get; set; }
        public long? NoticeId { get; set; }
        public short? State { get; set; }
        public bool? Status { get; set; }
    }
}
