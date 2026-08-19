using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Noticias.Queries.GetNoticiaById
{
    public class GetNoticiaByIdHandler : IRequestHandler<GetNoticiaByIdQuery, Result<NoticiaDetail>>
    {
        private readonly INoticiaRepository repository;
        public GetNoticiaByIdHandler(INoticiaRepository _repository) => repository = _repository;

        public async Task<Result<NoticiaDetail>> Handle(GetNoticiaByIdQuery request, CancellationToken ct)
        {
            var n = await repository.GetByIdAsync(request.Id, ct);
            if (n is null)
                return Result<NoticiaDetail>.Failure("Noticia no encontrada.", NotFound);

            return Result<NoticiaDetail>.Success(new NoticiaDetail
            {
                Id = n.Id,
                Title = n.Title,
                Category = n.Category,
                Body = n.Body,
                ImageUrl = n.ImageUrl,
                IsActive = n.IsActive
            });
        }
    }
}
