using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Noticias.Commands.DeleteNoticia
{
    public record DeleteNoticiaCommand(int Id) : IRequest<Result>;
}
