using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.App.Helper
{
    /// <summary>
    /// Interface untuk pengiriman objek antar form
    /// </summary>
    public interface IListener
    {
        void Ok(object sender, object data);
        void Ok(object sender, bool isNewData, object data);
    }
}
