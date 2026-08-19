using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;

namespace YachayPeru.Application.Features.Noticias.Queries.GetNoticias
{
    public class GetNoticiasHandler : IRequestHandler<GetNoticiasQuery, IReadOnlyList<NoticiaListItem>>
    {
        private readonly INoticiaRepository repository;
        public GetNoticiasHandler(INoticiaRepository _repository) => repository = _repository;

        public async Task<IReadOnlyList<NoticiaListItem>> Handle(GetNoticiasQuery request, CancellationToken ct)
        {
            var noticias = await repository.ListAsync(ct);
            return noticias.Select(n => new NoticiaListItem
            {
                Id = n.Id,
                Title = n.Title,
                Category = n.Category,
                ImageUrl = n.ImageUrl,
                IsActive = n.IsActive,
                CreatedAt = n.CreatedAt
            }).ToList();
        }
    }
}
