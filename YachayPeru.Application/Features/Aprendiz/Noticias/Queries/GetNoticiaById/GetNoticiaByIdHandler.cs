using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Noticias.Queries.GetNoticiaById
{
    public class GetNoticiaByIdHandler : IRequestHandler<GetNoticiaByIdQuery, Result<AprendizNoticiaDetail>>
    {
        private readonly INoticiaRepository repository;
        public GetNoticiaByIdHandler(INoticiaRepository _repository) => repository = _repository;

        public async Task<Result<AprendizNoticiaDetail>> Handle(GetNoticiaByIdQuery request, CancellationToken ct)
        {
            var n = await repository.GetByIdAsync(request.Id, ct);
            if (n is null || !n.IsActive)
                return Result<AprendizNoticiaDetail>.Failure("Noticia no encontrada.", NotFound);

            return Result<AprendizNoticiaDetail>.Success(new AprendizNoticiaDetail
            {
                Id = n.Id,
                Title = n.Title,
                Category = n.Category,
                Body = n.Body,
                ImageUrl = n.ImageUrl,
                CreatedAt = n.CreatedAt
            });
        }
    }
}
