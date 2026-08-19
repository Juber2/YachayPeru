using Microsoft.Extensions.Configuration;
using YachayPeru.Application.Abstractions.Services;

namespace YachayPeru.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string webRootPath;
        private readonly string filesPath;

        public LocalFileStorageService(IConfiguration configuration)
        {
            var configured = configuration["Storage:LocalPath"]
                ?? "wwwroot";
            filesPath = configuration["Storage:FilesPath"]
                ?? "defaultFiles";

            webRootPath = Path.IsPathRooted(configured)
                ? configured
                : Path.Combine(Directory.GetCurrentDirectory(), configured);
        }

        public async Task<string> SaveAsync(Stream stream, string fileName, string folder, CancellationToken ct = default)
        {
            var ext = Path.GetExtension(fileName);
            var uniqueName = $"{Guid.NewGuid():N}{ext}";
            var relativePath = Path.Combine(filesPath,folder, uniqueName).Replace('\\', '/');
            var absolutePath = Path.Combine(webRootPath, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);

            await using var fs = new FileStream(absolutePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fs, ct);

            return $"/{relativePath}";
        }

        public void Delete(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;
            var absolutePath = Path.Combine(webRootPath, relativePath.TrimStart('/'));
            if (File.Exists(absolutePath))
                File.Delete(absolutePath);
        }

        public async Task<byte[]?> ReadAsync(string relativePath, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return null;
            var absolutePath = Path.Combine(webRootPath, relativePath.TrimStart('/'));
            if (!File.Exists(absolutePath)) return null;
            return await File.ReadAllBytesAsync(absolutePath, ct);
        }
    }
}
