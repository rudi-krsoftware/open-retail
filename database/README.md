Struktur Database OpenRetail
==============================================

OpenRetail menggunakan database [PostgreSQL](https://www.postgresql.org/) versi 9.3.16, disarankan Anda menggunakan versi yang sama untuk meminimalkan issue kompatibilitas.

Import Database OpenRetail
-----------------------------------------------
Untuk meng-import database OpenRetail Anda bisa menggunakan tool [psql](https://www.postgresql.org/docs/9.2/static/app-psql.html) dengan perintah berikut: 

```
psql -U USERNAME DbOpenRetail < DbOpenRetail.sql
```

Untuk `USERNAME` bisa menggunakan user default [PostgreSQL](https://www.postgresql.org/) yaitu `postgres`, dan pastikan sebelum menjalankan perintah di atas database `DbOpenRetail` sudah dibuat terlebih dulu.

Import Data Awal (prerequisites)
-----------------------------------------------
Sama seperti meng-import database OpenRetail, kita juga bisa menggunakan tool [psql](https://www.postgresql.org/docs/9.2/static/app-psql.html) dengan perintah berikut: 

```
psql -U postgres DbOpenRetail < 01_data-menu.sql
psql -U postgres DbOpenRetail < 02_data-item_menu.sql
psql -U postgres DbOpenRetail < 03_data-role.sql
psql -U postgres DbOpenRetail < 04_data-role_privilege.sql
psql -U postgres DbOpenRetail < 05_data-pengguna.sql
psql -U postgres DbOpenRetail < 06_data-alasan_penyesuaian_stok.sql
psql -U postgres DbOpenRetail < 07_data-database_version.sql
psql -U postgres DbOpenRetail < 08_data-jenis_pengeluaran.sql
psql -U postgres DbOpenRetail < 09_data-jabatan.sql
psql -U postgres DbOpenRetail < 10_data-profil.sql
psql -U postgres DbOpenRetail < 11_data-header-nota.sql
psql -U postgres DbOpenRetail < 12_data-label-nota.sql
psql -U postgres DbOpenRetail < 13_data-provinsi.sql
psql -U postgres DbOpenRetail < 14_data-kabupaten.sql
psql -U postgres DbOpenRetail < 15_data-header_nota_mini_pos.sql
psql -U postgres DbOpenRetail < 16_data-footer_nota_mini_pos.sql
```