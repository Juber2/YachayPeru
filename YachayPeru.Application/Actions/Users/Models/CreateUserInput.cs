namespace YachayPeru.Application.Actions.Users.Models
{
    public class CreateUserInput
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserTypeCode { get; set; } = string.Empty;
        public int? RoleId { get; set; }
        public int? ReactivateUserId { get; set; }
        public bool SendWelcomeMessage { get; set; }
    }
}
