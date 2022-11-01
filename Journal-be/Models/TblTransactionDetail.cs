using System.Text.Json.Serialization;

namespace Journal_be.Models
{
    public partial class TblTransactionDetail
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        [JsonIgnore]
        public DateTime? CreatedTime { get; set; }
        public int? TransactionId { get; set; }
        public int? ArticleId { get; set; }
        [JsonIgnore]
        public virtual TblArticle? Article { get; set; }
        [JsonIgnore]
        public virtual TblTransaction? Transaction { get; set; }
    }
}
