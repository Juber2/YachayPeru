namespace YachayPeru.Application.Abstractions.Persistence.Auth
{
    public record DeletedUserInfo(int Id, string FullName, DateTime? DeletedAt);
}
