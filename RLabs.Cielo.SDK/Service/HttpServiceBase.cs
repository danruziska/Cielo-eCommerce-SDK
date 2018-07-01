using RLabs.Cielo.SDK.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Service
{
    internal abstract class HttpServiceBase<TRequest, TResponse> : IService<TRequest, TResponse>
    {
        public string Url { get; set; }
        public Credential Credentials { get; set; }
        public string Body { get; set; }
        public Header Header { get; private set; }

        protected HttpServiceBase(Credential credential, string url)
        {
            this.Credentials = credential;
            this.Url = url;
        }

        public abstract TResponse Execute(TRequest request);                
    }
}
