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
-- Name: t_alamat_panjang; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_alamat_panjang AS character varying(250);


ALTER DOMAIN t_alamat_panjang OWNER TO postgres;

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
-- Name: t_kode_pos; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_kode_pos AS character varying(6);


ALTER DOMAIN t_kode_pos OWNER TO postgres;

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
-- Name: t_nama_panjang; Type: DOMAIN; Schema: public; Owner: postgres
--

CREATE DOMAIN t_nama_panjang AS character varying(300);


ALTER DOMAIN t_nama_panjang OWNER TO postgres;

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
                           
            var_stok_gudang_sekarang = var_stok_gudang_sekarang - var_jumlah_retur_lama + var_jumlah_retur_baru;
			            
            UPDATE m_produk SET stok_gudang = var_stok_gudang_sekarang WHERE produk_id = var_produk_id;
        END IF;        
    END IF;
	
    IF (is_retur = FALSE) THEN -- bukan retur    	    	        
        IF TG_OP = 'INSERT' THEN
            var_stok_gudang_sekarang := var_stok_gudang_sekarang - var_jumlah_baru;
            
            IF (var_stok_gudang_sekarang < 0 AND var_stok_sekarang > 0) THEN -- stok gudang kurang, sisanya ambil dari stok etalase
            	var_stok_sekarang = var_stok_sekarang - abs(var_stok_gudang_sekarang);
                
                IF (var_stok_sekarang >= 0) THEN
                	var_stok_gudang_sekarang := 0; --stok gudang habis
                ELSE
                	var_stok_gudang_sekarang := var_stok_sekarang;
                    var_stok_sekarang := 0;
                END IF;
            END IF;
                        
            
        ELSIF TG_OP = 'UPDATE' THEN      
            var_stok_gudang_sekarang = var_stok_gudang_sekarang + var_jumlah_lama - var_jumlah_baru;
        ELSE
            var_stok_gudang_sekarang = var_stok_gudang_sekarang + var_jumlah_lama;
        END IF;
        
        IF TG_OP = 'INSERT' THEN             
            UPDATE m_produk SET stok = var_stok_sekarang, stok_gudang = var_stok_gudang_sekarang, harga_jual = var_harga WHERE produk_id = var_produk_id;        
        ELSE        
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
	var_produk_id 				t_guid;
	
    var_stok_awal				t_jumlah;
	var_stok_akhir				t_jumlah;
    var_stok_sekarang 			t_jumlah; -- stok etalase
	var_stok_gudang_sekarang 	t_jumlah; -- stok gudang
    
    var_jumlah_lama 			t_jumlah;
	var_jumlah_baru 			t_jumlah;
	
    var_jumlah_retur_lama 		t_jumlah;
    var_jumlah_retur_baru 		t_jumlah;
    
	var_harga 					t_harga; -- harga beli di item beli
    var_hpp_awal				t_harga; -- harga beli di master produk
    var_hpp_akhir				t_harga;
	var_saldo_awal				t_harga; -- (stok etalasi + stok gudang) * harga beli di master produk 
    var_saldo_pembelian			t_harga;
    
    is_retur					t_bool;
  
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
        var_jumlah_baru = 0;
        var_jumlah_lama = OLD.jumlah;
        var_harga = OLD.harga;
    END IF;        
    
    SELECT stok, stok_gudang, harga_beli INTO var_stok_sekarang, var_stok_gudang_sekarang, var_hpp_awal
    FROM m_produk WHERE produk_id = var_produk_id;
    
    IF var_stok_sekarang IS NULL THEN
    	var_stok_sekarang := 0;  
	END IF;
    
    IF var_stok_gudang_sekarang IS NULL THEN
    	var_stok_gudang_sekarang := 0;  
	END IF;
    
    IF var_hpp_awal IS NULL THEN
    	var_hpp_awal := 0;  
	END IF;        
    
    var_stok_akhir := 1;
    var_stok_awal := var_stok_sekarang + var_stok_gudang_sekarang;    
    var_saldo_awal := var_stok_awal * var_hpp_awal;
    
    IF TG_OP = 'UPDATE' THEN -- pengecekan retur
    	IF var_jumlah_retur_lama <> var_jumlah_retur_baru THEN
        	is_retur := TRUE;                    
            
            IF var_jumlah_retur_lama IS NULL THEN
                var_jumlah_retur_lama := 0;  
            END IF;
            
            IF var_jumlah_retur_baru IS NULL THEN
                var_jumlah_retur_baru := 0;  
            END IF;
			          
            var_saldo_pembelian := ABS(var_jumlah_retur_lama - var_jumlah_retur_baru) * var_harga;  
            
            var_stok_akhir := (var_stok_awal - ABS(var_jumlah_retur_lama - var_jumlah_retur_baru));
            IF (var_stok_akhir = 0) THEN
            	var_stok_akhir := 1;
            END IF;
            
            var_hpp_akhir := (var_saldo_awal - var_saldo_pembelian) / var_stok_akhir;
                                       
            var_stok_gudang_sekarang = var_stok_gudang_sekarang + var_jumlah_retur_lama - var_jumlah_retur_baru;			            
                          			            
            UPDATE m_produk SET stok_gudang = var_stok_gudang_sekarang, harga_beli = ROUND(var_hpp_akhir, 0) WHERE produk_id = var_produk_id;
        END IF;        
    END IF;
    
    IF (is_retur = FALSE) THEN -- bukan retur    	        	                                    
      IF TG_OP = 'INSERT' THEN                        
          var_saldo_pembelian := var_jumlah_baru * var_harga;          
          
          var_stok_akhir := (var_stok_awal + var_jumlah_baru);
          
          IF (var_stok_akhir = 0) THEN
              var_stok_akhir := 1;
          END IF;
          
		  var_hpp_akhir := (var_saldo_awal + var_saldo_pembelian) / var_stok_akhir;                                         
          
          var_stok_gudang_sekarang = var_stok_gudang_sekarang + var_jumlah_baru;
          
      ELSIF TG_OP = 'UPDATE' THEN             
          var_saldo_pembelian := ABS(var_jumlah_baru - var_jumlah_lama) * var_harga;
          var_hpp_akhir := var_hpp_awal;
          
      	  IF (var_jumlah_baru > var_jumlah_lama) THEN      
          	
          	 var_stok_akhir := (var_stok_awal + var_jumlah_baru - var_jumlah_lama);
             IF (var_stok_akhir = 0) THEN
             	var_stok_akhir := 1;
             END IF;

          	 var_hpp_akhir := (var_saldo_awal + var_saldo_pembelian) / var_stok_akhir;                     
             
          ELSEIF (var_jumlah_baru < var_jumlah_lama) THEN          	 
          	 
          	 var_stok_akhir := (var_stok_awal - ABS(var_jumlah_baru - var_jumlah_lama));
             IF (var_stok_akhir = 0) THEN
             	var_stok_akhir := 1;
             END IF;
             
          	 var_hpp_akhir := (var_saldo_awal - var_saldo_pembelian) / var_stok_akhir;                                   
          END IF;          
                                        
          var_stok_gudang_sekarang = var_stok_gudang_sekarang - var_jumlah_lama + var_jumlah_baru;
      ELSE -- DELETE
          var_saldo_pembelian := var_jumlah_lama * var_harga;
          
          var_stok_akhir := (var_stok_awal - var_jumlah_lama);
          IF (var_stok_akhir = 0) THEN
             var_stok_akhir := 1;
          END IF;
             
          var_hpp_akhir := (var_saldo_awal - var_saldo_pembelian) / var_stok_akhir;                     
                    
          var_stok_gudang_sekarang = var_stok_gudang_sekarang - var_jumlah_lama;          
      END IF;          
      
	  UPDATE m_produk SET stok_gudang = var_stok_gudang_sekarang, harga_beli = ROUND(var_hpp_akhir, 0) WHERE produk_id = var_produk_id;              
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
	var_kasbon_id				t_guid;   
    var_gaji_karyawan_id		t_guid; 
	var_total_pelunasan_kasbon 	t_harga;

