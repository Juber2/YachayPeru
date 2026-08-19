namespace YachayPeru.Application.Abstractions.Persistence.Auth
{
    public record PlatformUserListRow(
        int      UserId,
        string   FirstName,
        string   LastName,
        string?  Email,
        bool     IsLocked,
        string?  RoleName,
        string?  RoleCode,
        DateTime? LastAccess);
}
