using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Journal_be.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblArticles = new HashSet<TblArticle>();
            TblPayments = new HashSet<TblPayment>();
        }
        [JsonIgnore]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        [JsonIgnore]
        public DateTime? CreateTimed { get; set; }
        public string? Address { get; set; }
        public int? RoleId { get; set; }
        [JsonIgnore]
        public virtual TblRole? Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblArticle> TblArticles { get; set; }
        [JsonIgnore]
        public virtual ICollection<TblPayment> TblPayments { get; set; }
    }
}
