--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

--
-- Data for Name: m_menu; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_menu (menu_id, nama_menu, judul_menu, parent_id, order_number, is_active, nama_form, is_enabled) FROM stdin;
0a516b56-ed73-dafe-aaa0-332fadd2f088	mnuPengaturanUmum	Pengaturan Umum	593c989d-be87-42bf-a11d-f177afcc2180	2	t	FrmPengaturanUmum	t
8a0ba72f-67d2-481f-9e10-a188f09effa5	mnuProduk	Produk	07b24b4b-cf52-4b3c-ab06-51f7312e4813	3	t	FrmListProduk	t
8a8c6d23-963b-4819-819d-b9cdeaad7718	mnuGolongan	Golongan	07b24b4b-cf52-4b3c-ab06-51f7312e4813	2	t	FrmListGolongan	t
ed926af3-61a5-40e7-8975-de78c90eb784	mnuLapPenjualanPerGolongan	Penjualan Per Golongan	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	9	t	FrmLapPenjualanPerGolongan	t
95e9e230-c4f3-4fbc-9652-78cf4155d7ea	mnuPenyesuaianStok	Penyesuaian Stok	07b24b4b-cf52-4b3c-ab06-51f7312e4813	6	t	FrmListPenyesuaianStok	t
fd48562f-9096-4cec-ad9c-37229fc072a3	mnuSupplier	Supplier	07b24b4b-cf52-4b3c-ab06-51f7312e4813	7	t	FrmListSupplier	t
5ab9c82d-a116-4032-8891-cbfb7b71b8e3	mnuCustomer	Customer	07b24b4b-cf52-4b3c-ab06-51f7312e4813	8	t	FrmListCustomer	t
7c7a2763-ed8b-41a7-a42d-b79233d02e02	mnuDropshipper	Dropshipper	07b24b4b-cf52-4b3c-ab06-51f7312e4813	9	t	FrmListDropshipper	t
f18fbb6e-bd6f-5d21-fa8d-11923327b436	mnuKartu	Kartu	07b24b4b-cf52-4b3c-ab06-51f7312e4813	1	t	FrmListKartu	t
fa7e83ee-9b49-4cda-badd-d68cda7b7a9a	mnuCetakLabelBarcodeProduk	Cetak Label Barcode Produk	07b24b4b-cf52-4b3c-ab06-51f7312e4813	5	t	FrmCetakLabelBarcodeProduk	t
7cca3d24-3fc3-4c64-b361-78c0c7581920	mnuCetakLabelHargaProduk	Cetak Label Harga Produk	07b24b4b-cf52-4b3c-ab06-51f7312e4813	4	t	FrmCetakLabelHargaProduk	t
6f13d6e5-4322-4b4b-8fca-2278b04bd4eb	mnuLapPenjualan	Penjualan	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	18	t		t
a0c6daf8-b0c8-4e9d-9702-7a2bb781580c	mnuLapPemasukanPengeluaran	Pemasukan dan Pengeluaran	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	25	t	FrmLapPemasukanPengeluaran	t
65afc8bf-4df2-486a-b878-e77638ae2688	mnuLapKartuStokProduk	Kartu Stok Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	21	t	FrmLapKartuStokProduk	t
d62d3352-56e7-4802-973e-32d590febdab	mnuLapStokProduk	Stok Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	19	t	FrmLapStokProduk	t
986182a1-8b33-457b-9151-38676f0f0869	mnuLapPenyesuaianStok	Penyesuaian Stok	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	20	t	FrmLapPenyesuaianStok	t
b94a2365-c063-491e-a798-c68dccd2d80b	mnuLapPenjualanProdukFavorit	Penjualan Produk Favorit	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	11	t	FrmLapPenjualanProdukFavorit	t
a1b976fc-99d3-4b5f-89b5-bc7e7fc4c0d2	mnuLapPenjualanPerKasir	Penjualan Per Kasir	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	10	t	FrmLapPenjualanPerKasir	t
33d081b5-8e4d-424b-a00a-eccf1f1a8809	mnuLapCustomerProduk	Customer Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	12	t	FrmLapCustomerProduk	t
5a114aab-28cc-4655-b676-d08075bae831	mnuLapPembelian	Pembelian	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	6	t		t
07b24b4b-cf52-4b3c-ab06-51f7312e4813	mnuReferensi	Referensi	\N	1	t	\N	t
73e32548-da86-4db9-b3f9-f2ecd81ea3c9	mnuTransaksi	Transaksi	\N	2	t	\N	t
d1bd5f93-996c-46a3-b80f-f4f50681a1f9	mnuPengeluaran	Pengeluaran	\N	3	t	\N	t
3a62ea0b-0f48-495c-947b-ad5aa9af77f7	mnuLaporan	Laporan	\N	4	t	\N	t
593c989d-be87-42bf-a11d-f177afcc2180	mnuPengaturan	Pengaturan	\N	5	t	\N	t
a6043b21-18d0-4fcd-9ea9-f146542081d5	mnuPembelianProduk	Pembelian Produk	73e32548-da86-4db9-b3f9-f2ecd81ea3c9	1	t	FrmListPembelianProduk	t
084488a3-092d-4e8c-8bf2-72dcf90262b4	mnuPembayaranHutangPembelianProduk	Pembayaran Hutang Pembelian Produk	73e32548-da86-4db9-b3f9-f2ecd81ea3c9	2	t	FrmListPembayaranHutangPembelianProduk	t
870ec1d3-5b71-47dc-b241-1b0ae933217c	mnuReturPembelianProduk	Retur Pembelian Produk	73e32548-da86-4db9-b3f9-f2ecd81ea3c9	3	t	FrmListReturPembelianProduk	t
b4be7b7c-4587-4af4-af07-fce34df723df	mnuPenjualanProduk	Penjualan Produk	73e32548-da86-4db9-b3f9-f2ecd81ea3c9	4	t	FrmListPenjualanProduk	t
e7be0d85-9f96-4095-be35-1da049028cef	mnuJabatan	Jabatan	07b24b4b-cf52-4b3c-ab06-51f7312e4813	10	t	FrmListJabatan	t
b7ade8cc-22aa-43c8-be9c-af6cb71d11a6	mnuKaryawan	Karyawan	07b24b4b-cf52-4b3c-ab06-51f7312e4813	11	t	FrmListKaryawan	t
99302348-4d3c-48dd-8d67-c422e3061f1c	mnuPembayaranPiutangPenjualanProduk	Pembayaran Piutang Penjualan Produk	73e32548-da86-4db9-b3f9-f2ecd81ea3c9	5	t	FrmListPembayaranPiutangPenjualanProduk	t
576b9f00-c29d-4c2c-9a6b-5a563344de93	mnuReturPenjualanProduk	Retur Penjualan Produk	73e32548-da86-4db9-b3f9-f2ecd81ea3c9	6	t	FrmListReturPenjualanProduk	t
36aabcfa-60bc-48f5-9af6-30aa7600eb5b	mnuProfilPerusahaan	Profil Perusahaan	593c989d-be87-42bf-a11d-f177afcc2180	1	t	FrmProfilPerusahaan	t
295f6ddd-b934-4215-8e44-74905efd2273	mnuLapPembelianProduk	Pembelian Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	1	t	FrmLapPembelianProduk	t
335e115b-8f25-4cad-96cf-22481c98c525	mnuLapHutangPembelianProduk	Hutang Pembelian Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	2	t	FrmLapHutangPembelianProduk	t
42a3ffaa-da90-44d8-ae9b-cfcb7a7bdac6	mnuLapPembayaranHutangPembelianProduk	Pembayaran Hutang Pembelian Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	3	t	FrmLapPembayaranHutangPembelianProduk	t
b0b538ba-3535-4308-8012-9e2fa0daa0b0	mnuLapKartuHutangPembelianProduk	Kartu Hutang Pembelian Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	4	t	FrmLapKartuHutangPembelianProduk	t
5a4b0eab-0a7b-463a-bcf8-0cca0fbddded	mnuLapReturPembelianProduk	Retur Pembelian Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	5	t	FrmLapReturPembelianProduk	t
6a593127-4efb-4d7e-be38-53894c4828d0	mnuLapPenjualanProduk	Penjualan Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	7	t	FrmLapPenjualanProduk	t
f87b0496-d9cb-4aef-b9dd-b7fe62ac3a18	mnuLapPenjualanPerProduk	Penjualan Per Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	8	t	FrmLapPenjualanPerProduk	t
b52e8eac-3bf6-4ebf-95a0-46ab9e7b0888	mnuJenisPengeluaran	Jenis Pengeluaran	07b24b4b-cf52-4b3c-ab06-51f7312e4813	12	t	FrmListJenisPengeluaran	t
5c336652-4465-4b62-8de1-dac26fc696b6	mnuLapPengeluaranBiaya	Pengeluaran Biaya	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	22	t	FrmLapPengeluaranBiaya	t
e9e7ff66-d302-49a3-a9e5-8a02ff87e016	mnuLapKasbon	Kasbon	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	23	t	FrmLapKasbon	t
08392673-2e61-4266-a6ae-5cb75fdf42e8	mnuPengeluaranBiaya	Pengeluaran Biaya	d1bd5f93-996c-46a3-b80f-f4f50681a1f9	1	t	FrmListPengeluaranBiaya	t
13b929f3-d349-4686-b803-b350732003c8	mnuKasbon	Kasbon	d1bd5f93-996c-46a3-b80f-f4f50681a1f9	2	t	FrmListKasbon	t
d5bd3b31-7feb-4c55-9fed-8d67ac18fef4	mnuPenggajian	Penggajian Karyawan	d1bd5f93-996c-46a3-b80f-f4f50681a1f9	3	t	FrmListPenggajianKaryawan	t
0702e90c-cb7c-42a4-a447-478fba5a7443	mnuHakAksesAplikasi	Hak Akses Aplikasi	593c989d-be87-42bf-a11d-f177afcc2180	3	t	FrmListHakAkses	t
e8f83478-a577-4cff-a06f-8d921f9367c7	mnuManajemenOperator	Manajemen Operator	593c989d-be87-42bf-a11d-f177afcc2180	4	t	FrmListOperator	t
942cf428-3e36-48d2-8932-7e55cde20b32	mnuLapPiutangPenjualanProduk	Piutang Penjualan Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	13	t	FrmLapPiutangPenjualanProduk	t
5b99b5c4-91a8-4492-80a2-71cd9b800f00	mnuLapPembayaranPiutangPenjualanProduk	Pembayaran Piutang Penjualan Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	14	t	FrmLapPembayaranPiutangPenjualanProduk	t
01803718-15d1-4ef4-9574-b9bb161c1638	mnuLapKartuPiutangPenjualanProduk	Kartu Piutang Penjualan Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	15	t	FrmLapKartuPiutangPenjualanProduk	t
8c46f173-f586-402d-b54a-ac2e4ba57f1e	mnuLapPenggajian	Penggajian Karyawan	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	24	t	FrmLapPenggajianKaryawan	t
948af0c2-5c0d-4887-8d81-4cd42e1b02a0	mnuLapLabaRugiPenjualan	Laba/Rugi Penjualan	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	17	t	FrmLapLabaRugiPenjualan	t
56bd7e72-5e17-4105-bf4d-5f698e94a7fb	mnuLapReturPenjualanProduk	Retur Penjualan Produk	3a62ea0b-0f48-495c-947b-ad5aa9af77f7	16	t	FrmLapReturPenjualanProduk	t
\.


--
-- PostgreSQL database dump complete
--

