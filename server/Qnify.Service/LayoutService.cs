using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class LayoutService : ILayoutService
    {
        private static Database _database;

        public LayoutService()
        {
            _database = new Database(Config.AppSettings);
        }

        public LayoutsResponse GetLayouts()
        {
            var result = new LayoutsResponse();
            List<Layout> layoutList;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                layoutList = new LayoutRepository(unitOfWork).GetLayouts();                
            }
            result.Layouts = layoutList;
            return result;
        }
    }
}
