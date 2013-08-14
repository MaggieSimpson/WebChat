using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace WebChat.Data.Repositories
{
    public class DatabaseRepository<T> : IRepository<T>
        where T : class
    {
        protected DbContext db { get; set; }

        public DatabaseRepository(DbContext context)
        {
            this.db = context;
        }

        public IEnumerable<T> All()
        {
            return this.db.Set<T>().AsQueryable<T>();
        }

        public T Get(int id)
        {
            return this.db.Set<T>().Find(id);
        }

        public void Add(T item)
        {
            this.db.Set<T>().Add(item);
            this.db.SaveChanges();
        }

        public bool Remove(int id)
        {
            if (this.db.Set<T>().Find(id) == null)
            {
                return false;
            }

            var entity = this.Get(id);
            this.db.Set<T>().Remove(entity);
            this.db.SaveChanges();
            return true;
        }

        public bool Update(int id, T item)
        {
            if (this.db.Set<T>().Find(id) == null)
            {
                return false;
            }

            var entry = this.Get(id);

            foreach (var prop in item.GetType().GetProperties())
            {
                prop.SetValue(entry, prop.GetValue(item));
            }

            this.db.SaveChanges();
            return true;
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}