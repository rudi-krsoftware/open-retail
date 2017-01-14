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

using log4net.Core;
using log4net.Appender;

using Dapper.Contrib.Extensions;
using OpenRetail.Model;
using OpenRetail.Repository.Api;

namespace OpenRetail.Repository.Service
{
    public class Log4NetRepository : ILog4NetRepository
    {
        private IDapperContext _context;

        public Log4NetRepository(IDapperContext context)
        {
            this._context = context;
        }

        public int Save(Log obj)
        {
            var result = 0;

            try
            {
                _context.db.Insert<Log>(obj);
                result = 1;
            }
            catch
            {
            }

            return result;
        }

        public int Update(Log obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(Log obj)
        {
            throw new NotImplementedException();
        }

        public IList<Log> GetAll()
        {
            throw new NotImplementedException();
        }
    }

    public class Log4NetAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var appName = (loggingEvent.LookupProperty("AppName") != null) ? loggingEvent.LookupProperty("AppName").ToString() : string.Empty;
            var createdBy = (loggingEvent.LookupProperty("Operator") != null) ? loggingEvent.LookupProperty("Operator").ToString() : string.Empty;

            var log = new Log
            {
                app_name = appName,
                thread = loggingEvent.ThreadName,
                level = loggingEvent.Level.ToString(),
                location = loggingEvent.LocationInformation.FullInfo,
                message = loggingEvent.RenderedMessage,
                exception = loggingEvent.GetExceptionString(),
                created_by = createdBy
            };

            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, null);
                result = uow.Log4NetRepository.Save(log);
            }
        }
    }
}
