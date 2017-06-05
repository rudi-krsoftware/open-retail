using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OpenRetail.WebAPI.Controllers.Helper
{
    public class HttpActionResultHelper : IHttpActionResult
    {
        private HttpRequestMessage _request;
        private HttpStatusCode _statusCode;
        private object _message;

        public HttpActionResultHelper(HttpRequestMessage request, HttpStatusCode statusCode, object message)
        {
            this._request = request;
            this._statusCode = statusCode;
            this._message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_request.CreateResponse(_statusCode, _message));
        }
    }
}