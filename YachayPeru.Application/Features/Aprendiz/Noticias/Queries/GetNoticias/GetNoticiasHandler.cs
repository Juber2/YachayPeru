using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;

namespace YachayPeru.Application.Features.Aprendiz.Noticias.Queries.GetNoticias
{
    public class GetNoticiasHandler : IRequestHandler<GetNoticiasQuery, IReadOnlyList<AprendizNoticiaListItem>>
    {
        private readonly INoticiaRepository repository;
        public GetNoticiasHandler(INoticiaRepository _repository) => repository = _repository;

        public async Task<IReadOnlyList<AprendizNoticiaListItem>> Handle(GetNoticiasQuery request, CancellationToken ct)
        {
            var noticias = await repository.ListAsync(n => n.IsActive, ct);
            return noticias
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new AprendizNoticiaListItem
                {
                    Id = n.Id,
                    Title = n.Title,
                    Category = n.Category,
                    ImageUrl = n.ImageUrl,
                    CreatedAt = n.CreatedAt,
                    Body=n.Body
                }).ToList();
        }
    }
}
