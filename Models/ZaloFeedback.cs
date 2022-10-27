using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models
{
    public partial class ZaloFeedback
    {
        public long FeedbackId { get; set; }
        public long? UserId { get; set; }
        public string? Feedbacks { get; set; }
        public long? Timestamp { get; set; }

        public virtual ZaloUser? User { get; set; }
    }
}
