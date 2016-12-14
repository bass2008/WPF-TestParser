using System.Collections.Generic;
using ResumeParser.Domain.Models;

namespace TestParse.Helpers.DbHelper
{
    public interface IDbHelper
    {
        void SaveToDb(List<Resume> result);

        List<Resume> GetAll();

        void ClearAll();
    }
}
