using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenRetail.Repository.Api;
 
namespace OpenRetail.Repository.Service
{    
    public class UnitOfWork : IUnitOfWork
    {
        private IDapperContext _context;
        private IAlasanPenyesuaianStokRepository _alasanpenyesuaianstokRepository;
        
        public UnitOfWork(IDapperContext context)
        {
            this._context = context;
        }

        public IAlasanPenyesuaianStokRepository AlasanPenyesuaianStokRepository
        {
            get { return _alasanpenyesuaianstokRepository ?? (_alasanpenyesuaianstokRepository = new AlasanPenyesuaianStokRepository(_context)); }
        }

    }
}     
