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

namespace OpenRetail.App.Helper
{
    public static class DatabaseVersionHelper
    {
        /// <summary>
        /// Versi database yang terakhir
        /// </summary>
        public const int DatabaseVersion = 6;

        /// <summary>
        /// Script SQL untuk mengupgrade database v1 ke v2
        /// </summary>
        public const string UpgradeStrukturDatabase_v1_to_v2 = "db_v1_to_v2.sql";

        /// <summary>
        /// Script SQL untuk mengupgrade database v2 ke v3
        /// </summary>
        public const string UpgradeStrukturDatabase_v2_to_v3 = "db_v2_to_v3.sql";

        /// <summary>
        /// Script SQL untuk mengupgrade database v3 ke v4
        /// </summary>
        public const string UpgradeStrukturDatabase_v3_to_v4 = "db_v3_to_v4.sql";

        /// <summary>
        /// Script SQL untuk mengupgrade database v4 ke v5
        /// </summary>
        public const string UpgradeStrukturDatabase_v4_to_v5 = "db_v4_to_v5.sql";

        /// <summary>
        /// Script SQL untuk mengupgrade database v5 ke v6
        /// </summary>
        public const string UpgradeStrukturDatabase_v5_to_v6 = "db_v5_to_v6.sql";
    }
}
