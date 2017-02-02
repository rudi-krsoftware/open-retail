using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.DTO
{
    public class ItemBeliProdukDto
    {
        public string item_beli_produk_id { get; set; }
        public string beli_produk_id { get; set; }
        public string pengguna_id { get; set; }
        public string produk_id { get; set; }
        public double harga { get; set; }
        public double jumlah { get; set; }
        public double diskon { get; set; }
        public double jumlah_retur { get; set; }
        
        public double diskon_rupiah
        {
            get { return diskon / 100 * harga; }
        }

        public double harga_setelah_diskon
        {
            get { return harga - diskon_rupiah; }
        }

        public double sub_total
        {
            get { return (jumlah - jumlah_retur) * harga_setelah_diskon; }
        }
    }
}
