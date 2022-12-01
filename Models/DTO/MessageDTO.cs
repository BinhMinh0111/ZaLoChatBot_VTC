namespace ZaloOA_v2.Models.DTO
{
    //Message State: 0 = unsent, 1 = sent, 2 = received, 3 = seen
    public class MessageDTO
    {
        public string MessageId { get; set; } = null!;
        public long? UserId { get; set; }
        public long? NoticeId { get; set; }
        public short? State { get; set; }
        public bool? Status { get; set; }
    }
}
