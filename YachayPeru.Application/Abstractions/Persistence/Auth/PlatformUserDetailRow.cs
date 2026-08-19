namespace YachayPeru.Application.Abstractions.Persistence.Auth
{
    public record PlatformUserDetailRow(
        int      UserId,
        string   FirstName,
        string   LastName,
        string?  Email,
        string   Username,
        bool     IsLocked,
        int?     RoleId,
        string?  RoleName);
}
