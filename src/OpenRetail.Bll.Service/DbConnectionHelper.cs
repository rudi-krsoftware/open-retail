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

using OpenRetail.Bll.Api;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;

namespace OpenRetail.Bll.Service
{
    public class DbConnectionHelper : IDbConnectionHelper
    {
        public bool IsOpenConnection()
        {
            using (IDapperContext context = new DapperContext())
            {
                return context.IsOpenConnection();
            }
        }

        public bool ExecSQL(string sql)
        {
            var result = false;

            using (IDapperContext context = new DapperContext())
            {
                if (context.IsOpenConnection())
                {
                    result = context.ExecSQL(sql);
                }
            }

            return result;
        }
    }
}