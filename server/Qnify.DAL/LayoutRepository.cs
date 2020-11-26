using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class LayoutRepository : BaseRepository
    {
        public LayoutRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<Layout> GetLayouts()
        {
            const string commandText =
@"SELECT `id`, `row`, `column`
FROM `layout`";

            var result = Conn.Query<Layout>(commandText).ToList();
            return result;
        }
    }
}
