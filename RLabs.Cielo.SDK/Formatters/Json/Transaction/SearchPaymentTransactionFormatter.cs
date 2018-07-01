using Newtonsoft.Json.Linq;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLabs.Cielo.SDK.Util;

namespace RLabs.Cielo.SDK.Formatters.Json.Transaction
{
    internal sealed class SearchPaymentTransactionFormatter : IFormatter<PaymentRequest, BaseResponse, string>
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public BaseResponse ParseMessageToResponse(string message)
        {
            BaseResponse searchPaymentResponse;
            try
            {
                JObject resultObj = JObject.Parse(message);
                int status = Convert.ToInt32(resultObj["Payment"]["Status"]);

                searchPaymentResponse = new BaseResponse()
                {
                    Status = status
                };
            }
            catch (Exception ex)
            {
                logger.ErrorWithMetadata(new Dictionary<string, string> { { "ExceptionType", ex.GetType().ToString() }, { "ErrorMessage", ex.Message }, { "StackTrace", ex.StackTrace }, { "Message", message } });
                searchPaymentResponse = new BaseResponse() { ReturnMessage = "Ocorreu um erro ao formatar a mensagem de resposta na API da Cielo. Verifique o log de erro para mais detalhes" };
            }

            return searchPaymentResponse;
        }

        public string ParseRequestToMessage(PaymentRequest requestData)
        {
            return string.Empty;
        }
    }
}
