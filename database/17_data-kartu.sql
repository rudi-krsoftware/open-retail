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
-- Data for Name: m_kartu; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_kartu (kartu_id, nama_kartu, is_debit) FROM stdin;
23f39815-30ed-44a0-92ef-95bbebe86857	Debit BNI	t
4939cbfa-0eb2-4f43-89a8-58285573762f	Visa	f
eadb5ebe-aca8-44b3-aa28-34263507c8ad	Debit Mandiri	t
fbf224d1-d109-4280-a465-e3ff04494cc2	Mastercard	f
\.


--
-- PostgreSQL database dump complete
--

