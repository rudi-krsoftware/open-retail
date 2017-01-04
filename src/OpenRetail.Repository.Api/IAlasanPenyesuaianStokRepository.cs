using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenRetail.Model;
 
namespace OpenRetail.Repository.Api
{        
    public interface IAlasanPenyesuaianStokRepository : IBaseRepository<AlasanPenyesuaianStok>
    {
		AlasanPenyesuaianStok GetByID(string id);            
        IList<AlasanPenyesuaianStok> GetByName(string name);
    }
}     
