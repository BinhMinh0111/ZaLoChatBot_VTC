using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DatabaseModels
{
    public partial class OaCategory
    {
        public OaCategory()
        {
            OaKeywords = new HashSet<OaKeyword>();
        }

        public int CategoryId { get; set; }
        public string? Category { get; set; }

        public virtual ICollection<OaKeyword> OaKeywords { get; set; }
    }
}
