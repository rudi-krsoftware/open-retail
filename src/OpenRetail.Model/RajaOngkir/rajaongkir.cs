using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model.RajaOngkir
{
    public class rajaongkir
    {
        public query query { get; set; }
        public status status { get; set; }
        public List<results> results { get; set; }

        public origin_details origin_details { get; set; }
        public destination_details destination_details { get; set; }
    }
}
