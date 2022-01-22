using Dapper;
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
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
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
    }
}
