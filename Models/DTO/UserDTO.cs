namespace ZaloOA_v2.Models.DTO
{
    public class UserDTO
    {
        public long UserId { get; set; }
        public long? IdByApp { get; set; }
        public string? DisplayName { get; set; }
        public short? Gender { get; set; }
        public bool? UserState { get; set; }
    }
}
