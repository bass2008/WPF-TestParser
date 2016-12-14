using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ResumeParser.Common.DataAccess.Entity;

namespace ResumeParser.Common.DataAccess.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : DbElementBase
    {
        protected readonly DbContext Context;

        protected readonly DbSet<T> Set;

        public GenericRepository(DbContext context)
        {
            Context = context;
            Set = Context.Set<T>();
        }

        public virtual void Add(T item)
        {
            Set.Add(item);
        }

        public virtual void Delete(T item)
        {
            Set.Remove(item);
        }

        public void Delete(int id)
        {
            var entity = Set.First(x => x.Id == id);
            Set.Remove(entity);
        }

        public T Get(int id)
        {
            return Set.FirstOrDefault(c => c.Id == id);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return Set.FirstOrDefault(where);
        }

        public T Get<TProperty>(Expression<Func<T, bool>> where, Expression<Func<T, TProperty>>[] childSelector)
        {
            var temp = Set.AsQueryable();

            for (var i = 0; i < childSelector.Length; i++)
            {
                temp = temp.Include(childSelector[i]);
            }

            return temp.FirstOrDefault();
        }

        public T[] GetAll()
        {
            return Set.Where(c => c.Id > 0).ToArray();
        }

        public T[] GetAll(Expression<Func<T, bool>> where)
        {
            return Set.Where(c => c.Id > 0).Where(where).ToArray();
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public T[] GetAll(
            int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Expression<Func<T, bool>> where)
        {
            return orderBy(Set.Where(c => c.Id > 0).Where(where).Skip(skip).Take(take)).ToArray();
        }

        public T[] Find(Expression<Func<T, bool>> where)
        {
            return Set.Where(where).ToArray();
        }

        public IQueryable<T> GetSet()
        {
            return Set;
        }

        public virtual void Edit(T entity)
        {
            var entry = Context.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Modified;
                return;
            }

            var attachedEntity = Set.Local.SingleOrDefault(c => c.Id == entity.Id);

            if (attachedEntity != null)
            {
                var attachedEntry = Context.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entity);
            }
            else
            {
                // If current entity has attached relationship - attaching an entity will be failed
                entry.State = EntityState.Modified;
            }
        }
    }
}