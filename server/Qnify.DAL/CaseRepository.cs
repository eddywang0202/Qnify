using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class CaseRepository : BaseRepository
    {
        public CaseRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<Case> GetCases(List<uint> caseIds)
        {
            const string commandText =
@"SELECT * FROM `case` Where `id` in @CaseIds";

            var param = new DynamicParameters();
            param.Add("@CaseIds", caseIds);
            var result = Conn.Query<Case>(commandText, param).ToList();
            return result;
        }

        public bool Delete(List<uint> caseIds)
        {
            const string commandText =
@"DELETE FROM `case` where id in @CaseIds;";

            var param = new DynamicParameters();
            param.Add("@CaseIds", caseIds);
            return Conn.Execute(commandText, param) > 0;
        }

        public bool Update(uint testSetId, uint cellId, string addCellPropJson, string imagePath)
        {
            const string commandText =
@"UPDATE `case`
SET
additional_cell_property_json = @AdditionalCellPropertyJson,
image_path = @ImagePath
WHERE id IN (SELECT case_id FROM test_question WHERE test_set_id = @testSetId)
AND `cell_id` = @CellId;";

            var param = new DynamicParameters();
            param.Add("@AdditionalCellPropertyJson", addCellPropJson);
            param.Add("@ImagePath", imagePath);
            param.Add("@testSetId", testSetId);
            param.Add("@CellId", cellId);
            return Conn.Execute(commandText, param) > 0;
        }

        public uint Insert(uint cellId, string addCellPropJson, string imageName)
        {
            const string commandText =
@"INSERT INTO `case`
(`cell_id`,`additional_cell_property_json`,`image_path`)
VALUES
(@CellId, @AdditionalCellPropertyJson, @ImagePath);
select LAST_INSERT_ID();";
            var param = new DynamicParameters();
            param.Add("@CellId", cellId);
            param.Add("@AdditionalCellPropertyJson", addCellPropJson);
            param.Add("@ImagePath", imageName);

            return Conn.Query<uint>(commandText, param).FirstOrDefault();
        }
    }
}
