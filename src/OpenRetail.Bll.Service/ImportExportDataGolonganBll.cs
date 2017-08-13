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
    public class ImportExportDataGolonganBll : IImportExportDataBll<Golongan>
    {
        private ILog _log;
        private string _fileName;
        private XLWorkbook _workbook;

        public ImportExportDataGolonganBll(string fileName, ILog log)
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

                var colums = new string[] { "GOLONGAN", "DISKON" };

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
                var golonganRow = firstRowUsed.RowUsed();

                // First possible address of the company table:
                var firstPossibleAddress = ws.Row(golonganRow.RowNumber()).FirstCell().Address;

                // Last possible address of the company table:
                var lastPossibleAddress = ws.LastCellUsed().Address;

                // Get a range with the remainder of the worksheet data (the range used)
                var golonganRange = ws.Range(firstPossibleAddress, lastPossibleAddress).RangeUsed();

                // Treat the range as a table (to be able to use the column names)
                var golonganTable = golonganRange.AsTable();

                var listOfGolongan = new List<Golongan>();

                listOfGolongan = golonganTable.DataRange.Rows().Select(row => new Golongan
                {
                    nama_golongan = row.Field("GOLONGAN").GetString(),
                    diskon = row.Field("DISKON").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("DISKON").GetString())
                }).ToList();

                if (listOfGolongan.Count == 1 && listOfGolongan[0].nama_golongan.Length == 0)
                {
                    rowCount = 0;
                    return false;
                }

                rowCount = listOfGolongan.Count;

                using (IDapperContext context = new DapperContext())
                {
                    IUnitOfWork uow = new UnitOfWork(context, _log);

                    foreach (var golongan in listOfGolongan)
                    {
                        if (golongan.nama_golongan.Length > 0)
                        {
                            if (golongan.nama_golongan.Length > 50)
                                golongan.nama_golongan = golongan.nama_golongan.Substring(0, 50);

                            var oldGolongan = uow.GolonganRepository.GetByName(golongan.nama_golongan, false)
                                                                    .FirstOrDefault();

                            if (oldGolongan == null) // data golongan belum ada
                                result = Convert.ToBoolean(uow.GolonganRepository.Save(golongan));
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

        public void Export(IList<Golongan> listOfObject)
        {
            try
            {
                // Creating a new workbook

                using (var wb = new XLWorkbook())
                {
                    // Adding a worksheet
                    var ws = wb.Worksheets.Add("golongan");

                    // Set header table
                    ws.Cell(1, 1).Value = "NO";
                    ws.Cell(1, 2).Value = "GOLONGAN";
                    ws.Cell(1, 3).Value = "DISKON";

                    var noUrut = 1;
                    foreach (var golongan in listOfObject)
                    {
                        ws.Cell(1 + noUrut, 1).Value = noUrut;
                        ws.Cell(1 + noUrut, 2).Value = golongan.nama_golongan;
                        ws.Cell(1 + noUrut, 3).Value = golongan.diskon;

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
