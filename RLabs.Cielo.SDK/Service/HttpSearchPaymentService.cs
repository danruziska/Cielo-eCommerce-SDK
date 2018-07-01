using Newtonsoft.Json.Linq;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using NLog;

namespace RLabs.Cielo.SDK.Service
{
    internal sealed class HttpSearchPaymentService : HttpServiceBase<PaymentRequest, HttpResponseMessage>
    {
        private readonly ILogger logger;
        private readonly IHttpWrapper httpWrapper;

        public HttpSearchPaymentService(ILogger logger, Credential credentials, IHttpWrapper httpWrapper, string url): base(credentials, url)
        {
            this.Url = url;
            this.Credentials = credentials;
            this.httpWrapper = httpWrapper;
            this.logger = logger;
        }

        public override HttpResponseMessage Execute(PaymentRequest request)
        {
            logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", request.PaymentId }, { "Message", "Iniciando a execução da transação de consulta" }, { "Body", this.Body }, { "Url", this.Url } });            
            return httpWrapper.Get(this.Credentials, this.Header, this.Body, this.Url);            
        }
    }
}
