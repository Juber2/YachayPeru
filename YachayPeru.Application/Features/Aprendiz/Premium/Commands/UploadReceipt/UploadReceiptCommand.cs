using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Premium.Commands.UploadReceipt
{
    public sealed record UploadReceiptCommand : IRequest<Result<string>>
    {
        public int UserId { get; init; }
        public Stream FileStream { get; init; } = Stream.Null;
        public string FileName { get; init; } = string.Empty;
    }
}
