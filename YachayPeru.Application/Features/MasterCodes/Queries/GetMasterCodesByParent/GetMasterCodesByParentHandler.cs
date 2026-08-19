using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Common;

namespace YachayPeru.Application.Features.MasterCodes.Queries.GetMasterCodesByParent
{
    public class GetMasterCodesByParentHandler : IRequestHandler<GetMasterCodesByParentQuery, IReadOnlyList<MasterCodeItem>>
    {
        private readonly IMasterCodeRepository _repository;

        public GetMasterCodesByParentHandler(IMasterCodeRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<MasterCodeItem>> Handle(GetMasterCodesByParentQuery request, CancellationToken cancellationToken)
            => _repository.GetByParentCodeAsync(request.ParentCode, cancellationToken);
    }
}
