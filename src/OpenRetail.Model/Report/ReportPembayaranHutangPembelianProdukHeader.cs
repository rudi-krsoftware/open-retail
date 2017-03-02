using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.Report
{
    public class ReportPembayaranHutangPembelianProdukHeader
    {
        public string supplier_id { get; set; }
        public string nama_supplier { get; set; }
        public DateTime tanggal { get; set; }
        public double total_pembayaran { get; set; }
        public string keterangan { get; set; }
    }
}
