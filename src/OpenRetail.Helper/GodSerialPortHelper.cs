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

using OpenRetail.Model;

namespace OpenRetail.Helper
{
    public enum Line : byte
    {
        One = 0x31,
        Two = 0x32,
        Three = 0x33,
        Four = 0x34
    };

    public static class GodSerialPortHelper
    {
        public static bool IsConnected(GodSerialPort serial, SettingPort settingPort)
        {
            serial = new GodSerialPort(settingPort.portNumber, settingPort.baudRate, settingPort.parity, settingPort.dataBits, settingPort.stopBits);
            return serial.Open();
        }

        public static void SendStringToCustomerDisplay(string displayLine1, string displayLine2, GodSerialPort serialPort)
        {
            serialPort.ClearScreen();

            serialPort.SetCursorToLine(Line.One);
            serialPort.WriteAsciiString(displayLine1);

            serialPort.SetCursorToLine(Line.Two);
            serialPort.WriteAsciiString(displayLine2);

            serialPort.Close();
        }

        /// <summary>
        /// Clears the entire display and sets the cursor to line 1 column 1.
        /// </summary>
        private static void ClearScreen(this GodSerialPort serial)
        {
            var command = new byte[4] { 0x1B, 0x5B, 0x32, 0x4A };

            try
            {
                serial.Write(command, 0, 4);
                //SetCursorToLine(serial, Line.One);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Sets the cursor to line 1 column 1.
        /// </summary>
        private static void SetCursorToLine(this GodSerialPort serial, Line lineNumber)
        {
            var command = new byte[6] { 0x1B, 0x5B, (byte)lineNumber, 0x3B, 0x31, 0x48 };
            serial.Write(command, 0, 6);
        }
    }
}