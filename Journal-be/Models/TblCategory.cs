using System;
using System.Collections.Generic;

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

        public virtual ICollection<TblArticle> TblArticles { get; set; }
    }
}
