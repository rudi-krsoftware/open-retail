using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.RajaOngkir
{
    public class costs
    {
        /// <summary>
        /// Nama layanan yang digunakan dalam pengiriman
        /// </summary>
        public string service { get; set; }

        /// <summary>
        /// Deskripsi dari layanan pengiriman terkait
        /// </summary>
        public string description { get; set; }

        public List<cost> cost { get; set; }
    }
}
