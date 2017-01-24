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

using Dapper.Contrib.Extensions;

namespace OpenRetail.Model
{
    /// <summary>
    /// Class model untuk library log4net
    /// </summary>
    [Table("t_logs")]
    public class Log
    {
        public string level { get; set; }
        public string class_name { get; set; }
        public string method_name { get; set; }
        public string message { get; set; }
        public string new_value { get; set; }
        public string old_value { get; set; }
        public string exception { get; set; }
        public string created_by { get; set; }        
    }
}
