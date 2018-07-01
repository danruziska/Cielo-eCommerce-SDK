using RLabs.Cielo.SDK.Formatters;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RLabs.Cielo.SDK.Util;
using NLog;
using RLabs.Cielo.SDK.Formatters.Json;

namespace RLabs.Cielo.SDK.Transaction
{
    internal sealed class VoidTransaction : IBaseTransaction<PaymentRequest, BaseResponse>
    {
        private readonly IFormatter<PaymentRequest, BaseResponse, string> formatter;
        private readonly ILogger logger;
        private readonly HttpServiceBase<PaymentRequest, HttpResponseMessage> voidService;

        public VoidTransaction(IFormatter<PaymentRequest, BaseResponse, string> formatter, HttpServiceBase<PaymentRequest, HttpResponseMessage> voidService, ILogger logger)
        {
            this.formatter = formatter;
            this.voidService = voidService;
            this.logger = logger;
        }

        public BaseResponse Execute(PaymentRequest voidRequestData)
        {
            BaseResponse voidResponse = null;
            try
            {
                string body = formatter.ParseRequestToMessage(voidRequestData);

                voidService.Url = string.Format(voidService.Url, voidRequestData.PaymentId, voidRequestData.Amount.ToString());
                voidService.Body = body;
                HttpResponseMessage serviceResponse = voidService.Execute(voidRequestData);
                string responseBody = serviceResponse.Content.ReadAsStringAsync().Result;

                if (!serviceResponse.IsSuccessStatusCode)
                {
                    voidResponse = HttpResponseFormatter.ParseHttpResponseMessageToObject(serviceResponse, logger);
                }
                else
                {
                    voidResponse = formatter.ParseMessageToResponse(responseBody);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorWithMetadata(new Dictionary<string, string> { { "ExceptionType", ex.GetType().ToString() }, { "ErrorMessage", ex.Message }, { "StackTrace", ex.StackTrace } });
                voidResponse = new BaseResponse() { ReturnMessage = "Ocorreu um erro na chamada da transação de cancelamento. Verifique o log de erro para mais detalhes" };
            }

            return voidResponse;
        }
    }
}
