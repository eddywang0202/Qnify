using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class CaseService : ICaseService
    {
        private static Database _database;

        public CaseService()
        {
            _database = new Database(Config.AppSettings);
        }

        public CasesResponse GetCases(List<uint> caseIds)
        {
            var result = new CasesResponse();
            List<Case> caseList;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                caseList = new CaseRepository(unitOfWork).GetCases(caseIds);
            }
            result.Cases = caseList;
            return result;
        }

        public uint Insert(uint cellId, string addCellPropJson, string imageName)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new CaseRepository(unitOfWork).Insert(cellId, addCellPropJson, imageName);
            }
        }

        public bool Delete(List<uint> caseIds)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new CaseRepository(unitOfWork).Delete(caseIds);
            }
        }

        public bool Update(uint testSetId, uint cellId, string addCellPropJson, string imagePath)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                return new CaseRepository(unitOfWork).Update(testSetId, cellId, addCellPropJson, imagePath);
            }
        }
    }
}
