using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.RajaOngkir
{
    public class cost
    {
        /// <summary>
        /// Tarif pengiriman (ongkos kirim)
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// Perkiraan waktu pengiriman (dalam hari)
        /// </summary>
        public string etd { get; set; }

        /// <summary>
        /// Catatan terkait tarif pengiriman
        /// </summary>
        public string note { get; set; }
    }
}
