using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLabs.Cielo.SDK.Util;
using System.Net.Http;

namespace RLabs.Cielo.SDK.Service
{
    internal sealed class HttpAuthorizationService : HttpServiceBase<AuthorizationRequest, HttpResponseMessage>
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IHttpWrapper httpWrapper;

        public HttpAuthorizationService(Credential credential, IHttpWrapper httpWrapper, string url) : base(credential, url)
        {
            Url = url;
            Credentials = credential;
            this.httpWrapper = httpWrapper;
        }

        public override HttpResponseMessage Execute(AuthorizationRequest request)
        {            
            return httpWrapper.Post(this.Credentials, this.Header, this.Body, this.Url);                        
        }
    }
}

