using NLog;
using RLabs.Cielo.SDK.Formatters;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLabs.Cielo.SDK.Util;
using Newtonsoft.Json;
using System.Net.Http;

namespace RLabs.Cielo.SDK.Transaction
{
    internal sealed class SearchPaymentTransaction : IBaseTransaction<PaymentRequest, BaseResponse>
    {                
        private readonly IFormatter<PaymentRequest, BaseResponse, string> formatter;
        private readonly HttpServiceBase<PaymentRequest, HttpResponseMessage> searchPaymentService;
        private readonly ILogger logger;

        public SearchPaymentTransaction(IFormatter<PaymentRequest, BaseResponse, string> formatter, HttpServiceBase<PaymentRequest, HttpResponseMessage> searchPaymentService, ILogger logger)
        {
            this.formatter = formatter;
            this.searchPaymentService = searchPaymentService;
            this.logger = logger;
        }

        public BaseResponse Execute(PaymentRequest searchPaymentRequestData)
        {
            BaseResponse searchPaymentResponse = null;
            try
            {
                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", searchPaymentRequestData.PaymentId }, { "Message", "Iniciando a execução da transação de busca" } });
                searchPaymentService.Url = string.Format(searchPaymentService.Url, searchPaymentRequestData.PaymentId);
                HttpResponseMessage serviceResponse = searchPaymentService.Execute(searchPaymentRequestData);
                string responseBody = serviceResponse.Content.ReadAsStringAsync().Result;
                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", searchPaymentRequestData.PaymentId }, { "Message", "Transação executada com sucesso" }, { "Response", responseBody } });                

                if (!serviceResponse.IsSuccessStatusCode)
                    searchPaymentResponse = new BaseResponse() { ReturnCode = (int)serviceResponse.StatusCode };
                else
                    searchPaymentResponse = formatter.ParseMessageToResponse(responseBody);

                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", searchPaymentRequestData.PaymentId }, { "Message", "Mensagem de resposta formatada" }, { "Response", JsonConvert.SerializeObject(searchPaymentResponse) } });
            }
            catch (Exception ex)
            {
                logger.ErrorWithMetadata(new Dictionary<string, string> { { "ExceptionType", ex.GetType().ToString() }, { "ErrorMessage", ex.Message }, { "StackTrace", ex.StackTrace } });
                searchPaymentResponse = new BaseResponse() { ReturnMessage = "Ocorreu um erro na chamada da transação de busca de pagamento. Verifique o log de erro para mais detalhes" };
            }

            return searchPaymentResponse;
        }
    }
}
