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
    public class ImportExportDataSupplierBll : IImportExportDataBll<Supplier>
    {
        private ILog _log;
        private string _fileName;
        private XLWorkbook _workbook;

        public ImportExportDataSupplierBll(string fileName, ILog log)
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

                var colums = new string[] { "NAMA", "ALAMAT", "KONTAK", "TELEPON" };

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
                var supplierRange = ws.Range(firstPossibleAddress, lastPossibleAddress).RangeUsed();

                // Treat the range as a table (to be able to use the column names)
                var supplierTable = supplierRange.AsTable();

                var listOfSupplier = new List<Supplier>();

                listOfSupplier = supplierTable.DataRange.Rows().Select(row => new Supplier
                {
                    nama_supplier = row.Field("NAMA").GetString(),
                    alamat = row.Field("ALAMAT").GetString(),
                    kontak = row.Field("KONTAK").GetString(),
                    telepon = row.Field("TELEPON").GetString()
                }).ToList();

                if (listOfSupplier.Count == 1 && listOfSupplier[0].nama_supplier.Length == 0)
                {
                    rowCount = 0;
                    return false;
                }

                rowCount = listOfSupplier.Count;

                using (IDapperContext context = new DapperContext())
                {
                    IUnitOfWork uow = new UnitOfWork(context, _log);

                    foreach (var supplier in listOfSupplier)
                    {
                        if (supplier.nama_supplier.Length > 0)
                        {
                            if (supplier.nama_supplier.Length > 50)
                                supplier.nama_supplier = supplier.nama_supplier.Substring(0, 50);

                            if (supplier.alamat.Length > 100)
                                supplier.alamat = supplier.alamat.Substring(0, 100);

                            if (supplier.kontak.Length > 50)
                                supplier.kontak = supplier.kontak.Substring(0, 50);

                            if (supplier.telepon.Length > 20)
                                supplier.telepon = supplier.telepon.Substring(0, 20);

                            result = Convert.ToBoolean(uow.SupplierRepository.Save(supplier));
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

        public void Export(IList<Supplier> listOfObject)
        {
            try
            {
                // Creating a new workbook
                using (var wb = new XLWorkbook())
                {
                    // Adding a worksheet
                    var ws = wb.Worksheets.Add("supplier");

                    // Set header table
                    ws.Cell(1, 1).Value = "NO";
                    ws.Cell(1, 2).Value = "NAMA";
                    ws.Cell(1, 3).Value = "ALAMAT";
                    ws.Cell(1, 4).Value = "KONTAK";
                    ws.Cell(1, 5).Value = "TELEPON";

                    var noUrut = 1;
                    foreach (var supplier in listOfObject)
                    {
                        ws.Cell(1 + noUrut, 1).Value = noUrut;
                        ws.Cell(1 + noUrut, 2).Value = supplier.nama_supplier;
                        ws.Cell(1 + noUrut, 3).Value = supplier.alamat;
                        ws.Cell(1 + noUrut, 4).Value = supplier.kontak;
                        ws.Cell(1 + noUrut, 5).SetValue(supplier.telepon).SetDataType(XLCellValues.Text);

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
