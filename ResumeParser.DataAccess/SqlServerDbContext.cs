using System.Data.Entity;
using ResumeParser.DataAccess.Migrations;
using ResumeParser.Domain.Models;

namespace ResumeParser.DataAccess
{
    public class SqlServerDbContext : DbContext
    {
        static SqlServerDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SqlServerDbContext, Configuration>());
        }

        public SqlServerDbContext()
            : base("ParserSqlServer")
        { }

        public DbSet<Resume> Resumes { get; set; }        
    }
}