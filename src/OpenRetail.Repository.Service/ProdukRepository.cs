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

using Dapper.Contrib.Extensions;
using OpenRetail.Model;
using OpenRetail.Repository.Api;
using Dapper;
 
namespace OpenRetail.Repository.Service
{        
    public class ProdukRepository : IProdukRepository
    {
        private const string SQL_TEMPLATE = @"SELECT m_produk.produk_id, m_produk.kode_produk, m_produk.nama_produk, m_produk.satuan, m_produk.stok, m_produk.harga_beli, m_produk.harga_jual, 
                                              m_produk.minimal_stok, m_produk.stok_gudang, m_produk.minimal_stok_gudang, m_golongan.golongan_id, m_golongan.nama_golongan
                                              FROM m_produk INNER JOIN public.m_golongan ON m_produk.golongan_id = m_golongan.golongan_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private string _sql;

        public ProdukRepository(IDapperContext context)
        {
            this._context = context;
        }

        private IEnumerable<Produk> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<Produk> oList = _context.db.Query<Produk, Golongan, Produk>(sql, (p, g) =>
            {
                p.golongan_id = g.golongan_id; p.Golongan = g;
                return p;
            }, param, splitOn: "golongan_id");

            return oList;
        }

        public Produk GetByID(string id)
        {
            Produk obj = null;
            
            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_produk.produk_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");
                
                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();
            }
            catch
            {
            }
            
            return obj;
        }

        public IList<Produk> GetByName(string name)
        {
            IList<Produk> oList = new List<Produk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_produk.nama_produk) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY m_produk.nama_produk");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();
            }
            catch
            {
            }            

            return oList;
        }

        public IList<Produk> GetAll()
        {
            IList<Produk> oList = new List<Produk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY m_produk.nama_produk");

                oList = MappingRecordToObject(_sql).ToList();
            }
            catch
            {
            }

            return oList;
        }

        private bool IsExist(string kodeProduk)
        {
            var count = _context.db.GetAll<Produk>()
                                .Where(f => f.kode_produk == kodeProduk)
                                .Count();

            return count > 0;
        }

        public int Save(Produk obj)
        {
            var result = 0;

            try
            {
                if (!IsExist(obj.kode_produk))
                {
                    obj.produk_id = _context.GetGUID();

                    _context.db.Insert<Produk>(obj);
                    result = 1;
                }
            }
            catch
            {
            }

            return result;
        }

        public int Update(Produk obj)
        {
            var result = 0;

            //try
            //{
                if (!(IsExist(obj.kode_produk) && obj.kode_produk != obj.kode_produk_old))
                {
                    result = _context.db.Update<Produk>(obj) ? 1 : 0;
                }                
            //}
            //catch
            //{
            //}

            return result;
        }

        public int Delete(Produk obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<Produk>(obj) ? 1 : 0;
            }
            catch
            {
            }

            return result;
        }
    }
}     