BEGIN
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_kasbon_id := NEW.kasbon_id;    
        var_gaji_karyawan_id := NEW.gaji_karyawan_id;
    ELSE
    	var_kasbon_id := OLD.kasbon_id;
        var_gaji_karyawan_id := OLD.gaji_karyawan_id;
    END IF;
	        
    -- pelunasan kasbon
    var_total_pelunasan_kasbon := (SELECT SUM(nominal) FROM t_pembayaran_kasbon 
    							   WHERE kasbon_id = var_kasbon_id);	    
	
    IF var_total_pelunasan_kasbon IS NULL THEN
    	var_total_pelunasan_kasbon := 0;  
	END IF;        
    
    -- kasbon
    UPDATE t_kasbon SET total_pelunasan = var_total_pelunasan_kasbon 
    WHERE kasbon_id = var_kasbon_id;    
    
    -- hitung total pelunasan yang dibayar pake gaji (potongan gaji untuk kasbon) 
    IF NOT (var_gaji_karyawan_id IS NULL) THEN
    	-- pelunasan kasbon
    	var_total_pelunasan_kasbon := (SELECT SUM(nominal) FROM t_pembayaran_kasbon 
    							   	   WHERE gaji_karyawan_id = var_gaji_karyawan_id);	    
	
	    IF var_total_pelunasan_kasbon IS NULL THEN
    		var_total_pelunasan_kasbon := 0;  
		END IF;
    	
        UPDATE t_gaji_karyawan SET kasbon = var_total_pelunasan_kasbon 
	    WHERE gaji_karyawan_id = var_gaji_karyawan_id;
	END IF;
    
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
    var_diskon				t_harga;
    var_ppn					t_harga;
	var_total_nota 			t_harga;
  	var_jumlah_retur_lama 	t_jumlah;
    var_jumlah_retur_baru 	t_jumlah;
    var_tanggal_tempo		DATE;  
    is_retur				t_bool;
    
