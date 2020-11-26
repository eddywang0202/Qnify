using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class CellService : ICellService
    {
        private static Database _database;

        public CellService()
        {
            _database = new Database(Config.AppSettings);
        }

        public List<Cell> GetCell()
        {
            var response = new List<Cell>();

            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                response = new CellRepository(unitOfWork).GetCell();                
            }                        
            return response;
        }
    }
}
