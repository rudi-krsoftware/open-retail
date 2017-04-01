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
using System.Windows.Forms;

using OpenRetail.Model;

namespace OpenRetail.App.Helper
{
    public static class FillDataHelper
    {
        public static void FillProduct(CheckedListBox chkListbox, IList<Produk> listOfProduct, bool isShowSatuan = true)
        {
            chkListbox.Items.Clear();

            foreach (var produk in listOfProduct)
            {
                if (isShowSatuan)
                    chkListbox.Items.Add(string.Format("{0} ({1})", produk.nama_produk, produk.satuan));
                else
                    chkListbox.Items.Add(produk.nama_produk);
            }
        }

        public static void FillSupplier(CheckedListBox chkListbox, IList<Supplier> listOfSupplier)
        {
            chkListbox.Items.Clear();

            foreach (var supplier in listOfSupplier)
            {
                chkListbox.Items.Add(supplier.nama_supplier);
            }
        }        

        public static void FillCustomer(CheckedListBox chkListbox, IList<Customer> listOfCustomer)
        {
            chkListbox.Items.Clear();

            foreach (var customer in listOfCustomer)
            {
                chkListbox.Items.Add(customer.nama_customer);
            }
        }

        public static void FillKaryawan(CheckedListBox chkListbox, IList<Karyawan> listOfKaryawan)
        {
            chkListbox.Items.Clear();

            foreach (var karyawan in listOfKaryawan)
            {
                chkListbox.Items.Add(karyawan.nama_karyawan);
            }
        }

        public static void FillKaryawan(ComboBox cmbBox, IList<Karyawan> listOfKaryawan, bool isClearItem = true)
        {
            if (isClearItem)
                cmbBox.Items.Clear();

            foreach (var karyawan in listOfKaryawan)
            {
                cmbBox.Items.Add(karyawan.nama_karyawan);
            }
        }

        public static void FillPengguna(CheckedListBox chkListbox, IList<Pengguna> listOfPengguna)
        {
            chkListbox.Items.Clear();

            foreach (var pengguna in listOfPengguna)
            {
                chkListbox.Items.Add(pengguna.nama_pengguna);
            }
        }

        public static void FillAlasanPenyesuaianStok(CheckedListBox chkListbox, IList<AlasanPenyesuaianStok> listOfAlasanPenyesuaianStok)
        {
            chkListbox.Items.Clear();

            foreach (var alasan in listOfAlasanPenyesuaianStok)
            {
                chkListbox.Items.Add(alasan.alasan);
            }
        }

        public static void FillJenisPengeluaranBiaya(CheckedListBox chkListbox, IList<JenisPengeluaran> listOfJenisPengeluaranBiaya)
        {
            chkListbox.Items.Clear();

            foreach (var jenisPengeluaran in listOfJenisPengeluaranBiaya)
            {
                chkListbox.Items.Add(jenisPengeluaran.nama_jenis_pengeluaran);
            }
        }

        public static void FillBulan(ComboBox obj, bool isSetDefaultMonth = false)
        {
            obj.Items.Clear();

            for (int i = 1; i < 13; i++)
            {
                obj.Items.Add(DayMonthHelper.GetBulanIndonesia(i));
            }

            if (isSetDefaultMonth)
                obj.SelectedItem = DayMonthHelper.GetBulanIndonesia(DateTime.Today.Month);
        }

        public static void FillTahun(ComboBox obj, bool isSetDefaultYear = false, int startYear = 2015)
        {
            obj.Items.Clear();

            for (int i = startYear; i <= DateTime.Today.Year + 1; i++)
            {
                obj.Items.Add(i.ToString());
            }

            if (isSetDefaultYear)
                obj.SelectedItem = DateTime.Today.Year.ToString();
        }
    }
}
