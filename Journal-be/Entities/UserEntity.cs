using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json.Serialization;

namespace Journal_be.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        [JsonIgnore]
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? Address { get; set; }
        public int? Status { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? Image { get; set; }
        public string? Role { get; set; }
    }
}