BEGIN
	is_retur := FALSE;
    
	IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
    	var_beli_produk_id := NEW.beli_produk_id;    
    ELSE
    	var_beli_produk_id := OLD.beli_produk_id;
    END IF;	 
                       
    var_total_nota := (SELECT SUM((jumlah - jumlah_retur) * (harga - (diskon / 100 * harga))) 
    				   FROM t_item_beli_produk
					   WHERE beli_produk_id = var_beli_produk_id);
	
    IF var_total_nota IS NULL THEN
    	var_total_nota := 0;  
	END IF;              
    --
    SELECT ppn, diskon INTO var_ppn, var_diskon
    FROM t_beli_produk WHERE beli_produk_id = var_beli_produk_id;            
    
    IF var_ppn IS NULL THEN
    	var_ppn := 0;  
	END IF;
    
    IF var_diskon IS NULL THEN
    	var_diskon := 0;  
	END IF;           

	IF TG_OP = 'UPDATE' THEN -- pengecekan retur
    	-- jumlah retur
        var_jumlah_retur_baru = NEW.jumlah_retur;
        var_jumlah_retur_lama = OLD.jumlah_retur;        
        
    	IF var_jumlah_retur_lama <> var_jumlah_retur_baru THEN
			is_retur := TRUE;
			
            var_retur_beli_id := (SELECT retur_beli_produk_id FROM t_retur_beli_produk WHERE beli_produk_id = var_beli_produk_id LIMIT 1);
			var_tanggal_tempo := (SELECT tanggal_tempo FROM t_beli_produk WHERE beli_produk_id = var_beli_produk_id);                                    
            
            IF var_tanggal_tempo IS NULL THEN -- nota tunai            	
                UPDATE t_beli_produk SET total_nota = ROUND(var_total_nota, 0), retur_beli_produk_id = var_retur_beli_id 
                WHERE beli_produk_id = var_beli_produk_id;                
                
                UPDATE t_item_pembayaran_hutang_produk SET nominal = ROUND(var_total_nota, 0) - var_diskon + var_ppn
                WHERE beli_produk_id = var_beli_produk_id;
            ELSE
            	UPDATE t_beli_produk SET total_nota = ROUND(var_total_nota, 0), retur_beli_produk_id = var_retur_beli_id 
                WHERE beli_produk_id = var_beli_produk_id;
            END IF;        
        END IF;        
    END IF;
    
    IF (is_retur = FALSE) THEN -- bukan retur    	
	    UPDATE t_beli_produk SET total_nota = ROUND(var_total_nota, 0) 
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
    FROM t_beli_produk 
    WHERE tanggal_tempo IS NOT NULL AND supplier_id = var_supplier_id;

    IF var_total_hutang_produk IS NULL THEN
        var_total_hutang_produk := 0;  
    END IF; 
        
    IF var_total_pelunasan_produk IS NULL THEN
        var_total_pelunasan_produk := 0;  
    END IF;
                
    UPDATE m_supplier SET total_hutang = var_total_hutang_produk, total_pembayaran_hutang = var_total_pelunasan_produk 
    WHERE supplier_id = var_supplier_id;       
    
    IF TG_OP = 'UPDATE' THEN
    	var_supplier_id_old := OLD.supplier_id;
        
    	IF var_supplier_id <> var_supplier_id_old THEN
            -- hitung total hutang dan pelunasan pembelian produk
            SELECT SUM(total_nota - diskon + ppn) AS total_hutang, SUM(total_pelunasan) AS total_pelunasan
            INTO var_total_hutang_produk, var_total_pelunasan_produk
            FROM t_beli_produk             
            WHERE tanggal_tempo IS NOT NULL AND supplier_id = var_supplier_id_old;

            IF var_total_hutang_produk IS NULL THEN
                var_total_hutang_produk := 0;  
            END IF; 
                
            IF var_total_pelunasan_produk IS NULL THEN
                var_total_pelunasan_produk := 0;  
            END IF;
                
            UPDATE m_supplier SET total_hutang = var_total_hutang_produk, total_pembayaran_hutang = var_total_pelunasan_produk 
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
    var_diskon				t_harga;
    var_ppn					t_harga;
	var_ongkos_kirim		t_harga;
	var_total_nota 			t_harga;        
    var_jumlah_retur_lama 	t_jumlah;
    var_jumlah_retur_baru 	t_jumlah;
    var_tanggal_tempo		DATE;
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
    
	SELECT ppn, ongkos_kirim, diskon INTO var_ppn, var_ongkos_kirim, var_diskon
    FROM t_jual_produk WHERE jual_id = var_jual_id;            
    
    IF var_ppn IS NULL THEN
    	var_ppn := 0;  
	END IF;
    
	IF var_ongkos_kirim IS NULL THEN
    	var_ongkos_kirim := 0;  
	END IF;
	
    IF var_diskon IS NULL THEN
    	var_diskon := 0;  
	END IF;
    
    IF TG_OP = 'UPDATE' THEN -- pengecekan retur
    	-- jumlah retur
        var_jumlah_retur_baru = NEW.jumlah_retur;
        var_jumlah_retur_lama = OLD.jumlah_retur;        
        
    	IF var_jumlah_retur_lama <> var_jumlah_retur_baru THEN
			is_retur := TRUE;
			
            var_retur_jual_id := (SELECT retur_jual_id FROM t_retur_jual_produk WHERE jual_id = var_jual_id LIMIT 1);
            var_tanggal_tempo := (SELECT tanggal_tempo FROM t_jual_produk WHERE jual_id = var_jual_id);                                                
            
            IF var_tanggal_tempo IS NULL THEN -- nota tunai            	
                UPDATE t_jual_produk SET total_nota = ROUND(var_total_nota, 0), retur_jual_id = var_retur_jual_id 
                WHERE jual_id = var_jual_id;                
                
                UPDATE t_item_pembayaran_piutang_produk SET nominal = ROUND(var_total_nota, 0) - var_diskon + var_ppn + var_ongkos_kirim
                WHERE jual_id = var_jual_id;
            ELSE
            	UPDATE t_jual_produk SET total_nota = ROUND(var_total_nota, 0), retur_jual_id = var_retur_jual_id 
                WHERE jual_id = var_jual_id;
            END IF;        
        END IF;        
    END IF;              

	IF (is_retur = FALSE) THEN -- bukan retur    	
    	UPDATE t_jual_produk SET total_nota = ROUND(var_total_nota, 0), retur_jual_id = NULL 
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
	FROM t_kasbon WHERE karyawan_id = var_karyawan_id;
        
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
        
    v_total := (SELECT SUM(jumlah * harga) FROM t_item_pengeluaran_biaya
			    WHERE pengeluaran_id = v_pengeluaran_id);
	
    IF v_total IS NULL THEN
    	v_total := 0;  
	END IF;
    
    UPDATE t_pengeluaran_biaya SET total = v_total WHERE pengeluaran_id = v_pengeluaran_id;                        
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
	    	            
	SELECT SUM(total_nota - diskon + ongkos_kirim + ppn), SUM(total_pelunasan)
    INTO var_total_piutang, var_total_pelunasan
	FROM t_jual_produk     
    WHERE tanggal_tempo IS NOT NULL AND customer_id = var_customer_id;
    
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
        	SELECT SUM(total_nota - diskon + ongkos_kirim + ppn), SUM(total_pelunasan)
            INTO var_total_piutang, var_total_pelunasan
            FROM t_jual_produk             
            WHERE tanggal_tempo IS NOT NULL AND customer_id = var_customer_id_old;
            
            IF var_total_piutang IS NULL THEN
                var_total_piutang := 0;  
            END IF;
            
            IF var_total_pelunasan IS NULL THEN
                var_total_pelunasan := 0;  
            END IF;                           
			
			UPDATE m_customer SET total_piutang = var_total_piutang, total_pembayaran_piutang = var_total_pelunasan
            WHERE customer_id = var_customer_id_old;
            
            UPDATE t_pembayaran_piutang_produk SET customer_id = var_customer_id 
            WHERE pembayaran_piutang_id IN (SELECT pembayaran_piutang_id FROM t_item_pembayaran_piutang_produk WHERE jual_id = NEW.jual_id);
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
    alamat t_alamat_panjang,
    kontak t_nama,
    telepon t_telepon,
    plafon_piutang t_harga,
    total_piutang t_harga,
    total_pembayaran_piutang t_harga,
    kecamatan t_alamat,
    kelurahan t_alamat,
    kota t_alamat,
    kode_pos t_kode_pos,
    diskon t_jumlah,
    desa t_alamat,
    kabupaten t_alamat
);


