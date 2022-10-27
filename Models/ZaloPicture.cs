using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models
{
    public partial class ZaloPicture
    {
        public long PictureId { get; set; }
        public long? UserId { get; set; }
        public string? PicUrl { get; set; }
        public long? Timestamp { get; set; }

        public virtual ZaloUser? User { get; set; }
    }
}
