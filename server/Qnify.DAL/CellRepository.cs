using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class CellRepository : BaseRepository
    {
        public CellRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<Cell> GetCell()
        {
            const string commandText =
@"SELECT
`id`,
`row`,
`column`,
`grid_id` as GridId,
`cell_property_json` as CellPropertyJson
FROM `cell`";

            var result = Conn.Query<Cell>(commandText).ToList();
            return result;
        }
    }
}
