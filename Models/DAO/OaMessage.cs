using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DTO
{
    public partial class OaMessage
    {
        public string MessageId { get; set; } = null!;
        public long? UserId { get; set; }
        public long? NoticeId { get; set; }

        public virtual OaNotice? Notice { get; set; }
        public virtual OaUser? User { get; set; }
    }
}
