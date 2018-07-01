using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Service
{
    internal sealed class HttpPaymentService : HttpServiceBase<PaymentRequest, HttpResponseMessage>
    {
        private readonly IHttpWrapper httpWrapper;

        public HttpPaymentService(Credential credential, IHttpWrapper httpWrapper, string url) : base(credential, url)
        {
            this.Credentials = credential;
            this.httpWrapper = httpWrapper;
        }

        public override HttpResponseMessage Execute(PaymentRequest request)
        {            
            return httpWrapper.Put(this.Credentials, this.Header, this.Body, this.Url);
        }
    }
}
