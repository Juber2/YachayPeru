using MediatR;

namespace YachayPeru.Application.Features.MasterCodes.Queries.GetMasterCodesByParent
{
    public record GetMasterCodesByParentQuery(string ParentCode) : IRequest<IReadOnlyList<MasterCodeItem>>;
}
