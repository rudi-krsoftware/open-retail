/**
 * Copyright (C) 2017 Kamarudin (http://coding4ever.net/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * The latest version of this file can be found at https://github.com/rudi-krsoftware/open-retail
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace OpenRetail.Model
{        
	[Table("m_menu")]
    public class MenuAplikasi
    {
		[ExplicitKey]
		[Display(Name = "menu_id")]		
		public string menu_id { get; set; }
		
		[Display(Name = "nama_menu")]
		public string nama_menu { get; set; }
		
		[Display(Name = "judul_menu")]
		public string judul_menu { get; set; }
		
		[Display(Name = "parent_id")]
		public string parent_id { get; set; }
		
		[Display(Name = "order_number")]
		public int order_number { get; set; }
		
		[Display(Name = "is_active")]
		public bool is_active { get; set; }
		
		[Display(Name = "nama_form")]
		public string nama_form { get; set; }
		
		[Display(Name = "is_enabled")]
		public bool is_enabled { get; set; }
	}    
}
