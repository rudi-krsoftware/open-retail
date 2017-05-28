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
    public static class ESCCommandHelper
    {
        public static string InitializePrinter()
        {
            var sb = new StringBuilder();
            sb.Append((char)27);
            sb.Append((char)64);

            return sb.ToString();
        }

        public static string CutPaper()
        {
            var sb = new StringBuilder();
            sb.Append((char)27); // ESC
            sb.Append((char)64); // @
            sb.Append((char)29); // GS
            sb.Append((char)86); // V
            sb.Append((char)1);

            return sb.ToString();
        }

        public static string LineSpacing(int spacing = 20)
        {
            var sb = new StringBuilder();
            sb.Append((char)27); // Chr$(&H1B)
            sb.Append((char)51); // "3"
            sb.Append((char)spacing); // Chr$(20)

            return sb.ToString();
        }

        public static string LineFeed(int line)
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= line; i++)
            {
                sb.Append((char)10); // Chr$(&HA)
            }

            return sb.ToString();
        }

        public static string LeftText()
        {
            var sb = new StringBuilder();
            sb.Append((char)27); // Chr$(&H1B)
            sb.Append((char)97); // "a"
            sb.Append((char)0); // Chr$(0)

            return sb.ToString();
        }

        public static string CenterText()
        {
            var sb = new StringBuilder();
            sb.Append((char)27); // Chr$(&H1B)
            sb.Append((char)97); // "a"
            sb.Append((char)1); // Chr$(1)

            return sb.ToString();
        }

        public static string FontBold()
        {
            var sb = new StringBuilder();
            sb.Append((char)27); // Chr$(&H1B
            sb.Append((char)33); // !
            sb.Append((char)24); // Chr$(24)

            return sb.ToString();
        }

        public static string FontNormal()
        {
            var sb = new StringBuilder();
            sb.Append((char)27); // Chr$(&H1B
            sb.Append((char)33); // !
            sb.Append((char)0); // Chr$(0)

            return sb.ToString();
        }
    }
}
