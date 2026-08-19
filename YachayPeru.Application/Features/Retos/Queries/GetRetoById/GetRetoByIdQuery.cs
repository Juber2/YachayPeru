using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Queries.GetRetoById
{
    public record GetRetoByIdQuery(int RetoId) : IRequest<Result<RetoDetail>>;
}
