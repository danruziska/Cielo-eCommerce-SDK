using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RLabs.Cielo.SDK.Formatters.Json.Model;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Util;
using PaymentModel = RLabs.Cielo.SDK.Model.Entity;

namespace RLabs.Cielo.SDK.Formatters.Json.Transaction
{
    internal sealed class AuthorizationTransactionFormatter : IFormatter<AuthorizationRequest, AuthorizationResponse, string>
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ParseRequestToMessage(AuthorizationRequest authorizationRequestData)
        {
            logger.Info("Formatando mensagem de requisição", new { Env = "DEV", ServerName = "ABCXYZ", LibLocation = "C:\\xxx\\RLabs.Cielo.SDK.dll", TransactionId = "12345", Date = "2018-05-06:09:03:00", Level = "INFO" });
            dynamic bodyMessage = new JObject();

            if(authorizationRequestData != null)
            {
                bodyMessage.MerchantOrderId = authorizationRequestData.MerchantOrderId;

                if (authorizationRequestData.Customer != null)
                    bodyMessage.Customer = CustomerFormatter.BuildCustomer(authorizationRequestData.Customer);

                if (authorizationRequestData.Payment != null)
                    bodyMessage.Payment = PaymentFormatter.BuildPayment(authorizationRequestData.Payment);

                if (authorizationRequestData.Wallet != null)
                    bodyMessage.Wallet = PaymentFormatter.BuildWallet(authorizationRequestData.Wallet);
            }
                                    
            return bodyMessage.ToString();
        }        

        public AuthorizationResponse ParseMessageToResponse(string message)
        {            
            AuthorizationResponse authResponse;
            try
            {
                JObject resultObj = JObject.Parse(message);
                int status = Convert.ToInt32(resultObj["Payment"]["Status"]);
                int returnCode = Convert.ToInt32(resultObj["Payment"]["ReturnCode"]);

                authResponse = BuildResponsObject(returnCode, status, resultObj);
            }         
            catch (Exception ex)
            {
                logger.ErrorWithMetadata(new Dictionary<string, string> { { "ExceptionType", ex.GetType().ToString() }, { "ErrorMessage", ex.Message }, { "StackTrace", ex.StackTrace }, { "Message", message } });
                authResponse = new AuthorizationResponse() { ReturnMessage = "Ocorreu um erro ao formatar a mensagem de resposta na API da Cielo. Verifique o log de erro para mais detalhes" };
            }          
            return authResponse;
        }

        private AuthorizationResponse BuildResponsObject(int returnCode, int status, JObject resultObj)
        {
            return new AuthorizationResponse()
            {
                ReturnCode = returnCode,
                Status = status,
                Payment = new PaymentModel.Payment()
                {
                    PaymentId = resultObj["Payment"]["PaymentId"].ToString(),
                    ReturnCode = returnCode,
                    ReturnMessage = resultObj["Payment"]["ReturnMessage"].ToString(),
                    TransactionId = resultObj["Payment"]["Tid"].ToString(),
                    Status = status
                }
            };
        }
    }
}
