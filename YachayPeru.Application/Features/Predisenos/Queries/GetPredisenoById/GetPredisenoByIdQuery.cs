using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Predisenos.Queries.GetPredisenoById
{
    public record GetPredisenoByIdQuery(int Id) : IRequest<Result<PredisenoDetail>>;

    public class PredisenoDetail
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TreeJson { get; set; } = string.Empty;
    }
}
