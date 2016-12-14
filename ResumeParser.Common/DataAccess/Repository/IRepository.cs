using System;
using System.Linq;
using System.Linq.Expressions;
using ResumeParser.Common.DataAccess.Entity;

namespace ResumeParser.Common.DataAccess.Repository
{
    public interface IRepository<T> : IDisposable where T : DbElementBase
    {
        T Get(int id);

        T Get(Expression<Func<T, bool>> where);

        void Edit(T item);

        T Get<TProperty>(Expression<Func<T, bool>> where, Expression<Func<T, TProperty>>[] childSelector);

        T[] GetAll();

        IQueryable<T> GetSet();

        T[] GetAll(Expression<Func<T, bool>> where);
        
        void Add(T item);

        void Delete(T item);

        void Delete(int id);
    }
}