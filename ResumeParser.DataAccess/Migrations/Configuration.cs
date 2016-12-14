using System.Data.Entity.Migrations;

namespace ResumeParser.DataAccess.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<SqlServerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SqlServerDbContext context)
        {
            // TODO : Add existing entity here
        }
    }
}
