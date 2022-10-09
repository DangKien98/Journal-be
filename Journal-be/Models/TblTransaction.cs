using System;
using System.Collections.Generic;

namespace Journal_be.Models
{
    public partial class TblTransaction
    {
        public TblTransaction()
        {
            TblTransactionDetails = new HashSet<TblTransactionDetail>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? PaymentId { get; set; }

        public virtual TblPayment? Payment { get; set; }
        public virtual ICollection<TblTransactionDetail> TblTransactionDetails { get; set; }
    }
}
