using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DatabaseModels
{
    public partial class OaFeedback
    {
        public long FeedbackId { get; set; }
        public long? UserId { get; set; }
        public string? Feedbacks { get; set; }
        public long? Timestamp { get; set; }

        public virtual OaUser? User { get; set; }
    }
}
