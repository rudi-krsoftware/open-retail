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

using RestSharp;
using Newtonsoft.Json;

namespace OpenRetail.Helper
{
    public class RestSharpHelper<T>
        where T : new()
    {
        /// <summary>
        /// Method untuk request layanan web api via method GET
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="api"></param>
        /// <returns></returns>
        public static IRestResponse<T> GetRequest(string baseUrl, string api)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(api, Method.GET);
            var response = client.Execute<T>(request);

            return response;
        }

        /// <summary>
        /// Method untuk request layanan web api via method POST
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="api"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T PostRequest(string baseUrl, string api, object obj)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(api, Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(obj);

            var response = client.Execute(request);
            var content = JsonConvert.DeserializeObject<T>(response.Content);

            return content;
        }

        /// <summary>
        /// Method untuk request layanan web api via method POST
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="api"></param>
        /// <param name="json">objek json dalam format string</param>
        /// <returns></returns>
        public static T PostRequest(string baseUrl, string api, string json)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(api, Method.POST);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);
            var content = JsonConvert.DeserializeObject<T>(response.Content);

            return content;
        }
    }
}
