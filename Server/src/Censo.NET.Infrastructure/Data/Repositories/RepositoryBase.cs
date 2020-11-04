using Censo.NET.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Censo.NET.Infrastructure.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly CensoContext _censoContext;

        protected RepositoryBase(CensoContext censoContext)
        {
            _censoContext = censoContext;
        }

        public virtual async Task<T> Criar(T t)
        {
            var entity = _censoContext.Set<T>().Add(t);
            await _censoContext.SaveChangesAsync();
            return entity.Entity;
        }

        public virtual async Task Atualizar(T t)
        {
            _censoContext.Entry(await _censoContext.Set<T>().FirstOrDefaultAsync(x => x.Id == t.Id)).CurrentValues.SetValues(t);
            await _censoContext.SaveChangesAsync();
        }

        public virtual async Task<T> Obter(Guid id) =>
            await _censoContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        public virtual async Task<IEnumerable<T>> ObterTodos() =>
            await _censoContext.Set<T>().ToListAsync();
    }
}
