using MediatR;
using YachayPeru.Application.Actions.Courses;

namespace YachayPeru.Application.Features.Retos.Queries.GetRetosLookup
{
    public record GetRetosLookupQuery : IRequest<IReadOnlyList<RetoLookupItem>>;
}
