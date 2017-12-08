using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using Dapper;
using OpenRetail.Model.Report;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Api.Report;

namespace OpenRetail.Repository.Service.Report
{
    public class ReportKartuStokRepository : IReportKartuStokRepository
    {
        private const string SQL_TEMPLATE = @"SELECT 1 AS jenis_nota, m_produk.produk_id, m_produk.nama_produk, m_produk.stok + m_produk.stok_gudang AS stok_akhir, t_beli_produk.nota, t_beli_produk.tanggal, 
                                              m_supplier.nama_supplier AS supplier_or_customer, SUM(t_item_beli_produk.jumlah) AS qty, COALESCE(t_beli_produk.keterangan, t_beli_produk.keterangan, '') AS keterangan
                                              FROM public.t_beli_produk INNER JOIN public.t_item_beli_produk ON t_item_beli_produk.beli_produk_id = t_beli_produk.beli_produk_id
                                              INNER JOIN public.m_supplier ON t_beli_produk.supplier_id = m_supplier.supplier_id
                                              INNER JOIN public.m_produk ON t_item_beli_produk.produk_id = m_produk.produk_id
                                              {WHERE_1}
                                              GROUP BY m_produk.produk_id, m_produk.nama_produk, t_beli_produk.nota, t_beli_produk.tanggal, m_supplier.nama_supplier, COALESCE(t_beli_produk.keterangan, t_beli_produk.keterangan, '')
                                              UNION
                                              SELECT 2 AS jenis_nota, m_produk.produk_id, m_produk.nama_produk, m_produk.stok + m_produk.stok_gudang AS stok_akhir, t_retur_jual_produk.nota, t_retur_jual_produk.tanggal, 
                                              m_customer.nama_customer AS supplier_or_customer, SUM(t_item_retur_jual_produk.jumlah_retur) AS qty, COALESCE(t_retur_jual_produk.keterangan, t_retur_jual_produk.keterangan, '') AS keterangan
                                              FROM public.t_retur_jual_produk INNER JOIN public.t_item_retur_jual_produk ON t_item_retur_jual_produk.retur_jual_id = t_retur_jual_produk.retur_jual_id
                                              INNER JOIN public.m_produk ON t_item_retur_jual_produk.produk_id = m_produk.produk_id
                                              INNER JOIN public.m_customer ON t_retur_jual_produk.customer_id = m_customer.customer_id
                                              {WHERE_2}
                                              GROUP BY m_produk.produk_id, m_produk.nama_produk, t_retur_jual_produk.nota, t_retur_jual_produk.tanggal, m_customer.nama_customer, COALESCE(t_retur_jual_produk.keterangan, t_retur_jual_produk.keterangan, '')
                                              UNION 
                                              SELECT 3 AS jenis_nota, m_produk.produk_id, m_produk.nama_produk, m_produk.stok + m_produk.stok_gudang AS stok_akhir, t_jual_produk.nota, t_jual_produk.tanggal, 
                                              m_customer.nama_customer AS supplier_or_customer, SUM(t_item_jual_produk.jumlah) AS qty, COALESCE(t_jual_produk.keterangan, t_jual_produk.keterangan, '') AS keterangan
                                              FROM public.t_jual_produk INNER JOIN public.t_item_jual_produk ON t_item_jual_produk.jual_id = t_jual_produk.jual_id
                                              INNER JOIN public.m_customer ON t_jual_produk.customer_id = m_customer.customer_id
                                              INNER JOIN public.m_produk ON t_item_jual_produk.produk_id = m_produk.produk_id
                                              {WHERE_3}
                                              GROUP BY m_produk.produk_id, m_produk.nama_produk, t_jual_produk.nota, t_jual_produk.tanggal, m_customer.nama_customer, COALESCE(t_jual_produk.keterangan, t_jual_produk.keterangan, '')
                                              UNION
                                              SELECT 4 AS jenis_nota, m_produk.produk_id, m_produk.nama_produk, m_produk.stok + m_produk.stok_gudang AS stok_akhir, t_retur_beli_produk.nota, t_retur_beli_produk.tanggal, 
                                              m_supplier.nama_supplier AS supplier_or_customer, SUM(t_item_retur_beli_produk.jumlah_retur) AS qty, COALESCE(t_retur_beli_produk.keterangan, t_retur_beli_produk.keterangan, '') AS keterangan
                                              FROM public.t_retur_beli_produk INNER JOIN public.t_item_retur_beli_produk ON t_item_retur_beli_produk.retur_beli_produk_id = t_retur_beli_produk.retur_beli_produk_id
                                              INNER JOIN public.m_produk ON t_item_retur_beli_produk.produk_id = m_produk.produk_id
                                              INNER JOIN public.m_supplier ON t_retur_beli_produk.supplier_id = m_supplier.supplier_id
                                              {WHERE_4}
                                              GROUP BY m_produk.produk_id, m_produk.nama_produk, t_retur_beli_produk.nota, t_retur_beli_produk.tanggal, m_supplier.nama_supplier, COALESCE(t_retur_beli_produk.keterangan, t_retur_beli_produk.keterangan, '')
                                              ORDER BY 3, 5, 1";

