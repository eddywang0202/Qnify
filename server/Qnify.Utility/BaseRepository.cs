using MySql.Data.MySqlClient;
using Qnify.Utility.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qnify.Utility
{
    public abstract class BaseRepository
    {
        public IUnitOfWorkFactory UnitOfWorkFactory;
        public DapperUnitOfWork DapperUnitOfWork;
        public int DefaultReportTimeout = 90;
        protected MySqlConnection Conn;
        protected MySqlTransaction Transaction;

        //static BaseRepository()
        //{
        //    //Initialize Dapper FluentMapper
        //    FluentMapper.Initialize(config =>
        //    {
        //        config.AddConvention<Convention>()
        //            .ForEntitiesInAssembly(typeof(OperatorModel).GetTypeInfo().Assembly);//OperatorModel - Either one of the class since all have same assembly
        //    });
        //}

        protected BaseRepository(IUnitOfWorkFactory iUnitOfWork)
        {
            UnitOfWorkFactory = iUnitOfWork;
            if (UnitOfWorkFactory == null) throw new ArgumentNullException(nameof(iUnitOfWork));

            DapperUnitOfWork = iUnitOfWork as DapperUnitOfWork;

            if (DapperUnitOfWork == null) return;
            Conn = DapperUnitOfWork.Connection;
            Transaction = DapperUnitOfWork.Transaction;
        }
    }
}
