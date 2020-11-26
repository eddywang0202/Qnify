using Qnify.DAL;
using Qnify.Model;
using Qnify.Service.Interface;
using Qnify.Utility;
using System.Collections.Generic;

namespace Qnify.Service
{
    public class CasePropertyService : ICasePropertyService
    {
        private static Database _database;

        public CasePropertyService()
        {
            _database = new Database(Config.AppSettings);
        }

        public CasePropertiesResponse GetCaseProperties()
        {
            var result = new CasePropertiesResponse();
            List<CaseProperty> casePropertyList;
            using (var unitOfWork = UnitOfWorkFactory.Create(_database.Default))
            {
                casePropertyList = new CasePropertyRepository(unitOfWork).GetCaseProperties();                
            }
            result.CaseProperties = casePropertyList;
            return result;
        }
    }
}
