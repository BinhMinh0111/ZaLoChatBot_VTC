using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DTO
{
    public partial class OaNotice
    {
        public OaNotice()
        {
            OaMessages = new HashSet<OaMessage>();
        }

        public long NoticeId { get; set; }
        public DateTime? NoticeDate { get; set; }
        public int? NumNotice { get; set; }
        public string? ContentUrl { get; set; }

        public virtual ICollection<OaMessage> OaMessages { get; set; }
    }
}
