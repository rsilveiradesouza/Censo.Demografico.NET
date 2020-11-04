using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Censo.NET.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Obter(Guid id);
        Task<IEnumerable<T>> ObterTodos();
        Task<T> Criar(T t);
        Task Atualizar(T t);
    }
}
