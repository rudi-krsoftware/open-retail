--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

--
-- Name: t_alamat; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_alamat AS character varying(100);


ALTER DOMAIN t_alamat OWNER TO postgres;

--
-- Name: t_bool; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_bool AS boolean DEFAULT true;


ALTER DOMAIN t_bool OWNER TO postgres;

--
-- Name: t_diskon; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_diskon AS numeric(5,2) DEFAULT 0.00;


ALTER DOMAIN t_diskon OWNER TO postgres;

--
-- Name: t_guid; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_guid AS character(36);


ALTER DOMAIN t_guid OWNER TO postgres;

--
-- Name: t_harga; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_harga AS numeric(15,2) DEFAULT 0.00;


ALTER DOMAIN t_harga OWNER TO postgres;

--
-- Name: t_jumlah; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_jumlah AS numeric(10,2) DEFAULT 0.00;


ALTER DOMAIN t_jumlah OWNER TO postgres;

--
-- Name: t_keterangan; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_keterangan AS character varying(100);


ALTER DOMAIN t_keterangan OWNER TO postgres;

--
-- Name: t_kode_produk; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_kode_produk AS character varying(15);


ALTER DOMAIN t_kode_produk OWNER TO postgres;

--
-- Name: t_nama; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_nama AS character varying(50);


ALTER DOMAIN t_nama OWNER TO postgres;

--
-- Name: t_nota; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_nota AS character varying(20);


ALTER DOMAIN t_nota OWNER TO postgres;

--
-- Name: t_password; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_password AS character(32);


ALTER DOMAIN t_password OWNER TO postgres;

--
-- Name: t_satuan; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_satuan AS character varying(20);


ALTER DOMAIN t_satuan OWNER TO postgres;

--
-- Name: t_telepon; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_telepon AS character varying(20);


ALTER DOMAIN t_telepon OWNER TO postgres;

