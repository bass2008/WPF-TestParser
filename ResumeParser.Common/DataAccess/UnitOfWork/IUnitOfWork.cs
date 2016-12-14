namespace ResumeParser.Common.DataAccess.UnitOfWork
{
    /// <summary>
    ///     Единица работы в рамках одной бизнес-транзакции.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Сохранить изменения c логгированем изменений.
        /// </summary>
        void SaveChanges();
    }
}