using System;
using System.Collections.Generic;

namespace Journal_be.Models
{
    public partial class TblTransactionDetail
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? TransactionId { get; set; }
        public int? ArticleId { get; set; }

        public virtual TblArticle? Article { get; set; }
        public virtual TblTransaction? Transaction { get; set; }
    }
}
