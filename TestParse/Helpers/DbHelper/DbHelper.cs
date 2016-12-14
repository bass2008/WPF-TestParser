using System.Collections.Generic;
using System.Linq;
using ResumeParser.Common.DataAccess.Repository;
using ResumeParser.Common.DataAccess.UnitOfWork;
using ResumeParser.DataAccess;
using ResumeParser.Domain.Models;

namespace TestParse.Helpers.DbHelper
{
    public class DbHelper : IDbHelper
    {
        public void SaveToDb(List<Resume> result)
        {
            // Из-за отсутсвтия внедрения зависимостей, мы вынуждены управлять жизненным циклом зависимостей.
            // Это нарушает SRP (SOLID), т.к. обязаность управления жизненным циклом можно переложить на DI-контейнер
            using (var dbContext = new SqlServerDbContext())
            {
                // В принципе UnitOfWork тут не нужен. Это неправильный пример использования UnitOfWork, поскольку нет DI-контейнера
                // Но мы выбираем из двух зол меньшее, т.к. работать без репозитория напрямую через контекст ещё хуже.
                var repository = new GenericRepository<Resume>(dbContext);
                var unitOfWork = new UnitOfWork(dbContext);

                foreach (var item in result)
                {
                    if(repository.GetSet().Any(x => x.Url == item.Url))
                        continue;
                    repository.Add(item);
                }

                unitOfWork.SaveChanges();
            }
        }

        public List<Resume> GetAll()
        {
            using (var dbContext = new SqlServerDbContext())
            {
                var repository = new GenericRepository<Resume>(dbContext);
                return repository.GetAll().ToList();
            }
        }

        public void ClearAll()
        {
            using (var dbContext = new SqlServerDbContext())
            {
                var repository = new GenericRepository<Resume>(dbContext);
                var unitOfWork = new UnitOfWork(dbContext);

                var all = repository.GetAll();
                foreach (var item in all)
                    repository.Delete(item);

                unitOfWork.SaveChanges();
            }
        }
    }
}
