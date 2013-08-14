using System;
using System.Collections.Generic;

namespace WebChat.Data.Repositories
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> All();

        T Get(int id);

        void Add(T item);

        bool Remove(int id);

        bool Update(int id, T item);
    }
}