using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;

namespace YachayPeru.Application.Features.Predisenos.Queries.GetPredisenos
{
    public class GetPredisenosHandler : IRequestHandler<GetPredisenosQuery, IReadOnlyList<PredisenoListItem>>
    {
        private readonly IPredisenoRepository repository;

        public GetPredisenosHandler(IPredisenoRepository _repository) => repository = _repository;

        public async Task<IReadOnlyList<PredisenoListItem>> Handle(GetPredisenosQuery request, CancellationToken ct)
        {
            var entities = await repository.ListAsync(ct);
            return entities.Select(e => new PredisenoListItem
            {
                Id = e.Id,
                Title = e.Title,
                TreeJson = e.TreeJson,
                CreatedAt = e.CreatedAt
            }).ToList();
        }
    }
}
