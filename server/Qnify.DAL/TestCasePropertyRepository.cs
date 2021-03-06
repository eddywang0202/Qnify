﻿using Dapper;
using Qnify.Model;
using Qnify.Utility;
using Qnify.Utility.Interface;
using System.Linq;
using System.Collections.Generic;

namespace Qnify.DAL
{
    public class TestCasePropertyRepository : BaseRepository
    {
        public TestCasePropertyRepository(IUnitOfWorkFactory iUnitOfWork) : base(iUnitOfWork)
        {
        }

        public List<TestCaseProperty> GetTestCaseProperties()
        {
            const string commandText =
@"SELECT `id`,
`name`,
`value`
FROM `test_case_property`";

            var result = Conn.Query<TestCaseProperty>(commandText).ToList();
            return result;
        }
    }
}
