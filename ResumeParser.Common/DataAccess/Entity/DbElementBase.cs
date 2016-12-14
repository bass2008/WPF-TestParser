namespace ResumeParser.Common.DataAccess.Entity
{
    public abstract class DbElementBase
    {
        public int Id { get; set; }

        public bool IsNew => Id == 0;
    }
}