using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;
 
namespace OpenRetail.Bll.Service
{    
    public class AlasanPenyesuaianStokBll : IAlasanPenyesuaianStokBll
    {
		private AlasanPenyesuaianStokValidator _validator;

		public AlasanPenyesuaianStokBll()
        {
            _validator = new AlasanPenyesuaianStokValidator();
        }

        public AlasanPenyesuaianStok GetByID(string id)
        {
            AlasanPenyesuaianStok obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                obj = uow.AlasanPenyesuaianStokRepository.GetByID(id);
            }

            return obj;
        }

        public IList<AlasanPenyesuaianStok> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<AlasanPenyesuaianStok> GetAll()
        {
            IList<AlasanPenyesuaianStok> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                oList = uow.AlasanPenyesuaianStokRepository.GetAll();
            }

            return oList;
        }

		public int Save(AlasanPenyesuaianStok obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                result = uow.AlasanPenyesuaianStokRepository.Save(obj);
            }

            return result;
        }

        public int Save(AlasanPenyesuaianStok obj, ref ValidationError validationError)
        {
			var validatorResults = _validator.Validate(obj);

            if (!validatorResults.IsValid)
            {
                foreach (var failure in validatorResults.Errors)
                {
                    validationError.Message = failure.ErrorMessage;
                    validationError.PropertyName = failure.PropertyName;
                    return 0;
                }
            }

            return Save(obj);
        }

		public int Update(AlasanPenyesuaianStok obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                result = uow.AlasanPenyesuaianStokRepository.Update(obj);
            }

            return result;
        }

        public int Update(AlasanPenyesuaianStok obj, ref ValidationError validationError)
        {
            var validatorResults = _validator.Validate(obj);

            if (!validatorResults.IsValid)
            {
                foreach (var failure in validatorResults.Errors)
                {
                    validationError.Message = failure.ErrorMessage;
                    validationError.PropertyName = failure.PropertyName;
                    return 0;
                }
            }

            return Update(obj);
        }

        public int Delete(AlasanPenyesuaianStok obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                result = uow.AlasanPenyesuaianStokRepository.Delete(obj);
            }

            return result;
        }
    }
}     
