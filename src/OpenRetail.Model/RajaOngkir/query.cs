using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.RajaOngkir
{
    public class query
    {
        public string id { get; set; }

        /// <summary>
        /// id kota asal
        /// </summary>
        public string origin { get; set; }

        /// <summary>
        /// id kota tujuan
        /// </summary>
        public string destination { get; set; }

        /// <summary>
        /// Berat kiriman (gram)
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// Kode kurir yang dipakai
        /// </summary>
        public string courier { get; set; }
    }
}
