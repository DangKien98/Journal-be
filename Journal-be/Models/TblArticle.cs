using System;
using System.Collections.Generic;

namespace Journal_be.Models
{
    public partial class TblArticle
    {
        public TblArticle()
        {
            TblTransactionDetails = new HashSet<TblTransactionDetail>();
        }

        public int Id { get; set; }
        public string? Titile { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? Description { get; set; }
        public string? AuthorName { get; set; }
        public int? Status { get; set; }
        public double? Price { get; set; }
        public int UserId { get; set; }
        public int? CategoryId { get; set; }
        public string? Image { get; set; }

        public virtual TblCategory? Category { get; set; }
        public virtual TblUser User { get; set; } = null!;
        public virtual ICollection<TblTransactionDetail> TblTransactionDetails { get; set; }
    }
}
