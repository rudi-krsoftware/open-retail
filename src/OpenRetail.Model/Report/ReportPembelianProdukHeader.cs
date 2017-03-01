using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.Report
{
    public class ReportPembelianProdukHeader
    {
        public string supplier_id { get; set; }
        public string nama_supplier { get; set; }
        public string nota { get; set; }
        public DateTime tanggal { get; set; }
        public DateTime tanggal_tempo { get; set; }
        public double ppn { get; set; }
        public double diskon { get; set; }
        public double total_nota { get; set; }
        public double total_pelunasan { get; set; }
        public string keterangan { get; set; }

        public double grand_total
        {
            get { return total_nota - diskon + ppn; }
        }

        public double sisa_nota
        {
            get { return grand_total - total_pelunasan; }
        }

        public bool is_tunai { get; set; }
    }
}
