using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.Report
{
    public class ReportHutangPembelianProdukHeader
    {
        public string supplier_id { get; set; }
        public string nama_supplier { get; set; }

        public double ppn { get; set; }
        public double diskon { get; set; }
        public double total_nota { get; set; }
        public double grand_total
        {
            get { return total_nota - diskon + ppn; }
        }

        public double total_pelunasan { get; set; }

        public double sisa_nota
        {
            get { return grand_total - total_pelunasan; }
        }
    }
}
