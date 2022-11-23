using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DTO
{
    public partial class OaUser
    {
        public OaUser()
        {
            OaMessages = new HashSet<OaMessage>();
            OaPictures = new HashSet<OaPicture>();
        }

        public long UserId { get; set; }
        public long? IdByApp { get; set; }
        public string? DisplayName { get; set; }
        public short? Gender { get; set; }
        public bool? UserState { get; set; }

        public virtual ICollection<OaMessage> OaMessages { get; set; }
        public virtual ICollection<OaPicture> OaPictures { get; set; }
    }
}
