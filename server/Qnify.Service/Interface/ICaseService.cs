using Qnify.Model;
using System.Collections.Generic;

namespace Qnify.Service.Interface
{
    public interface ICaseService
    {
        CasesResponse GetCases(List<uint> caseIds);
        uint Insert(uint cellId, string addCellPropJson, string imageName);
        bool Delete(List<uint> caseIds);
        bool Update(uint testSetId, uint cellId, string addCellPropJson, string imagePath);
    }
}
