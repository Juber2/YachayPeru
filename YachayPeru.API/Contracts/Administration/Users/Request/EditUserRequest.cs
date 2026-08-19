namespace YachayPeru.API.Contracts.Administration.Users.Request
{
    public class EditUserRequest
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Password { get; set; }
        public bool IsLocked { get; set; }
        public int? RoleId { get; set; }
    }
}
