using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.Report
{
    public class ReportKartuHutang
    {
        public int no_urut { get; set; }
        public string supplier_id { get; set; }
        public string nama_supplier { get; set; }
        public DateTime tanggal { get; set; }
        public string nama_produk { get; set; }
        public string satuan { get; set; }
        public double jumlah { get; set; }
        public double total { get; set; }
        public int jenis { get; set; }
    }
}
