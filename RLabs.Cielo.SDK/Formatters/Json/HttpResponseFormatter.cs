using Newtonsoft.Json.Linq;
using NLog;
using RLabs.Cielo.SDK.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RLabs.Cielo.SDK.Util;

namespace RLabs.Cielo.SDK.Formatters.Json
{
    internal static class HttpResponseFormatter
    {
        internal static BaseResponse ParseHttpResponseMessageToObject(HttpResponseMessage errorResponseMessage, ILogger logger)
        {            
            string responseBody = errorResponseMessage.Content.ReadAsStringAsync().Result;
            BaseResponse httpErrorResponse = null;
            int returnCode = (int)errorResponseMessage.StatusCode;
            string reasonPhrase = errorResponseMessage.ReasonPhrase;
            //Caso ocorra um erro de negócio da Cielo, existe internal code e internal message
            if (!string.IsNullOrEmpty(responseBody))
            {
                httpErrorResponse = ParseBusinessError(responseBody, logger, returnCode, reasonPhrase);
            }
            //Caso seja apenas um erro http, retornar apenas o código e mensagem do http
            else
            {
                httpErrorResponse = ParseHttpError(logger, returnCode, reasonPhrase);   
            }
            return httpErrorResponse;
        }   
        
        private static BaseResponse ParseBusinessError(string responseBody, ILogger logger, int returnCode, string reasonPhrase)
        {
            BaseResponse httpErrorResponse = null;
            JArray errorBodyObj = JArray.Parse(responseBody);

            string internalCode = errorBodyObj[0]["Code"].ToString();
            string internalMessage = errorBodyObj[0]["Message"].ToString();
            string httpMessage = string.Format("A requisição http foi executada com sucesso, mas retornou um código de erro: {0}:{1}", returnCode.ToString(), reasonPhrase);

            httpErrorResponse = new BaseResponse()
            {
                ReturnCode = returnCode,
                ReturnMessage = httpMessage,
                InternalCode = internalCode,
                InternalMessage = internalMessage
            };

            if (logger != null) logger.ErrorWithMetadata(new Dictionary<string, string>() { { "InternalCode", httpErrorResponse.InternalCode }, { "InternalError", httpErrorResponse.InternalMessage } });
            return httpErrorResponse;
        }

        private static BaseResponse ParseHttpError(ILogger logger, int returnCode, string reasonPhrase)
        {
            BaseResponse httpErrorResponse = null;
            httpErrorResponse = new BaseResponse()
            {
                ReturnCode = returnCode,
                ReturnMessage = string.Format("A requisição http foi executada com sucesso, mas retornou um código de erro: {0}:{1}", returnCode.ToString(), reasonPhrase)
            };

            if (logger != null) logger.ErrorWithMetadata(new Dictionary<string, string>() { { "Code", returnCode.ToString() }, { "Error", reasonPhrase } });
            return httpErrorResponse;
        }
    }
}
