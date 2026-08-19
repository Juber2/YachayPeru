namespace YachayPeru.Application.Abstractions.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(Stream stream, string fileName, string folder, CancellationToken ct = default);
        void Delete(string relativePath);
        Task<byte[]?> ReadAsync(string relativePath, CancellationToken ct = default);
    }
}
