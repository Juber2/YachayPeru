namespace YachayPeru.API.Contracts.Administration.Users.Request
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public bool SendWelcomeMessage { get; set; }
        public int? RoleId { get; set; }
        public int? ReactivateUserId { get; set; }
    }
}
