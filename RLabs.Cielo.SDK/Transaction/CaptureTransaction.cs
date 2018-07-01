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
    internal sealed class CaptureTransaction : IBaseTransaction<PaymentRequest, BaseResponse>
    {                      
        private readonly IFormatter<PaymentRequest, BaseResponse, string> formatter;
        private readonly HttpServiceBase<PaymentRequest, HttpResponseMessage> captureService;
        private readonly ILogger logger;

        public CaptureTransaction(IFormatter<PaymentRequest, BaseResponse, string> formatter, HttpServiceBase<PaymentRequest, HttpResponseMessage> captureService, ILogger logger)
        {            
            this.formatter = formatter;
            this.captureService = captureService;
            this.logger = logger;
        }

        public BaseResponse Execute(PaymentRequest captureRequestData)
        {
            BaseResponse captureResponse = null;
            try
            {
                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", captureRequestData.PaymentId }, { "Message", "Iniciando a execução da transação de captura" } });
                string requestBody = formatter.ParseRequestToMessage(captureRequestData);

                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", captureRequestData.PaymentId }, { "Message", "Mensagem de requisição formatada" }, { "Body", requestBody } });
                captureService.Url = string.Format(captureService.Url, captureRequestData.PaymentId);
                captureService.Body = requestBody;
                HttpResponseMessage serviceResponse = captureService.Execute(captureRequestData);
                string responseBody = serviceResponse.Content.ReadAsStringAsync().Result;
                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", captureRequestData.PaymentId }, { "Message", "Transação executada com sucesso" }, { "Response", responseBody } });                

                if (!serviceResponse.IsSuccessStatusCode)
                    captureResponse = new BaseResponse() { ReturnCode = (int)serviceResponse.StatusCode };
                else
                    captureResponse = formatter.ParseMessageToResponse(responseBody);

                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", captureRequestData.PaymentId }, { "Message", "Mensagem de resposta formatada" }, { "Response", JsonConvert.SerializeObject(captureResponse) } });
            }
            catch (Exception ex)
            {
                logger.ErrorWithMetadata(new Dictionary<string, string> { { "ExceptionType", ex.GetType().ToString() }, { "ErrorMessage", ex.Message }, { "StackTrace", ex.StackTrace } });
                captureResponse = new BaseResponse() { ReturnMessage = "Ocorreu um erro na chamada da transação de captura. Verifique o log de erro para mais detalhes" };
            }

            return captureResponse;
        }
    }
}
