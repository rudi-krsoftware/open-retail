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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Repository.Service
{
    public sealed class WhereBuilder
    {
        IList<string> _parameters = new List<string>();
        string _query;

        public WhereBuilder(string query)
        {
            this._query = query;
        }

        public void Add(string paramInfo)
        {
            _parameters.Add(paramInfo);
        }

        public void Clear()
        {
            _parameters.Clear();
        }

        public string ToSql(string whereAttribute = "{WHERE}")
        {
            var sb = new StringBuilder();

            sb.Append("WHERE ");

            var index = 0;
            foreach (var param in _parameters)
            {
                if (index < (_parameters.Count - 1))
                {
                    sb.Append(param).Append(" AND\n");
                }
                else
                {
                    sb.Append(param);
                }

                index++;
            }

            _query = _query.Replace(whereAttribute, sb.ToString());

            return _query;
        }    
    }
}
