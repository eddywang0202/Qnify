//using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Qnify.Utility.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Qnify.Utility
{
    public class DapperUnitOfWork : IUnitOfWorkFactory, IDisposable
    {
        public MySqlConnection Connection { get; set; }
        private bool _disposed;
        public MySqlTransaction Transaction;

        public DapperUnitOfWork(MySqlConnection connection)
        {
            Connection = connection;
            Connection.Open();
        }

        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            if (Connection.State != ConnectionState.Open)
            {
                throw new Exception("ConnectionState is not Open.");
            }
            Transaction = Connection.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            Transaction?.Commit();
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            Transaction?.Rollback();
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (Transaction != null)
                {
                    if (Connection.State != ConnectionState.Closed)
                    {
                        Transaction.Rollback();
                    }
                    Transaction.Dispose();
                }
                if (Connection != null)
                {
                    if (Connection.State != ConnectionState.Closed)
                    {
                        Connection.Close();
                    }
                    Connection.Dispose();
                }
            }

            _disposed = true;
        }

        public List<T> CreateList<T>(IDataReader reader)
        {
            var results = new List<T>();

            while (reader.Read())
            {
                var item = PopulateT<T>(reader);
                results.Add(item);
            }
            return results;
        }

        public T CreateDTO<T>(IDataReader reader)
        {
            var item = Activator.CreateInstance<T>();

            int recordCount = 0;
            while (reader.Read())
            {
                item = PopulateT<T>(reader);
                recordCount++;
            }

            return recordCount == 1 ? item : default(T);
        }

        private T PopulateT<T>(IDataReader reader)
        {
            var item = Activator.CreateInstance<T>();

            //.net core 的Type没有GetProperties方法，移到了TypeInfo中
            //foreach (var property in typeof(T).GetProperties())
            foreach (var property in typeof(T).GetTypeInfo().GetProperties())
            {
                var dnAttribute = property.GetCustomAttributes(typeof(DoNotPopulateAttribute), true).FirstOrDefault() as DoNotPopulateAttribute;
                if (dnAttribute != null)
                {
                    continue;
                }

                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                {
                    Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                }
            }

            return item;
        }

        public Dictionary<string, object> RowToDictionary(dynamic rs)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (var p in rs)
            {
                dict.Add(p.Key, p.Value);
            }

            return dict;
        }
    }
}
