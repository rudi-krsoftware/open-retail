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
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace OpenRetail.Model
{
    public class SettingPort
    {
        public SettingPort()
        {
            portNumber = "COM1";
            baudRate = 9600;
            parity = Parity.Odd;
            dataBits = 8;
            stopBits = StopBits.One;
        }

        public string portNumber { get; set; }
        public int baudRate { get; set; }
        public Parity parity { get; set; }
        public int dataBits { get; set; }
        public StopBits stopBits { get; set; }
    }
}
