using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Text;

namespace Qnify.Utility
{
    public class UnitOfWorkFactory
    {
        /// <summary>
        /// Create SQL server connection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="createTransaction"></param>
        /// <returns></returns>
        public static AdoNetUnitOfWork Create(string connectionString, bool createTransaction)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();

            return new AdoNetUnitOfWork(connection, createTransaction);
        }

        /// <summary>
        /// Create MySql connection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DapperUnitOfWork Create(string connectionString)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            return new DapperUnitOfWork(conn);
        }
    }
}
