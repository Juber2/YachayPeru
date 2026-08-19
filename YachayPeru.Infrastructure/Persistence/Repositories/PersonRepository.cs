using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Common;
using YachayPeru.Domain.Entities.Common;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext context;

        public PersonRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<Person> AddAsync(Person entity, CancellationToken cancellationToken = default)
        {
            await context.Persons.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<Person> entities, CancellationToken cancellationToken = default)
        {
            await context.Persons.AddRangeAsync(entities, cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<Person, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Persons
                .Where(x => !x.Deleted)
                .AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<Person, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Person> query = context.Persons.Where(x => !x.Deleted);
            if (predicate is not null)
                query = query.Where(predicate);
            return await query.CountAsync(cancellationToken);
        }

        public void Delete(Person entity) => context.Persons.Remove(entity);

        public void DeleteRange(IEnumerable<Person> entities) => context.Persons.RemoveRange(entities);

        public async Task<Person?> FirstOrDefaultAsync(Expression<Func<Person, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Persons
                .Where(x => !x.Deleted)
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<Person?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            if (id is not int personId) return null;
            return await context.Persons
                .FirstOrDefaultAsync(x => x.Id == personId && !x.Deleted, cancellationToken);
        }

        public async Task<IReadOnlyList<Person>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await context.Persons
                .Where(x => !x.Deleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Person>> ListAsync(Expression<Func<Person, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Persons
                .Where(x => !x.Deleted)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Person?> SingleOrDefaultAsync(Expression<Func<Person, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Persons
                .Where(x => !x.Deleted)
                .SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public void Update(Person entity) => context.Persons.Update(entity);
    }
}
