using System.Text.Json.Serialization;

namespace Journal_be.Entities
{
    public class ArticleEntity
    {
        public int Id { get; set; }
        public string? Titile { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? Description { get; set; }
        public string? AuthorName { get; set; }
        public int? Status { get; set; }
        public double? Price { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Image { get; set; }
        public DateTime? LastEditedTime { get; set; }
    }
}
