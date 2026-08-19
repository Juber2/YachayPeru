namespace YachayPeru.Application.Abstractions.Services
{
    public interface IInsigniaEvaluator
    {
        Task EvaluateAsync(int userId, CancellationToken ct = default);
    }
}
