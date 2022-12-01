using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DAO
{
    //Message State: 0 = unsent, 1 = sent, 2 = received, 3 = seen
    public partial class OaMessage
    {
        public string MessageId { get; set; } = null!;
        public long? UserId { get; set; }
        public long? NoticeId { get; set; }
        public short? State { get; set; }
        public bool? Status { get; set; }

        public virtual OaNotice? Notice { get; set; }
        public virtual OaUser? User { get; set; }
    }
}
