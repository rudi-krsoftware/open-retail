using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.RajaOngkir
{
    public class results
    {
        public string province_id { get; set; }
        public string province { get; set; }

        /// <summary>
        /// id kota/kabupaten
        /// </summary>
        public string city_id { get; set; }

        /// <summary>
        /// Jenis Daerah Tingkat II. Berisi "Kota" atau "Kabupaten"
        /// </summary>
        public string type { get; set; }
        public string city_name { get; set; }
        public string postal_code { get; set; }

        public string code { get; set; }
        public string name { get; set; }
        public List<costs> costs { get; set; }
    }
}
