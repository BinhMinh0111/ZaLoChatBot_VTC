using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DAO
{
    public partial class OaPicture
    {
        public int PictureId { get; set; }
        public long? UserId { get; set; }
        public string? PicUrl { get; set; }
        public DateTime? PicTime { get; set; }

        public virtual OaUser? User { get; set; }
    }
}
