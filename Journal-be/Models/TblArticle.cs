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
        public string? Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
        public string? AuthorName { get; set; }
        public int? Status { get; set; }
        public float? Price { get; set; }
        public byte[]? ArtFile { get; set; }
        public DateTime? LastEditedTime { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }

        public virtual TblCategory? Category { get; set; }
        public virtual TblUser? User { get; set; }
        public virtual ICollection<TblTransactionDetail> TblTransactionDetails { get; set; }
    }
}
