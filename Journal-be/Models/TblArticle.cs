using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Journal_be.Models
{
    public partial class TblArticle
    {
        public TblArticle()
        {
            TblTransactions = new HashSet<TblTransaction>();
        }
        [JsonIgnore]
        public int Id { get; set; }
        public string? Title { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public float? Price { get; set; }
        [JsonIgnore]
        public byte[]? ArtFile { get; set; }
        [JsonIgnore]
        public DateTime? LastEditedTime { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }
        public int? StatusPost { get; set; }
        public string? Comment { get; set; }
        public string? ArtFileName { get; set; }
        [JsonIgnore]
        public virtual TblCategory? Category { get; set; }
        [JsonIgnore]
        public virtual TblUser? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblTransaction> TblTransactions { get; set; }
    }
}
