using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DatabaseModels
{
    public partial class OaUser
    {
        public OaUser()
        {
            OaFeedbacks = new HashSet<OaFeedback>();
            OaPictures = new HashSet<OaPicture>();
        }

        public long UserId { get; set; }
        public long? UserIdByApp { get; set; }
        public string? DisplayName { get; set; }
        public int? UserGender { get; set; }
        public bool? UserState { get; set; }

        public virtual ICollection<OaFeedback> OaFeedbacks { get; set; }
        public virtual ICollection<OaPicture> OaPictures { get; set; }
    }
}
