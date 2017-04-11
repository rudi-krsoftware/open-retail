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
    public class ImportExportDataProdukBll : IImportExportDataBll
    {
        private ILog _log;
        private string _fileName;
        private string _workBookName = "produk";

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

                var colums = new string[] { 
                                            "GOLONGAN", "KODE PRODUK", "NAMA PRODUK", "SATUAN",
                                            "HARGA BELI", "HARGA JUAL", "STOK ETALASE", "STOK GUDANG", "MINIMAL STOK GUDANG"
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

                listOfProduk = supplierTable.DataRange.Rows().Select(row => new Produk
                {
                    Golongan = new Golongan { nama_golongan = row.Field("GOLONGAN").GetString() },
                    kode_produk = row.Field("KODE PRODUK").GetString(),
                    nama_produk = row.Field("NAMA PRODUK").GetString(),
                    satuan = row.Field("SATUAN").GetString(),
                    harga_beli = row.Field("HARGA BELI").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("HARGA BELI").GetString()),
                    harga_jual = row.Field("HARGA JUAL").GetString().Length == 0 ? 0 : Convert.ToDouble(row.Field("HARGA JUAL").GetString()),
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

                            if (produk.nama_produk.Length > 50)
                                produk.nama_produk = produk.nama_produk.Substring(0, 50);

                            if (produk.satuan.Length > 20)
                                produk.satuan = produk.satuan.Substring(0, 20);

                            result = Convert.ToBoolean(uow.ProdukRepository.Save(produk));
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
