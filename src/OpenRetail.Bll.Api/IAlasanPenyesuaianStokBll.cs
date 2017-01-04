using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenRetail.Model;
 
namespace OpenRetail.Bll.Api
{    
    public interface IAlasanPenyesuaianStokBll : IBaseBll<AlasanPenyesuaianStok>
    {
        AlasanPenyesuaianStok GetByID(string id);    
        IList<AlasanPenyesuaianStok> GetByName(string name);

		int Save(AlasanPenyesuaianStok obj, ref ValidationError validationError);
		int Update(AlasanPenyesuaianStok obj, ref ValidationError validationError);
    }
}     
