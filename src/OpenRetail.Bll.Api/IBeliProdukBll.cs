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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenRetail.Model;
 
namespace OpenRetail.Bll.Api
{    
    public interface IBeliProdukBll : IBaseBll<BeliProduk>
    {
        string GetLastNota();
        BeliProduk GetByID(string id);

        IList<BeliProduk> GetAll(string name);
        IList<BeliProduk> GetNotaSupplier(string id, string nota);
        IList<BeliProduk> GetNotaKreditBySupplier(string id, bool isLunas);
        IList<BeliProduk> GetNotaKreditByNota(string id, string nota);
        IList<BeliProduk> GetByName(string name);
        IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai);
        IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name);

        IList<ItemBeliProduk> GetItemBeli(string beliId);

		int Save(BeliProduk obj, ref ValidationError validationError);
		int Update(BeliProduk obj, ref ValidationError validationError);
    }
}     
