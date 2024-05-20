
using ECommereceSiteData.Data;
using ECommereceSiteData.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace ECommereceSiteData.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        } 
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        // retrieves a single record from a data source, likely a database table.
        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            //IQueryable<T>: This interface represents a sequence of elements of type T that can be queried further.
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        //retrieves all elements from a data source and returns them as an IEnumerable<T>
        //IEnumerable<T>:The return type specifies that the method returns a collection of elements of type T.
        //IEnumerable<T> represents an interface that allows you to iterate over the
        //elements in the collection, but it doesn't guarantee any specific order or
        //the ability to modify the elements.
        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            //retrieves all elements from the query and converts them to a list.
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);   
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
