/**
 * Copyright (C) 2017 Kamarudin (http://coding4ever.net/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * The latest version of this file can be found at https://github.com/rudi-krsoftware/open-retail
 */

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
