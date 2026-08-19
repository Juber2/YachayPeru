using Microsoft.EntityFrameworkCore.Storage;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Infrastructure.Persistence.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        private IDbContextTransaction? currentTransaction;


        public UnitOfWork(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public bool HasActiveTransaction() {

            return currentTransaction is not null;
        }

        public async Task BeginTransactionAsync(CancellationToken ct)
        {
            if(currentTransaction is not null){
                return;
            }
            currentTransaction= await dbContext.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitTransactionAsync(CancellationToken ct)
        {
            if (currentTransaction is  null)
            {
                return; //No existe transaccion
            }

            try
            {
                await currentTransaction.CommitAsync(ct);

            }
            finally 
            {
                await currentTransaction.DisposeAsync();
                currentTransaction= null;
            }

        }

        public async Task RollbackTransactionAsync(CancellationToken ct)
        {
            if (currentTransaction is null)
            {
                return; //No existe transaccion
            }

            try
            {
                await currentTransaction.RollbackAsync(ct);

            }
            finally
            {
                await currentTransaction.DisposeAsync();
                currentTransaction = null;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return dbContext.SaveChangesAsync(ct);
        }
    }
}
