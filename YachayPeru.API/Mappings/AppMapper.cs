using Riok.Mapperly.Abstractions;
using YachayPeru.API.Contracts.Administration.Users.Request;
using YachayPeru.Application.Features.Administration.Users.Commands.CreateUser;

namespace YachayPeru.API.Mappings
{
    [Mapper]
    public partial class AppMapper
    {
        // ── Users ─────────────────────────────────────────────────────────────
        public partial CreateUserCommand ToCommand(CreateUserRequest request);
    }
}
