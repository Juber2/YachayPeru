using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Predisenos.Queries.GetPredisenoById
{
    public class GetPredisenoByIdHandler : IRequestHandler<GetPredisenoByIdQuery, Result<PredisenoDetail>>
    {
        private readonly IPredisenoRepository repository;

        public GetPredisenoByIdHandler(IPredisenoRepository _repository) => repository = _repository;

        public async Task<Result<PredisenoDetail>> Handle(GetPredisenoByIdQuery request, CancellationToken ct)
        {
            var e = await repository.GetByIdAsync(request.Id, ct);
            if (e is null)
                return Result<PredisenoDetail>.Failure("Prediseño no encontrado.", NotFound);

            return Result<PredisenoDetail>.Success(new PredisenoDetail
            {
                Id = e.Id,
                Title = e.Title,
                TreeJson = e.TreeJson
            });
        }
    }
}
