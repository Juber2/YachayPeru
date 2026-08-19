using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using YachayPeru.Application.Abstractions.Messaging;
using YachayPeru.Application.Abstractions.Persistence;

namespace YachayPeru.Application.Common.Behaviors
{
    public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionalCommand<TResponse>
    {
        private readonly IUnitOfWork _uow;

        public TransactionBehavior(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            try
            {
                var response = await next();

                return response;
            }
            catch
            {
                if (_uow.HasActiveTransaction())
                {
                    await _uow.RollbackTransactionAsync(ct);
                }
                throw;
            }
        }

    }
}
