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
    public interface IProdukBll : IBaseBll<Produk>
    {
        Produk GetByID(string id);
        Produk GetByKode(string kodeProduk);
        IList<Produk> GetByName(string name);
        IList<Produk> GetByName(string name, int sortByIndex, int pageNumber, int pageSize, ref int pagesCount);
        IList<Produk> GetByGolongan(string golonganId);
        IList<Produk> GetByGolongan(string golonganId, int sortByIndex, int pageNumber, int pageSize, ref int pagesCount);
        IList<Produk> GetInfoMinimalStok();
        IList<Produk> GetAll(int sortByIndex);
        IList<Produk> GetAll(int sortByIndex, int pageNumber, int pageSize, ref int pagesCount);
        string GetLastKodeProduk();

		int Save(Produk obj, ref ValidationError validationError);
		int Update(Produk obj, ref ValidationError validationError);
    }
}     