ALTER TABLE m_customer OWNER TO postgres;

--
-- Name: m_database_version; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_database_version (
    version_number integer NOT NULL
);


ALTER TABLE m_database_version OWNER TO postgres;

--
-- Name: m_footer_nota_mini_pos; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_footer_nota_mini_pos (
    footer_nota_id t_guid NOT NULL,
    keterangan t_keterangan,
    order_number integer,
    is_active t_bool
);


ALTER TABLE m_footer_nota_mini_pos OWNER TO postgres;

--
-- Name: m_golongan; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_golongan (
    golongan_id t_guid NOT NULL,
    nama_golongan t_nama,
    diskon t_jumlah
);


ALTER TABLE m_golongan OWNER TO postgres;

--
-- Name: m_harga_grosir; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_harga_grosir (
    harga_grosir_id t_guid NOT NULL,
    produk_id t_guid,
    harga_ke integer,
    harga_grosir t_harga,
    jumlah_minimal t_jumlah,
    diskon t_jumlah
);


ALTER TABLE m_harga_grosir OWNER TO postgres;

--
-- Name: m_header_nota; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_header_nota (
    header_nota_id t_guid NOT NULL,
    keterangan t_keterangan,
    order_number integer,
    is_active t_bool
);


ALTER TABLE m_header_nota OWNER TO postgres;

