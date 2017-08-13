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
using System.IO;
using System.Diagnostics;

namespace OpenRetail.Bll.Service
{
    public class ImportExportDataCustomerBll : IImportExportDataBll<Customer>
    {
        private ILog _log;
        private string _fileName;
        private XLWorkbook _workbook;

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
                _workbook = new XLWorkbook(_fileName);
            }
            catch
            {
                result = true;
            }

            return result;
        }

        public bool IsValidFormat(string workSheetName)
        {
            var result = true;

            try
            {
                var ws = _workbook.Worksheet(workSheetName);

                // Look for the first row used
                var firstRowUsed = ws.FirstRowUsed();

                var colums = new string[] { 
                                            "NAMA", "ALAMAT", "DESA", "KELURAHAN", "KECAMATAN", "KOTA", "KABUPATEN", 
                                            "KODE POS", "KONTAK", "TELEPON", "DISKON RESELLER", "PLAFON PIUTANG" 
                                          };

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

        public bool Import(string workSheetName, ref int rowCount)
        {
            var result = false;

            try
            {
                var ws = _workbook.Worksheet(workSheetName);

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
                    desa = row.Field("DESA").GetString(),
                    kelurahan = row.Field("KELURAHAN").GetString(),
                    kecamatan = row.Field("KECAMATAN").GetString(),                    
                    kota = row.Field("KOTA").GetString(),
                    kabupaten = row.Field("KABUPATEN").GetString(),
                    kode_pos = row.Field("KODE POS").GetString(),
                    kontak = row.Field("KONTAK").GetString(),
                    telepon = row.Field("TELEPON").GetString(),
                    diskon = row.Field("DISKON RESELLER").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("DISKON RESELLER").GetString()),
                    plafon_piutang = row.Field("PLAFON PIUTANG").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("PLAFON PIUTANG").GetString())
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
                            if (customer.nama_customer.Length > 50)
                                customer.nama_customer = customer.nama_customer.Substring(0, 50);

                            if (customer.alamat.Length > 250)
                                customer.alamat = customer.alamat.Substring(0, 250);

                            if (customer.kontak.Length > 50)
                                customer.kontak = customer.kontak.Substring(0, 50);

                            if (customer.telepon.Length > 20)
                                customer.telepon = customer.telepon.Substring(0, 20);

                            result = Convert.ToBoolean(uow.CustomerRepository.Save(customer));
                        }
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }
            finally
            {
                _workbook.Dispose();
            }

            return result;
        }

        public void Export(IList<Customer> listOfObject)
        {
            try
            {
                // Creating a new workbook
                using (var wb = new XLWorkbook())
                {
                    // Adding a worksheet
                    var ws = wb.Worksheets.Add("customer");

                    // Set header table
                    ws.Cell(1, 1).Value = "NO";
                    ws.Cell(1, 2).Value = "NAMA";
                    ws.Cell(1, 3).Value = "ALAMAT";
                    ws.Cell(1, 4).Value = "KECAMATAN";
                    ws.Cell(1, 5).Value = "KELURAHAN";
                    ws.Cell(1, 6).Value = "KOTA";
                    ws.Cell(1, 7).Value = "KODE POS";
                    ws.Cell(1, 8).Value = "KONTAK";
                    ws.Cell(1, 9).Value = "TELEPON";
                    ws.Cell(1, 10).Value = "DISKON RESELLER";
                    ws.Cell(1, 11).Value = "PLAFON PIUTANG";

                    var noUrut = 1;
                    foreach (var customer in listOfObject)
                    {
                        ws.Cell(1 + noUrut, 1).Value = noUrut;
                        ws.Cell(1 + noUrut, 2).Value = customer.nama_customer;
                        ws.Cell(1 + noUrut, 3).Value = customer.alamat;
                        ws.Cell(1 + noUrut, 4).Value = customer.kecamatan;
                        ws.Cell(1 + noUrut, 5).Value = customer.kelurahan;
                        ws.Cell(1 + noUrut, 6).Value = customer.kota;
                        ws.Cell(1 + noUrut, 7).SetValue(customer.kode_pos).SetDataType(XLCellValues.Text);
                        ws.Cell(1 + noUrut, 8).Value = customer.kontak;
                        ws.Cell(1 + noUrut, 9).SetValue(customer.telepon).SetDataType(XLCellValues.Text);
                        ws.Cell(1 + noUrut, 10).Value = customer.diskon;
                        ws.Cell(1 + noUrut, 11).Value = customer.plafon_piutang;

                        noUrut++;
                    }

                    // Saving the workbook
                    wb.SaveAs(_fileName);

                    var fi = new FileInfo(_fileName);
                    if (fi.Exists)
                        Process.Start(_fileName);
                }                
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }    
        }


        public IList<string> GetWorksheets()
        {
            var listOfWorksheet = new List<string>();

            foreach (IXLWorksheet worksheet in _workbook.Worksheets)
            {
                listOfWorksheet.Add(worksheet.Name);
            }

            return listOfWorksheet;
        }
    }
}
