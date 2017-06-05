using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using System.Net;
using OpenRetail.WebAPI.Models;

namespace OpenRetail.WebAPI.Controllers
{
    public class BaseApiController : ApiController
    {
        protected ResponsePackage GenerateOutput(HttpStatusCode httpStatusCode, object results)
        {
            var output = new ResponsePackage
            {
                Status = new Status
                {
                    Code = Convert.ToInt32(httpStatusCode),
                    Description = httpStatusCode.ToString()
                },
                Results = results
            };

            return output;
        }
    }
}