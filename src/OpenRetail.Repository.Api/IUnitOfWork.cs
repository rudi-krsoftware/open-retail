using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace OpenRetail.Repository.Api
{    
    public interface IUnitOfWork
    {
        IAlasanPenyesuaianStokRepository AlasanPenyesuaianStokRepository { get; }                
    }
}     
