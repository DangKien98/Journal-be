using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Journal_be.Models
{
    public partial class TblTransaction
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int? Status { get; set; }
        public int? PaymentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
        public int? ArticleId { get; set; }
        [JsonIgnore]
        public virtual TblArticle? Article { get; set; }
        [JsonIgnore]
        public virtual TblPayment? Payment { get; set; }
    }
}
