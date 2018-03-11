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
    public class ImportExportDataProdukBll : IImportExportDataBll<Produk>
    {
        private ILog _log;
        private string _fileName;
        private XLWorkbook _workbook;

        public ImportExportDataProdukBll(string fileName, ILog log)
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
                                            "GOLONGAN", "KODE PRODUK", "NAMA PRODUK", "SATUAN",
                                            "HARGA BELI", "HARGA JUAL (RETAIL)", "DISKON (RETAIL)", 
                                            "HARGA GROSIR #1", "JUMLAH MINIMAL GROSIR #1", "DISKON GROSIR #1", 
                                            "HARGA GROSIR #2", "JUMLAH MINIMAL GROSIR #2", "DISKON GROSIR #2", 
                                            "HARGA GROSIR #3", "JUMLAH MINIMAL GROSIR #3", "DISKON GROSIR #3",
                                            "STOK ETALASE", "STOK GUDANG", "MINIMAL STOK GUDANG"
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
                var produkRow = firstRowUsed.RowUsed();

                // First possible address of the company table:
                var firstPossibleAddress = ws.Row(produkRow.RowNumber()).FirstCell().Address;

                // Last possible address of the company table:
                var lastPossibleAddress = ws.LastCellUsed().Address;

                // Get a range with the remainder of the worksheet data (the range used)
                var produkRange = ws.Range(firstPossibleAddress, lastPossibleAddress).RangeUsed();

                // Treat the range as a table (to be able to use the column names)
                var supplierTable = produkRange.AsTable();

                var listOfProduk = new List<Produk>();

                var hargaGrosir1 = new HargaGrosir();
                var hargaGrosir2 = new HargaGrosir();
                var hargaGrosir3 = new HargaGrosir();

                listOfProduk = supplierTable.DataRange.Rows().Select(row => new Produk
                {
                    Golongan = new Golongan { nama_golongan = row.Field("GOLONGAN").GetString() },
                    kode_produk = row.Field("KODE PRODUK").GetString(),
                    nama_produk = row.Field("NAMA PRODUK").GetString(),
                    satuan = row.Field("SATUAN").GetString(),
                    harga_beli = row.Field("HARGA BELI").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("HARGA BELI").GetString()),
                    harga_jual = row.Field("HARGA JUAL (RETAIL)").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("HARGA JUAL (RETAIL)").GetString()),
                    diskon = row.Field("DISKON (RETAIL)").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("DISKON (RETAIL)").GetString()),

                    list_of_harga_grosir = new List<HargaGrosir> 
                    {
                        new HargaGrosir 
                        { 
                            harga_ke = 1,
                            harga_grosir = row.Field("HARGA GROSIR #1").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("HARGA GROSIR #1").GetString()), 
                            jumlah_minimal = row.Field("JUMLAH MINIMAL GROSIR #1").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("JUMLAH MINIMAL GROSIR #1").GetString()), 
                            diskon = row.Field("DISKON GROSIR #1").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("DISKON GROSIR #1").GetString()) 
                        },
                        new HargaGrosir 
                        { 
                            harga_ke = 2,
                            harga_grosir = row.Field("HARGA GROSIR #2").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("HARGA GROSIR #2").GetString()), 
                            jumlah_minimal = row.Field("JUMLAH MINIMAL GROSIR #2").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("JUMLAH MINIMAL GROSIR #2").GetString()), 
                            diskon = row.Field("DISKON GROSIR #2").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("DISKON GROSIR #2").GetString()) 
                        },
                        new HargaGrosir 
                        { 
                            harga_ke = 3,
                            harga_grosir = row.Field("HARGA GROSIR #3").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("HARGA GROSIR #3").GetString()), 
                            jumlah_minimal = row.Field("JUMLAH MINIMAL GROSIR #3").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("JUMLAH MINIMAL GROSIR #3").GetString()), 
                            diskon = row.Field("DISKON GROSIR #3").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("DISKON GROSIR #3").GetString()) 
                        }
                    },

                    stok = row.Field("STOK ETALASE").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("STOK ETALASE").GetString()),
                    stok_gudang = row.Field("STOK GUDANG").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("STOK GUDANG").GetString()),
                    minimal_stok_gudang = row.Field("MINIMAL STOK GUDANG").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("MINIMAL STOK GUDANG").GetString())
                }).ToList();

                if (listOfProduk.Count == 1 && listOfProduk[0].nama_produk.Length == 0)
                {
                    rowCount = 0;
                    return false;
                }

                rowCount = listOfProduk.Where(f => f.nama_produk.Length > 0 && f.Golongan.nama_golongan.Length > 0)
                                       .Count();

                using (IDapperContext context = new DapperContext())
                {
                    IUnitOfWork uow = new UnitOfWork(context, _log);
                    
                    foreach (var produk in listOfProduk)
                    {
                        if (produk.nama_produk.Length > 0 && produk.Golongan.nama_golongan.Length > 0)
                        {
                            var golongan = uow.GolonganRepository.GetByName(produk.Golongan.nama_golongan, false)
                                                                 .FirstOrDefault();

                            if (golongan != null)
                            {
                                produk.golongan_id = golongan.golongan_id;
                                produk.Golongan = golongan;
                            }

                            if (produk.kode_produk.Length == 0)
                                produk.kode_produk = uow.ProdukRepository.GetLastKodeProduk();

                            if (produk.kode_produk.Length > 15)
                                produk.kode_produk = produk.kode_produk.Substring(0, 15);

                            if (produk.nama_produk.Length > 300)
                                produk.nama_produk = produk.nama_produk.Substring(0, 300);

                            if (produk.satuan.Length > 20)
                                produk.satuan = produk.satuan.Substring(0, 20);

                            var oldProduk = uow.ProdukRepository.GetByKode(produk.kode_produk);
                            if (oldProduk == null)
                            {
                                result = Convert.ToBoolean(uow.ProdukRepository.Save(produk));
                            }                                
                            else
                            {
                                // khusus stok etalase dan gudang diabaikan (tidak diupdate)
                                produk.produk_id = oldProduk.produk_id;
                                produk.kode_produk_old = oldProduk.kode_produk;
                                produk.stok = oldProduk.stok;
                                produk.stok_gudang = oldProduk.stok_gudang;

                                foreach (var grosir in produk.list_of_harga_grosir.OrderBy(f => f.harga_ke))
                                {
                                    var oldGrosir = oldProduk.list_of_harga_grosir
                                                             .Where(f => f.produk_id == produk.produk_id && f.harga_ke == grosir.harga_ke)
                                                             .SingleOrDefault();

                                    if (oldGrosir != null)
                                    {
                                        grosir.harga_grosir_id = oldGrosir.harga_grosir_id;
                                        grosir.produk_id = oldGrosir.produk_id;
                                    }                                    
                                }

                                result = Convert.ToBoolean(uow.ProdukRepository.Update(produk));
                            }
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

        public void Export(IList<Produk> listOfObject)
        {
            try
            {
                // Creating a new workbook
                using (var wb = new XLWorkbook())
                {
                    // Adding a worksheet
                    var ws = wb.Worksheets.Add("produk");

                    // Set header table
                    ws.Cell(1, 1).Value = "NO";
                    ws.Cell(1, 2).Value = "GOLONGAN";
                    ws.Cell(1, 3).Value = "KODE PRODUK";
                    ws.Cell(1, 4).Value = "NAMA PRODUK";
                    ws.Cell(1, 5).Value = "SATUAN";
                    ws.Cell(1, 6).Value = "HARGA BELI";
                    ws.Cell(1, 7).Value = "HARGA JUAL (RETAIL)";
                    ws.Cell(1, 8).Value = "DISKON (RETAIL)";

                    ws.Cell(1, 9).Value = "HARGA GROSIR #1";
                    ws.Cell(1, 10).Value = "JUMLAH MINIMAL GROSIR #1";
                    ws.Cell(1, 11).Value = "DISKON GROSIR #1";

                    ws.Cell(1, 12).Value = "HARGA GROSIR #2";
                    ws.Cell(1, 13).Value = "JUMLAH MINIMAL GROSIR #2";
                    ws.Cell(1, 14).Value = "DISKON GROSIR #2";

                    ws.Cell(1, 15).Value = "HARGA GROSIR #3";
                    ws.Cell(1, 16).Value = "JUMLAH MINIMAL GROSIR #3";
                    ws.Cell(1, 17).Value = "DISKON GROSIR #3";

                    ws.Cell(1, 18).Value = "STOK ETALASE";
                    ws.Cell(1, 19).Value = "STOK GUDANG";
                    ws.Cell(1, 20).Value = "MINIMAL STOK GUDANG";

                    var noUrut = 1;
                    foreach (var produk in listOfObject)
                    {
                        ws.Cell(1 + noUrut, 1).Value = noUrut;
                        ws.Cell(1 + noUrut, 2).Value = produk.Golongan != null ? produk.Golongan.nama_golongan : string.Empty;
                        ws.Cell(1 + noUrut, 3).SetValue(produk.kode_produk).SetDataType(XLCellValues.Text);
                        ws.Cell(1 + noUrut, 4).Value = produk.nama_produk;
                        ws.Cell(1 + noUrut, 5).Value = produk.satuan;
                        ws.Cell(1 + noUrut, 6).Value = produk.harga_beli;
                        ws.Cell(1 + noUrut, 7).Value = produk.harga_jual;
                        ws.Cell(1 + noUrut, 8).Value = produk.diskon;

                        var listOfHargaGrosir = produk.list_of_harga_grosir;
                        if (listOfHargaGrosir.Count > 0)
                        {
                            var column = 9;
                            foreach (var grosir in listOfHargaGrosir)
                            {
                                ws.Cell(1 + noUrut, column).Value = grosir.harga_grosir;
                                ws.Cell(1 + noUrut, column + 1).Value = grosir.jumlah_minimal;
                                ws.Cell(1 + noUrut, column + 2).Value = grosir.diskon;

                                column += 3;
                            }
                        }

                        ws.Cell(1 + noUrut, 18).Value = produk.stok;
                        ws.Cell(1 + noUrut, 19).Value = produk.stok_gudang;
                        ws.Cell(1 + noUrut, 20).Value = produk.minimal_stok_gudang;

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
