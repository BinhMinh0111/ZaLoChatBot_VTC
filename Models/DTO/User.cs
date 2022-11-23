namespace ZaloOA_v2.Models.ServiceModels
{
    public class User
    {
        public long UserId { get; set; }
        public long? IdByApp { get; set; }
        public string? DisplayName { get; set; }
        public short? Gender { get; set; }
        public bool? UserState { get; set; }
    }
}
