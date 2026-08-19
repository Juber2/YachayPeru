using MediatR;
using YachayPeru.Application.Actions.Courses;

namespace YachayPeru.Application.Features.Retos.Queries.GetRetos
{
    public record GetRetosQuery(int CourseId) : IRequest<IReadOnlyList<RetoListItem>>;
}
