using Qnify.Utility.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Qnify.Utility
{
    public class AdoNetUnitOfWork : IUnitOfWorkFactory, IDisposable
    {
        public IDbConnection _connection { get; set; }
        public IDbTransaction _transaction { get; set; }
        public bool _createTransaction { get; set; }
        private bool disposed = false;

        public AdoNetUnitOfWork(IDbConnection connection, bool createTransaction)
        {
            _connection = connection;
            _createTransaction = createTransaction;
            _transaction = _createTransaction ? connection.BeginTransaction() : null;
        }

        public IDbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            command.Transaction = _transaction;
            return command;
        }

        public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException();

            _transaction.Commit();
            _transaction = null;
        }

        public void BeginTransaction()
        {

        }
        public void Commit()
        {
        }
        public void Rollback()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                }

                if (_connection != null)
                    _connection.Dispose();
            }

            disposed = true;
        }
    }
}
