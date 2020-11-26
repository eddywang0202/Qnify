using System.Data;

namespace Qnify.Utility.Interface
{
    public interface IUnitOfWorkFactory
    {
        IDbCommand CreateCommand();
        void SaveChanges();
        void Dispose();
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
