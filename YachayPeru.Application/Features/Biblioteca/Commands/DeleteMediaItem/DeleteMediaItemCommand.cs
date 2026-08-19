using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Biblioteca.Commands.DeleteMediaItem
{
    public record DeleteMediaItemCommand(int Id) : IRequest<Result>;
}
