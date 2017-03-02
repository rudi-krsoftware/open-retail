using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.Report
{
    public class ReportPembayaranHutangPembelianProdukDetail
    {
        public string supplier_id { get; set; }
        public string nama_supplier { get; set; }
        public string nota_beli { get; set; }
        public string nota_bayar { get; set; }
        public DateTime tanggal { get; set; }
        public double ppn { get; set; }
        public double diskon { get; set; }
        public double total_nota { get; set; }
        public string keterangan_beli { get; set; }
        public string keterangan_bayar { get; set; }

        public double grand_total
        {
            get { return total_nota - diskon + ppn; }
        }

        public double pelunasan { get; set; }
    }
}
