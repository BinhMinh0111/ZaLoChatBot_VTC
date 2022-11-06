using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DatabaseModels
{
    public partial class ZaloUser
    {
        public ZaloUser()
        {
            ZaloFeedbacks = new HashSet<ZaloFeedback>();
            ZaloPictures = new HashSet<ZaloPicture>();
        }

        public long UserId { get; set; }
        public long? UserIdByApp { get; set; }
        public string? DisplayName { get; set; }
        public int? UserGender { get; set; }
        public bool? UserState { get; set; }

        public virtual ICollection<ZaloFeedback> ZaloFeedbacks { get; set; }
        public virtual ICollection<ZaloPicture> ZaloPictures { get; set; }
    }
}
