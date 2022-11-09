using System;
using System.Collections.Generic;

namespace ZaloOA_v2.Models.DatabaseModels
{
    public partial class OaKeyword
    {
        public int KeywordId { get; set; }
        public int? CategoryId { get; set; }
        public string? Keyword { get; set; }

        public virtual OaCategory? Category { get; set; }
    }
}