--
-- Name: m_header_nota_mini_pos; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_header_nota_mini_pos (
    header_nota_id t_guid NOT NULL,
    keterangan t_keterangan,
    order_number integer,
    is_active t_bool
);


ALTER TABLE m_header_nota_mini_pos OWNER TO postgres;

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
-- Name: m_kabupaten; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_kabupaten (
    kabupaten_id integer NOT NULL,
    provinsi_id integer,
    tipe character varying(15),
    nama_kabupaten t_keterangan,
    kode_pos t_kode_pos
);


ALTER TABLE m_kabupaten OWNER TO postgres;

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
-- Name: m_label_nota; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_label_nota (
    label_nota_id t_guid NOT NULL,
    keterangan t_keterangan,
    order_number integer,
    is_active t_bool
);


ALTER TABLE m_label_nota OWNER TO postgres;

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
    nama_produk t_nama_panjang,
    satuan t_satuan,
    stok t_jumlah,
    harga_beli t_harga,
    harga_jual t_harga,
    kode_produk t_kode_produk,
    golongan_id t_guid,
    minimal_stok t_jumlah DEFAULT 0,
    stok_gudang t_jumlah,
    minimal_stok_gudang t_jumlah,
    diskon t_jumlah
);


ALTER TABLE m_produk OWNER TO postgres;

--
-- Name: m_produk_produk_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE m_produk_produk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE m_produk_produk_id_seq OWNER TO postgres;

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
-- Name: m_provinsi; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE m_provinsi (
    provinsi_id integer NOT NULL,
    nama_provinsi t_keterangan
);


ALTER TABLE m_provinsi OWNER TO postgres;

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
    jam integer DEFAULT 0,
    lainnya t_harga DEFAULT 0,
    keterangan t_keterangan,
    jumlah_hari integer DEFAULT 0,
    tunjangan t_harga DEFAULT 0,
    kasbon t_harga DEFAULT 0,
    tanggal date,
    nota t_nota
);


ALTER TABLE t_gaji_karyawan OWNER TO postgres;

--
-- Name: t_gaji_karyawan_gaji_karyawan_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_gaji_karyawan_gaji_karyawan_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_gaji_karyawan_gaji_karyawan_id_seq OWNER TO postgres;

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
-- Name: t_item_pengeluaran_biaya; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_item_pengeluaran_biaya (
    item_pengeluaran_id t_guid NOT NULL,
    pengeluaran_id t_guid,
    pengguna_id t_guid,
    jumlah t_jumlah,
    harga t_harga,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    jenis_pengeluaran_id t_guid
);


