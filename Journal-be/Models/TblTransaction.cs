using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Journal_be.Models
{
    public partial class TblTransaction
    {
        public TblTransaction()
        {
            TblTransactionDetails = new HashSet<TblTransactionDetail>();
        }
        [JsonIgnore]
        public int Id { get; set; }
        public int? Status { get; set; }
        public int? PaymentId { get; set; }
        [JsonIgnore]
        public virtual TblPayment? Payment { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblTransactionDetail> TblTransactionDetails { get; set; }
    }
}
