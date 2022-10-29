using System;
using System.Collections.Generic;

namespace Journal_be.Models
{
    public partial class TblPayment
    {
        public TblPayment()
        {
            TblTransactions = new HashSet<TblTransaction>();
        }

        public int Id { get; set; }
        public string? Method { get; set; }
        public int? Status { get; set; }
        public int? UserId { get; set; }

        public virtual TblUser? User { get; set; }
        public virtual ICollection<TblTransaction> TblTransactions { get; set; }
    }
}