ALTER TABLE t_item_pengeluaran_biaya OWNER TO postgres;

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
    shift_id t_guid,
    is_sdac t_bool,
    kirim_kecamatan t_alamat_panjang,
    kirim_kelurahan t_alamat_panjang,
    kirim_kota t_alamat_panjang,
    kirim_kode_pos t_kode_pos,
    kirim_kepada t_nama,
    kirim_alamat t_alamat_panjang,
    kirim_telepon t_telepon,
    ongkos_kirim t_harga,
    label_dari1 t_keterangan,
    label_dari2 t_keterangan,
    label_dari3 t_keterangan,
    label_dari4 t_keterangan,
    label_kepada1 t_alamat_panjang,
    label_kepada2 t_alamat_panjang,
    label_kepada3 t_alamat_panjang,
    label_kepada4 t_alamat_panjang,
    kurir t_keterangan,
    is_dropship boolean,
    kirim_desa t_alamat_panjang,
    kirim_kabupaten t_alamat_panjang
);


ALTER TABLE t_jual_produk OWNER TO postgres;

--
-- Name: COLUMN t_jual_produk.is_sdac; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN t_jual_produk.is_sdac IS 'Sama dengan alamat customer';


--
-- Name: t_jual_produk_jual_produk_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_jual_produk_jual_produk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_jual_produk_jual_produk_id_seq OWNER TO postgres;

--
-- Name: t_kasbon; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_kasbon (
    kasbon_id t_guid NOT NULL,
    karyawan_id t_guid,
    pengguna_id t_guid,
    nota t_nota,
    tanggal date,
    nominal t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    total_pelunasan t_harga DEFAULT 0
);


ALTER TABLE t_kasbon OWNER TO postgres;

--
-- Name: t_kasbon_kasbon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_kasbon_kasbon_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_kasbon_kasbon_id_seq OWNER TO postgres;

--
-- Name: t_logs; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_logs (
    log_id bigint DEFAULT nextval(('public.t_logs_log_id_seq'::text)::regclass) NOT NULL,
    level character varying(10),
    class_name character varying(200),
    method_name character varying(100),
    message character varying(100),
    new_value character varying(5000),
    old_value character varying(5000),
    exception character varying(5000),
    created_by character varying(50),
    log_date timestamp(0) without time zone DEFAULT now()
);


ALTER TABLE t_logs OWNER TO postgres;

--
-- Name: t_logs_log_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_logs_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_logs_log_id_seq OWNER TO postgres;

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
-- Name: t_pembayaran_kasbon; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_pembayaran_kasbon (
    pembayaran_kasbon_id t_guid NOT NULL,
    kasbon_id t_guid,
    gaji_karyawan_id t_guid,
    tanggal date,
    nominal t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now(),
    nota t_nota,
    pengguna_id t_guid
);


ALTER TABLE t_pembayaran_kasbon OWNER TO postgres;

--
-- Name: t_pembayaran_kasbon_pembayaran_kasbon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_pembayaran_kasbon_pembayaran_kasbon_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_pembayaran_kasbon_pembayaran_kasbon_id_seq OWNER TO postgres;

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
-- Name: t_pembayaran_piutang_produk_pembayaran_piutang_produk_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_pembayaran_piutang_produk_pembayaran_piutang_produk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_pembayaran_piutang_produk_pembayaran_piutang_produk_id_seq OWNER TO postgres;

--
-- Name: t_pengeluaran_biaya; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE t_pengeluaran_biaya (
    pengeluaran_id t_guid NOT NULL,
    pengguna_id t_guid,
    nota t_nota,
    tanggal date,
    total t_harga,
    keterangan t_keterangan,
    tanggal_sistem timestamp without time zone DEFAULT now()
);


ALTER TABLE t_pengeluaran_biaya OWNER TO postgres;

--
-- Name: t_pengeluaran_biaya_pengeluaran_biaya_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_pengeluaran_biaya_pengeluaran_biaya_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_pengeluaran_biaya_pengeluaran_biaya_id_seq OWNER TO postgres;

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
-- Name: t_retur_jual_produk_retur_jual_produk_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE t_retur_jual_produk_retur_jual_produk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE t_retur_jual_produk_retur_jual_produk_id_seq OWNER TO postgres;

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
-- Name: m_database_version_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_database_version
    ADD CONSTRAINT m_database_version_pkey PRIMARY KEY (version_number);


--
-- Name: m_footer_nota_mini_pos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_footer_nota_mini_pos
    ADD CONSTRAINT m_footer_nota_mini_pos_pkey PRIMARY KEY (footer_nota_id);


