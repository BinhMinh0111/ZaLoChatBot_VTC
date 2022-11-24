namespace ZaloOA_v2.Models.DTO
{
    public class PictureDTO
    {
        public int PictureId { get; set; }
        public long? UserId { get; set; }
        public string? PicUrl { get; set; }
        public DateTime? PicTime { get; set; }
    }
}
