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
-- Data for Name: m_pengguna; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_pengguna (pengguna_id, role_id, nama_pengguna, pass_pengguna, is_active, status_user) FROM stdin;
00b5acfa-b533-454b-8dfd-e7881edd180f	11dc1faf-2c66-4525-932d-a90e24da8987	admin	74521f341a6473f2bea7fa0ef052e7a8	t	2
ef8860ce-4108-4ea9-8030-9977232bac12	42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	andi	b79f6fe35c28cbb72373b30f726141c7	t	2
\.


--
-- PostgreSQL database dump complete
--