--
-- Name: f_hapus_header_bayar_hutang_produk(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_hapus_header_bayar_hutang_produk() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE var_pembayaran_hutang_produk_id t_guid;
DECLARE var_row_count integer;

BEGIN		    
    var_pembayaran_hutang_produk_id := OLD.pembayaran_hutang_produk_id;
	
    var_row_count := (select count(*) from t_item_pembayaran_hutang_produk
                      where pembayaran_hutang_produk_id = var_pembayaran_hutang_produk_id);
                      
    IF var_row_count IS NULL THEN
        var_row_count := 0;  
    END IF;
	
	IF (var_row_count = 0) THEN
    	DELETE FROM t_pembayaran_hutang_produk WHERE pembayaran_hutang_produk_id = var_pembayaran_hutang_produk_id;
    END IF;
    
  	RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_hapus_header_bayar_hutang_produk() OWNER TO postgres;

--
-- Name: f_hapus_header_bayar_piutang_produk(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_hapus_header_bayar_piutang_produk() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE var_pembayaran_piutang_id t_guid;
DECLARE var_row_count integer;

BEGIN		    
    var_pembayaran_piutang_id := OLD.pembayaran_piutang_id;
	
    var_row_count := (select count(*) from t_item_pembayaran_piutang_produk
                      where pembayaran_piutang_id = var_pembayaran_piutang_id);
                      
    IF var_row_count IS NULL THEN
        var_row_count := 0;  
    END IF;
	
	IF (var_row_count = 0) THEN
    	DELETE FROM t_pembayaran_piutang_produk WHERE pembayaran_piutang_id = var_pembayaran_piutang_id;
    END IF;
    
  	RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_hapus_header_bayar_piutang_produk() OWNER TO postgres;

--
-- Name: f_kurangi_stok_produk(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_kurangi_stok_produk() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE 
	var_produk_id 				t_guid;
    
	var_stok_sekarang 			t_jumlah; -- stok etalase	
    var_stok_gudang_sekarang 	t_jumlah; -- stok gudang
    
    var_jumlah_lama 		t_jumlah;
	var_jumlah_baru 		t_jumlah;
    
    var_jumlah_retur_lama 	t_jumlah;
    var_jumlah_retur_baru 	t_jumlah;

	var_harga 				t_harga;
    
    is_retur				t_bool;
  
BEGIN		
	is_retur := FALSE;
            
    IF TG_OP = 'INSERT' THEN
    	var_produk_id := NEW.produk_id;
        var_jumlah_baru = NEW.jumlah;        
        var_harga = NEW.harga_jual;

	ELSIF TG_OP = 'UPDATE' THEN      
    	var_produk_id := NEW.produk_id;
        var_jumlah_baru = NEW.jumlah;
        var_jumlah_lama = OLD.jumlah;
        var_harga = NEW.harga_jual;
        
        -- jumlah retur
        var_jumlah_retur_baru = NEW.jumlah_retur;
        var_jumlah_retur_lama = OLD.jumlah_retur;        
    ELSE
    	var_produk_id := OLD.produk_id;
        var_jumlah_lama = OLD.jumlah;
        var_harga = OLD.harga_jual;
    END IF;        
            
    SELECT stok, stok_gudang INTO var_stok_sekarang, var_stok_gudang_sekarang
    FROM m_produk WHERE produk_id = var_produk_id;
    
    IF var_stok_sekarang IS NULL THEN -- stok etalase
        var_stok_sekarang := 0;  
    END IF;
    
    IF var_stok_gudang_sekarang IS NULL THEN -- stok gudang
        var_stok_gudang_sekarang := 0;  
    END IF;
        
    IF TG_OP = 'UPDATE' THEN -- pengecekan retur
    	IF var_jumlah_retur_lama <> var_jumlah_retur_baru THEN
        	is_retur := TRUE;                    
            
            IF var_jumlah_retur_lama IS NULL THEN
                var_jumlah_retur_lama := 0;  
            END IF;
            
            IF var_jumlah_retur_baru IS NULL THEN
                var_jumlah_retur_baru := 0;  
            END IF;
               
            -- diganti stok gudang                 	
            -- var_stok_sekarang = var_stok_sekarang - var_jumlah_retur_lama + var_jumlah_retur_baru;
            var_stok_gudang_sekarang = var_stok_gudang_sekarang - var_jumlah_retur_lama + var_jumlah_retur_baru;
			            
            UPDATE m_produk SET stok_gudang = var_stok_gudang_sekarang WHERE produk_id = var_produk_id;
        END IF;        
    END IF;
	
    IF (is_retur = FALSE) THEN -- bukan retur    	    	        
        IF TG_OP = 'INSERT' THEN
            -- var_stok_sekarang = var_stok_sekarang - var_jumlah_baru;
            var_stok_gudang_sekarang = var_stok_gudang_sekarang - var_jumlah_baru;
            
            IF (var_stok_gudang_sekarang < 0) THEN -- stok gudang kurang, sisanya ambil dari stok etalase
            	var_stok_sekarang = var_stok_sekarang - abs(var_stok_gudang_sekarang);
	            var_stok_gudang_sekarang = 0; --stok gudang habis
            END IF;
            
            IF (var_stok_sekarang < 0) then 
            	var_stok_sekarang = 0;
            END IF;
            
        ELSIF TG_OP = 'UPDATE' THEN      
            -- var_stok_sekarang = var_stok_sekarang + var_jumlah_lama - var_jumlah_baru;
            var_stok_gudang_sekarang = var_stok_gudang_sekarang + var_jumlah_lama - var_jumlah_baru;
        ELSE
            -- var_stok_sekarang = var_stok_sekarang + var_jumlah_lama;
            var_stok_gudang_sekarang = var_stok_gudang_sekarang + var_jumlah_lama;
        END IF;
        
        IF TG_OP = 'INSERT' THEN 
            -- UPDATE m_produk SET stok = var_stok_sekarang, harga_jual = var_harga WHERE produk_id = var_produk_id;        
            UPDATE m_produk SET stok = var_stok_sekarang, stok_gudang = var_stok_gudang_sekarang, harga_jual = var_harga WHERE produk_id = var_produk_id;        
        ELSE        
            -- UPDATE m_produk SET stok = var_stok_sekarang WHERE produk_id = var_produk_id;        
            UPDATE m_produk SET stok = var_stok_sekarang, stok_gudang = var_stok_gudang_sekarang WHERE produk_id = var_produk_id;        
        END IF;    
    END IF;	
            
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_kurangi_stok_produk() OWNER TO postgres;

--
-- Name: f_penyesuaian_stok_aiud(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_penyesuaian_stok_aiud() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
	var_produk_id 						t_guid;
    
    var_stok_etalase					t_jumlah;
    var_stok_gudang						t_jumlah;
    
    var_penambahan_stok_etalase			t_jumlah;
    var_penambahan_stok_gudang			t_jumlah;
    
    var_pengurangan_stok_etalase		t_jumlah;
    var_pengurangan_stok_gudang			t_jumlah;
    
    var_penambahan_stok_etalase_old		t_jumlah;
    var_penambahan_stok_gudang_old		t_jumlah;
    
    var_pengurangan_stok_etalase_old	t_jumlah;
    var_pengurangan_stok_gudang_old		t_jumlah;
    
BEGIN    
	IF TG_OP = 'INSERT' THEN
    	var_produk_id := NEW.produk_id;
        
        var_penambahan_stok_etalase = NEW.penambahan_stok;        
		var_penambahan_stok_gudang = NEW.penambahan_stok_gudang;        
        
        var_pengurangan_stok_etalase = NEW.pengurangan_stok;        
		var_pengurangan_stok_gudang = NEW.pengurangan_stok_gudang;        

	ELSIF TG_OP = 'UPDATE' THEN      
    	var_produk_id := NEW.produk_id;
        
        var_penambahan_stok_etalase = NEW.penambahan_stok;        
        var_penambahan_stok_etalase_old = OLD.penambahan_stok;        
        
		var_penambahan_stok_gudang = NEW.penambahan_stok_gudang;        
        var_penambahan_stok_gudang_old = OLD.penambahan_stok_gudang;        
        
        var_pengurangan_stok_etalase = NEW.pengurangan_stok;        
        var_pengurangan_stok_etalase_old = OLD.pengurangan_stok;        
        
		var_pengurangan_stok_gudang = NEW.pengurangan_stok_gudang;        
        var_pengurangan_stok_gudang_old = OLD.pengurangan_stok_gudang;        
               
    ELSE
    	var_produk_id := OLD.produk_id;
        
        var_penambahan_stok_etalase_old = OLD.penambahan_stok;                
        var_penambahan_stok_gudang_old = OLD.penambahan_stok_gudang;        
        
        var_pengurangan_stok_etalase_old = OLD.pengurangan_stok;                
        var_pengurangan_stok_gudang_old = OLD.pengurangan_stok_gudang; 
    END IF;
    
    SELECT stok, stok_gudang INTO var_stok_etalase, var_stok_gudang
    FROM m_produk WHERE produk_id = var_produk_id;
    
    IF var_stok_etalase IS NULL THEN
        var_stok_etalase := 0;  
    END IF;
    
    IF var_stok_gudang IS NULL THEN
        var_stok_gudang := 0;  
    END IF;
    
    IF TG_OP = 'INSERT' THEN		         
        IF (var_penambahan_stok_etalase > 0 OR var_penambahan_stok_gudang > 0) THEN
        	var_stok_etalase := var_stok_etalase + var_penambahan_stok_etalase;
            var_stok_gudang := var_stok_gudang + var_penambahan_stok_gudang;                    	
        END IF;   	        
        
        IF (var_pengurangan_stok_etalase > 0 OR var_pengurangan_stok_gudang > 0) THEN
        	var_stok_etalase := var_stok_etalase - var_pengurangan_stok_etalase;
            var_stok_gudang := var_stok_gudang - var_pengurangan_stok_gudang;                    	
        END IF;        

	ELSIF TG_OP = 'UPDATE' THEN      		      	                     
        IF (var_penambahan_stok_etalase > 0 OR var_penambahan_stok_etalase_old > 0 OR var_penambahan_stok_gudang > 0 OR var_penambahan_stok_gudang_old > 0) THEN
        	var_stok_etalase := var_stok_etalase - var_penambahan_stok_etalase_old + var_penambahan_stok_etalase;
            var_stok_gudang := var_stok_gudang - var_penambahan_stok_gudang_old + var_penambahan_stok_gudang;                    	
        END IF;
        
        IF (var_pengurangan_stok_etalase > 0 OR var_pengurangan_stok_etalase_old > 0 OR var_pengurangan_stok_gudang > 0 OR var_pengurangan_stok_gudang_old > 0) THEN
			var_stok_etalase := var_stok_etalase + var_pengurangan_stok_etalase_old - var_pengurangan_stok_etalase;
            var_stok_gudang := var_stok_gudang + var_pengurangan_stok_gudang_old - var_pengurangan_stok_gudang;                    	
        END IF;                        
        
    ELSE    	        
        IF (var_penambahan_stok_etalase_old > 0 OR var_penambahan_stok_gudang_old > 0) THEN
        	var_stok_etalase := var_stok_etalase - var_penambahan_stok_etalase_old;
            var_stok_gudang := var_stok_gudang - var_penambahan_stok_gudang_old;
        END IF;
        
        IF (var_pengurangan_stok_etalase_old > 0 OR var_pengurangan_stok_gudang_old > 0) THEN
        	var_stok_etalase := var_stok_etalase + var_pengurangan_stok_etalase_old;
            var_stok_gudang := var_stok_gudang + var_pengurangan_stok_gudang_old;
        END IF;
    END IF;    
    
    UPDATE m_produk SET stok = var_stok_etalase, stok_gudang = var_stok_gudang WHERE produk_id = var_produk_id;
            
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_penyesuaian_stok_aiud() OWNER TO postgres;

--
-- Name: f_tambah_stok_produk(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_tambah_stok_produk() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE 
	var_produk_id 			t_guid;
	
    var_stok_sekarang 		t_jumlah; -- stok etalase
	var_stok_gudang_sekarang 	t_jumlah; -- stok gudang
    
    var_jumlah_lama 		t_jumlah;
	var_jumlah_baru 		t_jumlah;
	
    var_jumlah_retur_lama 	t_jumlah;
    var_jumlah_retur_baru 	t_jumlah;
    
	var_harga 				t_harga;
    
    is_retur				t_bool;
  
BEGIN		   
	is_retur := FALSE;
	     
    IF TG_OP = 'INSERT' THEN
    	var_produk_id := NEW.produk_id;
        var_jumlah_baru = NEW.jumlah;
        var_harga = NEW.harga;
        
	ELSIF TG_OP = 'UPDATE' THEN      
    	var_produk_id := NEW.produk_id;
        var_jumlah_baru = NEW.jumlah;
        var_jumlah_lama = OLD.jumlah;
        var_harga = NEW.harga;
        
        -- jumlah retur
        var_jumlah_retur_baru = NEW.jumlah_retur;
        var_jumlah_retur_lama = OLD.jumlah_retur;                
    ELSE
    	var_produk_id := OLD.produk_id;
        var_jumlah_lama = OLD.jumlah;
        var_harga = OLD.harga;
    END IF;        
    
    SELECT stok, stok_gudang INTO var_stok_sekarang, var_stok_gudang_sekarang
    FROM m_produk WHERE produk_id = var_produk_id;
    
    IF var_stok_sekarang IS NULL THEN
    	var_stok_sekarang := 0;  
	END IF;
    
    IF var_stok_gudang_sekarang IS NULL THEN
    	var_stok_gudang_sekarang := 0;  
	END IF;
    
    IF TG_OP = 'UPDATE' THEN -- pengecekan retur
    	IF var_jumlah_retur_lama <> var_jumlah_retur_baru THEN
        	is_retur := TRUE;                    
            
            IF var_jumlah_retur_lama IS NULL THEN
                var_jumlah_retur_lama := 0;  
            END IF;
            
            IF var_jumlah_retur_baru IS NULL THEN
                var_jumlah_retur_baru := 0;  
            END IF;
               
            -- diganti stok gudang                 	
            -- var_stok_sekarang = var_stok_sekarang - var_jumlah_retur_lama + var_jumlah_retur_baru;
            var_stok_gudang_sekarang = var_stok_gudang_sekarang - var_jumlah_retur_lama + var_jumlah_retur_baru;
			            
            UPDATE m_produk SET stok_gudang = var_stok_gudang_sekarang WHERE produk_id = var_produk_id;
        END IF;        
    END IF;
    
    IF (is_retur = FALSE) THEN -- bukan retur    	    	        
      IF TG_OP = 'INSERT' THEN
          -- var_stok_sekarang = var_stok_sekarang + var_jumlah_baru;
          var_stok_gudang_sekarang = var_stok_gudang_sekarang + var_jumlah_baru;
      ELSIF TG_OP = 'UPDATE' THEN      
          -- var_stok_sekarang = var_stok_sekarang - var_jumlah_lama + var_jumlah_baru;
          var_stok_gudang_sekarang = var_stok_gudang_sekarang - var_jumlah_lama + var_jumlah_baru;
      ELSE
          -- var_stok_sekarang = var_stok_sekarang - var_jumlah_lama;
          var_stok_gudang_sekarang = var_stok_gudang_sekarang - var_jumlah_lama;
      END IF;
      
      IF TG_OP = 'INSERT' THEN 
          UPDATE m_produk SET stok_gudang = var_stok_gudang_sekarang, harga_beli = var_harga WHERE produk_id = var_produk_id;        
      ELSE        
          UPDATE m_produk SET stok_gudang = var_stok_gudang_sekarang WHERE produk_id = var_produk_id;        
      END IF;
    END IF;    	
            
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_tambah_stok_produk() OWNER TO postgres;

--
-- Name: f_update_jumlah_retur_beli(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_jumlah_retur_beli() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
	var_item_beli_id 		t_guid;
    var_jumlah_retur		t_jumlah;
        
BEGIN	
      
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_item_beli_id := NEW.item_beli_id;    
        var_jumlah_retur := NEW.jumlah_retur;
    ELSE
    	var_item_beli_id := OLD.item_beli_id;
        var_jumlah_retur := 0;
    END IF;
    
    IF var_jumlah_retur IS NULL THEN
        var_jumlah_retur := 0;  
    END IF; 
            
    UPDATE t_item_beli_produk SET jumlah_retur = var_jumlah_retur 
    WHERE item_beli_produk_id = var_item_beli_id;
	
	RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_jumlah_retur_beli() OWNER TO postgres;

--
-- Name: f_update_jumlah_retur_jual(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_jumlah_retur_jual() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
	var_item_jual_id 		t_guid;
    var_jumlah_retur		t_jumlah;
        
BEGIN	
      
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_item_jual_id := NEW.item_jual_id;    
        var_jumlah_retur := NEW.jumlah_retur;
    ELSE
    	var_item_jual_id := OLD.item_jual_id;
        var_jumlah_retur := 0;
    END IF;
    
    IF var_jumlah_retur IS NULL THEN
        var_jumlah_retur := 0;  
    END IF; 
            
    UPDATE t_item_jual_produk SET jumlah_retur = var_jumlah_retur 
    WHERE item_jual_id = var_item_jual_id;
	
	RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_jumlah_retur_jual() OWNER TO postgres;

--
-- Name: f_update_pelunasan_beli_produk(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_pelunasan_beli_produk() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE var_beli_produk_id t_guid;
DECLARE var_pelunasan_nota t_harga;
  
BEGIN
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_beli_produk_id := NEW.beli_produk_id;    
    ELSE
    	var_beli_produk_id := OLD.beli_produk_id;
    END IF;
	        
    var_pelunasan_nota := (SELECT SUM(nominal) FROM t_item_pembayaran_hutang_produk 
    			 	       WHERE beli_produk_id = var_beli_produk_id);
	
    IF var_pelunasan_nota IS NULL THEN
    	var_pelunasan_nota := 0;  
	END IF;
    
    UPDATE t_beli_produk SET total_pelunasan = var_pelunasan_nota 
    WHERE beli_produk_id = var_beli_produk_id;                        
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_pelunasan_beli_produk() OWNER TO postgres;

--
-- Name: f_update_pelunasan_jual_produk(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_pelunasan_jual_produk() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE var_jual_id t_guid;
DECLARE var_pelunasan_nota t_harga;
  
BEGIN
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_jual_id := NEW.jual_id;    
    ELSE
    	var_jual_id := OLD.jual_id;
    END IF;
	        
    var_pelunasan_nota := (SELECT SUM(nominal) FROM t_item_pembayaran_piutang_produk 
    			 	       WHERE jual_id = var_jual_id);	
    IF var_pelunasan_nota IS NULL THEN
    	var_pelunasan_nota := 0;  
	END IF;
    
    UPDATE t_jual_produk SET total_pelunasan = var_pelunasan_nota 
    WHERE jual_id = var_jual_id;                        
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_pelunasan_jual_produk() OWNER TO postgres;

--
-- Name: f_update_pelunasan_kasbon(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_pelunasan_kasbon() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE 
	var_bon_id					t_guid;    
	var_total_pelunasan_kasbon 	t_harga;
    
BEGIN
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_bon_id := NEW.bon_id;        
    ELSE
    	var_bon_id := OLD.bon_id;
    END IF;
	        
    -- pelunasan kasbon
    var_total_pelunasan_kasbon := (SELECT SUM(nominal) FROM t_pembayaran_bon 
    							   WHERE bon_id = var_bon_id);	    
	
    IF var_total_pelunasan_kasbon IS NULL THEN
    	var_total_pelunasan_kasbon := 0;  
	END IF;        
    
    -- kasbon
    UPDATE t_bon SET total_pelunasan = var_total_pelunasan_kasbon 
    WHERE bon_id = var_bon_id;    
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_pelunasan_kasbon() OWNER TO postgres;

--
-- Name: f_update_total_beli_produk(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_total_beli_produk() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE 
	var_beli_produk_id 		t_guid;
    var_retur_beli_id		t_guid;
	var_total_nota 			t_harga;
  	var_jumlah_retur_lama 	t_jumlah;
    var_jumlah_retur_baru 	t_jumlah;
    var_tahun_tempo			INTEGER;    
    is_retur				t_bool;
    
BEGIN
	is_retur := FALSE;
    
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_beli_produk_id := NEW.beli_produk_id;    
    ELSE
    	var_beli_produk_id := OLD.beli_produk_id;
    END IF;	 

    var_total_nota := (SELECT SUM((jumlah - jumlah_retur) * harga) 
    				   FROM t_item_beli_produk
					   WHERE beli_produk_id = var_beli_produk_id);
	
    IF var_total_nota IS NULL THEN
    	var_total_nota := 0;  
	END IF;                         

	IF TG_OP = 'UPDATE' THEN -- pengecekan retur
    	-- jumlah retur
        var_jumlah_retur_baru = NEW.jumlah_retur;
        var_jumlah_retur_lama = OLD.jumlah_retur;        
        
    	IF var_jumlah_retur_lama <> var_jumlah_retur_baru THEN
			is_retur := TRUE;
			
            var_retur_beli_id := (SELECT retur_beli_produk_id FROM t_retur_beli_produk WHERE beli_produk_id = var_beli_produk_id LIMIT 1);
            var_tahun_tempo := (SELECT EXTRACT(YEAR FROM tanggal_tempo) FROM t_beli_produk WHERE beli_produk_id = var_beli_produk_id);                                    
            
			IF var_tahun_tempo IS NULL THEN
                var_tahun_tempo := 0;  
            END IF;            
            
            IF var_tahun_tempo < 2010 THEN -- nota tunai            	
                UPDATE t_beli_produk SET total_nota = var_total_nota, retur_beli_produk_id = var_retur_beli_id 
                WHERE beli_produk_id = var_beli_produk_id;                
                
                UPDATE t_item_pembayaran_hutang_produk SET nominal = var_total_nota 
                WHERE beli_produk_id = var_beli_produk_id;
            ELSE
            	UPDATE t_beli_produk SET total_nota = var_total_nota, retur_beli_produk_id = var_retur_beli_id 
                WHERE beli_produk_id = var_beli_produk_id;
            END IF;        
        END IF;        
    END IF;
    
    IF (is_retur = FALSE) THEN -- bukan retur    	
	    UPDATE t_beli_produk SET total_nota = var_total_nota 
        WHERE beli_produk_id = var_beli_produk_id;
    END IF;
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_total_beli_produk() OWNER TO postgres;

--
-- Name: f_update_total_hutang_supplier(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_total_hutang_supplier() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE 	
    var_supplier_id 				t_guid;  
    var_supplier_id_old				t_guid;
    
    var_total_hutang_produk			t_harga;
  	var_total_pelunasan_produk		t_harga;
    
    var_grand_total_hutang			t_harga;
  	var_grand_total_pelunasan		t_harga;
    
BEGIN	    
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_supplier_id := NEW.supplier_id;        
    ELSE
    	var_supplier_id := OLD.supplier_id;
        var_supplier_id_old := OLD.supplier_id;
    END IF;
	    	           	                
    -- hitung total hutang dan pelunasan pembelian produk
    SELECT SUM(total_nota - diskon + ppn) AS total_hutang, SUM(total_pelunasan) AS total_pelunasan
    INTO var_total_hutang_produk, var_total_pelunasan_produk
    FROM t_beli_produk WHERE EXTRACT(YEAR FROM tanggal_tempo) > 2010 AND supplier_id = var_supplier_id;

    IF var_total_hutang_produk IS NULL THEN
        var_total_hutang_produk := 0;  
    END IF; 
        
    IF var_total_pelunasan_produk IS NULL THEN
        var_total_pelunasan_produk := 0;  
    END IF;
        
    var_grand_total_hutang := var_total_hutang_produk;
    var_grand_total_pelunasan := var_total_pelunasan_produk;        
        
    UPDATE m_supplier SET total_hutang = var_grand_total_hutang, total_pembayaran_hutang = var_grand_total_pelunasan 
    WHERE supplier_id = var_supplier_id;       
    
    IF TG_OP = 'UPDATE' THEN
    	var_supplier_id_old := OLD.supplier_id;
        
    	IF var_supplier_id <> var_supplier_id_old THEN
            -- hitung total hutang dan pelunasan pembelian produk
            SELECT SUM(total_nota - diskon + ppn) AS total_hutang, SUM(total_pelunasan) AS total_pelunasan
            INTO var_total_hutang_produk, var_total_pelunasan_produk
            FROM t_beli_produk WHERE EXTRACT(YEAR FROM tanggal_tempo) > 2010 AND supplier_id = var_supplier_id_old;

            IF var_total_hutang_produk IS NULL THEN
                var_total_hutang_produk := 0;  
            END IF; 
                
            IF var_total_pelunasan_produk IS NULL THEN
                var_total_pelunasan_produk := 0;  
            END IF;
                
            var_grand_total_hutang := var_total_hutang_produk;
            var_grand_total_pelunasan := var_total_pelunasan_produk;                
                            
            UPDATE m_supplier SET total_hutang = var_grand_total_hutang, total_pembayaran_hutang = var_grand_total_pelunasan 
            WHERE supplier_id = var_supplier_id_old;                                    
    		                        
            UPDATE t_pembayaran_hutang_produk SET supplier_id = var_supplier_id
            WHERE pembayaran_hutang_produk_id IN (SELECT pembayaran_hutang_produk_id FROM t_item_pembayaran_hutang_produk WHERE beli_produk_id = NEW.beli_produk_id);                
            
        END IF;
    END IF;            
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_total_hutang_supplier() OWNER TO postgres;

--
-- Name: f_update_total_jual_produk(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_total_jual_produk() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE 
	var_jual_id 			t_guid;
    var_retur_jual_id		t_guid;
	var_total_nota 			t_harga;        
    var_jumlah_retur_lama 	t_jumlah;
    var_jumlah_retur_baru 	t_jumlah;
    var_tahun_tempo			INTEGER;
    is_retur				t_bool;
    
BEGIN
	is_retur := FALSE;
    
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_jual_id := NEW.jual_id;    
    ELSE
    	var_jual_id := OLD.jual_id;        
    END IF;	    	          
    
    var_total_nota := (SELECT SUM((jumlah - jumlah_retur) * (harga_jual - (diskon / 100 * harga_jual))) 
    				   FROM t_item_jual_produk
					   WHERE jual_id = var_jual_id);	    

    IF var_total_nota IS NULL THEN
    	var_total_nota := 0;  
	END IF;     
    
    var_total_nota := ROUND(var_total_nota, 0);      
    
    IF TG_OP = 'UPDATE' THEN -- pengecekan retur
    	-- jumlah retur
        var_jumlah_retur_baru = NEW.jumlah_retur;
        var_jumlah_retur_lama = OLD.jumlah_retur;        
        
    	IF var_jumlah_retur_lama <> var_jumlah_retur_baru THEN
			is_retur := TRUE;
			
            var_retur_jual_id := (SELECT retur_jual_id FROM t_retur_jual_produk WHERE jual_id = var_jual_id LIMIT 1);
            var_tahun_tempo := (SELECT EXTRACT(YEAR FROM tanggal_tempo) FROM t_jual_produk WHERE jual_id = var_jual_id);                                    
            
			IF var_tahun_tempo IS NULL THEN
                var_tahun_tempo := 0;  
            END IF;            
            
            IF var_tahun_tempo < 2010 THEN -- nota tunai            	
                UPDATE t_jual_produk SET total_nota = var_total_nota, retur_jual_id = var_retur_jual_id 
                WHERE jual_id = var_jual_id;                
                
                UPDATE t_item_pembayaran_piutang_produk SET nominal = var_total_nota 
                WHERE jual_id = var_jual_id;
            ELSE
            	UPDATE t_jual_produk SET total_nota = var_total_nota, retur_jual_id = var_retur_jual_id 
                WHERE jual_id = var_jual_id;
            END IF;        
        END IF;        
    END IF;              

	IF (is_retur = FALSE) THEN -- bukan retur    	
    	UPDATE t_jual_produk SET total_nota = var_total_nota, retur_jual_id = NULL 
        WHERE jual_id = var_jual_id;
    END IF;
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_total_jual_produk() OWNER TO postgres;

--
-- Name: f_update_total_kasbon_karyawan(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_total_kasbon_karyawan() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE 
    var_karyawan_id		t_guid;
    
	var_total_kasbon 	t_harga;
  	var_total_pelunasan	t_harga;
    
BEGIN
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_karyawan_id := NEW.karyawan_id;    
    ELSE
    	var_karyawan_id := OLD.karyawan_id;
    END IF;	            
	
    SELECT SUM(nominal), SUM(total_pelunasan)
    INTO var_total_kasbon, var_total_pelunasan
	FROM t_bon WHERE karyawan_id = var_karyawan_id;
        
    IF var_total_kasbon IS NULL THEN
    	var_total_kasbon := 0;  
	END IF;
    
    IF var_total_pelunasan IS NULL THEN
    	var_total_pelunasan := 0;  
	END IF;
        
    UPDATE m_karyawan SET total_kasbon = var_total_kasbon, total_pembayaran_kasbon = var_total_pelunasan 
    WHERE karyawan_id = var_karyawan_id;
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_total_kasbon_karyawan() OWNER TO postgres;

--
-- Name: f_update_total_pengeluaran(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_total_pengeluaran() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE v_pengeluaran_id t_guid;
DECLARE v_total t_harga;
  
BEGIN
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	v_pengeluaran_id := NEW.pengeluaran_id;    
    ELSE
    	v_pengeluaran_id := OLD.pengeluaran_id;
    END IF;
        
    v_total := (SELECT SUM(jumlah * harga) FROM t_item_pengeluaran
			    WHERE pengeluaran_id = v_pengeluaran_id);
	
    IF v_total IS NULL THEN
    	v_total := 0;  
	END IF;
    
    UPDATE t_pengeluaran SET total = v_total WHERE pengeluaran_id = v_pengeluaran_id;                        
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_total_pengeluaran() OWNER TO postgres;

--
-- Name: f_update_total_piutang_customer(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_total_piutang_customer() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE 	
    var_total_piutang	t_harga;
    var_total_pelunasan	t_harga;
  	var_customer_id		t_guid;
    var_customer_id_old	t_guid;
    
BEGIN
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_customer_id := NEW.customer_id;        
    ELSE
    	var_customer_id := OLD.customer_id;
        var_customer_id_old = OLD.customer_id;
    END IF;
	    	            
	SELECT SUM(total_nota - diskon + transport + ppn), SUM(total_pelunasan)
    INTO var_total_piutang, var_total_pelunasan
	FROM t_jual_produk WHERE EXTRACT(YEAR FROM tanggal_tempo) > 2010 AND customer_id = var_customer_id;
    
    IF var_total_piutang IS NULL THEN
        var_total_piutang := 0;  
    END IF;
    
    IF var_total_pelunasan IS NULL THEN
        var_total_pelunasan := 0;  
    END IF;
        
    UPDATE m_customer SET total_piutang = var_total_piutang, total_pembayaran_piutang = var_total_pelunasan
	WHERE customer_id = var_customer_id;        
    
    IF TG_OP = 'UPDATE' THEN
		var_customer_id_old = OLD.customer_id;
		
        IF var_customer_id <> var_customer_id_old THEN
        	SELECT SUM(total_nota - diskon + transport + ppn), SUM(total_pelunasan)
            INTO var_total_piutang, var_total_pelunasan
            FROM t_jual_produk WHERE EXTRACT(YEAR FROM tanggal_tempo) > 2010 AND customer_id = var_customer_id_old;
            
            IF var_total_piutang IS NULL THEN
                var_total_piutang := 0;  
            END IF;
            
            IF var_total_pelunasan IS NULL THEN
                var_total_pelunasan := 0;  
            END IF;                           
			
			UPDATE m_customer SET total_piutang = var_total_piutang, total_pembayaran_piutang = var_total_pelunasan
            WHERE customer_id = var_customer_id_old;
            
            UPDATE t_pembayaran_piutang_produk SET customer_id = var_customer_id 
            WHERE pembayaran_piutang_id IN (SELECT pembayaran_piutang_id FROM t_item_pembayaran_piutang WHERE jual_id = NEW.jual_id);
        END IF;
    END IF;
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_total_piutang_customer() OWNER TO postgres;

--
-- Name: f_update_total_retur_beli_aiud(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_total_retur_beli_aiud() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
	var_retur_beli_produk_id	t_guid;
	var_total_nota 		t_harga;
    
BEGIN    
    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_retur_beli_produk_id := NEW.retur_beli_produk_id;    
    ELSE
    	var_retur_beli_produk_id := OLD.retur_beli_produk_id;
    END IF;
        
    var_total_nota := (SELECT SUM(jumlah_retur * harga) 
    				   FROM t_item_retur_beli_produk
					   WHERE retur_beli_produk_id = var_retur_beli_produk_id);
                           
    IF var_total_nota IS NULL THEN
    	var_total_nota := 0;  
	END IF;                         
    
    UPDATE t_retur_beli_produk SET total_nota = var_total_nota 
    WHERE retur_beli_produk_id = var_retur_beli_produk_id;
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_total_retur_beli_aiud() OWNER TO postgres;

--
-- Name: f_update_total_retur_produk_aiud(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION f_update_total_retur_produk_aiud() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
	var_retur_jual_id	t_guid;
	var_total_nota 		t_harga;
    
BEGIN    
    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_retur_jual_id := NEW.retur_jual_id;    
    ELSE
    	var_retur_jual_id := OLD.retur_jual_id;
    END IF;
        
    var_total_nota := (SELECT SUM(jumlah_retur * harga_jual) 
    				   FROM t_item_retur_jual_produk
					   WHERE retur_jual_id = var_retur_jual_id);
                           
    IF var_total_nota IS NULL THEN
    	var_total_nota := 0;  
	END IF;                         
    
    UPDATE t_retur_jual_produk SET total_nota = var_total_nota 
    WHERE retur_jual_id = var_retur_jual_id;
    
    RETURN NULL;
END;
$$;


ALTER FUNCTION public.f_update_total_retur_produk_aiud() OWNER TO postgres;

--
-- Name: kode_produk_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE kode_produk_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE kode_produk_seq OWNER TO postgres;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: m_alasan_penyesuaian_stok; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_alasan_penyesuaian_stok (
    alasan_penyesuaian_stok_id t_guid NOT NULL,
    alasan t_keterangan
);


ALTER TABLE m_alasan_penyesuaian_stok OWNER TO postgres;

--
-- Name: m_customer; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_customer (
    customer_id t_guid NOT NULL,
    nama_customer t_nama,
    alamat t_alamat,
    kontak t_nama,
    telepon t_telepon,
    plafon_piutang t_harga,
    total_piutang t_harga,
    total_pembayaran_piutang t_harga
);


ALTER TABLE m_customer OWNER TO postgres;

--
-- Name: m_golongan; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_golongan (
    golongan_id t_guid NOT NULL,
    nama_golongan t_nama
);


ALTER TABLE m_golongan OWNER TO postgres;

--
-- Name: m_item_menu; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_item_menu (
    item_menu_id t_guid NOT NULL,
    menu_id t_guid,
    grant_id integer,
    keterangan t_keterangan
);


ALTER TABLE m_item_menu OWNER TO postgres;

--
-- Name: COLUMN m_item_menu.grant_id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN m_item_menu.grant_id IS 'Mereference ke tabel m_role_privilege';


--
-- Name: m_jabatan; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_jabatan (
    jabatan_id t_guid NOT NULL,
    nama_jabatan t_nama,
    keterangan t_keterangan
);


ALTER TABLE m_jabatan OWNER TO postgres;

--
-- Name: m_jenis_pengeluaran; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_jenis_pengeluaran (
    jenis_pengeluaran_id t_guid NOT NULL,
    nama_jenis_pengeluaran t_keterangan
);


ALTER TABLE m_jenis_pengeluaran OWNER TO postgres;

--
-- Name: m_karyawan; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_karyawan (
    karyawan_id t_guid NOT NULL,
    jabatan_id t_guid,
    nama_karyawan t_nama,
    alamat t_alamat,
    telepon t_telepon,
    gaji_pokok t_harga,
    is_active t_bool,
    keterangan t_keterangan,
    jenis_gajian integer DEFAULT 1,
    gaji_lembur t_harga DEFAULT 0,
    total_kasbon t_harga,
    total_pembayaran_kasbon t_harga
);


ALTER TABLE m_karyawan OWNER TO postgres;

--
-- Name: m_menu; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_menu (
    menu_id t_guid NOT NULL,
    nama_menu t_nama,
    judul_menu t_keterangan,
    parent_id t_guid,
    order_number integer,
    is_active t_bool,
    nama_form t_keterangan,
    is_enabled t_bool
);


ALTER TABLE m_menu OWNER TO postgres;

--
-- Name: COLUMN m_menu.parent_id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN m_menu.parent_id IS 'Diisi dengan menu_id';


--
-- Name: m_pengguna; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_pengguna (
    pengguna_id t_guid NOT NULL,
    role_id t_guid,
    nama_pengguna t_nama,
    pass_pengguna t_password,
    is_active t_bool,
    status_user integer DEFAULT 2
);


ALTER TABLE m_pengguna OWNER TO postgres;

--
-- Name: COLUMN m_pengguna.status_user; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN m_pengguna.status_user IS '1 = Kasir
2 = Server
3 = Kasir dan Server';


--
-- Name: m_prefix_nota; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_prefix_nota (
    prefix_nota_id integer DEFAULT 1 NOT NULL,
    prefix_nota character varying(3),
    keterangan t_keterangan
);


ALTER TABLE m_prefix_nota OWNER TO postgres;

--
-- Name: m_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_produk (
    produk_id t_guid NOT NULL,
    nama_produk t_nama,
    satuan t_satuan,
    stok t_jumlah,
    harga_beli t_harga,
    harga_jual t_harga,
    kode_produk t_kode_produk,
    golongan_id t_guid,
    minimal_stok t_jumlah DEFAULT 0,
    stok_gudang t_jumlah,
    minimal_stok_gudang t_jumlah
);


ALTER TABLE m_produk OWNER TO postgres;

--
-- Name: m_profil; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_profil (
    profil_id t_guid NOT NULL,
    nama_profil t_keterangan,
    alamat t_alamat,
    kota t_alamat,
    telepon t_telepon
);


ALTER TABLE m_profil OWNER TO postgres;

--
-- Name: m_role; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_role (
    role_id t_guid NOT NULL,
    nama_role t_nama,
    is_active t_bool
);


ALTER TABLE m_role OWNER TO postgres;

--
-- Name: m_role_privilege; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_role_privilege (
    role_id t_guid NOT NULL,
    menu_id t_guid NOT NULL,
    grant_id integer NOT NULL,
    is_grant t_bool
);


ALTER TABLE m_role_privilege OWNER TO postgres;

--
-- Name: COLUMN m_role_privilege.grant_id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN m_role_privilege.grant_id IS 'Tambah, Perbaiki, Hapus, Dll';


--
-- Name: m_setting_aplikasi; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_setting_aplikasi (
    setting_aplikasi_id t_guid NOT NULL,
    url_update t_keterangan,
    db_version integer
);


ALTER TABLE m_setting_aplikasi OWNER TO postgres;

--
-- Name: m_shift; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_shift (
    shift_id t_guid NOT NULL,
    nama_shift t_keterangan,
    jam_mulai timestamp without time zone,
    jam_selesai timestamp without time zone,
    is_active t_bool
);


ALTER TABLE m_shift OWNER TO postgres;

--
-- Name: m_supplier; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_supplier (
    supplier_id t_guid NOT NULL,
    nama_supplier t_nama,
    alamat t_alamat,
    kontak t_nama,
    telepon t_telepon,
    total_hutang t_harga,
    total_pembayaran_hutang t_harga
);


ALTER TABLE m_supplier OWNER TO postgres;

--
-- Name: t_beli_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_beli_produk (
    beli_produk_id t_guid NOT NULL,
    pengguna_id t_guid,
    supplier_id t_guid,
    retur_beli_produk_id t_guid,
    nota t_nota,
    tanggal date,
    tanggal_tempo date,
    ppn t_harga,
    diskon t_harga,
    total_nota t_harga,
    total_pelunasan t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now()
);


ALTER TABLE t_beli_produk OWNER TO postgres;

--
-- Name: t_beli_produk_beli_produk_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_beli_produk_beli_produk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_beli_produk_beli_produk_id_seq OWNER TO postgres;

--
-- Name: t_bon; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_bon (
    bon_id t_guid NOT NULL,
    karyawan_id t_guid,
    pengguna_id t_guid,
    nota t_nota,
    tanggal date,
    nominal t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    total_pelunasan t_harga DEFAULT 0
);


ALTER TABLE t_bon OWNER TO postgres;

--
-- Name: t_bon_bon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_bon_bon_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_bon_bon_id_seq OWNER TO postgres;

--
-- Name: t_gaji_karyawan; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_gaji_karyawan (
    gaji_karyawan_id t_guid NOT NULL,
    karyawan_id t_guid,
    pengguna_id t_guid,
    bulan integer,
    tahun integer,
    kehadiran integer,
    absen integer,
    gaji_pokok t_harga,
    lembur t_harga,
    bonus t_harga,
    potongan t_harga,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    minggu integer DEFAULT 1,
    jam integer DEFAULT 0,
    lainnya t_harga DEFAULT 0,
    keterangan t_keterangan,
    jumlah_hari integer DEFAULT 0,
    tunjangan t_harga DEFAULT 0,
    kasbon t_harga DEFAULT 0,
    tanggal date
);


ALTER TABLE t_gaji_karyawan OWNER TO postgres;

--
-- Name: t_item_beli_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_item_beli_produk (
    item_beli_produk_id t_guid NOT NULL,
    beli_produk_id t_guid,
    pengguna_id t_guid,
    produk_id t_guid,
    harga t_harga,
    jumlah t_jumlah,
    diskon t_jumlah,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    jumlah_retur t_jumlah
);


ALTER TABLE t_item_beli_produk OWNER TO postgres;

--
-- Name: t_item_jual_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_item_jual_produk (
    item_jual_id t_guid NOT NULL,
    jual_id t_guid,
    pengguna_id t_guid,
    produk_id t_guid,
    harga_beli t_harga,
    harga_jual t_harga,
    jumlah t_jumlah,
    diskon t_jumlah,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    jumlah_retur t_jumlah
);


ALTER TABLE t_item_jual_produk OWNER TO postgres;

--
-- Name: t_item_pembayaran_hutang_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_item_pembayaran_hutang_produk (
    item_pembayaran_hutang_produk_id t_guid NOT NULL,
    pembayaran_hutang_produk_id t_guid,
    beli_produk_id t_guid,
    nominal t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now()
);


ALTER TABLE t_item_pembayaran_hutang_produk OWNER TO postgres;

--
-- Name: t_item_pembayaran_piutang_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_item_pembayaran_piutang_produk (
    item_pembayaran_piutang_id t_guid NOT NULL,
    pembayaran_piutang_id t_guid,
    jual_id t_guid,
    nominal t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now()
);


ALTER TABLE t_item_pembayaran_piutang_produk OWNER TO postgres;

--
-- Name: t_item_pengeluaran; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_item_pengeluaran (
    item_pengeluaran_id t_guid NOT NULL,
    pengeluaran_id t_guid,
    pengguna_id t_guid,
    jumlah t_jumlah,
    harga t_harga,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    jenis_pengeluaran_id t_guid
);


ALTER TABLE t_item_pengeluaran OWNER TO postgres;

--
-- Name: t_item_retur_beli_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_item_retur_beli_produk (
    item_retur_beli_produk_id t_guid NOT NULL,
    retur_beli_produk_id t_guid,
    pengguna_id t_guid,
    produk_id t_guid,
    harga t_harga,
    jumlah t_jumlah,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    jumlah_retur t_jumlah,
    item_beli_id t_guid
);


ALTER TABLE t_item_retur_beli_produk OWNER TO postgres;

--
-- Name: t_item_retur_jual_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_item_retur_jual_produk (
    item_retur_jual_id t_guid NOT NULL,
    retur_jual_id t_guid,
    pengguna_id t_guid,
    produk_id t_guid,
    harga_jual t_harga,
    jumlah t_jumlah,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    jumlah_retur t_jumlah,
    item_jual_id t_guid
);


ALTER TABLE t_item_retur_jual_produk OWNER TO postgres;

--
-- Name: t_jual_jual_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_jual_jual_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_jual_jual_id_seq OWNER TO postgres;

--
-- Name: t_jual_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_jual_produk (
    jual_id t_guid NOT NULL,
    pengguna_id t_guid,
    customer_id t_guid,
    nota t_nota,
    tanggal date,
    tanggal_tempo date,
    ppn t_harga,
    diskon t_harga,
    total_nota t_harga,
    total_pelunasan t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    retur_jual_id t_guid,
    shift_id t_guid
);


ALTER TABLE t_jual_produk OWNER TO postgres;

--
-- Name: t_mesin; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_mesin (
    mesin_id t_guid NOT NULL,
    pengguna_id t_guid,
    tanggal date DEFAULT ('now'::text)::date,
    saldo_awal t_harga,
    uang_masuk t_harga,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    shift_id t_guid,
    uang_keluar t_harga
);


ALTER TABLE t_mesin OWNER TO postgres;

--
-- Name: t_pembayaran_bon; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_pembayaran_bon (
    pembayaran_bon_id t_guid NOT NULL,
    bon_id t_guid,
    gaji_karyawan_id t_guid,
    tanggal date,
    nominal t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now()
);


ALTER TABLE t_pembayaran_bon OWNER TO postgres;

--
-- Name: t_pembayaran_hutang_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_pembayaran_hutang_produk (
    pembayaran_hutang_produk_id t_guid NOT NULL,
    supplier_id t_guid,
    pengguna_id t_guid,
    tanggal date,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    nota t_nota,
    is_tunai t_bool
);


ALTER TABLE t_pembayaran_hutang_produk OWNER TO postgres;

--
-- Name: t_pembayaran_hutang_produk_pembayaran_hutang_produk_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_pembayaran_hutang_produk_pembayaran_hutang_produk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_pembayaran_hutang_produk_pembayaran_hutang_produk_id_seq OWNER TO postgres;

--
-- Name: t_pembayaran_piutang_pembayaran_piutang_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_pembayaran_piutang_pembayaran_piutang_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_pembayaran_piutang_pembayaran_piutang_id_seq OWNER TO postgres;

--
-- Name: t_pembayaran_piutang_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_pembayaran_piutang_produk (
    pembayaran_piutang_id t_guid NOT NULL,
    customer_id t_guid,
    pengguna_id t_guid,
    tanggal date,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    nota t_nota,
    is_tunai t_bool
);


ALTER TABLE t_pembayaran_piutang_produk OWNER TO postgres;

--
-- Name: t_pengeluaran; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_pengeluaran (
    pengeluaran_id t_guid NOT NULL,
    pengguna_id t_guid,
    nota t_nota,
    tanggal date,
    total t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now()
);


ALTER TABLE t_pengeluaran OWNER TO postgres;

--
-- Name: t_pengeluaran_pengeluaran_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_pengeluaran_pengeluaran_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_pengeluaran_pengeluaran_id_seq OWNER TO postgres;

--
-- Name: t_penyesuaian_stok; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_penyesuaian_stok (
    penyesuaian_stok_id t_guid NOT NULL,
    produk_id t_guid,
    alasan_penyesuaian_id t_guid,
    tanggal date,
    penambahan_stok t_jumlah,
    pengurangan_stok t_jumlah,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    penambahan_stok_gudang t_jumlah,
    pengurangan_stok_gudang t_jumlah
);
ALTER TABLE ONLY t_penyesuaian_stok ALTER COLUMN alasan_penyesuaian_id SET STATISTICS 0;


ALTER TABLE t_penyesuaian_stok OWNER TO postgres;

--
-- Name: t_produksi_produksi_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_produksi_produksi_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_produksi_produksi_id_seq OWNER TO postgres;

--
-- Name: t_retur_beli_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_retur_beli_produk (
    retur_beli_produk_id t_guid NOT NULL,
    beli_produk_id t_guid,
    pengguna_id t_guid,
    supplier_id t_guid,
    nota t_nota,
    tanggal date,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    total_nota t_harga
);


ALTER TABLE t_retur_beli_produk OWNER TO postgres;

--
-- Name: t_retur_beli_produk_retur_beli_produk_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_retur_beli_produk_retur_beli_produk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_retur_beli_produk_retur_beli_produk_id_seq OWNER TO postgres;

--
-- Name: t_retur_jual_produk; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_retur_jual_produk (
    retur_jual_id t_guid NOT NULL,
    jual_id t_guid,
    pengguna_id t_guid,
    customer_id t_guid,
    nota t_nota,
    tanggal date,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    total_nota t_harga
);


ALTER TABLE t_retur_jual_produk OWNER TO postgres;

--
-- Name: t_retur_jual_retur_jual_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_retur_jual_retur_jual_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_retur_jual_retur_jual_id_seq OWNER TO postgres;

--
-- Name: t_sppd_sppd_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_sppd_sppd_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_sppd_sppd_id_seq OWNER TO postgres;

--
-- Name: kode_produk_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('kode_produk_seq', 57, true);


--
-- Data for Name: m_alasan_penyesuaian_stok; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_alasan_penyesuaian_stok (alasan_penyesuaian_stok_id, alasan) FROM stdin;
83bbf9aa-0d45-7041-e53b-d6c9073a49c0	Hilang (Barang hilang)
e4ef2a27-6600-365f-1e07-2963d55cc4bf	Koreksi (Koreksi karena kesalahan input)
1c23364b-e65d-62ef-4180-b2f3f7f560c1	Rusak (Barang rusak)
b1ad1bca-b590-2231-06a3-8c3cc5445eaf	Saldo Awal (Stok awal barang)
f227318c-72dc-5284-6b00-58005d511043	Stock Opname (Selisih stock buku dengan stok opname)
f9b35798-6725-244f-fec0-fdee38c5ad44	Pindah stok gudang ke etalase
7aa5ecff-4ed3-2e57-ead3-a493d822ab96	Lainnya
\.


--
-- Data for Name: m_customer; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_customer (customer_id, nama_customer, alamat, kontak, telepon, plafon_piutang, total_piutang, total_pembayaran_piutang) FROM stdin;
576c503f-69a7-46a5-b4be-107c634db7e3	Rudi				0.00	100000.00	0.00
c7b1ac7f-d201-474f-b018-1dc363d5d7f3	Swalayan Citrouli	Seturan		08138383838	0.00	0.00	0.00
0b9940b5-33a9-415b-9d44-8ee1d47e706d	Swalayan WS	Jl. Magelang	Mas Adi	0274-4444433	2000000.00	1500000.00	500000.00
\.


--
-- Data for Name: m_golongan; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_golongan (golongan_id, nama_golongan) FROM stdin;
0a8b59e5-bb3b-4081-b963-9dc9584dcda6	Accessories
2aae21ba-8954-4db6-a6dc-c648e27255ad	Hardward 2nd
71f7c928-7430-4484-bb0a-265c08f661f2	Service
338f5b8e-c425-424e-aa53-1698d499ccb3	Lain-lain
f96af6ff-b854-4ca7-b49c-cf277c4343b7	Notebook
fd9d730e-ed74-4041-9b17-6dd4433e6bc5	Lainnya
6ae85958-80c6-4f3a-bc01-53a715e25bf1	Hardware new
\.


--
-- Data for Name: m_item_menu; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_item_menu (item_menu_id, menu_id, grant_id, keterangan) FROM stdin;
ca7dd26a-e233-479f-b54c-e83f3b76612b	003a36a3-5992-4b29-ba63-0df45d3a5674	0	Melihat Data
653717d1-3e21-42f0-ad7e-f033a70b4d5e	003a36a3-5992-4b29-ba63-0df45d3a5674	1	Tambah Data
74ee753d-3a6a-4a11-a5a0-9d2ae111e580	003a36a3-5992-4b29-ba63-0df45d3a5674	2	Edit Data
827f17cf-0166-4c70-8103-09642de535ea	003a36a3-5992-4b29-ba63-0df45d3a5674	3	Hapus Data
da644e2b-50ab-4837-8b34-22feda2552ff	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	0	Melihat Data
b8ec714b-2e36-4d62-a03a-d1aada2ac560	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	1	Tambah Data
937f3348-69f0-482a-9320-becd9caec656	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	2	Edit Data
28eaeed1-28ca-48ea-8830-08b603d4500f	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	3	Hapus Data
a4250fac-85d8-409d-81f7-051884303ef9	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	0	Melihat Data
f6067ea1-5164-4b83-9a17-b08aaa646cbc	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	1	Tambah Data
41e83772-d33e-4b86-a9cf-ab85d16be91a	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	2	Edit Data
793a284e-bd71-451f-806b-cec41cac0dba	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	3	Hapus Data
6d4035f8-76d2-468a-ad77-257be6db1735	eeae3470-9109-4784-951a-30dcb6f836f4	0	Melihat Data
a52f86aa-fbab-4822-8cab-7d2dc7c068d5	eeae3470-9109-4784-951a-30dcb6f836f4	1	Tambah Data
d2fb9190-b4a6-4c66-934d-5aec6dae0cf8	eeae3470-9109-4784-951a-30dcb6f836f4	2	Edit Data
a0eb553a-68e2-4d51-b448-b0439b5b8bc2	eeae3470-9109-4784-951a-30dcb6f836f4	3	Hapus Data
d4721455-9c01-4647-83f1-695d605d42c1	d040ab9e-523a-419b-9705-3ebd49f58f08	0	Melihat Data
e7608a4a-38d8-4988-bc68-990b283838f8	d040ab9e-523a-419b-9705-3ebd49f58f08	1	Tambah Data
094c78a3-fe1c-4ad2-8ed8-bb1a315be762	d040ab9e-523a-419b-9705-3ebd49f58f08	2	Edit Data
722350fb-9864-4caf-939b-d57e3d0962c5	d040ab9e-523a-419b-9705-3ebd49f58f08	3	Hapus Data
638fe238-6d6e-4c0f-9364-2671e0efc66b	6bfac243-7b37-48a9-b324-7e548fa2fce3	0	Melihat Data
fdd62db3-6275-4f08-95f6-96b55139ebfe	6bfac243-7b37-48a9-b324-7e548fa2fce3	1	Tambah Data
8d4098fc-bc22-43f7-aa5a-d3c2d9cd7c6e	6bfac243-7b37-48a9-b324-7e548fa2fce3	2	Edit Data
decfd45f-8463-4096-bfc1-e331e712f36d	6bfac243-7b37-48a9-b324-7e548fa2fce3	3	Hapus Data
8f51448c-b8ae-4f8e-9f2b-d0cf2ffe102b	c341d139-d39b-4159-984c-40923ac3c68e	0	Melihat Data
72b8f544-c22e-49db-b7ef-54a425ca9c35	c341d139-d39b-4159-984c-40923ac3c68e	1	Tambah Data
674ac4cb-e37b-4471-ba17-e5a70202d779	c341d139-d39b-4159-984c-40923ac3c68e	2	Edit Data
288a85c7-1820-4e76-89d3-8760a6cfa826	c341d139-d39b-4159-984c-40923ac3c68e	3	Hapus Data
578f533f-85d3-4f49-b563-899ad6170d53	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	0	Melihat Data
bcdc1f65-291a-4039-96cd-7db0f2d4393a	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	1	Tambah Data
633f401f-e364-4865-b712-0c2436567631	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	2	Edit Data
43ba872c-44b6-4aee-88b2-d5ee3e1ca437	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	3	Hapus Data
349209c1-7222-42fc-a914-a4d54abb982d	5c5c0469-5edf-4422-96a0-0b173a4fe042	0	Melihat Data
51033ae6-0f30-4c92-b5dc-7ca7ee9c9a4d	5c5c0469-5edf-4422-96a0-0b173a4fe042	1	Tambah Data
f8e72358-6f5f-4564-be5d-d86320955804	5c5c0469-5edf-4422-96a0-0b173a4fe042	2	Edit Data
8599c45a-0716-4fe7-bc6a-84f712b019d5	5c5c0469-5edf-4422-96a0-0b173a4fe042	3	Hapus Data
c869cb1f-a5cd-49cf-8075-0dd4647246a2	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	0	Melihat Data
a8affe1f-d251-40c5-9926-b3a4302f357a	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	1	Tambah Data
e31dadf2-ce88-4587-9580-91b4216b3764	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	2	Edit Data
dec78b70-1675-428f-bea0-d6029e1e72fc	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	3	Hapus Data
b772a5e6-81bf-4849-813e-53fc437c59ee	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	0	Melihat Data
67e4cebc-cd38-43e3-b19e-6239384c89cd	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	1	Tambah Data
fc4a92d2-f5e1-4be5-a39f-786b7afeca5c	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	2	Edit Data
fb74d9be-7c23-4770-a890-5623021a1eb7	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	3	Hapus Data
1991ab93-5cc5-44b6-a321-d3a7d1b9c8ef	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	0	Melihat Data
fc8aeba8-cb8b-467e-b920-09deddc285c1	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	1	Tambah Data
4c172048-3b25-45cc-b070-5c3ef46297f6	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	2	Edit Data
5f68c189-3523-4774-adef-aeb0dd30316a	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	3	Hapus Data
b971e436-b832-4c65-8b23-103a532d266a	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	0	Melihat Data
ba4cb592-ccbf-4741-b7c6-778c830f6294	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	1	Tambah Data
5b07420d-fd42-43f5-bfc6-e39f82c8677d	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	2	Edit Data
c11f1dd5-efec-433a-8fa9-7fdcefc65a94	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	3	Hapus Data
c9d03257-59ce-4e3d-9ed0-a2bce2139bab	5df78447-219a-47c8-8a28-53b8e71ffb9d	0	Melihat Data
ba924c93-7fd7-410f-9eb4-d89e8e9055e7	5df78447-219a-47c8-8a28-53b8e71ffb9d	1	Tambah Data
e15d4f3e-050d-4e9c-b53d-13e32f619aea	5df78447-219a-47c8-8a28-53b8e71ffb9d	2	Edit Data
d44aff2c-c70d-4ff6-b2fe-d42e05f1d9a2	5df78447-219a-47c8-8a28-53b8e71ffb9d	3	Hapus Data
e42615f7-ea3f-4b1d-b9a0-2085bb1cc00d	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	0	Melihat Data
9b0c3c6b-5c1c-436b-8692-2f2c0c2a97d1	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	1	Tambah Data
7334ea68-359f-414f-b55b-467625732044	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	2	Edit Data
bd585781-e1ac-453b-b5c4-fcc85ac5939a	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	3	Hapus Data
d3d8998c-8f23-486d-b5dd-a681535ee437	df4fede3-7b52-4ff7-b5ce-82e924befbe3	0	Melihat Data
688c26c7-785b-4c95-9393-7f10b5304ffd	df4fede3-7b52-4ff7-b5ce-82e924befbe3	1	Tambah Data
11bf5888-c080-4ef9-ad0e-7db15adc7479	df4fede3-7b52-4ff7-b5ce-82e924befbe3	2	Edit Data
09670d46-bd3e-475a-a8c8-537c317f1ea1	df4fede3-7b52-4ff7-b5ce-82e924befbe3	3	Hapus Data
c36a7247-51a0-4ba0-8cf2-9373a2e6efd0	461d86dc-e2b9-4709-9e58-c58071463eb3	0	Melihat Data
f4f6238a-4cbf-4f24-badc-2e31316b1e87	461d86dc-e2b9-4709-9e58-c58071463eb3	1	Tambah Data
b23b0941-c7a8-4ba0-9e4f-f6b43cca5caa	461d86dc-e2b9-4709-9e58-c58071463eb3	2	Edit Data
639afbae-e245-4b54-a698-505be6d00e97	461d86dc-e2b9-4709-9e58-c58071463eb3	3	Hapus Data
f1faf5d9-b775-40c3-9abf-d79da3257ea0	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	0	Melihat Data
94e44c21-45a0-40ef-8d80-6e5ca2415057	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	1	Tambah Data
c26d312f-e286-4fe6-93a3-0ba7cc930087	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	2	Edit Data
23e6322b-f66b-4687-8bec-8c5cf1259caa	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	3	Hapus Data
f33fa3da-546f-44ff-bde2-70715caed6ad	b81248f7-25a9-4b86-9fe6-4e773b916a5c	0	Melihat Data
5d166683-3d28-4c0a-a6cc-bd43e3a38a37	b81248f7-25a9-4b86-9fe6-4e773b916a5c	1	Tambah Data
bc878bae-acc5-4e68-9d50-c5dc2780b39b	b81248f7-25a9-4b86-9fe6-4e773b916a5c	2	Edit Data
1fef33c4-35db-4aa4-aa38-ef2c6928df8f	b81248f7-25a9-4b86-9fe6-4e773b916a5c	3	Hapus Data
f498c54c-38dc-474b-b9d2-02e98f73e0e2	95a6d4d3-7875-442e-84a0-1db439879d0c	0	Melihat Data
1d89ef7a-67b8-4601-863f-07a52c1eaa3e	95a6d4d3-7875-442e-84a0-1db439879d0c	1	Tambah Data
85b86036-8f14-4068-8ff2-4ad532535d0f	95a6d4d3-7875-442e-84a0-1db439879d0c	2	Edit Data
764fb8ed-7367-451d-9400-22c49e334433	95a6d4d3-7875-442e-84a0-1db439879d0c	3	Hapus Data
f0788714-9ccc-4308-a2bf-6bb8f82a266c	0e07404a-00c8-43c4-9802-9718744bfb15	0	Melihat Data
e03d7c8c-56e0-49f4-8da9-31a93c5455c8	0e07404a-00c8-43c4-9802-9718744bfb15	1	Tambah Data
c62adc70-b595-4718-bf03-8b6966b3c25a	0e07404a-00c8-43c4-9802-9718744bfb15	2	Edit Data
74980bf3-165e-4430-b7b2-5b951755ce32	0e07404a-00c8-43c4-9802-9718744bfb15	3	Hapus Data
c45ddc8d-de0b-49b0-b3ce-4bb5b12eff66	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	0	Melihat Data
20d70994-76f7-43b7-9921-320e455b2b83	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	1	Tambah Data
7b03e564-5838-4714-aba2-f800c83b9fcc	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	2	Edit Data
2af89dad-7855-4f32-9d9e-afdc782ca98c	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	3	Hapus Data
90eb0f45-60c1-4a9c-a7e3-80886d998fa1	f47f139d-5d7d-4127-b389-aeebc38fbe05	0	Melihat Data
62068203-b3b8-47dd-a581-7005071e13c9	f47f139d-5d7d-4127-b389-aeebc38fbe05	1	Tambah Data
5bf19aa8-b921-49bc-9022-8be329e63565	f47f139d-5d7d-4127-b389-aeebc38fbe05	2	Edit Data
7766fcc3-6992-400f-ade6-41a89e0dcb26	f47f139d-5d7d-4127-b389-aeebc38fbe05	3	Hapus Data
d45a9715-ce2b-457e-8158-545ed5fa4ca4	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	0	Melihat Data
73311e6d-5ddb-41e8-843d-b44fb11a45b8	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	1	Tambah Data
8b6e035d-e958-4bb4-988a-2b9c5767dd55	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	2	Edit Data
13e08a2f-106f-49dd-b978-6ee74fe4db13	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	3	Hapus Data
9fad6102-b8e1-46eb-bfc9-fa9f503dfb25	1efb8e4c-b97e-4183-8bab-9560ddcee684	0	Melihat Data
fbb597d8-206a-42e4-a0e2-21b212dfbca2	1efb8e4c-b97e-4183-8bab-9560ddcee684	1	Tambah Data
3bfb613b-35a3-4a87-b0b2-aed6eb89ee2b	1efb8e4c-b97e-4183-8bab-9560ddcee684	2	Edit Data
8fd07940-55bf-4c6c-9db5-b0f06057e513	1efb8e4c-b97e-4183-8bab-9560ddcee684	3	Hapus Data
b6be7797-eb10-424d-b5e7-bd7a91208a4c	d7a5b577-c13d-4f05-954e-6e503affeb42	0	Melihat Data
08f10805-e513-4ca7-b1a3-291c0fa9d148	d7a5b577-c13d-4f05-954e-6e503affeb42	1	Tambah Data
4ee9ea9d-8a0a-4958-bc0c-68c8e4753cfc	d7a5b577-c13d-4f05-954e-6e503affeb42	2	Edit Data
5fd4106c-77ae-4edb-9822-e05d59ee5da7	d7a5b577-c13d-4f05-954e-6e503affeb42	3	Hapus Data
73adcddd-7706-478c-9086-c94f12255611	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	0	Melihat Data
ae5bc873-b735-4088-a871-0750aa7e1af2	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	1	Tambah Data
148e405b-7510-4240-a1ef-c22ec4984808	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	2	Edit Data
da6223cf-fa33-41d4-ac46-36e1b8eefd44	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	3	Hapus Data
06a00bc7-3999-45b0-a8c7-2e1b25942bc7	7cc65f85-a8ac-4cdf-99a7-2a38aa0526d0	2	Edit Data
8d9db475-d554-4c46-b56b-dd18962997d1	aa38c310-0400-4240-ae3d-a1445f4d7c5e	2	Edit Data
22e29d72-3b93-4e09-a01e-eb42ed005b37	1e4d2010-03c7-4013-8014-15681a344cf8	0	Melihat Data
e7448811-f9b5-4581-8922-3020b697b19b	1e4d2010-03c7-4013-8014-15681a344cf8	1	Tambah Data
a1304396-49e0-4c4b-ae2f-8318c18f0d75	1e4d2010-03c7-4013-8014-15681a344cf8	2	Edit Data
1668d0cc-bc2e-44db-a603-2e510440cbf5	1e4d2010-03c7-4013-8014-15681a344cf8	3	Hapus Data
0f4343cb-2e6d-45eb-bf24-7e2a33d451b2	69512f8a-ae0c-469e-ad40-75b40bcfa320	0	Melihat Data
c8ed8c76-8f63-4d10-8ac6-7f2558a3bb28	69512f8a-ae0c-469e-ad40-75b40bcfa320	1	Tambah Data
6a6ba61a-db4d-40b4-a404-f1500b38e95a	69512f8a-ae0c-469e-ad40-75b40bcfa320	2	Edit Data
4ac460b4-248d-449d-849e-918a6aade7fc	69512f8a-ae0c-469e-ad40-75b40bcfa320	3	Hapus Data
38648850-895b-499b-affe-b45715e26c06	e9dfcc3e-fdf9-4b34-b26b-39de50b7821d	0	Melihat Data
a5a36e8f-30f6-4752-b33e-edb22380fcd1	68c5c0b9-476c-4e90-98ca-9042b58c1a1c	0	Melihat Data
6e7ab11a-0f4a-42fe-91ec-557b756bc6dd	457a1049-0cad-4d0f-9e5b-827804f98505	0	Melihat Data
56891d62-9c44-40c5-9629-661d9d718773	72c92ee7-e265-4549-90db-de66f6be3a10	0	Melihat Data
b8d8858f-cc44-495f-995c-b04fac7e03c4	c37dc28b-558f-435a-8ae1-929a1903af22	0	Melihat Data
3f507e94-c565-42ce-a0fa-3f24631aa0f5	87b398c4-f0a6-49bb-b842-0ba5575f5aa2	0	Melihat Data
d2c27c04-e48a-4271-a649-08b035f349d3	e2014005-b6f7-4e41-bc4a-fac1c0965b35	0	Melihat Data
0a6343d1-8571-46b0-bba1-937d61d9928f	85194f97-1b92-46be-8250-d27e8edd0ae0	0	Melihat Data
6834dd08-2869-4b91-9c1b-896dbdc3d0e2	99038701-32a2-475a-bfe0-ba9b6fbff9f2	0	Melihat Data
bec7a75c-5202-4193-a044-21a068a0c30a	36c201f6-3b6a-4472-b561-833e854c1136	0	Melihat Data
3e95d139-eceb-4bc4-845b-7c6ef559e65b	91c848cf-af7f-4921-8be5-1247212ce8b8	0	Melihat Data
873fea5f-68f3-4df8-9575-41873d154f68	1a40f98b-5fcd-47ea-ac28-8da9f2595415	0	Melihat Data
8c0cdd0f-8366-44c6-9043-b8ce0267257a	f3b11c04-36c3-4f62-8fbb-63b56bcea30f	0	Melihat Data
de34aa9a-a85a-411d-b0d8-41df8d01d608	30cf483b-7b71-4f48-8e0b-d9e8c77e172c	0	Melihat Data
62d5df74-0b2c-4205-8106-bfabd00f0684	1f57066a-c2bf-4e47-a091-2945dabb3234	0	Melihat Data
20b9bc09-3345-41f7-b017-51f6dfa92064	a3e519ec-f1a9-499a-bbf5-8b3c84f6619b	0	Melihat Data
299081e3-9922-4d04-8f0e-cab2ad860684	c81c61b2-ee72-413b-bad9-84a90a1a4420	0	Melihat Data
b367153e-ed64-443c-a3c5-bc2463fea536	0cf37a9a-66f1-412a-971e-a2000528a2f5	0	Melihat Data
5f353c68-8816-e6ce-103d-c16ff5a63d2b	059c46c8-51af-2adc-6ed3-74ab78e50ef6	0	Melihat Data
da7409d8-fc7e-3bb7-7ba8-2ad09cdffdea	059c46c8-51af-2adc-6ed3-74ab78e50ef6	1	Tambah Data
3353ca3f-f4a7-60d1-039a-9fe0ac721367	059c46c8-51af-2adc-6ed3-74ab78e50ef6	2	Edit Data
0fa05bb1-a2df-c4bb-c7f9-e10bdeeb9d19	059c46c8-51af-2adc-6ed3-74ab78e50ef6	3	Hapus Data
7f4e6cf4-f8b1-08f0-19fb-82156bd98dfc	08a76a4e-04f3-ad71-bc8f-14f7d16c5cd6	0	Melihat Data
662d9ceb-fc21-4744-8706-fa61ff676aea	427a0fe7-c80a-44e7-ac92-1ca5edc31d56	0	Melihat Data
3fede9eb-93f5-5ab1-2b8a-bc298965b1e8	4d375f06-d18f-5bbe-885e-42e1f60d6f41	0	Melihat Data
e82c3a08-1bb0-9787-52fd-8e4e8d1958db	4d375f06-d18f-5bbe-885e-42e1f60d6f41	1	Tambah Data
89c3fc29-2e10-e247-081b-21c71a1a48e7	4d375f06-d18f-5bbe-885e-42e1f60d6f41	2	Edit Data
515760ed-f621-c41d-e830-d67cb29ee214	4d375f06-d18f-5bbe-885e-42e1f60d6f41	3	Hapus Data
55625caa-d7cf-6046-960a-8a04bf3f428c	56b17113-b1df-640c-36e5-ced6528603f9	0	Melihat Data
0c91d24d-17c5-720a-d2d4-7053d5bdd54f	5970451e-566b-cecf-0635-f9a2690161ff	0	Melihat Data
60be0f81-b465-5eec-33ec-8722e98e8f5b	ee3f7dd4-53f0-fc59-9d70-10840fefe273	0	Melihat Data
113429cb-0fed-a683-9beb-f57d7a2e1769	14b96c86-8baa-a1ad-d7d9-9ce8c195b1b1	0	Melihat Data
e8a696f8-1f64-1173-0927-08ff956b4a36	7683e0f5-bf9b-022b-4343-e34fdbf2c259	0	Melihat Data
bca9baef-add6-2c79-30b4-fef549a806fa	7683e0f5-bf9b-022b-4343-e34fdbf2c259	1	Tambah Data
2901a4cb-c044-9e47-015f-6442ff28faef	7683e0f5-bf9b-022b-4343-e34fdbf2c259	2	Edit Data
c3389833-5b0a-47ef-f504-585eea031325	7683e0f5-bf9b-022b-4343-e34fdbf2c259	3	Hapus Data
fabe93af-52d5-eb9a-a3f9-bd7947476c63	a9a1c62f-b33c-2b20-fb89-fe97174aa644	0	Melihat Data
0c7e8049-74b7-1d97-9f1f-29e3c8270893	4b09624d-01ed-3749-a6a5-f4a1e3769eaf	0	Melihat Data
08e478c2-402a-642f-7bcc-e39ed5dec355	e4c7f7e9-41f4-9470-cab9-f5dbb8c91a16	0	Melihat Data
6c5f6d91-98d2-4620-602e-c2eee263fa1e	e4c7f7e9-41f4-9470-cab9-f5dbb8c91a16	1	Tambah Data
3c86deeb-3f3f-f915-001d-7cdb859d90f7	e4c7f7e9-41f4-9470-cab9-f5dbb8c91a16	2	Edit Data
34942fd8-49ee-bbf8-9621-ab0da52ae676	e4c7f7e9-41f4-9470-cab9-f5dbb8c91a16	3	Hapus Data
38159a4a-1c89-98c4-7d14-ace823a8a129	4da9d8fa-1c86-82c9-6a6f-db6864f56022	0	Melihat Data
88ce07af-e1af-7ee4-f64f-a7003bd92377	4da9d8fa-1c86-82c9-6a6f-db6864f56022	1	Tambah Data
5a3ae738-a76e-15f5-901c-2ee79206549e	4da9d8fa-1c86-82c9-6a6f-db6864f56022	2	Edit Data
09c48b56-3c35-2b9b-8f2c-e110080d5141	4da9d8fa-1c86-82c9-6a6f-db6864f56022	3	Hapus Data
a2994efa-e529-1dab-9129-41a955ff7580	2cac36d0-9861-d5c9-2fd9-f72fc1c0f1c1	0	Melihat Data
6ba279d7-6de2-484a-bec8-2a9c87a10d16	2cac36d0-9861-d5c9-2fd9-f72fc1c0f1c1	1	Tambah Data
ebae387a-fe3a-01ae-bb7a-4bc7ddff50bc	2cac36d0-9861-d5c9-2fd9-f72fc1c0f1c1	2	Edit Data
6ef989cb-1779-638e-3cda-9bb030e86e86	2cac36d0-9861-d5c9-2fd9-f72fc1c0f1c1	3	Hapus Data
89deb2aa-1699-d6b1-fbe2-b5bf991e681d	129f17c8-56bc-3c13-4e3f-6d85db6112fc	0	Melihat Data
4c616a13-95d4-1bf0-d285-87bfff68ec7f	129f17c8-56bc-3c13-4e3f-6d85db6112fc	1	Tambah Data
5e2fba3a-7931-5611-a10b-ba726343362d	129f17c8-56bc-3c13-4e3f-6d85db6112fc	2	Edit Data
5324baf4-a03d-08c3-0de5-68edcbdad920	129f17c8-56bc-3c13-4e3f-6d85db6112fc	3	Hapus Data
33624469-552e-f75c-793e-9823529d5af4	88db4a0f-9890-4f1a-8433-2856b2a55209	0	Melihat Data
bf4c4e09-80da-84f1-377d-fbf5d2523b72	1cbd9e74-7133-4b5e-d139-f9025ba89106	0	Melihat Data
ee63a3e5-4245-bbe9-56ef-fe3a25c5235f	84b8444c-13f4-ca99-f1e2-69fd0218be44	0	Melihat Data
d4ebf180-29ed-187b-2e62-152fbcd205f3	a62dc67c-6d9e-bbe3-79dc-5f15c506ca20	0	Melihat Data
a6bfb7ee-14d5-7bc9-78a6-4b2006370589	03c70546-4fe4-5a4a-c3bd-9622538fdaf3	0	Melihat Data
85b957b8-15a0-995a-0f73-a4dbefb3bd98	8494f427-f4fb-279e-9b06-7a08882a8b6b	0	Melihat Data
1f0247c1-28a1-35a5-5034-a0a92eb2f33e	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	0	Melihat Data
6bba061e-cfcc-d13c-1bfc-938af5188444	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	1	Tambah Data
c4b90bb3-0050-53cf-6fea-cacb16fd885c	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	2	Edit Data
4eb0b765-f234-2cdf-2663-b310bde747f0	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	3	Hapus Data
7b079ce1-ae3e-5ae0-dbbf-197091dea1ac	4eb57c99-3623-db52-bee6-1a5b56403a49	0	Melihat Data
bc666f05-06f4-d8c0-bf9c-95f1d48de25e	4eb57c99-3623-db52-bee6-1a5b56403a49	1	Tambah Data
6dba3a3a-c244-3037-8675-fa29db75c6fb	4eb57c99-3623-db52-bee6-1a5b56403a49	2	Edit Data
e8bc3022-0130-953b-be22-39b59f9fa525	4eb57c99-3623-db52-bee6-1a5b56403a49	3	Hapus Data
c76f228f-44e2-a591-6dfe-395af1d21857	35eec8d9-c95a-2e3d-45f5-b0df52155e64	0	Melihat Data
bfe0272c-b004-5213-dc12-fac6ffd7143c	021a3899-66c8-0bd5-63bb-8d96649582f5	0	Melihat Data
39083cb9-3a71-d355-d08c-bdbed4e52915	021a3899-66c8-0bd5-63bb-8d96649582f5	1	Tambah Data
c7e909c5-1954-b3cb-fcf8-39cf76b0c6e3	021a3899-66c8-0bd5-63bb-8d96649582f5	2	Edit Data
1b6d7782-e440-a93c-3cc7-ed91bd13c298	021a3899-66c8-0bd5-63bb-8d96649582f5	3	Hapus Data
73f9fd84-e518-55ee-1e48-c1b4b4a892fd	24eb2565-989a-6c92-0a9e-db0ec2b6d582	0	Melihat Data
6750eb38-115b-aff3-b990-c9b9d8e2f358	5f5b601c-8514-fb8c-6f7b-fb9c536b8bd2	0	Melihat Data
21894f8a-b4e5-d577-49a7-fb751ad44183	5f5b601c-8514-fb8c-6f7b-fb9c536b8bd2	1	Tambah Data
26104913-c2f9-23ac-5b5b-59d2ea75f8e9	5f5b601c-8514-fb8c-6f7b-fb9c536b8bd2	2	Edit Data
f0568c97-7aee-e7b0-b0f4-0c6e312ed2c8	5f5b601c-8514-fb8c-6f7b-fb9c536b8bd2	3	Hapus Data
5c991d3b-5cdb-f9f8-2650-2cd3093d5b35	c0b63ffa-91b1-960b-c2fe-bf0e1e09a0f2	0	Melihat Data
\.


--
-- Data for Name: m_jabatan; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_jabatan (jabatan_id, nama_jabatan, keterangan) FROM stdin;
120d3472-ea93-4e29-8abd-5bd7044d26db	Kasir	
f1e4ea09-b777-4e56-bb90-db2bf9211468	General Manager	
def6e55c-47d4-4381-9a67-4f2cdce1db4d	Sopir	
955d42f3-bd82-4aa7-8c9f-8a6207b0494d	Security	
583b27e0-1644-4884-8651-47789e7713e5	Staff Administrasi	
edb47227-da98-4d97-bff2-b7ee41ff3400	Owner	Pemilik toko
\.


--
-- Data for Name: m_jenis_pengeluaran; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_jenis_pengeluaran (jenis_pengeluaran_id, nama_jenis_pengeluaran) FROM stdin;
7fde2c41-5187-4fe9-a274-b96ad8e79451	Biaya Gaji Karyawan
6c262064-6453-4bea-9e0f-5ae1810d0557	Biaya Iklan
cd381cd3-dd95-46c3-aff4-d36d9ae02faa	Biaya Perlengkapan Kantor
26b062ad-7469-42c4-9326-bb84feeca746	Biaya Listrik
c2116c49-a940-4385-be94-302470b67b83	Biaya Penyusutan Kendaraan
b7968f37-5a92-4ea3-bff0-2909aed18d9d	Biaya Penyusutan Peralatan Kantor
184f087f-8bd6-4b2c-9c53-16aeefa1a346	Biaya Sewa Gedung
a8edb7ee-7807-4dfd-afc0-9ddc6006ca36	Biaya Lain Lain
2cc2ae56-dc3b-4991-af56-7768ae10816a	Biaya SPJ marketing
488aec3c-505a-484f-a698-53ce04b98ac7	Biaya Operasional AGRO
13757bbb-a43a-43cc-91e7-341acb0c58b9	Biaya Transport Panen Ayam
40bc64d4-9671-4220-a119-dfeb1c0adbc0	Biaya Ambil Jagung
2d921654-2646-4e38-b09c-d691a40469b4	Biaya Alat Tulis Kantor
1619f95e-eac3-40e9-ba8a-af2230d3c470	Biaya Lain Lain Penjualan
1877b092-1bc8-40f1-af6e-8b9ae2dd773b	Biaya Transport Kirim pakan
\.


--
-- Data for Name: m_karyawan; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_karyawan (karyawan_id, jabatan_id, nama_karyawan, alamat, telepon, gaji_pokok, is_active, keterangan, jenis_gajian, gaji_lembur, total_kasbon, total_pembayaran_kasbon) FROM stdin;
\.


--
-- Data for Name: m_menu; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_menu (menu_id, nama_menu, judul_menu, parent_id, order_number, is_active, nama_form, is_enabled) FROM stdin;
0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	tabReferensi	Referensi	\N	1	t	\N	f
a138abfd-73da-438e-a0fe-aa3e6c6ddce9	mnuTrxProduksi	Proses Produksi	bad7ba51-ce88-461c-8a47-a143d5464895	1	t	FrmListTransaksiProduksi	t
5df78447-219a-47c8-8a28-53b8e71ffb9d	mnuTrxPembelianBahanBaku	Pembelian Bahan Baku	cc69c800-5e36-4dc5-9242-4191f1373983	1	t	FrmListTransaksiPembelianBahanBaku	t
1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	mnuTrxPembayaranHutangPembelianBahanBaku	Pembayaran Hutang Pembelian Bahan Baku	cc69c800-5e36-4dc5-9242-4191f1373983	2	t	FrmListTransaksiPembayaranHutangBahanBaku	t
461d86dc-e2b9-4709-9e58-c58071463eb3	mnuTrxPembelianAsset	Pembelian Asset	cc69c800-5e36-4dc5-9242-4191f1373983	4	t	FrmListTransaksiPembelianAsset	t
1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	mnuTrxPembayaranHutangPembelianAsset	Pembayaran Hutang Pembelian Asset	cc69c800-5e36-4dc5-9242-4191f1373983	5	t	FrmListTransaksiPembayaranHutangAsset	t
d7a5b577-c13d-4f05-954e-6e503affeb42	mnuTrxPengeluaranBiaya	Pengeluaran Biaya	abc0ae5e-b327-4c5b-b761-081dfba41343	1	t	FrmListTransaksiPengeluaran	t
7cc65f85-a8ac-4cdf-99a7-2a38aa0526d0	mnuProfilPerusahaan	Profil Perusahaan	b54ccd7a-c45a-40df-9cf8-f0c0b3aaf002	1	t	FrmEntryProfil	t
abf57b9f-f9ce-4732-bfbf-9d1476ba3255	mnuTrxPenggajianKaryawan	Penggajian Karyawan	abc0ae5e-b327-4c5b-b761-081dfba41343	3	t	FrmListTransaksiPenggajian	f
b81248f7-25a9-4b86-9fe6-4e773b916a5c	mnuTrxReturPembelianAsset	Retur Pembelian Asset	cc69c800-5e36-4dc5-9242-4191f1373983	6	f	FrmListTransaksiReturPembelianAsset	f
1e4d2010-03c7-4013-8014-15681a344cf8	mnuRoleAplikasi	Role Aplikasi	b54ccd7a-c45a-40df-9cf8-f0c0b3aaf002	3	t	FrmListRole	t
69512f8a-ae0c-469e-ad40-75b40bcfa320	mnuManajemenOperator	Manajemen Operator	b54ccd7a-c45a-40df-9cf8-f0c0b3aaf002	4	t	FrmListOperator	t
457a1049-0cad-4d0f-9e5b-827804f98505	mnuLaporanPembelianProduk	Laporan Pembelian Produk	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	3	t	FrmLaporanPembelianProduk	t
87b398c4-f0a6-49bb-b842-0ba5575f5aa2	mnuLaporanHutangPembelianProduk	Laporan Hutang Pembelian Produk	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	6	t	FrmLaporanHutangPembelianProduk	t
059c46c8-51af-2adc-6ed3-74ab78e50ef6	mnuTrxKasbon	Kasbon	abc0ae5e-b327-4c5b-b761-081dfba41343	2	t	FrmListTransaksiKasbon	t
df4fede3-7b52-4ff7-b5ce-82e924befbe3	mnuTrxReturPembelianBahanBaku	Retur Pembelian Bahan Baku	cc69c800-5e36-4dc5-9242-4191f1373983	3	f	FrmListTransaksiReturPembelianBahanBaku	f
4d375f06-d18f-5bbe-885e-42e1f60d6f41	mnuSuratPerjalananDinas	Surat Perjalanan Dinas	3d36721e-1837-a582-f24b-454fdceb0f28	1	t	FrmListSuratPerjalananDinas	t
5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	mnuTrxReturPembelianProduk	Retur Pembelian Produk	8b479542-361d-dc87-0c6f-84a41b91064a	3	t	FrmListTransaksiReturPembelianProduk	t
1efb8e4c-b97e-4183-8bab-9560ddcee684	mnuTrxReturPenjualanProduk	Retur Penjualan Produk	8b479542-361d-dc87-0c6f-84a41b91064a	6	t	FrmListTransaksiReturPenjualanProduk	t
abc0ae5e-b327-4c5b-b761-081dfba41343	tabPengeluaran	Pengeluaran	\N	6	t	\N	f
b54ccd7a-c45a-40df-9cf8-f0c0b3aaf002	tabPengaturan	Pengaturan	\N	9	t	\N	f
06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	tabLaporan	Laporan	\N	8	t	\N	f
bad7ba51-ce88-461c-8a47-a143d5464895	tabProduksi	Produksi	\N	2	f	\N	f
e33c6cb1-66f7-4024-a4c9-49e9a498e1c4	tabTrxPenjualan	Penjualan	\N	5	f	\N	f
cc69c800-5e36-4dc5-9242-4191f1373983	tabTrxPembelian	Pembelian	\N	4	f	\N	f
3d36721e-1837-a582-f24b-454fdceb0f28	tabPerjalananDinas	Perjalanan Dinas	\N	7	f	\N	f
e9dfcc3e-fdf9-4b34-b26b-39de50b7821d	mnuLaporanPembelianBahanBaku	Laporan Pembelian Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	1	f	FrmLaporanPembelianBahanBaku	t
68c5c0b9-476c-4e90-98ca-9042b58c1a1c	mnuLaporanPembelianAsset	Laporan Pembelian Asset	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	2	f	FrmLaporanPembelianAsset	t
72c92ee7-e265-4549-90db-de66f6be3a10	mnuLaporanHutangPembelianBahanBaku	Laporan Hutang Pembelian Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	4	f	FrmLaporanHutangPembelianBahanBaku	t
c37dc28b-558f-435a-8ae1-929a1903af22	mnuLaporanHutangPembelianAsset	Laporan Hutang Pembelian Asset	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	5	f	FrmLaporanHutangPembelianAsset	t
95a6d4d3-7875-442e-84a0-1db439879d0c	mnuTrxPembelianProduk	Pembelian Produk	8b479542-361d-dc87-0c6f-84a41b91064a	1	t	FrmListTransaksiPembelianProduk	t
0e07404a-00c8-43c4-9802-9718744bfb15	mnuTrxPembayaranHutangPembelianProduk	Pembayaran Hutang Pembelian Produk	8b479542-361d-dc87-0c6f-84a41b91064a	2	t	FrmListTransaksiPembayaranHutangProduk	t
f47f139d-5d7d-4127-b389-aeebc38fbe05	mnuTrxPenjualanProduk	Penjualan Produk	8b479542-361d-dc87-0c6f-84a41b91064a	4	t	FrmListTransaksiPenjualanProduk	t
db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	mnuTrxPembayaranPiutangPenjualanProduk	Pembayaran Piutang Penjualan Produk	8b479542-361d-dc87-0c6f-84a41b91064a	5	t	FrmListTransaksiPembayaranPiutangProduk	t
e2014005-b6f7-4e41-bc4a-fac1c0965b35	mnuLaporanPembayaranHutangPembelianBahanBaku	Laporan Pembayaran Hutang Pembelian Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	9	f	FrmLaporanPembayaranHutangPembelianBahanBaku	t
4da9d8fa-1c86-82c9-6a6f-db6864f56022	mnuTrxPenjualanBahanBaku	Penjualan Bahan Baku	e33c6cb1-66f7-4024-a4c9-49e9a498e1c4	4	t	FrmListTransaksiPenjualanBahanBaku	t
2cac36d0-9861-d5c9-2fd9-f72fc1c0f1c1	mnuTrxPembayaranPiutangPenjualanBahanBaku	Pembayaran Piutang Penjualan Bahan Baku	e33c6cb1-66f7-4024-a4c9-49e9a498e1c4	5	t	FrmListTransaksiPembayaranPiutangBahanBaku	t
129f17c8-56bc-3c13-4e3f-6d85db6112fc	mnuTrxReturPenjualanBahanBaku	Retur Penjualan Bahan Baku	e33c6cb1-66f7-4024-a4c9-49e9a498e1c4	6	t	FrmListTransaksiReturPenjualanBahanBaku	t
9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	mnuProduk	Produk	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	4	t	FrmListProduk	t
f3b11c04-36c3-4f62-8fbb-63b56bcea30f	mnuLaporanPenjualan	Laporan Penjualan	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	15	t	FrmLaporanPenjualan	t
36c201f6-3b6a-4472-b561-833e854c1136	mnuLaporanReturPembelianBahanBaku	Laporan Retur Pembelian Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	12	f	FrmLaporanReturPembelianBahanBaku	f
91c848cf-af7f-4921-8be5-1247212ce8b8	mnuLaporanReturPembelianAsset	Laporan Retur Pembelian Asset	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	13	f	FrmLaporanReturPembelianAsset	f
ee3f7dd4-53f0-fc59-9d70-10840fefe273	mnuLaporanKartuHutangPembelianBahanBaku	Laporan Kartu Hutang Pembelian Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	7	f	FrmLaporanKartuHutangPembelianBahanBaku	t
85194f97-1b92-46be-8250-d27e8edd0ae0	mnuLaporanPembayaranHutangPembelianAsset	Laporan Pembayaran Hutang Pembelian Asset	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	10	f	FrmLaporanPembayaranHutangPembelianAsset	t
003a36a3-5992-4b29-ba63-0df45d3a5674	mnuBahanBaku	Bahan Baku	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	2	f	FrmListBahanBaku	t
73d39ea0-248f-4ce1-bbad-3678c1f9d9db	mnuPaketProduksi	Paket Produk	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	3	f	FrmListPaketProduk	t
1a40f98b-5fcd-47ea-ac28-8da9f2595415	mnuLaporanReturPembelianProduk	Laporan Retur Pembelian Produk	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	14	t	FrmLaporanReturPembelian	f
99038701-32a2-475a-bfe0-ba9b6fbff9f2	mnuLaporanPembayaranHutangPembelianProduk	Laporan Pembayaran Hutang Pembelian Produk	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	8	t	FrmLaporanPembayaranHutangPembelianProduk	t
eeae3470-9109-4784-951a-30dcb6f836f4	mnuJenisAsset	Jenis Asset	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	8	f	FrmListJenisAsset	t
d040ab9e-523a-419b-9705-3ebd49f58f08	mnuAsset	Asset	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	9	f	FrmListAsset	t
6bfac243-7b37-48a9-b324-7e548fa2fce3	mnuJenisKemitraan	Jenis Kemitraan	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	10	f	FrmListJenisKemitraan	t
4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	mnuGolongan	Golongan	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	1	t	FrmListGolongan	t
8b479542-361d-dc87-0c6f-84a41b91064a	tabTransaksi	Transaksi	\N	3	t	\N	t
021a3899-66c8-0bd5-63bb-8d96649582f5	mnuPenyesuaianStok	Penyesuaian Stok	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	6	t	FrmListPenyesuaianStok	t
4eb57c99-3623-db52-bee6-1a5b56403a49	mnuMutasiStokGudang	Mutasi Stok Gudang	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	5	f	FrmListMutasiStok	t
35eec8d9-c95a-2e3d-45f5-b0df52155e64	mnuLaporanKartuHutangPembelianProduk	Laporan Kartu Hutang Pembelian Produk	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	11	t	FrmLaporanKartuHutangPembelianProduk	t
c341d139-d39b-4159-984c-40923ac3c68e	mnuLangganan	Customer	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	11	t	FrmListCustomer	t
5c5c0469-5edf-4422-96a0-0b173a4fe042	mnuSupplier	Supplier	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	13	t	FrmListSupplier	t
d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	mnuJabatan	Jabatan	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	14	t	FrmListJabatan	t
1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	mnuKaryawan	Karyawan	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	15	t	FrmListKaryawan	t
b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	mnuJenisPengeluaran	Jenis Biaya	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	16	t	FrmListJenisBiaya	t
7683e0f5-bf9b-022b-4343-e34fdbf2c259	mnuDonasiProduk	Donasi Produk	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	7	f	FrmListDonasiProduk	t
4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	mnuJenisSupplier	Jenis Supplier	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	12	f	FrmListJenisSupplier	t
e4c7f7e9-41f4-9470-cab9-f5dbb8c91a16	mnuKendaraan	Kendaraan	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	17	f	FrmListKendaraan	t
c81c61b2-ee72-413b-bad9-84a90a1a4420	mnuLaporanPengeluaranBiaya	Laporan Pengeluaran Biaya	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	30	t	FrmLaporanPengeluaranBiaya	t
30cf483b-7b71-4f48-8e0b-d9e8c77e172c	mnuLaporanPiutangPenjualan	Laporan Piutang Penjualan	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	18	t	FrmLaporanPiutangPenjualan	t
a3e519ec-f1a9-499a-bbf5-8b3c84f6619b	mnuLaporanReturPenjualan	Laporan Retur Penjualan	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	21	t	FrmLaporanReturPenjualan	t
a9a1c62f-b33c-2b20-fb89-fe97174aa644	mnuLaporanPenjualanPerProduk	Laporan Penjualan Per Produk	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	17	t	FrmLaporanPenjualanPerProduk	t
1f57066a-c2bf-4e47-a091-2945dabb3234	mnuLaporanPembayaranPiutangPenjualan	Laporan Pembayaran Piutang Penjualan	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	19	t	FrmLaporanPembayaranPiutangPenjualan	t
14b96c86-8baa-a1ad-d7d9-9ce8c195b1b1	mnuLaporanKartuPiutangPenjualan	Laporan Kartu Piutang Penjualan	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	20	t	FrmLaporanKartuPiutangPenjualan	t
88db4a0f-9890-4f1a-8433-2856b2a55209	mnuLaporanPenjualanBahan	Laporan Penjualan Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	22	f	FrmLaporanPenjualanBahanBaku	t
1cbd9e74-7133-4b5e-d139-f9025ba89106	mnuLaporanPenjualanPerBahan	Laporan Penjualan Per Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	23	f	FrmLaporanPenjualanPerBahan	t
84b8444c-13f4-ca99-f1e2-69fd0218be44	mnuLaporanPiutangPenjualanBahan	Laporan Piutang Penjualan Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	24	f	FrmLaporanPiutangPenjualanBahan	t
a62dc67c-6d9e-bbe3-79dc-5f15c506ca20	mnuLaporanPembayaranPiutangPenjualanBahan	Laporan Pembayaran Piutang Penjualan Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	25	f	FrmLaporanPembayaranPiutangPenjualanBahan	t
03c70546-4fe4-5a4a-c3bd-9622538fdaf3	mnuLaporanKartuPiutangPenjualanBahan	Laporan Kartu Piutang Penjualan Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	26	f	FrmLaporanKartuPiutangPenjualanBahan	t
8494f427-f4fb-279e-9b06-7a08882a8b6b	mnuLaporanReturPenjualanBahan	Laporan Retur Penjualan Bahan Baku	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	27	f	FrmLaporanReturPenjualanBahan	t
4b09624d-01ed-3749-a6a5-f4a1e3769eaf	mnuLaporanStok	Laporan Stok Produk	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	28	t	FrmLaporanStok	t
427a0fe7-c80a-44e7-ac92-1ca5edc31d56	mnuLaporanProfit	Laporan Laba Rugi	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	33	t	FrmLaporanProfit	t
08a76a4e-04f3-ad71-bc8f-14f7d16c5cd6	mnuLaporanKasbon	Laporan Kasbon	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	31	t	FrmLaporanKasbon	t
0cf37a9a-66f1-412a-971e-a2000528a2f5	mnuLaporanGajiKaryawan	Laporan Gaji Karyawan	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	32	t	FrmLaporanGajiKaryawan	t
5970451e-566b-cecf-0635-f9a2690161ff	mnuLaporanDonasiTransaksi	Laporan Donasi Transaksi	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	35	f	FrmLaporanDonasiTransaksi	t
56b17113-b1df-640c-36e5-ced6528603f9	mnuLaporanPerjalananDinas	Laporan Perjalanan Dinas	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	34	f	FrmLaporanPerjalananDinas	t
24eb2565-989a-6c92-0a9e-db0ec2b6d582	mnuLaporanPenyesuaianStok	Laporan Penyesuaian Stok	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	29	t	FrmLaporanPenyesuaianStok	t
5f5b601c-8514-fb8c-6f7b-fb9c536b8bd2	mnuShift	Shift	0d6d2fac-3dd9-4fb8-83f0-a98bdb8ac4ca	18	f	FrmListShift	f
aa38c310-0400-4240-ae3d-a1445f4d7c5e	mnuSettingAplikasi	Setting Aplikasi	b54ccd7a-c45a-40df-9cf8-f0c0b3aaf002	2	f	FrmEntrySettingAplikasi	f
c0b63ffa-91b1-960b-c2fe-bf0e1e09a0f2	mnuLaporanPenjualanPerKasir	Laporan Penjualan Per Kasir	06bdc3d0-5008-4d8e-bd16-d4bc26fb1626	16	f	FrmLaporanPenjualanPerKasir	f
\.


--
-- Data for Name: m_pengguna; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_pengguna (pengguna_id, role_id, nama_pengguna, pass_pengguna, is_active, status_user) FROM stdin;
00b5acfa-b533-454b-8dfd-e7881edd180f	11dc1faf-2c66-4525-932d-a90e24da8987	admin	20b1ce8e61ee5b49320ef0a78c75521b	t	2
\.


--
-- Data for Name: m_prefix_nota; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_prefix_nota (prefix_nota_id, prefix_nota, keterangan) FROM stdin;
12	001	Nota Pembelian Produk
13	002	Nota Pembayaran Hutang Produk
14	003	Nota Retur Pembelian Produk
1	004	Nota Penjualan
6	005	Nota Pembayaran Piutang Penjualan
10	006	Nota Retur Penjualan
11	007	Nota Pengeluaran
15	008	Nota Kas BON
\.


--
-- Data for Name: m_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_produk (produk_id, nama_produk, satuan, stok, harga_beli, harga_jual, kode_produk, golongan_id, minimal_stok, stok_gudang, minimal_stok_gudang) FROM stdin;
eafc755f-cab6-4066-a793-660fcfab20d0	Adaptor NB ACER		1.00	53000.00	70000.00	201607000000053	0a8b59e5-bb3b-4081-b963-9dc9584dcda6	0.00	0.00	0.00
6e587b32-9d87-4ec3-8e7c-ce15c7b0aecd	HDD 20 Gb IDE 2nd		1.00	50000.00	60000.00	201607000000056	2aae21ba-8954-4db6-a6dc-c648e27255ad	0.00	0.00	0.00
17c7626c-e5ca-43f2-b075-af6b6cbcbf83	CD ROM ALL Merk 2nd		0.00	20000.00	50000.00	201607000000055	2aae21ba-8954-4db6-a6dc-c648e27255ad	0.00	0.00	0.00
7f09a4aa-e660-4de3-a3aa-4b3244675f9f	Access Point TPLINK TC-WA 500G		1.00	400000.00	400000.00	201607000000051	0a8b59e5-bb3b-4081-b963-9dc9584dcda6	0.00	0.00	0.00
53b63dc2-4505-4276-9886-3639b53b7458	Access Point TPLINK TL-MR3220 3,5G		2.00	200000.00	350000.00	201607000000052	0a8b59e5-bb3b-4081-b963-9dc9584dcda6	0.00	0.00	0.00
\.


--
-- Data for Name: m_profil; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_profil (profil_id, nama_profil, alamat, kota, telepon) FROM stdin;
14985585-c04b-5d3a-88dc-b8eea86d7740	INBOX COMPUTER	Jl. Kaliurang KM 14.3	Yogyakarta	Telp. 0274-7850060
\.


--
-- Data for Name: m_role; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_role (role_id, nama_role, is_active) FROM stdin;
11dc1faf-2c66-4525-932d-a90e24da8987	Administrator	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	Owner	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	Kasir	t
\.


--
-- Data for Name: m_role_privilege; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_role_privilege (role_id, menu_id, grant_id, is_grant) FROM stdin;
11dc1faf-2c66-4525-932d-a90e24da8987	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	5df78447-219a-47c8-8a28-53b8e71ffb9d	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	5df78447-219a-47c8-8a28-53b8e71ffb9d	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	5df78447-219a-47c8-8a28-53b8e71ffb9d	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	5df78447-219a-47c8-8a28-53b8e71ffb9d	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	df4fede3-7b52-4ff7-b5ce-82e924befbe3	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	df4fede3-7b52-4ff7-b5ce-82e924befbe3	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	df4fede3-7b52-4ff7-b5ce-82e924befbe3	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	df4fede3-7b52-4ff7-b5ce-82e924befbe3	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	461d86dc-e2b9-4709-9e58-c58071463eb3	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	461d86dc-e2b9-4709-9e58-c58071463eb3	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	461d86dc-e2b9-4709-9e58-c58071463eb3	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	461d86dc-e2b9-4709-9e58-c58071463eb3	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	b81248f7-25a9-4b86-9fe6-4e773b916a5c	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	003a36a3-5992-4b29-ba63-0df45d3a5674	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	eeae3470-9109-4784-951a-30dcb6f836f4	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	eeae3470-9109-4784-951a-30dcb6f836f4	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	d040ab9e-523a-419b-9705-3ebd49f58f08	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	d040ab9e-523a-419b-9705-3ebd49f58f08	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	d040ab9e-523a-419b-9705-3ebd49f58f08	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	d040ab9e-523a-419b-9705-3ebd49f58f08	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	6bfac243-7b37-48a9-b324-7e548fa2fce3	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	6bfac243-7b37-48a9-b324-7e548fa2fce3	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	6bfac243-7b37-48a9-b324-7e548fa2fce3	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	6bfac243-7b37-48a9-b324-7e548fa2fce3	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	003a36a3-5992-4b29-ba63-0df45d3a5674	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	b81248f7-25a9-4b86-9fe6-4e773b916a5c	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	b81248f7-25a9-4b86-9fe6-4e773b916a5c	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	b81248f7-25a9-4b86-9fe6-4e773b916a5c	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	c341d139-d39b-4159-984c-40923ac3c68e	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	c341d139-d39b-4159-984c-40923ac3c68e	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	c341d139-d39b-4159-984c-40923ac3c68e	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	c341d139-d39b-4159-984c-40923ac3c68e	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	5c5c0469-5edf-4422-96a0-0b173a4fe042	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	5c5c0469-5edf-4422-96a0-0b173a4fe042	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	5c5c0469-5edf-4422-96a0-0b173a4fe042	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	5c5c0469-5edf-4422-96a0-0b173a4fe042	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	7cc65f85-a8ac-4cdf-99a7-2a38aa0526d0	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	aa38c310-0400-4240-ae3d-a1445f4d7c5e	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	1e4d2010-03c7-4013-8014-15681a344cf8	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	1e4d2010-03c7-4013-8014-15681a344cf8	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	1e4d2010-03c7-4013-8014-15681a344cf8	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	1e4d2010-03c7-4013-8014-15681a344cf8	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	69512f8a-ae0c-469e-ad40-75b40bcfa320	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	69512f8a-ae0c-469e-ad40-75b40bcfa320	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	69512f8a-ae0c-469e-ad40-75b40bcfa320	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	69512f8a-ae0c-469e-ad40-75b40bcfa320	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	003a36a3-5992-4b29-ba63-0df45d3a5674	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	003a36a3-5992-4b29-ba63-0df45d3a5674	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	003a36a3-5992-4b29-ba63-0df45d3a5674	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	003a36a3-5992-4b29-ba63-0df45d3a5674	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	d7a5b577-c13d-4f05-954e-6e503affeb42	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	d7a5b577-c13d-4f05-954e-6e503affeb42	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	d7a5b577-c13d-4f05-954e-6e503affeb42	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	d7a5b577-c13d-4f05-954e-6e503affeb42	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	eeae3470-9109-4784-951a-30dcb6f836f4	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	eeae3470-9109-4784-951a-30dcb6f836f4	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	eeae3470-9109-4784-951a-30dcb6f836f4	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	eeae3470-9109-4784-951a-30dcb6f836f4	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d040ab9e-523a-419b-9705-3ebd49f58f08	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d040ab9e-523a-419b-9705-3ebd49f58f08	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d040ab9e-523a-419b-9705-3ebd49f58f08	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d040ab9e-523a-419b-9705-3ebd49f58f08	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	6bfac243-7b37-48a9-b324-7e548fa2fce3	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	6bfac243-7b37-48a9-b324-7e548fa2fce3	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	6bfac243-7b37-48a9-b324-7e548fa2fce3	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	6bfac243-7b37-48a9-b324-7e548fa2fce3	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	c341d139-d39b-4159-984c-40923ac3c68e	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	95a6d4d3-7875-442e-84a0-1db439879d0c	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	95a6d4d3-7875-442e-84a0-1db439879d0c	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	95a6d4d3-7875-442e-84a0-1db439879d0c	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	0e07404a-00c8-43c4-9802-9718744bfb15	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	0e07404a-00c8-43c4-9802-9718744bfb15	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	0e07404a-00c8-43c4-9802-9718744bfb15	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	0e07404a-00c8-43c4-9802-9718744bfb15	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	f47f139d-5d7d-4127-b389-aeebc38fbe05	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	f47f139d-5d7d-4127-b389-aeebc38fbe05	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	f47f139d-5d7d-4127-b389-aeebc38fbe05	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	1efb8e4c-b97e-4183-8bab-9560ddcee684	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	1efb8e4c-b97e-4183-8bab-9560ddcee684	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	1efb8e4c-b97e-4183-8bab-9560ddcee684	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	1efb8e4c-b97e-4183-8bab-9560ddcee684	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	c341d139-d39b-4159-984c-40923ac3c68e	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	c341d139-d39b-4159-984c-40923ac3c68e	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	c341d139-d39b-4159-984c-40923ac3c68e	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5c5c0469-5edf-4422-96a0-0b173a4fe042	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5c5c0469-5edf-4422-96a0-0b173a4fe042	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5c5c0469-5edf-4422-96a0-0b173a4fe042	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5c5c0469-5edf-4422-96a0-0b173a4fe042	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	a138abfd-73da-438e-a0fe-aa3e6c6ddce9	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5df78447-219a-47c8-8a28-53b8e71ffb9d	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5df78447-219a-47c8-8a28-53b8e71ffb9d	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5df78447-219a-47c8-8a28-53b8e71ffb9d	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5df78447-219a-47c8-8a28-53b8e71ffb9d	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1acd8d38-fa25-4fee-b76a-6a6b66ca31eb	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	df4fede3-7b52-4ff7-b5ce-82e924befbe3	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	df4fede3-7b52-4ff7-b5ce-82e924befbe3	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	df4fede3-7b52-4ff7-b5ce-82e924befbe3	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	df4fede3-7b52-4ff7-b5ce-82e924befbe3	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	461d86dc-e2b9-4709-9e58-c58071463eb3	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	461d86dc-e2b9-4709-9e58-c58071463eb3	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	461d86dc-e2b9-4709-9e58-c58071463eb3	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	461d86dc-e2b9-4709-9e58-c58071463eb3	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1dc3a04c-6a0a-4c4c-b512-c28ef0453e61	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	b81248f7-25a9-4b86-9fe6-4e773b916a5c	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	b81248f7-25a9-4b86-9fe6-4e773b916a5c	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	b81248f7-25a9-4b86-9fe6-4e773b916a5c	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	b81248f7-25a9-4b86-9fe6-4e773b916a5c	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	95a6d4d3-7875-442e-84a0-1db439879d0c	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	95a6d4d3-7875-442e-84a0-1db439879d0c	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	95a6d4d3-7875-442e-84a0-1db439879d0c	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	95a6d4d3-7875-442e-84a0-1db439879d0c	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	0e07404a-00c8-43c4-9802-9718744bfb15	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	0e07404a-00c8-43c4-9802-9718744bfb15	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	0e07404a-00c8-43c4-9802-9718744bfb15	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	0e07404a-00c8-43c4-9802-9718744bfb15	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	f47f139d-5d7d-4127-b389-aeebc38fbe05	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	f47f139d-5d7d-4127-b389-aeebc38fbe05	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	f47f139d-5d7d-4127-b389-aeebc38fbe05	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	f47f139d-5d7d-4127-b389-aeebc38fbe05	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1efb8e4c-b97e-4183-8bab-9560ddcee684	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1efb8e4c-b97e-4183-8bab-9560ddcee684	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1efb8e4c-b97e-4183-8bab-9560ddcee684	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1efb8e4c-b97e-4183-8bab-9560ddcee684	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d7a5b577-c13d-4f05-954e-6e503affeb42	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d7a5b577-c13d-4f05-954e-6e503affeb42	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d7a5b577-c13d-4f05-954e-6e503affeb42	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	d7a5b577-c13d-4f05-954e-6e503affeb42	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	7cc65f85-a8ac-4cdf-99a7-2a38aa0526d0	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	aa38c310-0400-4240-ae3d-a1445f4d7c5e	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1e4d2010-03c7-4013-8014-15681a344cf8	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1e4d2010-03c7-4013-8014-15681a344cf8	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1e4d2010-03c7-4013-8014-15681a344cf8	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1e4d2010-03c7-4013-8014-15681a344cf8	3	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	69512f8a-ae0c-469e-ad40-75b40bcfa320	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	69512f8a-ae0c-469e-ad40-75b40bcfa320	1	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	69512f8a-ae0c-469e-ad40-75b40bcfa320	2	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	69512f8a-ae0c-469e-ad40-75b40bcfa320	3	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	003a36a3-5992-4b29-ba63-0df45d3a5674	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	003a36a3-5992-4b29-ba63-0df45d3a5674	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	003a36a3-5992-4b29-ba63-0df45d3a5674	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	73d39ea0-248f-4ce1-bbad-3678c1f9d9db	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	eeae3470-9109-4784-951a-30dcb6f836f4	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	eeae3470-9109-4784-951a-30dcb6f836f4	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	eeae3470-9109-4784-951a-30dcb6f836f4	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	003a36a3-5992-4b29-ba63-0df45d3a5674	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	eeae3470-9109-4784-951a-30dcb6f836f4	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d040ab9e-523a-419b-9705-3ebd49f58f08	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d040ab9e-523a-419b-9705-3ebd49f58f08	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d040ab9e-523a-419b-9705-3ebd49f58f08	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d040ab9e-523a-419b-9705-3ebd49f58f08	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	6bfac243-7b37-48a9-b324-7e548fa2fce3	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	6bfac243-7b37-48a9-b324-7e548fa2fce3	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	6bfac243-7b37-48a9-b324-7e548fa2fce3	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	6bfac243-7b37-48a9-b324-7e548fa2fce3	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4fa48291-5b53-4d2e-8cc3-4d82dc7ca73d	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	1efb8e4c-b97e-4183-8bab-9560ddcee684	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	1efb8e4c-b97e-4183-8bab-9560ddcee684	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	1efb8e4c-b97e-4183-8bab-9560ddcee684	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	1efb8e4c-b97e-4183-8bab-9560ddcee684	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	c341d139-d39b-4159-984c-40923ac3c68e	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	c341d139-d39b-4159-984c-40923ac3c68e	1	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	c341d139-d39b-4159-984c-40923ac3c68e	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	c341d139-d39b-4159-984c-40923ac3c68e	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	5c5c0469-5edf-4422-96a0-0b173a4fe042	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	5c5c0469-5edf-4422-96a0-0b173a4fe042	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	5c5c0469-5edf-4422-96a0-0b173a4fe042	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	f47f139d-5d7d-4127-b389-aeebc38fbe05	1	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	f47f139d-5d7d-4127-b389-aeebc38fbe05	3	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	1	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	db3cde00-28c5-4fe8-aaa5-29cdac65b3f4	1	f
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	e9dfcc3e-fdf9-4b34-b26b-39de50b7821d	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	68c5c0b9-476c-4e90-98ca-9042b58c1a1c	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	457a1049-0cad-4d0f-9e5b-827804f98505	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	72c92ee7-e265-4549-90db-de66f6be3a10	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	c37dc28b-558f-435a-8ae1-929a1903af22	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	87b398c4-f0a6-49bb-b842-0ba5575f5aa2	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	e2014005-b6f7-4e41-bc4a-fac1c0965b35	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	85194f97-1b92-46be-8250-d27e8edd0ae0	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	99038701-32a2-475a-bfe0-ba9b6fbff9f2	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	36c201f6-3b6a-4472-b561-833e854c1136	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	91c848cf-af7f-4921-8be5-1247212ce8b8	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1a40f98b-5fcd-47ea-ac28-8da9f2595415	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	f3b11c04-36c3-4f62-8fbb-63b56bcea30f	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	30cf483b-7b71-4f48-8e0b-d9e8c77e172c	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	1f57066a-c2bf-4e47-a091-2945dabb3234	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	a3e519ec-f1a9-499a-bbf5-8b3c84f6619b	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	c81c61b2-ee72-413b-bad9-84a90a1a4420	0	t
c58ee40a-5ae2-4067-b6ad-8cae9c65913c	0cf37a9a-66f1-412a-971e-a2000528a2f5	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	72c92ee7-e265-4549-90db-de66f6be3a10	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	c37dc28b-558f-435a-8ae1-929a1903af22	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	68c5c0b9-476c-4e90-98ca-9042b58c1a1c	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	36c201f6-3b6a-4472-b561-833e854c1136	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	91c848cf-af7f-4921-8be5-1247212ce8b8	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	059c46c8-51af-2adc-6ed3-74ab78e50ef6	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	059c46c8-51af-2adc-6ed3-74ab78e50ef6	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	059c46c8-51af-2adc-6ed3-74ab78e50ef6	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	059c46c8-51af-2adc-6ed3-74ab78e50ef6	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	88db4a0f-9890-4f1a-8433-2856b2a55209	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	1cbd9e74-7133-4b5e-d139-f9025ba89106	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	84b8444c-13f4-ca99-f1e2-69fd0218be44	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	a62dc67c-6d9e-bbe3-79dc-5f15c506ca20	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	03c70546-4fe4-5a4a-c3bd-9622538fdaf3	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	8494f427-f4fb-279e-9b06-7a08882a8b6b	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	4d375f06-d18f-5bbe-885e-42e1f60d6f41	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	4d375f06-d18f-5bbe-885e-42e1f60d6f41	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	4d375f06-d18f-5bbe-885e-42e1f60d6f41	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	4d375f06-d18f-5bbe-885e-42e1f60d6f41	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	4da9d8fa-1c86-82c9-6a6f-db6864f56022	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	4da9d8fa-1c86-82c9-6a6f-db6864f56022	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	56b17113-b1df-640c-36e5-ced6528603f9	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	5970451e-566b-cecf-0635-f9a2690161ff	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	7683e0f5-bf9b-022b-4343-e34fdbf2c259	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	e4c7f7e9-41f4-9470-cab9-f5dbb8c91a16	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	e4c7f7e9-41f4-9470-cab9-f5dbb8c91a16	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	e4c7f7e9-41f4-9470-cab9-f5dbb8c91a16	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	e4c7f7e9-41f4-9470-cab9-f5dbb8c91a16	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	7683e0f5-bf9b-022b-4343-e34fdbf2c259	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	1	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	5c5c0469-5edf-4422-96a0-0b173a4fe042	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	1	f
11dc1faf-2c66-4525-932d-a90e24da8987	4da9d8fa-1c86-82c9-6a6f-db6864f56022	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	4da9d8fa-1c86-82c9-6a6f-db6864f56022	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	2cac36d0-9861-d5c9-2fd9-f72fc1c0f1c1	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	2cac36d0-9861-d5c9-2fd9-f72fc1c0f1c1	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	2cac36d0-9861-d5c9-2fd9-f72fc1c0f1c1	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	2cac36d0-9861-d5c9-2fd9-f72fc1c0f1c1	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	129f17c8-56bc-3c13-4e3f-6d85db6112fc	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	129f17c8-56bc-3c13-4e3f-6d85db6112fc	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	129f17c8-56bc-3c13-4e3f-6d85db6112fc	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	457a1049-0cad-4d0f-9e5b-827804f98505	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	1a40f98b-5fcd-47ea-ac28-8da9f2595415	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	1f57066a-c2bf-4e47-a091-2945dabb3234	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	14b96c86-8baa-a1ad-d7d9-9ce8c195b1b1	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	a3e519ec-f1a9-499a-bbf5-8b3c84f6619b	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	c81c61b2-ee72-413b-bad9-84a90a1a4420	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	0cf37a9a-66f1-412a-971e-a2000528a2f5	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	129f17c8-56bc-3c13-4e3f-6d85db6112fc	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	e9dfcc3e-fdf9-4b34-b26b-39de50b7821d	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	ee3f7dd4-53f0-fc59-9d70-10840fefe273	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	e2014005-b6f7-4e41-bc4a-fac1c0965b35	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	85194f97-1b92-46be-8250-d27e8edd0ae0	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	0e07404a-00c8-43c4-9802-9718744bfb15	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	0e07404a-00c8-43c4-9802-9718744bfb15	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	0e07404a-00c8-43c4-9802-9718744bfb15	3	f
11dc1faf-2c66-4525-932d-a90e24da8987	4eb57c99-3623-db52-bee6-1a5b56403a49	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	003a36a3-5992-4b29-ba63-0df45d3a5674	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	003a36a3-5992-4b29-ba63-0df45d3a5674	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	7683e0f5-bf9b-022b-4343-e34fdbf2c259	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	7683e0f5-bf9b-022b-4343-e34fdbf2c259	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	eeae3470-9109-4784-951a-30dcb6f836f4	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	eeae3470-9109-4784-951a-30dcb6f836f4	3	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	0e07404a-00c8-43c4-9802-9718744bfb15	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	5e7b0f49-67a0-40e9-9ffb-8133f9d2a43a	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	f47f139d-5d7d-4127-b389-aeebc38fbe05	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	f47f139d-5d7d-4127-b389-aeebc38fbe05	2	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	2	f
11dc1faf-2c66-4525-932d-a90e24da8987	95a6d4d3-7875-442e-84a0-1db439879d0c	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	f47f139d-5d7d-4127-b389-aeebc38fbe05	1	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4eb57c99-3623-db52-bee6-1a5b56403a49	0	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4eb57c99-3623-db52-bee6-1a5b56403a49	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4eb57c99-3623-db52-bee6-1a5b56403a49	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4eb57c99-3623-db52-bee6-1a5b56403a49	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	95a6d4d3-7875-442e-84a0-1db439879d0c	3	f
11dc1faf-2c66-4525-932d-a90e24da8987	4eb57c99-3623-db52-bee6-1a5b56403a49	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	4eb57c99-3623-db52-bee6-1a5b56403a49	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	4eb57c99-3623-db52-bee6-1a5b56403a49	3	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	021a3899-66c8-0bd5-63bb-8d96649582f5	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	95a6d4d3-7875-442e-84a0-1db439879d0c	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	95a6d4d3-7875-442e-84a0-1db439879d0c	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	1	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	021a3899-66c8-0bd5-63bb-8d96649582f5	1	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	021a3899-66c8-0bd5-63bb-8d96649582f5	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	021a3899-66c8-0bd5-63bb-8d96649582f5	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d7a5b577-c13d-4f05-954e-6e503affeb42	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d7a5b577-c13d-4f05-954e-6e503affeb42	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d7a5b577-c13d-4f05-954e-6e503affeb42	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	d7a5b577-c13d-4f05-954e-6e503affeb42	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	059c46c8-51af-2adc-6ed3-74ab78e50ef6	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	059c46c8-51af-2adc-6ed3-74ab78e50ef6	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	059c46c8-51af-2adc-6ed3-74ab78e50ef6	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	059c46c8-51af-2adc-6ed3-74ab78e50ef6	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	021a3899-66c8-0bd5-63bb-8d96649582f5	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	021a3899-66c8-0bd5-63bb-8d96649582f5	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	021a3899-66c8-0bd5-63bb-8d96649582f5	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	021a3899-66c8-0bd5-63bb-8d96649582f5	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	b4124e3b-2d04-47b9-b5f2-79c63dbbf6b3	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	87b398c4-f0a6-49bb-b842-0ba5575f5aa2	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	99038701-32a2-475a-bfe0-ba9b6fbff9f2	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	35eec8d9-c95a-2e3d-45f5-b0df52155e64	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	f3b11c04-36c3-4f62-8fbb-63b56bcea30f	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	30cf483b-7b71-4f48-8e0b-d9e8c77e172c	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	4b09624d-01ed-3749-a6a5-f4a1e3769eaf	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	24eb2565-989a-6c92-0a9e-db0ec2b6d582	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	08a76a4e-04f3-ad71-bc8f-14f7d16c5cd6	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	427a0fe7-c80a-44e7-ac92-1ca5edc31d56	0	t
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	1	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	2	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	abf57b9f-f9ce-4732-bfbf-9d1476ba3255	3	f
42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	95a6d4d3-7875-442e-84a0-1db439879d0c	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	4a5fb5e5-6a61-65db-0ab8-9e8b7ca34c59	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	9fb923f5-2ed0-4210-8a63-fdc924fb0d6b	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	d7b0e5cd-9fbe-4042-9281-d318fad9c9a1	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	1d3a934c-d8f2-4ed0-b7cd-c5ec994245f3	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	5f5b601c-8514-fb8c-6f7b-fb9c536b8bd2	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	5f5b601c-8514-fb8c-6f7b-fb9c536b8bd2	1	t
11dc1faf-2c66-4525-932d-a90e24da8987	5f5b601c-8514-fb8c-6f7b-fb9c536b8bd2	2	t
11dc1faf-2c66-4525-932d-a90e24da8987	5f5b601c-8514-fb8c-6f7b-fb9c536b8bd2	3	t
11dc1faf-2c66-4525-932d-a90e24da8987	c0b63ffa-91b1-960b-c2fe-bf0e1e09a0f2	0	t
11dc1faf-2c66-4525-932d-a90e24da8987	a9a1c62f-b33c-2b20-fb89-fe97174aa644	0	t
\.


--
-- Data for Name: m_setting_aplikasi; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_setting_aplikasi (setting_aplikasi_id, url_update, db_version) FROM stdin;
1ef7cbcb-bf73-44be-aad2-b9d8e1b1a178	http://krsoftware.me/online-update/retail/kr-retail-seed.xml	1
\.


--
-- Data for Name: m_shift; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_shift (shift_id, nama_shift, jam_mulai, jam_selesai, is_active) FROM stdin;
\.


--
-- Data for Name: m_supplier; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_supplier (supplier_id, nama_supplier, alamat, kontak, telepon, total_hutang, total_pembayaran_hutang) FROM stdin;
7560fd72-0538-4307-8f15-14ef32cf5158	Toko Komputer "XYZ"				0.00	0.00
e6201c8e-74e3-467c-a463-c8ea1763668e	Pixel Computer	Solo	Yusuf	08138383838	0.00	0.00
\.


--
-- Data for Name: t_beli_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_beli_produk (beli_produk_id, pengguna_id, supplier_id, retur_beli_produk_id, nota, tanggal, tanggal_tempo, ppn, diskon, total_nota, total_pelunasan, keterangan, tanggal_sistem) FROM stdin;
8f38f5c9-e092-4b16-ab04-b1ce87cc8cc4	00b5acfa-b533-454b-8dfd-e7881edd180f	7560fd72-0538-4307-8f15-14ef32cf5158	4a51da2b-d9c0-4ee8-a715-ab5cab618c32	001-0000001	2016-10-11	-infinity	0.00	0.00	1600000.00	1600000.00		2016-10-11 12:49:46.052
\.


--
-- Name: t_beli_produk_beli_produk_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_beli_produk_beli_produk_id_seq', 2, true);


--
-- Data for Name: t_bon; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_bon (bon_id, karyawan_id, pengguna_id, nota, tanggal, nominal, keterangan, tanggal_sistem, total_pelunasan) FROM stdin;
\.


--
-- Name: t_bon_bon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_bon_bon_id_seq', 1, false);


--
-- Data for Name: t_gaji_karyawan; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_gaji_karyawan (gaji_karyawan_id, karyawan_id, pengguna_id, bulan, tahun, kehadiran, absen, gaji_pokok, lembur, bonus, potongan, tanggal_sistem, minggu, jam, lainnya, keterangan, jumlah_hari, tunjangan, kasbon, tanggal) FROM stdin;
\.


--
-- Data for Name: t_item_beli_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_item_beli_produk (item_beli_produk_id, beli_produk_id, pengguna_id, produk_id, harga, jumlah, diskon, tanggal_sistem, jumlah_retur) FROM stdin;
d5e9c27a-b07a-41c6-af0c-65dc0e2fa0d5	8f38f5c9-e092-4b16-ab04-b1ce87cc8cc4	00b5acfa-b533-454b-8dfd-e7881edd180f	7f09a4aa-e660-4de3-a3aa-4b3244675f9f	400000.00	5.00	0.00	2016-10-11 12:49:46.052	1.00
\.


--
-- Data for Name: t_item_jual_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_item_jual_produk (item_jual_id, jual_id, pengguna_id, produk_id, harga_beli, harga_jual, jumlah, diskon, tanggal_sistem, jumlah_retur) FROM stdin;
da58f962-c499-4631-bbb9-eabde1691966	7577ee51-4659-4d21-a232-142bc69a7aff	00b5acfa-b533-454b-8dfd-e7881edd180f	17c7626c-e5ca-43f2-b075-af6b6cbcbf83	20000.00	50000.00	2.00	0.00	2016-09-20 20:41:43.489	0.00
d92c1b8f-e98c-47a7-988c-ecd9e19c1afe	4a65e917-eeca-4620-9399-4a43d3d4c7a8	00b5acfa-b533-454b-8dfd-e7881edd180f	53b63dc2-4505-4276-9886-3639b53b7458	200000.00	350000.00	5.00	0.00	2016-10-11 12:45:48.864	0.00
2c8ee1c2-848c-422b-b70c-f7019052f7f4	7577ee51-4659-4d21-a232-142bc69a7aff	00b5acfa-b533-454b-8dfd-e7881edd180f	53b63dc2-4505-4276-9886-3639b53b7458	200000.00	350000.00	1.00	0.00	2016-09-20 20:41:43.489	1.00
1a393626-a837-4929-b095-eb48e4bee9fd	98ac0476-ef66-4da1-a518-0a9029933ba2	00b5acfa-b533-454b-8dfd-e7881edd180f	53b63dc2-4505-4276-9886-3639b53b7458	200000.00	350000.00	5.00	0.00	2016-10-26 05:55:50.18	0.00
\.


--
-- Data for Name: t_item_pembayaran_hutang_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_item_pembayaran_hutang_produk (item_pembayaran_hutang_produk_id, pembayaran_hutang_produk_id, beli_produk_id, nominal, keterangan, tanggal_sistem) FROM stdin;
b9993de8-8f05-4b5d-806d-515e7760b918	d0caca75-4e38-4ebc-9efb-be27bc46c54d	8f38f5c9-e092-4b16-ab04-b1ce87cc8cc4	1600000.00		2016-10-11 12:49:46.052
\.


--
-- Data for Name: t_item_pembayaran_piutang_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_item_pembayaran_piutang_produk (item_pembayaran_piutang_id, pembayaran_piutang_id, jual_id, nominal, keterangan, tanggal_sistem) FROM stdin;
781b4cbe-0c59-49d0-b640-ace336375485	228bebb1-8480-4e42-b3a6-cae40d95d289	4a65e917-eeca-4620-9399-4a43d3d4c7a8	1750000.00		2016-10-11 12:45:48.864
0f95a017-22f0-40c6-a624-9462a5697e8f	f52a3337-eb4d-4f67-bb13-e25a3a7d094f	98ac0476-ef66-4da1-a518-0a9029933ba2	1700000.00		2016-10-26 05:55:50.18
\.


--
-- Data for Name: t_item_pengeluaran; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_item_pengeluaran (item_pengeluaran_id, pengeluaran_id, pengguna_id, jumlah, harga, tanggal_sistem, jenis_pengeluaran_id) FROM stdin;
\.


--
-- Data for Name: t_item_retur_beli_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_item_retur_beli_produk (item_retur_beli_produk_id, retur_beli_produk_id, pengguna_id, produk_id, harga, jumlah, tanggal_sistem, jumlah_retur, item_beli_id) FROM stdin;
156c81a2-572b-4534-aa28-f78000365ab5	4a51da2b-d9c0-4ee8-a715-ab5cab618c32	00b5acfa-b533-454b-8dfd-e7881edd180f	7f09a4aa-e660-4de3-a3aa-4b3244675f9f	400000.00	5.00	2016-10-11 12:49:58.61	1.00	d5e9c27a-b07a-41c6-af0c-65dc0e2fa0d5
\.


--
-- Data for Name: t_item_retur_jual_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_item_retur_jual_produk (item_retur_jual_id, retur_jual_id, pengguna_id, produk_id, harga_jual, jumlah, tanggal_sistem, jumlah_retur, item_jual_id) FROM stdin;
4f36a4c0-5296-449d-8e6d-f958cc1f09ca	57a329b6-1f36-416d-a006-d83143871b8b	00b5acfa-b533-454b-8dfd-e7881edd180f	53b63dc2-4505-4276-9886-3639b53b7458	350000.00	1.00	2016-10-11 12:46:07.858	1.00	2c8ee1c2-848c-422b-b70c-f7019052f7f4
\.


--
-- Name: t_jual_jual_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_jual_jual_id_seq', 7, true);


--
-- Data for Name: t_jual_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_jual_produk (jual_id, pengguna_id, customer_id, nota, tanggal, tanggal_tempo, ppn, diskon, total_nota, total_pelunasan, keterangan, tanggal_sistem, retur_jual_id, shift_id) FROM stdin;
4a65e917-eeca-4620-9399-4a43d3d4c7a8	00b5acfa-b533-454b-8dfd-e7881edd180f	576c503f-69a7-46a5-b4be-107c634db7e3	004-0000003	2016-10-11	-infinity	0.00	0.00	1750000.00	1750000.00		2016-10-11 12:45:48.864	\N	\N
7577ee51-4659-4d21-a232-142bc69a7aff	00b5acfa-b533-454b-8dfd-e7881edd180f	576c503f-69a7-46a5-b4be-107c634db7e3	004-0000001	2016-09-20	2016-10-21	0.00	0.00	100000.00	0.00		2016-09-20 20:41:43.489	57a329b6-1f36-416d-a006-d83143871b8b	\N
98ac0476-ef66-4da1-a518-0a9029933ba2	00b5acfa-b533-454b-8dfd-e7881edd180f	576c503f-69a7-46a5-b4be-107c634db7e3	004-0000006	2016-10-26	-infinity	0.00	50000.00	1750000.00	1700000.00		2016-10-26 05:55:50.18	\N	\N
\.


--
-- Data for Name: t_mesin; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_mesin (mesin_id, pengguna_id, tanggal, saldo_awal, uang_masuk, tanggal_sistem, shift_id, uang_keluar) FROM stdin;
\.


--
-- Data for Name: t_pembayaran_bon; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_pembayaran_bon (pembayaran_bon_id, bon_id, gaji_karyawan_id, tanggal, nominal, keterangan, tanggal_sistem) FROM stdin;
\.


--
-- Data for Name: t_pembayaran_hutang_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_pembayaran_hutang_produk (pembayaran_hutang_produk_id, supplier_id, pengguna_id, tanggal, keterangan, tanggal_sistem, nota, is_tunai) FROM stdin;
d0caca75-4e38-4ebc-9efb-be27bc46c54d	7560fd72-0538-4307-8f15-14ef32cf5158	00b5acfa-b533-454b-8dfd-e7881edd180f	2016-10-11	Pembelian tunai produk	2016-10-11 12:49:46.052	002-0000001	t
\.


--
-- Name: t_pembayaran_hutang_produk_pembayaran_hutang_produk_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_pembayaran_hutang_produk_pembayaran_hutang_produk_id_seq', 1, true);


--
-- Name: t_pembayaran_piutang_pembayaran_piutang_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_pembayaran_piutang_pembayaran_piutang_id_seq', 2, true);


--
-- Data for Name: t_pembayaran_piutang_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_pembayaran_piutang_produk (pembayaran_piutang_id, customer_id, pengguna_id, tanggal, keterangan, tanggal_sistem, nota, is_tunai) FROM stdin;
228bebb1-8480-4e42-b3a6-cae40d95d289	576c503f-69a7-46a5-b4be-107c634db7e3	00b5acfa-b533-454b-8dfd-e7881edd180f	2016-10-11	Penjualan tunai produk	2016-10-11 12:45:48.864	005-0000001	t
f52a3337-eb4d-4f67-bb13-e25a3a7d094f	576c503f-69a7-46a5-b4be-107c634db7e3	00b5acfa-b533-454b-8dfd-e7881edd180f	2016-10-26	Penjualan tunai produk	2016-10-26 05:55:50.18	005-0000002	t
\.


--
-- Data for Name: t_pengeluaran; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_pengeluaran (pengeluaran_id, pengguna_id, nota, tanggal, total, keterangan, tanggal_sistem) FROM stdin;
\.


--
-- Name: t_pengeluaran_pengeluaran_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_pengeluaran_pengeluaran_id_seq', 1, false);


--
-- Data for Name: t_penyesuaian_stok; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_penyesuaian_stok (penyesuaian_stok_id, produk_id, alasan_penyesuaian_id, tanggal, penambahan_stok, pengurangan_stok, keterangan, tanggal_sistem, penambahan_stok_gudang, pengurangan_stok_gudang) FROM stdin;
\.


--
-- Name: t_produksi_produksi_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_produksi_produksi_id_seq', 1, false);


--
-- Data for Name: t_retur_beli_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_retur_beli_produk (retur_beli_produk_id, beli_produk_id, pengguna_id, supplier_id, nota, tanggal, keterangan, tanggal_sistem, total_nota) FROM stdin;
4a51da2b-d9c0-4ee8-a715-ab5cab618c32	8f38f5c9-e092-4b16-ab04-b1ce87cc8cc4	00b5acfa-b533-454b-8dfd-e7881edd180f	7560fd72-0538-4307-8f15-14ef32cf5158	003-0000004	2016-10-11		2016-10-11 12:49:58.61	400000.00
\.


--
-- Name: t_retur_beli_produk_retur_beli_produk_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_retur_beli_produk_retur_beli_produk_id_seq', 1, false);


--
-- Data for Name: t_retur_jual_produk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY t_retur_jual_produk (retur_jual_id, jual_id, pengguna_id, customer_id, nota, tanggal, keterangan, tanggal_sistem, total_nota) FROM stdin;
57a329b6-1f36-416d-a006-d83143871b8b	7577ee51-4659-4d21-a232-142bc69a7aff	00b5acfa-b533-454b-8dfd-e7881edd180f	576c503f-69a7-46a5-b4be-107c634db7e3	006-0000002	2016-10-11		2016-10-11 12:46:07.858	350000.00
\.


--
-- Name: t_retur_jual_retur_jual_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_retur_jual_retur_jual_id_seq', 5, true);


--
-- Name: t_sppd_sppd_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('t_sppd_sppd_id_seq', 1, false);


--
-- Name: m_alasan_penyesuaian_stok_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_alasan_penyesuaian_stok
    ADD CONSTRAINT m_alasan_penyesuaian_stok_pkey PRIMARY KEY (alasan_penyesuaian_stok_id);


--
-- Name: m_customer_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_customer
    ADD CONSTRAINT m_customer_pkey PRIMARY KEY (customer_id);


--
-- Name: m_golongan_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_golongan
    ADD CONSTRAINT m_golongan_pkey PRIMARY KEY (golongan_id);


--
-- Name: m_item_menu_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_item_menu
    ADD CONSTRAINT m_item_menu_pkey PRIMARY KEY (item_menu_id);


--
-- Name: m_jabatan_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_jabatan
    ADD CONSTRAINT m_jabatan_pkey PRIMARY KEY (jabatan_id);


--
-- Name: m_jenis_pengeluaran_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_jenis_pengeluaran
    ADD CONSTRAINT m_jenis_pengeluaran_pkey PRIMARY KEY (jenis_pengeluaran_id);


--
-- Name: m_karyawan_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_karyawan
    ADD CONSTRAINT m_karyawan_pkey PRIMARY KEY (karyawan_id);


--
-- Name: m_menu_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_menu
    ADD CONSTRAINT m_menu_pkey PRIMARY KEY (menu_id);


--
-- Name: m_pengguna_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_pengguna
    ADD CONSTRAINT m_pengguna_pkey PRIMARY KEY (pengguna_id);


--
-- Name: m_prefix_nota_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_prefix_nota
    ADD CONSTRAINT m_prefix_nota_pkey PRIMARY KEY (prefix_nota_id);


--
-- Name: m_produk_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_produk
    ADD CONSTRAINT m_produk_pkey PRIMARY KEY (produk_id);


--
-- Name: m_profil_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_profil
    ADD CONSTRAINT m_profil_pkey PRIMARY KEY (profil_id);


--
-- Name: m_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_role
    ADD CONSTRAINT m_role_pkey PRIMARY KEY (role_id);


--
-- Name: m_role_privilege_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_role_privilege
    ADD CONSTRAINT m_role_privilege_pkey PRIMARY KEY (role_id, menu_id, grant_id);


--
-- Name: m_setting_aplikasi_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_setting_aplikasi
    ADD CONSTRAINT m_setting_aplikasi_pkey PRIMARY KEY (setting_aplikasi_id);


--
-- Name: m_shift_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_shift
    ADD CONSTRAINT m_shift_pkey PRIMARY KEY (shift_id);


--
-- Name: m_supplier_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_supplier
    ADD CONSTRAINT m_supplier_pkey PRIMARY KEY (supplier_id);


--
-- Name: t_beli_produk_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_beli_produk
    ADD CONSTRAINT t_beli_produk_pkey PRIMARY KEY (beli_produk_id);


--
-- Name: t_bon_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_bon
    ADD CONSTRAINT t_bon_pkey PRIMARY KEY (bon_id);


--
-- Name: t_gaji_karyawan_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_gaji_karyawan
    ADD CONSTRAINT t_gaji_karyawan_pkey PRIMARY KEY (gaji_karyawan_id);


--
-- Name: t_item_beli_produk_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_item_beli_produk
    ADD CONSTRAINT t_item_beli_produk_pkey PRIMARY KEY (item_beli_produk_id);


--
-- Name: t_item_jual_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_item_jual_produk
    ADD CONSTRAINT t_item_jual_pkey PRIMARY KEY (item_jual_id);


--
-- Name: t_item_pembayaran_hutang_produk_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_item_pembayaran_hutang_produk
    ADD CONSTRAINT t_item_pembayaran_hutang_produk_pkey PRIMARY KEY (item_pembayaran_hutang_produk_id);


--
-- Name: t_item_pembayaran_piutang_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_item_pembayaran_piutang_produk
    ADD CONSTRAINT t_item_pembayaran_piutang_pkey PRIMARY KEY (item_pembayaran_piutang_id);


--
-- Name: t_item_pengeluaran_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_item_pengeluaran
    ADD CONSTRAINT t_item_pengeluaran_pkey PRIMARY KEY (item_pengeluaran_id);


--
-- Name: t_item_retur_beli_produk_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_item_retur_beli_produk
    ADD CONSTRAINT t_item_retur_beli_produk_pkey PRIMARY KEY (item_retur_beli_produk_id);


--
-- Name: t_item_retur_jual_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_item_retur_jual_produk
    ADD CONSTRAINT t_item_retur_jual_pkey PRIMARY KEY (item_retur_jual_id);


--
-- Name: t_jual_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_jual_produk
    ADD CONSTRAINT t_jual_pkey PRIMARY KEY (jual_id);


--
-- Name: t_mesin_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_mesin
    ADD CONSTRAINT t_mesin_pkey PRIMARY KEY (mesin_id);


--
-- Name: t_pembayaran_bon_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_pembayaran_bon
    ADD CONSTRAINT t_pembayaran_bon_pkey PRIMARY KEY (pembayaran_bon_id);


--
-- Name: t_pembayaran_hutang_produk_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_pembayaran_hutang_produk
    ADD CONSTRAINT t_pembayaran_hutang_produk_pkey PRIMARY KEY (pembayaran_hutang_produk_id);


--
-- Name: t_pembayaran_piutang_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_pembayaran_piutang_produk
    ADD CONSTRAINT t_pembayaran_piutang_pkey PRIMARY KEY (pembayaran_piutang_id);


--
-- Name: t_pengeluaran_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_pengeluaran
    ADD CONSTRAINT t_pengeluaran_pkey PRIMARY KEY (pengeluaran_id);


--
-- Name: t_penyesuaian_stok_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_penyesuaian_stok
    ADD CONSTRAINT t_penyesuaian_stok_pkey PRIMARY KEY (penyesuaian_stok_id);


--
-- Name: t_retur_beli_produk_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_retur_beli_produk
    ADD CONSTRAINT t_retur_beli_produk_pkey PRIMARY KEY (retur_beli_produk_id);


--
-- Name: t_retur_jual_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_retur_jual_produk
    ADD CONSTRAINT t_retur_jual_pkey PRIMARY KEY (retur_jual_id);


--
-- Name: m_produk_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX m_produk_idx ON m_produk USING btree (nama_produk, kode_produk);


--
-- Name: t_beli_produk_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_beli_produk_idx ON t_beli_produk USING btree (supplier_id, tanggal);


--
-- Name: t_jual_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_jual_idx ON t_jual_produk USING btree (pengguna_id, tanggal);


--
-- Name: tr_hapus_header_ad; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_hapus_header_ad AFTER DELETE ON t_item_pembayaran_hutang_produk FOR EACH ROW EXECUTE PROCEDURE f_hapus_header_bayar_hutang_produk();


--
-- Name: tr_hapus_header_ad; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_hapus_header_ad AFTER DELETE ON t_item_pembayaran_piutang_produk FOR EACH ROW EXECUTE PROCEDURE f_hapus_header_bayar_piutang_produk();


--
-- Name: tr_penyesuaian_stok_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_penyesuaian_stok_aiud AFTER INSERT OR DELETE OR UPDATE ON t_penyesuaian_stok FOR EACH ROW EXECUTE PROCEDURE f_penyesuaian_stok_aiud();


--
-- Name: tr_update_jumlah_retur_beli; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_jumlah_retur_beli AFTER INSERT OR DELETE OR UPDATE ON t_item_retur_beli_produk FOR EACH ROW EXECUTE PROCEDURE f_update_jumlah_retur_beli();


--
-- Name: tr_update_jumlah_retur_jual; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_jumlah_retur_jual AFTER INSERT OR DELETE OR UPDATE ON t_item_retur_jual_produk FOR EACH ROW EXECUTE PROCEDURE f_update_jumlah_retur_jual();


--
-- Name: tr_update_pelunasan_beli_produk_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_pelunasan_beli_produk_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_pembayaran_hutang_produk FOR EACH ROW EXECUTE PROCEDURE f_update_pelunasan_beli_produk();


--
-- Name: tr_update_pelunasan_jual_produk_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_pelunasan_jual_produk_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_pembayaran_piutang_produk FOR EACH ROW EXECUTE PROCEDURE f_update_pelunasan_jual_produk();


--
-- Name: tr_update_pelunasan_kasbon_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_pelunasan_kasbon_aiud AFTER INSERT OR DELETE OR UPDATE ON t_pembayaran_bon FOR EACH ROW EXECUTE PROCEDURE f_update_pelunasan_kasbon();


--
-- Name: tr_update_stok_produk_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_stok_produk_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_beli_produk FOR EACH ROW EXECUTE PROCEDURE f_tambah_stok_produk();


--
-- Name: tr_update_stok_produk_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_stok_produk_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_jual_produk FOR EACH ROW EXECUTE PROCEDURE f_kurangi_stok_produk();


--
-- Name: tr_update_total_beli_produk_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_total_beli_produk_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_beli_produk FOR EACH ROW EXECUTE PROCEDURE f_update_total_beli_produk();


--
-- Name: tr_update_total_hutang_produk_supplier; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_total_hutang_produk_supplier AFTER INSERT OR DELETE OR UPDATE ON t_beli_produk FOR EACH ROW EXECUTE PROCEDURE f_update_total_hutang_supplier();


--
-- Name: tr_update_total_jual_produk_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_total_jual_produk_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_jual_produk FOR EACH ROW EXECUTE PROCEDURE f_update_total_jual_produk();


--
-- Name: tr_update_total_kasbon_karyawan; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_total_kasbon_karyawan AFTER INSERT OR DELETE OR UPDATE ON t_bon FOR EACH ROW EXECUTE PROCEDURE f_update_total_kasbon_karyawan();


--
-- Name: tr_update_total_pengeluaran_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_total_pengeluaran_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_pengeluaran FOR EACH ROW EXECUTE PROCEDURE f_update_total_pengeluaran();


--
-- Name: tr_update_total_piutang_customer; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_total_piutang_customer AFTER INSERT OR DELETE OR UPDATE ON t_jual_produk FOR EACH ROW EXECUTE PROCEDURE f_update_total_piutang_customer();


--
-- Name: tr_update_total_retur_beli_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_total_retur_beli_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_retur_beli_produk FOR EACH ROW EXECUTE PROCEDURE f_update_total_retur_beli_aiud();


--
-- Name: tr_update_total_retur_produk_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_total_retur_produk_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_retur_jual_produk FOR EACH ROW EXECUTE PROCEDURE f_update_total_retur_produk_aiud();


--
-- Name: m_item_menu_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY m_item_menu
    ADD CONSTRAINT m_item_menu_fk FOREIGN KEY (menu_id) REFERENCES m_menu(menu_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: m_karyawan_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY m_karyawan
    ADD CONSTRAINT m_karyawan_fk FOREIGN KEY (jabatan_id) REFERENCES m_jabatan(jabatan_id) ON UPDATE CASCADE;


--
-- Name: m_pengguna_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY m_pengguna
    ADD CONSTRAINT m_pengguna_fk FOREIGN KEY (role_id) REFERENCES m_role(role_id) ON UPDATE CASCADE;


--
-- Name: m_produk_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY m_produk
    ADD CONSTRAINT m_produk_fk FOREIGN KEY (golongan_id) REFERENCES m_golongan(golongan_id) ON UPDATE CASCADE;


--
-- Name: m_role_privilege_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY m_role_privilege
    ADD CONSTRAINT m_role_privilege_fk FOREIGN KEY (menu_id) REFERENCES m_menu(menu_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: m_role_privilege_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY m_role_privilege
    ADD CONSTRAINT m_role_privilege_fk1 FOREIGN KEY (role_id) REFERENCES m_role(role_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_beli_produk_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_beli_produk
    ADD CONSTRAINT t_beli_produk_fk FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_beli_produk_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_beli_produk
    ADD CONSTRAINT t_beli_produk_fk1 FOREIGN KEY (supplier_id) REFERENCES m_supplier(supplier_id) ON UPDATE CASCADE;


--
-- Name: t_beli_produk_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_beli_produk
    ADD CONSTRAINT t_beli_produk_fk2 FOREIGN KEY (retur_beli_produk_id) REFERENCES t_retur_beli_produk(retur_beli_produk_id) ON UPDATE CASCADE ON DELETE SET NULL;


--
-- Name: t_bon_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_bon
    ADD CONSTRAINT t_bon_fk FOREIGN KEY (karyawan_id) REFERENCES m_karyawan(karyawan_id) ON UPDATE CASCADE;


--
-- Name: t_bon_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_bon
    ADD CONSTRAINT t_bon_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_gaji_karyawan_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_gaji_karyawan
    ADD CONSTRAINT t_gaji_karyawan_fk FOREIGN KEY (karyawan_id) REFERENCES m_karyawan(karyawan_id) ON UPDATE CASCADE;


--
-- Name: t_gaji_karyawan_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_gaji_karyawan
    ADD CONSTRAINT t_gaji_karyawan_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_item_beli_produk_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_beli_produk
    ADD CONSTRAINT t_item_beli_produk_fk FOREIGN KEY (beli_produk_id) REFERENCES t_beli_produk(beli_produk_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_beli_produk_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_beli_produk
    ADD CONSTRAINT t_item_beli_produk_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_item_beli_produk_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_beli_produk
    ADD CONSTRAINT t_item_beli_produk_fk2 FOREIGN KEY (produk_id) REFERENCES m_produk(produk_id) ON UPDATE CASCADE;


--
-- Name: t_item_jual_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_jual_produk
    ADD CONSTRAINT t_item_jual_fk FOREIGN KEY (jual_id) REFERENCES t_jual_produk(jual_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_jual_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_jual_produk
    ADD CONSTRAINT t_item_jual_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_item_jual_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_jual_produk
    ADD CONSTRAINT t_item_jual_fk2 FOREIGN KEY (produk_id) REFERENCES m_produk(produk_id) ON UPDATE CASCADE;


--
-- Name: t_item_pembayaran_hutang_produk_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_pembayaran_hutang_produk
    ADD CONSTRAINT t_item_pembayaran_hutang_produk_fk FOREIGN KEY (pembayaran_hutang_produk_id) REFERENCES t_pembayaran_hutang_produk(pembayaran_hutang_produk_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_pembayaran_hutang_produk_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_pembayaran_hutang_produk
    ADD CONSTRAINT t_item_pembayaran_hutang_produk_fk1 FOREIGN KEY (beli_produk_id) REFERENCES t_beli_produk(beli_produk_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_pembayaran_piutang_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_pembayaran_piutang_produk
    ADD CONSTRAINT t_item_pembayaran_piutang_fk FOREIGN KEY (pembayaran_piutang_id) REFERENCES t_pembayaran_piutang_produk(pembayaran_piutang_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_pembayaran_piutang_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_pembayaran_piutang_produk
    ADD CONSTRAINT t_item_pembayaran_piutang_fk1 FOREIGN KEY (jual_id) REFERENCES t_jual_produk(jual_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_pengeluaran_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_pengeluaran
    ADD CONSTRAINT t_item_pengeluaran_fk FOREIGN KEY (pengeluaran_id) REFERENCES t_pengeluaran(pengeluaran_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_pengeluaran_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_pengeluaran
    ADD CONSTRAINT t_item_pengeluaran_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_item_pengeluaran_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_pengeluaran
    ADD CONSTRAINT t_item_pengeluaran_fk2 FOREIGN KEY (jenis_pengeluaran_id) REFERENCES m_jenis_pengeluaran(jenis_pengeluaran_id) ON UPDATE CASCADE;


--
-- Name: t_item_retur_beli_produk_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_retur_beli_produk
    ADD CONSTRAINT t_item_retur_beli_produk_fk FOREIGN KEY (retur_beli_produk_id) REFERENCES t_retur_beli_produk(retur_beli_produk_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_retur_beli_produk_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_retur_beli_produk
    ADD CONSTRAINT t_item_retur_beli_produk_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_item_retur_beli_produk_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_retur_beli_produk
    ADD CONSTRAINT t_item_retur_beli_produk_fk2 FOREIGN KEY (produk_id) REFERENCES m_produk(produk_id) ON UPDATE CASCADE;


--
-- Name: t_item_retur_beli_produk_fk3; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_retur_beli_produk
    ADD CONSTRAINT t_item_retur_beli_produk_fk3 FOREIGN KEY (item_beli_id) REFERENCES t_item_beli_produk(item_beli_produk_id) ON UPDATE CASCADE;


--
-- Name: t_item_retur_jual_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_retur_jual_produk
    ADD CONSTRAINT t_item_retur_jual_fk FOREIGN KEY (retur_jual_id) REFERENCES t_retur_jual_produk(retur_jual_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_retur_jual_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_retur_jual_produk
    ADD CONSTRAINT t_item_retur_jual_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_item_retur_jual_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_retur_jual_produk
    ADD CONSTRAINT t_item_retur_jual_fk2 FOREIGN KEY (produk_id) REFERENCES m_produk(produk_id) ON UPDATE CASCADE;


--
-- Name: t_item_retur_jual_fk3; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_retur_jual_produk
    ADD CONSTRAINT t_item_retur_jual_fk3 FOREIGN KEY (item_jual_id) REFERENCES t_item_jual_produk(item_jual_id) ON UPDATE CASCADE;


--
-- Name: t_jual_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_jual_produk
    ADD CONSTRAINT t_jual_fk FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_jual_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_jual_produk
    ADD CONSTRAINT t_jual_fk1 FOREIGN KEY (customer_id) REFERENCES m_customer(customer_id) ON UPDATE CASCADE;


--
-- Name: t_jual_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_jual_produk
    ADD CONSTRAINT t_jual_fk2 FOREIGN KEY (retur_jual_id) REFERENCES t_retur_jual_produk(retur_jual_id) ON UPDATE CASCADE ON DELETE SET NULL;


--
-- Name: t_jual_fk3; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_jual_produk
    ADD CONSTRAINT t_jual_fk3 FOREIGN KEY (shift_id) REFERENCES m_shift(shift_id) ON UPDATE CASCADE;


--
-- Name: t_mesin_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_mesin
    ADD CONSTRAINT t_mesin_fk FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_pembayaran_bon_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pembayaran_bon
    ADD CONSTRAINT t_pembayaran_bon_fk FOREIGN KEY (bon_id) REFERENCES t_bon(bon_id) ON UPDATE CASCADE;


--
-- Name: t_pembayaran_bon_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pembayaran_bon
    ADD CONSTRAINT t_pembayaran_bon_fk1 FOREIGN KEY (gaji_karyawan_id) REFERENCES t_gaji_karyawan(gaji_karyawan_id) ON UPDATE CASCADE;


--
-- Name: t_pembayaran_hutang_produk_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pembayaran_hutang_produk
    ADD CONSTRAINT t_pembayaran_hutang_produk_fk FOREIGN KEY (supplier_id) REFERENCES m_supplier(supplier_id) ON UPDATE CASCADE;


--
-- Name: t_pembayaran_hutang_produk_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pembayaran_hutang_produk
    ADD CONSTRAINT t_pembayaran_hutang_produk_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_pembayaran_piutang_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pembayaran_piutang_produk
    ADD CONSTRAINT t_pembayaran_piutang_fk FOREIGN KEY (customer_id) REFERENCES m_customer(customer_id) ON UPDATE CASCADE;


--
-- Name: t_pembayaran_piutang_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pembayaran_piutang_produk
    ADD CONSTRAINT t_pembayaran_piutang_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_pengeluaran_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pengeluaran
    ADD CONSTRAINT t_pengeluaran_fk FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_penyesuaian_stok_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_penyesuaian_stok
    ADD CONSTRAINT t_penyesuaian_stok_fk FOREIGN KEY (produk_id) REFERENCES m_produk(produk_id) ON UPDATE CASCADE;


--
-- Name: t_penyesuaian_stok_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_penyesuaian_stok
    ADD CONSTRAINT t_penyesuaian_stok_fk1 FOREIGN KEY (alasan_penyesuaian_id) REFERENCES m_alasan_penyesuaian_stok(alasan_penyesuaian_stok_id) ON UPDATE CASCADE;


--
-- Name: t_retur_beli_produk_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_retur_beli_produk
    ADD CONSTRAINT t_retur_beli_produk_fk FOREIGN KEY (beli_produk_id) REFERENCES t_beli_produk(beli_produk_id) ON UPDATE CASCADE ON DELETE SET NULL;


--
-- Name: t_retur_beli_produk_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_retur_beli_produk
    ADD CONSTRAINT t_retur_beli_produk_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_retur_beli_produk_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_retur_beli_produk
    ADD CONSTRAINT t_retur_beli_produk_fk2 FOREIGN KEY (supplier_id) REFERENCES m_supplier(supplier_id) ON UPDATE CASCADE;


--
-- Name: t_retur_jual_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_retur_jual_produk
    ADD CONSTRAINT t_retur_jual_fk FOREIGN KEY (jual_id) REFERENCES t_jual_produk(jual_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_retur_jual_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_retur_jual_produk
    ADD CONSTRAINT t_retur_jual_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_retur_jual_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_retur_jual_produk
    ADD CONSTRAINT t_retur_jual_fk2 FOREIGN KEY (customer_id) REFERENCES m_customer(customer_id) ON UPDATE CASCADE;


--
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--

