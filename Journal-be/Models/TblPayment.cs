using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Journal_be.Models
{
    public partial class TblPayment
    {
        public TblPayment()
        {
            TblTransactions = new HashSet<TblTransaction>();
        }
        [JsonIgnore]
        public int Id { get; set; }
        public string? Method { get; set; }
        public int? Status { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public virtual TblUser? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblTransaction> TblTransactions { get; set; }
    }
}
