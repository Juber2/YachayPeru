namespace YachayPeru.Application.Actions.Users.Models
{
    public class UpdateUserInput
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public bool IsLocked { get; set; }
        public int? RoleId { get; set; }
    }
}
