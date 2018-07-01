using Newtonsoft.Json.Linq;
using NLog;
using RLabs.Cielo.SDK.Formatters;
using RLabs.Cielo.SDK.Model;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Service;
using RLabs.Cielo.SDK.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RLabs.Cielo.SDK.Model.Validators;

namespace RLabs.Cielo.SDK.Transaction
{
    internal sealed class AuthorizationTransaction : IBaseTransaction<AuthorizationRequest, AuthorizationResponse>
    {                            
        private readonly IFormatter<AuthorizationRequest, AuthorizationResponse, string> formatter;
        private readonly HttpServiceBase<AuthorizationRequest, HttpResponseMessage> authorizationService;
        private readonly ILogger logger;
        private readonly BaseValidator<AuthorizationRequest> validators;

        public AuthorizationTransaction(IFormatter<AuthorizationRequest, AuthorizationResponse, string> formatter, HttpServiceBase<AuthorizationRequest, HttpResponseMessage> authorizationService, 
            ILogger logger, BaseValidator<AuthorizationRequest> validators)
        {  
            this.formatter = formatter;
            this.authorizationService = authorizationService;
            this.logger = logger;
            this.validators = validators;
        }

        public AuthorizationResponse Execute(AuthorizationRequest authorizationRequestData)
        {
            AuthorizationResponse authResponse = null;
            try
            {
                if(!validators.ValidateField(authorizationRequestData)) return new AuthorizationResponse() { ReturnMessage = "Ocorreu um erro de validação na chamada da transação de autorização. Verifique o log de informação para mais detalhes" };
                
                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", authorizationRequestData.Payment.PaymentId }, { "Message", "Iniciando a execução da transação de autorização" } });
                string body = formatter.ParseRequestToMessage(authorizationRequestData);
                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", authorizationRequestData.Payment.PaymentId }, { "Message", "Mensagem de requisição formatada" }, { "Body", body } });                
                authorizationService.Body = body;
                HttpResponseMessage serviceResponse = authorizationService.Execute(authorizationRequestData);
                string responseBody = serviceResponse.Content.ReadAsStringAsync().Result;
                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", authorizationRequestData.Payment.PaymentId }, { "Message", "Transação executada com sucesso" }, { "Response", responseBody } });                

                if (!serviceResponse.IsSuccessStatusCode)
                    authResponse = new AuthorizationResponse() { ReturnCode = (int)serviceResponse.StatusCode };
                else
                    authResponse = formatter.ParseMessageToResponse(responseBody);

                logger.InfoWithMetadata(new Dictionary<string, string> { { "PaymentId", authorizationRequestData.Payment.PaymentId }, { "Message", "Mensagem de resposta formatada" }, { "Response", JsonConvert.SerializeObject(authResponse) } });
            }
            catch (Exception ex)
            {
                logger.ErrorWithMetadata(new Dictionary<string, string> { { "ExceptionType", ex.GetType().ToString() }, { "ErrorMessage", ex.Message }, { "StackTrace", ex.StackTrace } });
                authResponse = new AuthorizationResponse() { ReturnMessage = "Ocorreu um erro na chamada da transação de autorização. Verifique o log de erro para mais detalhes" };
            }
            return authResponse;
        }
    }
}
