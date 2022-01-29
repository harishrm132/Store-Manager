using System.Collections.Generic;

namespace StoreDataManager.Library.Internal.DataAccess
{
    public interface ISqlDataAccess
    {
        void CommitTransaction();
        void Dispose();
        string GetConnectionString(string name);
        List<T> LoadData<T, U>(string storedProcedure, U paramaters, string connStringName);
        List<T> LoadDataInTransaction<T, U>(string storedProcedure, U paramaters);
        void RollBackTransaction();
        void SaveData<U>(string storedProcedure, U paramaters, string connStringName);
        void SaveDataInTransaction<U>(string storedProcedure, U paramaters);
        void StartTransaction(string connStringName);
    }
}