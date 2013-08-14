using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebChat.Data.Repositories
{
    public interface IRepositoryAsync<T>
        where T : class
    {
        Task<IEnumerable<T>> All();

        Task<T> Get(int id);

        Task Add(T item);

        Task<bool> Remove(int id);

        Task<bool> Update(int id, T item);
    }
}