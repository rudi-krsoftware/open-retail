using System;
using System.Data;
 
namespace OpenRetail.Repository.Api
{    
    public interface IDapperContext : IDisposable
    {
        IDbConnection db { get; }
		IDbTransaction transaction { get; }
		bool IsOpenConnection();
        void ExecSQL(string sql);

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();

		int GetGeneratorIDByTable(string tableName);
        int GetGeneratorIDByTable(string tableName, IDbTransaction transaction);

        int GetGeneratorIDByName(string generatorName);
        int GetGeneratorIDByName(string generatorName, IDbTransaction transaction);
        string GetLastKode(string tableOrGeneratorName, bool isUseGeneratorName = false);

		string GetGUID();
		string GetLastNota(string prefix, int lastNota);
        string GetLastNota(string prefix, string tableOrGeneratorName, bool isUseGeneratorName = false);
    }
}
