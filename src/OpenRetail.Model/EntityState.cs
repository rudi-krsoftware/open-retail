using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Model
{
    public enum EntityState
    {
        /// <summary>
        /// Status objek tidak ada perubahan
        /// </summary>
        Unchanged = 1,

        /// <summary>
        /// Objek yang baru ditambahkan
        /// </summary>
        Added = 2,

        /// <summary>
        /// Objek yang diupdate
        /// </summary>
        Modified = 3,

        /// <summary>
        /// Objek yang siap dihapus
        /// </summary>
        Deleted = 4
    }
}
