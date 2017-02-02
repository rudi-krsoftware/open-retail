Struktur Database OpenRetail
==============================================

OpenRetail menggunakan database [PostgreSQL](https://www.postgresql.org/) versi 9.4.1, disarankan Anda menggunakan versi yang sama untuk meminimalkan issue kompatibilitas.

Import Database OpenRetail
-----------------------------------------------
Untuk meng-import database OpenRetail Anda bisa menggunakan tool [psql](https://www.postgresql.org/docs/9.2/static/app-psql.html) dengan perintah berikut: 

```
psql -U USERNAME DbOpenRetail < DbOpenRetail.sql
```

Untuk `USERNAME` bisa menggunakan user default [PostgreSQL](https://www.postgresql.org/) yaitu `postgres`, dan pastikan sebelum menjalankan perintah di atas database `DbOpenRetail` sudah dibuat terlebih dulu.

Import Data Awal (prerequisites) untuk Menu dan Hak Akses Aplikasi
-----------------------------------------------
Sama seperti meng-import database OpenRetail, kita juga bisa menggunakan tool [psql](https://www.postgresql.org/docs/9.2/static/app-psql.html) dengan perintah berikut: 

```
psql -U postgres DbOpenRetail < 01_data-menu.sql
psql -U postgres DbOpenRetail < 02_data-item_menu.sql
psql -U postgres DbOpenRetail < 03_data-pengguna.sql
psql -U postgres DbOpenRetail < 04_data-role.sql
psql -U postgres DbOpenRetail < 05_data-role_privilege.sql
```