        private IDapperContext _context;
        private ILog _log;
        private string _sql;
        private string _where;

        public ReportKartuStokRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportKartuStok> GetSaldoAwal(DateTime tanggal)
        {
            IList<ReportKartuStok> oList = new List<ReportKartuStok>();

            try
            {
                _where = @"WHERE t_beli_produk.tanggal < @tanggal";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE t_retur_jual_produk.tanggal < @tanggal";
                _sql = _sql.Replace("{WHERE_2}", _where);

                _where = @"WHERE t_jual_produk.tanggal < @tanggal";
                _sql = _sql.Replace("{WHERE_3}", _where);

                _where = @"WHERE t_retur_beli_produk.tanggal < @tanggal";
                _sql = _sql.Replace("{WHERE_4}", _where);

                oList = _context.db.Query<ReportKartuStok>(_sql, new { tanggal })
                                .ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKartuStok> GetByBulan(int bulan, int tahun)
        {
            IList<ReportKartuStok> oList = new List<ReportKartuStok>();

            try
            {
                _where = @"WHERE EXTRACT(MONTH FROM t_beli_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE EXTRACT(MONTH FROM t_retur_jual_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_retur_jual_produk.tanggal) = @tahun";
                _sql = _sql.Replace("{WHERE_2}", _where);

                _where = @"WHERE EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun";
                _sql = _sql.Replace("{WHERE_3}", _where);

                _where = @"WHERE EXTRACT(MONTH FROM t_retur_beli_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_retur_beli_produk.tanggal) = @tahun";
                _sql = _sql.Replace("{WHERE_4}", _where);

                oList = _context.db.Query<ReportKartuStok>(_sql, new { bulan, tahun })
                                .ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKartuStok> GetByBulan(int bulan, int tahun, IList<string> listOfKode)
        {
            IList<ReportKartuStok> oList = new List<ReportKartuStok>();

            try
            {
                var sb = new StringBuilder();

                foreach (var item in listOfKode)
                {
                    sb.Append("'").Append(item).Append("'").Append(",");
                }

                var param = sb.ToString();
                param = param.Substring(0, param.Length - 1);

                _where = "WHERE LOWER(m_produk.kode_produk) IN (" + param + ") AND EXTRACT(MONTH FROM t_beli_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = "WHERE LOWER(m_produk.kode_produk) IN (" + param + ") AND EXTRACT(MONTH FROM t_retur_jual_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_retur_jual_produk.tanggal) = @tahun";
                _sql = _sql.Replace("{WHERE_2}", _where);

                _where = "WHERE LOWER(m_produk.kode_produk) IN (" + param + ") AND EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun";
                _sql = _sql.Replace("{WHERE_3}", _where);

                _where = "WHERE LOWER(m_produk.kode_produk) IN (" + param + ") AND EXTRACT(MONTH FROM t_retur_beli_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_retur_beli_produk.tanggal) = @tahun";
                _sql = _sql.Replace("{WHERE_4}", _where);

                oList = _context.db.Query<ReportKartuStok>(_sql, new { bulan, tahun })
                                .ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKartuStok> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            throw new NotImplementedException();
        }

        public IList<ReportKartuStok> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportKartuStok> oList = new List<ReportKartuStok>();

            try
            {
                _where = @"WHERE t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE t_retur_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = _sql.Replace("{WHERE_2}", _where);

                _where = @"WHERE t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = _sql.Replace("{WHERE_3}", _where);

                _where = @"WHERE t_retur_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = _sql.Replace("{WHERE_4}", _where);

                oList = _context.db.Query<ReportKartuStok>(_sql, new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKartuStok> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, IList<string> listOfKode)
        {            
            IList<ReportKartuStok> oList = new List<ReportKartuStok>();

            try
            {
                var sb = new StringBuilder();

                foreach (var item in listOfKode)
                {
                    sb.Append("'").Append(item).Append("'").Append(",");
                }

                var param = sb.ToString();
                param = param.Substring(0, param.Length - 1);

                _where = "WHERE LOWER(m_produk.kode_produk) IN (" + param + ") AND t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = "WHERE LOWER(m_produk.kode_produk) IN (" + param + ") AND t_retur_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = _sql.Replace("{WHERE_2}", _where);

                _where = "WHERE LOWER(m_produk.kode_produk) IN (" + param + ") AND  t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = _sql.Replace("{WHERE_3}", _where);

                _where = "WHERE LOWER(m_produk.kode_produk) IN (" + param + ") AND t_retur_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = _sql.Replace("{WHERE_4}", _where);

                oList = _context.db.Query<ReportKartuStok>(_sql, new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKartuStok> GetAll(IList<string> listOfProdukId)
        {
            IList<ReportKartuStok> oList = new List<ReportKartuStok>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", "");
                _sql = _sql.Replace("{WHERE_2}", "");
                _sql = _sql.Replace("{WHERE_3}", "");
                _sql = _sql.Replace("{WHERE_4}", "");

                oList = _context.db.Query<ReportKartuStok>(_sql)
                                .Where(f => listOfProdukId.Contains(f.produk_id))
                                .ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }        
    }
}
