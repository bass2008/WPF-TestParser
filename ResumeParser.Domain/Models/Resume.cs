using ResumeParser.Common.DataAccess.Entity;
using ResumeParser.Domain.Enums;

namespace ResumeParser.Domain.Models
{
    public class Resume : DbElementBase
    {
        public string Name { get; set; }

        public int Salary { get; set; }

        public string Currency { get; set; }

        public int Age { get; set; }

        public Sex Sex { get; set; }

        public string Url { get; set; }
    }
}
