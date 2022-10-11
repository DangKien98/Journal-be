namespace Journal_be.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? Address { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
