/**
 * Copyright (C) 2017 Kamarudin (http://coding4ever.net/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * The latest version of this file can be found at https://github.com/rudi-krsoftware/open-retail
 */

using log4net;
using OpenRetail.Bll.Api;
using OpenRetail.Model;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;
using System;
using System.Collections.Generic;

namespace OpenRetail.Bll.Service
{
    public class LabelNotaBll : ILabelNotaBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;
        private LabelNotaValidator _validator;

        public LabelNotaBll()
        {
            _validator = new LabelNotaValidator();
        }

        public IList<LabelNota> GetAll()
        {
            IList<LabelNota> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.LabelNotaRepository.GetAll();
            }

            return oList;
        }

        public int Save(LabelNota obj)
        {
            throw new NotImplementedException();
        }

        public int Update(LabelNota obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                result = _unitOfWork.LabelNotaRepository.Update(obj);
            }

            return result;
        }

        public int Update(LabelNota obj, ref ValidationError validationError)
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

        public int Delete(LabelNota obj)
        {
            throw new NotImplementedException();
        }
    }
}