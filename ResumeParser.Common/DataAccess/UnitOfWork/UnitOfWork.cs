using System;
using System.Data.Entity;

namespace ResumeParser.Common.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly DbContext Context;

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }

        public void Dispose()
        {
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                // TODO: Log your exception here
                throw ex; 
            }
        }
    }
}