using System.Collections.Generic;
using System.Threading.Tasks;
using ResumeParser.Domain.Models;

namespace TestParse.Helpers.Parser
{
    public interface IParser
    {
        Task<List<Resume>> GetResumes();
    }
}
