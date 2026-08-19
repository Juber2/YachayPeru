namespace YachayPeru.Application.Abstractions.Services
{
    public interface ICurrentUser
    {
        int    Id           { get; }
        string Username     { get; }
        string Email        { get; }
        string FullName     { get; }
        string UserTypeCode { get; }
    }
}
