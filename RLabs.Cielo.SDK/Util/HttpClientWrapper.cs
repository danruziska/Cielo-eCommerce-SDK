using RLabs.Cielo.SDK.Enum;
using RLabs.Cielo.SDK.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RLabs.Cielo.SDK.Util
{
    internal sealed class HttpClientWrapper : IHttpWrapper
    {
        private LogTools logTools;

        public HttpResponseMessage Post(Credential credentials, Header header, string body, string url)
        {
            logTools = new LogTools();
            logTools.Start(MethodBase.GetCurrentMethod().Name);
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                if(credentials != null) AddItemsToDefaultHeader(httpClient, credentials.CredentialsList);

                if(header != null) AddItemsToDefaultHeader(httpClient, header.Headerlist);

                var response = httpClient.PostAsync(url, content).Result;
                logTools.End();
                return response;
            }
        }

        public HttpResponseMessage Put(Credential credentials, Header header, string body, string url)
        {
            logTools = new LogTools();            
            logTools.Start(MethodBase.GetCurrentMethod().Name);
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                if (credentials != null) AddItemsToDefaultHeader(httpClient, credentials.CredentialsList);

                if (header != null) AddItemsToDefaultHeader(httpClient, header.Headerlist);

                var response = httpClient.PutAsync(url, content).Result;
                logTools.End();
                return response;
            }
        }

        public HttpResponseMessage Get(Credential credentials, Header header, string body, string url)
        {
            logTools = new LogTools();
            logTools.Start(MethodBase.GetCurrentMethod().Name);
            using (var httpClient = new HttpClient())
            {
                if (credentials != null) AddItemsToDefaultHeader(httpClient, credentials.CredentialsList);

                if (header != null) AddItemsToDefaultHeader(httpClient, header.Headerlist);

                var response = httpClient.GetAsync(url).Result;
                logTools.End();
                return response;
            }
        }

        private void AddItemsToDefaultHeader(HttpClient httpClient, IDictionary<string,string> headerList)
        {
            if(headerList != null)
            {
                foreach (var headerItem in headerList)
                {
                    httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                }
            }
        }
    }
}
