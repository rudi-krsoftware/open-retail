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

using log4net;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using ClosedXML.Excel;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;

namespace OpenRetail.Bll.Service
{
    public class ImportExportDataCustomerBll : IImportExportDataBll
    {
        private ILog _log;
        private string _fileName;
        private string _workBookName = "customer";

        public ImportExportDataCustomerBll(string fileName, ILog log)
        {
            _fileName = fileName;
            _log = log;
        }

        public bool IsOpened()
        {
            var result = false;

            try
            {
                var wb = new XLWorkbook(_fileName);
            }
            catch
            {
                result = true;
            }

            return result;
        }

        public bool IsValidFormat()
        {
            var result = true;

            try
            {
                var wb = new XLWorkbook(_fileName);
                var ws = wb.Worksheet(_workBookName);

                // Look for the first row used
                var firstRowUsed = ws.FirstRowUsed();

                var colums = new string[] { "NAMA", "ALAMAT", "KONTAK", "TELEPON", "PLAFON PIUTANG" };

                for (int i = 0; i < colums.Length; i++)
                {
                    if (!(colums[i] == firstRowUsed.Cell(i + 1).GetString()))
                    {
                        result = false;
                        break;
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public bool Import(ref int rowCount)
        {
            var result = false;

            try
            {
                var wb = new XLWorkbook(_fileName);
                var ws = wb.Worksheet(_workBookName);

                // Look for the first row used
                var firstRowUsed = ws.FirstRowUsed();

                // Narrow down the row so that it only includes the used part
                var supplierRow = firstRowUsed.RowUsed();

                // First possible address of the company table:
                var firstPossibleAddress = ws.Row(supplierRow.RowNumber()).FirstCell().Address;

                // Last possible address of the company table:
                var lastPossibleAddress = ws.LastCellUsed().Address;

                // Get a range with the remainder of the worksheet data (the range used)
                var customerRange = ws.Range(firstPossibleAddress, lastPossibleAddress).RangeUsed();

                // Treat the range as a table (to be able to use the column names)
                var customerTable = customerRange.AsTable();

                var listOfCustomer = new List<Customer>();

                listOfCustomer = customerTable.DataRange.Rows().Select(row => new Customer
                {
                    nama_customer = row.Field("NAMA").GetString(),
                    alamat = row.Field("ALAMAT").GetString(),
                    kontak = row.Field("KONTAK").GetString(),
                    telepon = row.Field("TELEPON").GetString(),
                    plafon_piutang = row.Field("PLAFON_PIUTANG").GetDouble()

                }).ToList();

                if (listOfCustomer.Count == 1 && listOfCustomer[0].nama_customer.Length == 0)
                {
                    rowCount = 0;
                    return false;
                }

                rowCount = listOfCustomer.Count;

                using (IDapperContext context = new DapperContext())
                {
                    IUnitOfWork uow = new UnitOfWork(context, _log);

                    foreach (var customer in listOfCustomer)
                    {
                        if (customer.nama_customer.Length > 0)
                        {
                            result = Convert.ToBoolean(uow.CustomerRepository.Save(customer));
                        }
                    }
                }

                result = true;
            }
            catch
            {
            }

            return result;
        }

        public void Export()
        {
            throw new NotImplementedException();
        }
    }
}
