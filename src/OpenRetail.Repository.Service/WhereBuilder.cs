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
