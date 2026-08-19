using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Application.Abstractions.Persistence
{
    public interface IRepository<T> : IReadRepository<T> where T : class
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void Update(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);
    }
}
