using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;
using Qnify.Model.Table;

namespace Qnify.DAL
{
    public class ConstResultPerformanceRepository : BaseRepository
    {
        public ConstResultPerformanceRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<ConstResultPerformance> GetConstResultPerformances()
        {
            const string commandText =
@"SELECT * FROM `const_result_performance`";

            return Conn.Query<ConstResultPerformance>(commandText).ToList();
        }
    }
}
