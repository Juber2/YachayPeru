using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest { }

    public interface ICommand<out TResponse> : IRequest<TResponse> { }

    public interface IQuery<out TResponse> : IRequest<TResponse> { }

    public interface ITransactionalCommand<out TResponse> : ICommand<TResponse> { }
}