--
-- Name: m_golongan_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_golongan
    ADD CONSTRAINT m_golongan_pkey PRIMARY KEY (golongan_id);


--
-- Name: m_harga_grosir_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_harga_grosir
    ADD CONSTRAINT m_harga_grosir_pkey PRIMARY KEY (harga_grosir_id);


--
-- Name: m_header_nota_mini_pos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_header_nota_mini_pos
    ADD CONSTRAINT m_header_nota_mini_pos_pkey PRIMARY KEY (header_nota_id);


--
-- Name: m_header_nota_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_header_nota
    ADD CONSTRAINT m_header_nota_pkey PRIMARY KEY (header_nota_id);


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
-- Name: m_kabupaten_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_kabupaten
    ADD CONSTRAINT m_kabupaten_pkey PRIMARY KEY (kabupaten_id);


--
-- Name: m_karyawan_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_karyawan
    ADD CONSTRAINT m_karyawan_pkey PRIMARY KEY (karyawan_id);


--
-- Name: m_label_nota_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_label_nota
    ADD CONSTRAINT m_label_nota_pkey PRIMARY KEY (label_nota_id);


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
-- Name: m_provinsi_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY m_provinsi
    ADD CONSTRAINT m_provinsi_pkey PRIMARY KEY (provinsi_id);


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

ALTER TABLE ONLY t_item_pengeluaran_biaya
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
-- Name: t_kasbon_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_kasbon
    ADD CONSTRAINT t_kasbon_pkey PRIMARY KEY (kasbon_id);


--
-- Name: t_logs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_logs
    ADD CONSTRAINT t_logs_pkey PRIMARY KEY (log_id);


--
-- Name: t_mesin_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_mesin
    ADD CONSTRAINT t_mesin_pkey PRIMARY KEY (mesin_id);


--
-- Name: t_pembayaran_bon_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY t_pembayaran_kasbon
    ADD CONSTRAINT t_pembayaran_bon_pkey PRIMARY KEY (pembayaran_kasbon_id);


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

ALTER TABLE ONLY t_pengeluaran_biaya
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
-- Name: m_customer_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX m_customer_idx ON m_customer USING btree (nama_customer);


--
-- Name: m_produk_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX m_produk_idx ON m_produk USING btree (nama_produk);


--
-- Name: m_produk_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX m_produk_idx1 ON m_produk USING btree (kode_produk);


--
-- Name: m_supplier_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX m_supplier_idx ON m_supplier USING btree (nama_supplier);


--
-- Name: t_beli_produk_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_beli_produk_idx ON t_beli_produk USING btree (tanggal);


--
-- Name: t_beli_produk_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_beli_produk_idx1 ON t_beli_produk USING btree (nota);


--
-- Name: t_beli_produk_idx2; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_beli_produk_idx2 ON t_beli_produk USING btree (tanggal_tempo);


--
-- Name: t_gaji_karyawan_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_gaji_karyawan_idx ON t_gaji_karyawan USING btree (bulan, tahun);


--
-- Name: t_gaji_karyawan_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_gaji_karyawan_idx1 ON t_gaji_karyawan USING btree (tanggal);


--
-- Name: t_gaji_karyawan_idx2; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_gaji_karyawan_idx2 ON t_gaji_karyawan USING btree (nota);


--
-- Name: t_jual_produk_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_jual_produk_idx ON t_jual_produk USING btree (nota);


--
-- Name: t_jual_produk_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_jual_produk_idx1 ON t_jual_produk USING btree (tanggal);


--
-- Name: t_jual_produk_idx2; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_jual_produk_idx2 ON t_jual_produk USING btree (tanggal_tempo);


--
-- Name: t_kasbon_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_kasbon_idx ON t_kasbon USING btree (tanggal);


--
-- Name: t_kasbon_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_kasbon_idx1 ON t_kasbon USING btree (nota);


--
-- Name: t_pembayaran_hutang_produk_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_pembayaran_hutang_produk_idx ON t_pembayaran_hutang_produk USING btree (tanggal);


--
-- Name: t_pembayaran_hutang_produk_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_pembayaran_hutang_produk_idx1 ON t_pembayaran_hutang_produk USING btree (nota);


--
-- Name: t_pembayaran_kasbon_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_pembayaran_kasbon_idx ON t_pembayaran_kasbon USING btree (tanggal);


--
-- Name: t_pembayaran_kasbon_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_pembayaran_kasbon_idx1 ON t_pembayaran_kasbon USING btree (nota);


