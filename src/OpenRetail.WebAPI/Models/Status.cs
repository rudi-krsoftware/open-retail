using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenRetail.WebAPI.Models
{
    public class Status
    {
        public Status()
        {
            Errors = new List<string>();
        }

        public int Code { get; set; }
        public string Description { get; set; }
        public List<string> Errors { get; set; }
    }
}