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
-- Data for Name: m_kabupaten; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY m_kabupaten (kabupaten_id, provinsi_id, tipe, nama_kabupaten, kode_pos) FROM stdin;
1	21	Kabupaten	Aceh Barat	23681
2	21	Kabupaten	Aceh Barat Daya	23764
3	21	Kabupaten	Aceh Besar	23951
4	21	Kabupaten	Aceh Jaya	23654
5	21	Kabupaten	Aceh Selatan	23719
6	21	Kabupaten	Aceh Singkil	24785
7	21	Kabupaten	Aceh Tamiang	24476
8	21	Kabupaten	Aceh Tengah	24511
9	21	Kabupaten	Aceh Tenggara	24611
10	21	Kabupaten	Aceh Timur	24454
11	21	Kabupaten	Aceh Utara	24382
12	32	Kabupaten	Agam	26411
13	23	Kabupaten	Alor	85811
14	19	Kota	Ambon	97222
15	34	Kabupaten	Asahan	21214
16	24	Kabupaten	Asmat	99777
17	1	Kabupaten	Badung	80351
18	13	Kabupaten	Balangan	71611
19	15	Kota	Balikpapan	76111
20	21	Kota	Banda Aceh	23238
21	18	Kota	Bandar Lampung	35139
22	9	Kabupaten	Bandung	40311
23	9	Kota	Bandung	40115
24	9	Kabupaten	Bandung Barat	40721
25	29	Kabupaten	Banggai	94711
26	29	Kabupaten	Banggai Kepulauan	94881
27	2	Kabupaten	Bangka	33212
28	2	Kabupaten	Bangka Barat	33315
29	2	Kabupaten	Bangka Selatan	33719
30	2	Kabupaten	Bangka Tengah	33613
31	11	Kabupaten	Bangkalan	69118
32	1	Kabupaten	Bangli	80619
33	13	Kabupaten	Banjar	70619
34	9	Kota	Banjar	46311
35	13	Kota	Banjarbaru	70712
36	13	Kota	Banjarmasin	70117
37	10	Kabupaten	Banjarnegara	53419
38	28	Kabupaten	Bantaeng	92411
39	5	Kabupaten	Bantul	55715
40	33	Kabupaten	Banyuasin	30911
41	10	Kabupaten	Banyumas	53114
42	11	Kabupaten	Banyuwangi	68416
43	13	Kabupaten	Barito Kuala	70511
44	14	Kabupaten	Barito Selatan	73711
45	14	Kabupaten	Barito Timur	73671
46	14	Kabupaten	Barito Utara	73881
47	28	Kabupaten	Barru	90719
48	17	Kota	Batam	29413
49	10	Kabupaten	Batang	51211
50	8	Kabupaten	Batang Hari	36613
51	11	Kota	Batu	65311
52	34	Kabupaten	Batu Bara	21655
53	30	Kota	Bau-Bau	93719
54	9	Kabupaten	Bekasi	17837
55	9	Kota	Bekasi	17121
56	2	Kabupaten	Belitung	33419
57	2	Kabupaten	Belitung Timur	33519
58	23	Kabupaten	Belu	85711
59	21	Kabupaten	Bener Meriah	24581
60	26	Kabupaten	Bengkalis	28719
61	12	Kabupaten	Bengkayang	79213
62	4	Kota	Bengkulu	38229
63	4	Kabupaten	Bengkulu Selatan	38519
64	4	Kabupaten	Bengkulu Tengah	38319
65	4	Kabupaten	Bengkulu Utara	38619
66	15	Kabupaten	Berau	77311
67	24	Kabupaten	Biak Numfor	98119
68	22	Kabupaten	Bima	84171
69	22	Kota	Bima	84139
70	34	Kota	Binjai	20712
71	17	Kabupaten	Bintan	29135
72	21	Kabupaten	Bireuen	24219
73	31	Kota	Bitung	95512
74	11	Kabupaten	Blitar	66171
75	11	Kota	Blitar	66124
76	10	Kabupaten	Blora	58219
77	7	Kabupaten	Boalemo	96319
78	9	Kabupaten	Bogor	16911
79	9	Kota	Bogor	16119
80	11	Kabupaten	Bojonegoro	62119
81	31	Kabupaten	Bolaang Mongondow (Bolmong)	95755
82	31	Kabupaten	Bolaang Mongondow Selatan	95774
83	31	Kabupaten	Bolaang Mongondow Timur	95783
84	31	Kabupaten	Bolaang Mongondow Utara	95765
85	30	Kabupaten	Bombana	93771
86	11	Kabupaten	Bondowoso	68219
87	28	Kabupaten	Bone	92713
88	7	Kabupaten	Bone Bolango	96511
89	15	Kota	Bontang	75313
90	24	Kabupaten	Boven Digoel	99662
91	10	Kabupaten	Boyolali	57312
92	10	Kabupaten	Brebes	52212
93	32	Kota	Bukittinggi	26115
94	1	Kabupaten	Buleleng	81111
95	28	Kabupaten	Bulukumba	92511
96	16	Kabupaten	Bulungan (Bulongan)	77211
97	8	Kabupaten	Bungo	37216
98	29	Kabupaten	Buol	94564
99	19	Kabupaten	Buru	97371
100	19	Kabupaten	Buru Selatan	97351
101	30	Kabupaten	Buton	93754
102	30	Kabupaten	Buton Utara	93745
103	9	Kabupaten	Ciamis	46211
104	9	Kabupaten	Cianjur	43217
105	10	Kabupaten	Cilacap	53211
106	3	Kota	Cilegon	42417
107	9	Kota	Cimahi	40512
108	9	Kabupaten	Cirebon	45611
109	9	Kota	Cirebon	45116
110	34	Kabupaten	Dairi	22211
111	24	Kabupaten	Deiyai (Deliyai)	98784
112	34	Kabupaten	Deli Serdang	20511
113	10	Kabupaten	Demak	59519
114	1	Kota	Denpasar	80227
115	9	Kota	Depok	16416
116	32	Kabupaten	Dharmasraya	27612
117	24	Kabupaten	Dogiyai	98866
118	22	Kabupaten	Dompu	84217
119	29	Kabupaten	Donggala	94341
120	26	Kota	Dumai	28811
121	33	Kabupaten	Empat Lawang	31811
122	23	Kabupaten	Ende	86351
123	28	Kabupaten	Enrekang	91719
124	25	Kabupaten	Fakfak	98651
125	23	Kabupaten	Flores Timur	86213
126	9	Kabupaten	Garut	44126
127	21	Kabupaten	Gayo Lues	24653
128	1	Kabupaten	Gianyar	80519
129	7	Kabupaten	Gorontalo	96218
130	7	Kota	Gorontalo	96115
131	7	Kabupaten	Gorontalo Utara	96611
132	28	Kabupaten	Gowa	92111
133	11	Kabupaten	Gresik	61115
134	10	Kabupaten	Grobogan	58111
135	5	Kabupaten	Gunung Kidul	55812
136	14	Kabupaten	Gunung Mas	74511
137	34	Kota	Gunungsitoli	22813
138	20	Kabupaten	Halmahera Barat	97757
139	20	Kabupaten	Halmahera Selatan	97911
140	20	Kabupaten	Halmahera Tengah	97853
141	20	Kabupaten	Halmahera Timur	97862
142	20	Kabupaten	Halmahera Utara	97762
143	13	Kabupaten	Hulu Sungai Selatan	71212
144	13	Kabupaten	Hulu Sungai Tengah	71313
145	13	Kabupaten	Hulu Sungai Utara	71419
146	34	Kabupaten	Humbang Hasundutan	22457
147	26	Kabupaten	Indragiri Hilir	29212
148	26	Kabupaten	Indragiri Hulu	29319
149	9	Kabupaten	Indramayu	45214
150	24	Kabupaten	Intan Jaya	98771
151	6	Kota	Jakarta Barat	11220
152	6	Kota	Jakarta Pusat	10540
153	6	Kota	Jakarta Selatan	12230
154	6	Kota	Jakarta Timur	13330
155	6	Kota	Jakarta Utara	14140
156	8	Kota	Jambi	36111
157	24	Kabupaten	Jayapura	99352
158	24	Kota	Jayapura	99114
159	24	Kabupaten	Jayawijaya	99511
160	11	Kabupaten	Jember	68113
161	1	Kabupaten	Jembrana	82251
162	28	Kabupaten	Jeneponto	92319
163	10	Kabupaten	Jepara	59419
164	11	Kabupaten	Jombang	61415
165	25	Kabupaten	Kaimana	98671
166	26	Kabupaten	Kampar	28411
167	14	Kabupaten	Kapuas	73583
168	12	Kabupaten	Kapuas Hulu	78719
169	10	Kabupaten	Karanganyar	57718
170	1	Kabupaten	Karangasem	80819
171	9	Kabupaten	Karawang	41311
172	17	Kabupaten	Karimun	29611
173	34	Kabupaten	Karo	22119
174	14	Kabupaten	Katingan	74411
175	4	Kabupaten	Kaur	38911
176	12	Kabupaten	Kayong Utara	78852
177	10	Kabupaten	Kebumen	54319
178	11	Kabupaten	Kediri	64184
179	11	Kota	Kediri	64125
180	24	Kabupaten	Keerom	99461
181	10	Kabupaten	Kendal	51314
182	30	Kota	Kendari	93126
183	4	Kabupaten	Kepahiang	39319
184	17	Kabupaten	Kepulauan Anambas	29991
185	19	Kabupaten	Kepulauan Aru	97681
186	32	Kabupaten	Kepulauan Mentawai	25771
187	26	Kabupaten	Kepulauan Meranti	28791
188	31	Kabupaten	Kepulauan Sangihe	95819
189	6	Kabupaten	Kepulauan Seribu	14550
190	31	Kabupaten	Kepulauan Siau Tagulandang Biaro (Sitaro)	95862
191	20	Kabupaten	Kepulauan Sula	97995
192	31	Kabupaten	Kepulauan Talaud	95885
193	24	Kabupaten	Kepulauan Yapen (Yapen Waropen)	98211
194	8	Kabupaten	Kerinci	37167
195	12	Kabupaten	Ketapang	78874
196	10	Kabupaten	Klaten	57411
197	1	Kabupaten	Klungkung	80719
198	30	Kabupaten	Kolaka	93511
199	30	Kabupaten	Kolaka Utara	93911
200	30	Kabupaten	Konawe	93411
201	30	Kabupaten	Konawe Selatan	93811
202	30	Kabupaten	Konawe Utara	93311
203	13	Kabupaten	Kotabaru	72119
204	31	Kota	Kotamobagu	95711
205	14	Kabupaten	Kotawaringin Barat	74119
206	14	Kabupaten	Kotawaringin Timur	74364
207	26	Kabupaten	Kuantan Singingi	29519
208	12	Kabupaten	Kubu Raya	78311
209	10	Kabupaten	Kudus	59311
210	5	Kabupaten	Kulon Progo	55611
211	9	Kabupaten	Kuningan	45511
212	23	Kabupaten	Kupang	85362
213	23	Kota	Kupang	85119
214	15	Kabupaten	Kutai Barat	75711
215	15	Kabupaten	Kutai Kartanegara	75511
216	15	Kabupaten	Kutai Timur	75611
217	34	Kabupaten	Labuhan Batu	21412
218	34	Kabupaten	Labuhan Batu Selatan	21511
219	34	Kabupaten	Labuhan Batu Utara	21711
220	33	Kabupaten	Lahat	31419
221	14	Kabupaten	Lamandau	74611
222	11	Kabupaten	Lamongan	64125
223	18	Kabupaten	Lampung Barat	34814
224	18	Kabupaten	Lampung Selatan	35511
225	18	Kabupaten	Lampung Tengah	34212
226	18	Kabupaten	Lampung Timur	34319
227	18	Kabupaten	Lampung Utara	34516
228	12	Kabupaten	Landak	78319
229	34	Kabupaten	Langkat	20811
230	21	Kota	Langsa	24412
231	24	Kabupaten	Lanny Jaya	99531
232	3	Kabupaten	Lebak	42319
233	4	Kabupaten	Lebong	39264
234	23	Kabupaten	Lembata	86611
235	21	Kota	Lhokseumawe	24352
236	32	Kabupaten	Lima Puluh Koto/Kota	26671
237	17	Kabupaten	Lingga	29811
238	22	Kabupaten	Lombok Barat	83311
239	22	Kabupaten	Lombok Tengah	83511
240	22	Kabupaten	Lombok Timur	83612
241	22	Kabupaten	Lombok Utara	83711
242	33	Kota	Lubuk Linggau	31614
243	11	Kabupaten	Lumajang	67319
244	28	Kabupaten	Luwu	91994
245	28	Kabupaten	Luwu Timur	92981
246	28	Kabupaten	Luwu Utara	92911
247	11	Kabupaten	Madiun	63153
248	11	Kota	Madiun	63122
249	10	Kabupaten	Magelang	56519
250	10	Kota	Magelang	56133
251	11	Kabupaten	Magetan	63314
252	9	Kabupaten	Majalengka	45412
253	27	Kabupaten	Majene	91411
254	28	Kota	Makassar	90111
255	11	Kabupaten	Malang	65163
256	11	Kota	Malang	65112
257	16	Kabupaten	Malinau	77511
258	19	Kabupaten	Maluku Barat Daya	97451
259	19	Kabupaten	Maluku Tengah	97513
260	19	Kabupaten	Maluku Tenggara	97651
261	19	Kabupaten	Maluku Tenggara Barat	97465
262	27	Kabupaten	Mamasa	91362
263	24	Kabupaten	Mamberamo Raya	99381
264	24	Kabupaten	Mamberamo Tengah	99553
265	27	Kabupaten	Mamuju	91519
266	27	Kabupaten	Mamuju Utara	91571
267	31	Kota	Manado	95247
268	34	Kabupaten	Mandailing Natal	22916
269	23	Kabupaten	Manggarai	86551
270	23	Kabupaten	Manggarai Barat	86711
271	23	Kabupaten	Manggarai Timur	86811
272	25	Kabupaten	Manokwari	98311
273	25	Kabupaten	Manokwari Selatan	98355
274	24	Kabupaten	Mappi	99853
275	28	Kabupaten	Maros	90511
276	22	Kota	Mataram	83131
277	25	Kabupaten	Maybrat	98051
278	34	Kota	Medan	20228
279	12	Kabupaten	Melawi	78619
280	8	Kabupaten	Merangin	37319
281	24	Kabupaten	Merauke	99613
282	18	Kabupaten	Mesuji	34911
283	18	Kota	Metro	34111
284	24	Kabupaten	Mimika	99962
285	31	Kabupaten	Minahasa	95614
286	31	Kabupaten	Minahasa Selatan	95914
287	31	Kabupaten	Minahasa Tenggara	95995
288	31	Kabupaten	Minahasa Utara	95316
289	11	Kabupaten	Mojokerto	61382
290	11	Kota	Mojokerto	61316
291	29	Kabupaten	Morowali	94911
292	33	Kabupaten	Muara Enim	31315
293	8	Kabupaten	Muaro Jambi	36311
294	4	Kabupaten	Muko Muko	38715
295	30	Kabupaten	Muna	93611
296	14	Kabupaten	Murung Raya	73911
297	33	Kabupaten	Musi Banyuasin	30719
298	33	Kabupaten	Musi Rawas	31661
299	24	Kabupaten	Nabire	98816
300	21	Kabupaten	Nagan Raya	23674
301	23	Kabupaten	Nagekeo	86911
302	17	Kabupaten	Natuna	29711
303	24	Kabupaten	Nduga	99541
304	23	Kabupaten	Ngada	86413
305	11	Kabupaten	Nganjuk	64414
306	11	Kabupaten	Ngawi	63219
307	34	Kabupaten	Nias	22876
308	34	Kabupaten	Nias Barat	22895
309	34	Kabupaten	Nias Selatan	22865
310	34	Kabupaten	Nias Utara	22856
311	16	Kabupaten	Nunukan	77421
312	33	Kabupaten	Ogan Ilir	30811
313	33	Kabupaten	Ogan Komering Ilir	30618
314	33	Kabupaten	Ogan Komering Ulu	32112
315	33	Kabupaten	Ogan Komering Ulu Selatan	32211
316	33	Kabupaten	Ogan Komering Ulu Timur	32312
317	11	Kabupaten	Pacitan	63512
318	32	Kota	Padang	25112
319	34	Kabupaten	Padang Lawas	22763
320	34	Kabupaten	Padang Lawas Utara	22753
321	32	Kota	Padang Panjang	27122
322	32	Kabupaten	Padang Pariaman	25583
323	34	Kota	Padang Sidempuan	22727
324	33	Kota	Pagar Alam	31512
325	34	Kabupaten	Pakpak Bharat	22272
326	14	Kota	Palangka Raya	73112
327	33	Kota	Palembang	31512
328	28	Kota	Palopo	91911
329	29	Kota	Palu	94111
330	11	Kabupaten	Pamekasan	69319
331	3	Kabupaten	Pandeglang	42212
332	9	Kabupaten	Pangandaran	46511
333	28	Kabupaten	Pangkajene Kepulauan	90611
334	2	Kota	Pangkal Pinang	33115
335	24	Kabupaten	Paniai	98765
336	28	Kota	Parepare	91123
337	32	Kota	Pariaman	25511
338	29	Kabupaten	Parigi Moutong	94411
339	32	Kabupaten	Pasaman	26318
340	32	Kabupaten	Pasaman Barat	26511
341	15	Kabupaten	Paser	76211
342	11	Kabupaten	Pasuruan	67153
343	11	Kota	Pasuruan	67118
344	10	Kabupaten	Pati	59114
345	32	Kota	Payakumbuh	26213
346	25	Kabupaten	Pegunungan Arfak	98354
347	24	Kabupaten	Pegunungan Bintang	99573
348	10	Kabupaten	Pekalongan	51161
349	10	Kota	Pekalongan	51122
350	26	Kota	Pekanbaru	28112
351	26	Kabupaten	Pelalawan	28311
352	10	Kabupaten	Pemalang	52319
353	34	Kota	Pematang Siantar	21126
354	15	Kabupaten	Penajam Paser Utara	76311
355	18	Kabupaten	Pesawaran	35312
356	18	Kabupaten	Pesisir Barat	35974
357	32	Kabupaten	Pesisir Selatan	25611
358	21	Kabupaten	Pidie	24116
359	21	Kabupaten	Pidie Jaya	24186
360	28	Kabupaten	Pinrang	91251
361	7	Kabupaten	Pohuwato	96419
362	27	Kabupaten	Polewali Mandar	91311
363	11	Kabupaten	Ponorogo	63411
364	12	Kabupaten	Pontianak	78971
365	12	Kota	Pontianak	78112
366	29	Kabupaten	Poso	94615
367	33	Kota	Prabumulih	31121
368	18	Kabupaten	Pringsewu	35719
369	11	Kabupaten	Probolinggo	67282
370	11	Kota	Probolinggo	67215
371	14	Kabupaten	Pulang Pisau	74811
372	20	Kabupaten	Pulau Morotai	97771
373	24	Kabupaten	Puncak	98981
374	24	Kabupaten	Puncak Jaya	98979
375	10	Kabupaten	Purbalingga	53312
376	9	Kabupaten	Purwakarta	41119
377	10	Kabupaten	Purworejo	54111
378	25	Kabupaten	Raja Ampat	98489
379	4	Kabupaten	Rejang Lebong	39112
380	10	Kabupaten	Rembang	59219
381	26	Kabupaten	Rokan Hilir	28992
382	26	Kabupaten	Rokan Hulu	28511
383	23	Kabupaten	Rote Ndao	85982
384	21	Kota	Sabang	23512
385	23	Kabupaten	Sabu Raijua	85391
386	10	Kota	Salatiga	50711
387	15	Kota	Samarinda	75133
388	12	Kabupaten	Sambas	79453
389	34	Kabupaten	Samosir	22392
390	11	Kabupaten	Sampang	69219
391	12	Kabupaten	Sanggau	78557
392	24	Kabupaten	Sarmi	99373
393	8	Kabupaten	Sarolangun	37419
394	32	Kota	Sawah Lunto	27416
395	12	Kabupaten	Sekadau	79583
396	28	Kabupaten	Selayar (Kepulauan Selayar)	92812
397	4	Kabupaten	Seluma	38811
398	10	Kabupaten	Semarang	50511
399	10	Kota	Semarang	50135
400	19	Kabupaten	Seram Bagian Barat	97561
401	19	Kabupaten	Seram Bagian Timur	97581
402	3	Kabupaten	Serang	42182
403	3	Kota	Serang	42111
404	34	Kabupaten	Serdang Bedagai	20915
405	14	Kabupaten	Seruyan	74211
406	26	Kabupaten	Siak	28623
407	34	Kota	Sibolga	22522
408	28	Kabupaten	Sidenreng Rappang/Rapang	91613
409	11	Kabupaten	Sidoarjo	61219
410	29	Kabupaten	Sigi	94364
411	32	Kabupaten	Sijunjung (Sawah Lunto Sijunjung)	27511
412	23	Kabupaten	Sikka	86121
413	34	Kabupaten	Simalungun	21162
414	21	Kabupaten	Simeulue	23891
415	12	Kota	Singkawang	79117
416	28	Kabupaten	Sinjai	92615
417	12	Kabupaten	Sintang	78619
418	11	Kabupaten	Situbondo	68316
419	5	Kabupaten	Sleman	55513
420	32	Kabupaten	Solok	27365
421	32	Kota	Solok	27315
422	32	Kabupaten	Solok Selatan	27779
423	28	Kabupaten	Soppeng	90812
424	25	Kabupaten	Sorong	98431
425	25	Kota	Sorong	98411
426	25	Kabupaten	Sorong Selatan	98454
427	10	Kabupaten	Sragen	57211
428	9	Kabupaten	Subang	41215
429	21	Kota	Subulussalam	24882
430	9	Kabupaten	Sukabumi	43311
431	9	Kota	Sukabumi	43114
432	14	Kabupaten	Sukamara	74712
433	10	Kabupaten	Sukoharjo	57514
434	23	Kabupaten	Sumba Barat	87219
435	23	Kabupaten	Sumba Barat Daya	87453
436	23	Kabupaten	Sumba Tengah	87358
437	23	Kabupaten	Sumba Timur	87112
438	22	Kabupaten	Sumbawa	84315
439	22	Kabupaten	Sumbawa Barat	84419
440	9	Kabupaten	Sumedang	45326
441	11	Kabupaten	Sumenep	69413
442	8	Kota	Sungaipenuh	37113
443	24	Kabupaten	Supiori	98164
444	11	Kota	Surabaya	60119
445	10	Kota	Surakarta (Solo)	57113
446	13	Kabupaten	Tabalong	71513
447	1	Kabupaten	Tabanan	82119
448	28	Kabupaten	Takalar	92212
449	25	Kabupaten	Tambrauw	98475
450	16	Kabupaten	Tana Tidung	77611
451	28	Kabupaten	Tana Toraja	91819
452	13	Kabupaten	Tanah Bumbu	72211
453	32	Kabupaten	Tanah Datar	27211
454	13	Kabupaten	Tanah Laut	70811
455	3	Kabupaten	Tangerang	15914
456	3	Kota	Tangerang	15111
457	3	Kota	Tangerang Selatan	15332
458	18	Kabupaten	Tanggamus	35619
459	34	Kota	Tanjung Balai	21321
460	8	Kabupaten	Tanjung Jabung Barat	36513
461	8	Kabupaten	Tanjung Jabung Timur	36719
462	17	Kota	Tanjung Pinang	29111
463	34	Kabupaten	Tapanuli Selatan	22742
464	34	Kabupaten	Tapanuli Tengah	22611
465	34	Kabupaten	Tapanuli Utara	22414
466	13	Kabupaten	Tapin	71119
467	16	Kota	Tarakan	77114
468	9	Kabupaten	Tasikmalaya	46411
469	9	Kota	Tasikmalaya	46116
470	34	Kota	Tebing Tinggi	20632
471	8	Kabupaten	Tebo	37519
472	10	Kabupaten	Tegal	52419
473	10	Kota	Tegal	52114
474	25	Kabupaten	Teluk Bintuni	98551
475	25	Kabupaten	Teluk Wondama	98591
476	10	Kabupaten	Temanggung	56212
477	20	Kota	Ternate	97714
478	20	Kota	Tidore Kepulauan	97815
479	23	Kabupaten	Timor Tengah Selatan	85562
480	23	Kabupaten	Timor Tengah Utara	85612
481	34	Kabupaten	Toba Samosir	22316
482	29	Kabupaten	Tojo Una-Una	94683
483	29	Kabupaten	Toli-Toli	94542
484	24	Kabupaten	Tolikara	99411
485	31	Kota	Tomohon	95416
486	28	Kabupaten	Toraja Utara	91831
487	11	Kabupaten	Trenggalek	66312
488	19	Kota	Tual	97612
489	11	Kabupaten	Tuban	62319
490	18	Kabupaten	Tulang Bawang	34613
491	18	Kabupaten	Tulang Bawang Barat	34419
492	11	Kabupaten	Tulungagung	66212
493	28	Kabupaten	Wajo	90911
494	30	Kabupaten	Wakatobi	93791
495	24	Kabupaten	Waropen	98269
496	18	Kabupaten	Way Kanan	34711
497	10	Kabupaten	Wonogiri	57619
498	10	Kabupaten	Wonosobo	56311
499	24	Kabupaten	Yahukimo	99041
500	24	Kabupaten	Yalimo	99481
501	5	Kota	Yogyakarta	55222
\.


--
-- PostgreSQL database dump complete
--