--
-- Name: t_pembayaran_piutang_produk_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_pembayaran_piutang_produk_idx ON t_pembayaran_piutang_produk USING btree (tanggal);


--
-- Name: t_pembayaran_piutang_produk_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_pembayaran_piutang_produk_idx1 ON t_pembayaran_piutang_produk USING btree (nota);


--
-- Name: t_pengeluaran_biaya_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_pengeluaran_biaya_idx ON t_pengeluaran_biaya USING btree (tanggal);


--
-- Name: t_pengeluaran_biaya_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_pengeluaran_biaya_idx1 ON t_pengeluaran_biaya USING btree (nota);


--
-- Name: t_penyesuaian_stok_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_penyesuaian_stok_idx ON t_penyesuaian_stok USING btree (tanggal);


--
-- Name: t_retur_beli_produk_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_retur_beli_produk_idx ON t_retur_beli_produk USING btree (tanggal);


--
-- Name: t_retur_beli_produk_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_retur_beli_produk_idx1 ON t_retur_beli_produk USING btree (nota);


--
-- Name: t_retur_jual_produk_idx; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_retur_jual_produk_idx ON t_retur_jual_produk USING btree (tanggal);


--
-- Name: t_retur_jual_produk_idx1; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX t_retur_jual_produk_idx1 ON t_retur_jual_produk USING btree (nota);


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

CREATE TRIGGER tr_update_pelunasan_kasbon_aiud AFTER INSERT OR DELETE OR UPDATE ON t_pembayaran_kasbon FOR EACH ROW EXECUTE PROCEDURE f_update_pelunasan_kasbon();


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

CREATE TRIGGER tr_update_total_kasbon_karyawan AFTER INSERT OR DELETE OR UPDATE ON t_kasbon FOR EACH ROW EXECUTE PROCEDURE f_update_total_kasbon_karyawan();


--
-- Name: tr_update_total_pengeluaran_aiud; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_update_total_pengeluaran_aiud AFTER INSERT OR DELETE OR UPDATE ON t_item_pengeluaran_biaya FOR EACH ROW EXECUTE PROCEDURE f_update_total_pengeluaran();


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
-- Name: m_harga_grosir_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY m_harga_grosir
    ADD CONSTRAINT m_harga_grosir_fk FOREIGN KEY (produk_id) REFERENCES m_produk(produk_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: m_item_menu_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY m_item_menu
    ADD CONSTRAINT m_item_menu_fk FOREIGN KEY (menu_id) REFERENCES m_menu(menu_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: m_kabupaten_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY m_kabupaten
    ADD CONSTRAINT m_kabupaten_fk FOREIGN KEY (provinsi_id) REFERENCES m_provinsi(provinsi_id) ON UPDATE CASCADE ON DELETE CASCADE;


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

ALTER TABLE ONLY t_kasbon
    ADD CONSTRAINT t_bon_fk FOREIGN KEY (karyawan_id) REFERENCES m_karyawan(karyawan_id) ON UPDATE CASCADE;


--
-- Name: t_bon_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_kasbon
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

ALTER TABLE ONLY t_item_pengeluaran_biaya
    ADD CONSTRAINT t_item_pengeluaran_fk FOREIGN KEY (pengeluaran_id) REFERENCES t_pengeluaran_biaya(pengeluaran_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: t_item_pengeluaran_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_pengeluaran_biaya
    ADD CONSTRAINT t_item_pengeluaran_fk1 FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


--
-- Name: t_item_pengeluaran_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_item_pengeluaran_biaya
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
-- Name: t_pembayaran_bon_fk1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pembayaran_kasbon
    ADD CONSTRAINT t_pembayaran_bon_fk1 FOREIGN KEY (gaji_karyawan_id) REFERENCES t_gaji_karyawan(gaji_karyawan_id) ON UPDATE CASCADE;


--
-- Name: t_pembayaran_bon_fk2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pembayaran_kasbon
    ADD CONSTRAINT t_pembayaran_bon_fk2 FOREIGN KEY (kasbon_id) REFERENCES t_kasbon(kasbon_id) ON UPDATE CASCADE;


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
-- Name: t_pembayaran_kasbon_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY t_pembayaran_kasbon
    ADD CONSTRAINT t_pembayaran_kasbon_fk FOREIGN KEY (pengguna_id) REFERENCES m_pengguna(pengguna_id) ON UPDATE CASCADE;


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

ALTER TABLE ONLY t_pengeluaran_biaya
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

