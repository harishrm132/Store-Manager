using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess : IDisposable
    {
        public SqlDataAccess(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetConnectionString(string name)
        {
            //TODO - Create a constant for Connection String Name to use Everywhere
            return configuration.GetConnectionString(name);
        }

        public List<T> LoadData<T, U>(string storedProcedure, U paramaters, string connStringName)
        {
            string connString = GetConnectionString(connStringName);
            using (IDbConnection conn = new SqlConnection(connString))
            {
                List<T> rows = conn.Query<T>(storedProcedure, paramaters, commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }
        
        public void SaveData<U>(string storedProcedure, U paramaters, string connStringName)
        {
            string connString = GetConnectionString(connStringName);
            using (IDbConnection conn = new SqlConnection(connString))
            {
                conn.Execute(storedProcedure, paramaters, commandType: CommandType.StoredProcedure);
            }
        }

        //Open Connection/Start Transaction method
        //load using Transaction
        //save using Transaction
        //Close connection/stop Transaction method
        //Dispose 

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction(string connStringName)
        {
            string connString = GetConnectionString(connStringName);
            _connection = new SqlConnection(connString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U paramaters)
        {
            List<T> rows = _connection.Query<T>(storedProcedure, paramaters, commandType: CommandType.StoredProcedure,
                transaction: _transaction).ToList();
            return rows;
        }

        public void SaveDataInTransaction<U>(string storedProcedure, U paramaters)
        {
            _connection.Execute(storedProcedure, paramaters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        private bool isClosed = false;
        private readonly IConfiguration configuration;

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
            isClosed = true;
        }

        public void RollBackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            isClosed = true;
        }

        public void Dispose()
        {
            if (isClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    //TODO - Log this Issue
                }
            }

            _transaction = null;
            _connection = null;
        }
    }
}
