using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Journal_be.Models
{
    public partial class TblCategory
    {
        public TblCategory()
        {
            TblArticles = new HashSet<TblArticle>();
        }

        public int Id { get; set; }
        public string? CategoryName { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblArticle> TblArticles { get; set; }
    }
}
