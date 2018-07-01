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
    internal sealed class PaymentTransactionFormatter : IFormatter<PaymentRequest, BaseResponse, string>
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public BaseResponse ParseMessageToResponse(string message)
        {
            BaseResponse baseResponse;

            try
            {
                JObject resultObj = JObject.Parse(message);
                int status = Convert.ToInt32(resultObj["Status"]);
                int returnCode = Convert.ToInt32(resultObj["ReturnCode"]);

                baseResponse = new BaseResponse()
                {
                    ReturnCode = returnCode,
                    Status = status
                };

                if (status == 0 && returnCode == 0)
                {
                    throw new ArgumentNullException("ReturnCode and Status");
                }
            }
            catch (Exception ex)
            {
                logger.ErrorWithMetadata(new Dictionary<string, string> { { "ExceptionType", ex.GetType().ToString() }, { "ErrorMessage", ex.Message }, { "StackTrace", ex.StackTrace }, { "Message", message } });
                baseResponse = new BaseResponse() { ReturnMessage = "Ocorreu um erro ao formatar a mensagem de resposta na API da Cielo. Verifique o log de erro para mais detalhes" };
            }

            return baseResponse;
        }

        public string ParseRequestToMessage(PaymentRequest requestData)
        {
            dynamic bodyMessage = new JObject();

            if(requestData != null)
                bodyMessage.Amount = requestData.Amount;

            return bodyMessage.ToString();
        }
    }
}
