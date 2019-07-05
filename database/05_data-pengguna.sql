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

COPY m_pengguna (pengguna_id, role_id, nama_pengguna, pass_pengguna, is_active, status_user, email) FROM stdin;
577b035d-64d5-427c-80c5-79c0fe1ae074	c58ee40a-5ae2-4067-b6ad-8cae9c65913c	rudi	b79f6fe35c28cbb72373b30f726141c7	t	2	\N
98281a7b-4d74-4e85-a215-0f8740795588	42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a	kasir	b79f6fe35c28cbb72373b30f726141c7	t	2	\N
50c6d67c-d9dd-462a-a118-75ed0b185b14	11dc1faf-2c66-4525-932d-a90e24da8987	aaa	b79f6fe35c28cbb72373b30f726141c7	t	2	admin@gmail.com
00b5acfa-b533-454b-8dfd-e7881edd180f	11dc1faf-2c66-4525-932d-a90e24da8987	admin	74521f341a6473f2bea7fa0ef052e7a8	t	2	\N
72ab191f-8b04-4b16-9931-930ef70b670c	11dc1faf-2c66-4525-932d-a90e24da8987	xyz	a03180fabf135452f17a7e81e33860fe	t	2	\N
\.


--
-- PostgreSQL database dump complete
--

