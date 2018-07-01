using NLog;
using RLabs.Cielo.SDK.Config;
using RLabs.Cielo.SDK.Enum;
using RLabs.Cielo.SDK.Formatters.Json;
using RLabs.Cielo.SDK.Formatters.Json.Transaction;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Model.Validators;
using RLabs.Cielo.SDK.Service;
using RLabs.Cielo.SDK.Util;
using System.Collections.Generic;

namespace RLabs.Cielo.SDK.Transaction
{
    public class CieloTransaction : IBaseTransaction<BaseRequest, BaseResponse>
    {
        private readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly Credential credentials;

        private AuthorizationTransaction authorization;
        private CaptureTransaction capture;
        private SearchPaymentTransaction searchPayment;
        private VoidTransaction voidTransaction;
        private TransactionType transactionType;        
        private IDictionary<string, string> globalSettings;        

        public CieloTransaction(TransactionType transactionType, Credential credentials, ApiEnvironment environment)
        {
            this.credentials = credentials;
            InitializeGlobalSettings(environment);
            InitializeTransactions(transactionType);
        }

        public CieloTransaction(TransactionType transactionType, Credential credentials)
        {
            this.credentials = credentials;
            InitializeGlobalSettings(ApiEnvironment.Production);
            InitializeTransactions(transactionType);
        }

        private void InitializeTransactions(TransactionType transactionType)
        {
            this.transactionType = transactionType;
            switch (transactionType)
            {
                case TransactionType.Authorization:
                    this.authorization = new AuthorizationTransaction(new AuthorizationTransactionFormatter(), new HttpAuthorizationService(credentials, new HttpClientWrapper(), globalSettings["authorizationUrl"]), logger, new AuthorizationRequestValidator(logger));
                    break;
                case TransactionType.Capture:
                    this.capture = new CaptureTransaction(new PaymentTransactionFormatter(), new HttpPaymentService(credentials, new HttpClientWrapper(), globalSettings["captureUrl"]), logger);
                    break;
                case TransactionType.SearchPayment:
                    this.searchPayment = new SearchPaymentTransaction(new SearchPaymentTransactionFormatter(), new HttpSearchPaymentService(logger, credentials, new HttpClientWrapper(), globalSettings["searchPaymentUrl"]), logger);
                    break;
                case TransactionType.Void:
                    this.voidTransaction = new VoidTransaction(new PaymentTransactionFormatter(), new HttpPaymentService(credentials, new HttpClientWrapper(), globalSettings["voidUrl"]), logger);
                    break;
            }
        }

        public BaseResponse Execute(BaseRequest requestData)
        {
            switch (transactionType)
            {
                case TransactionType.Authorization:
                    return this.authorization.Execute((AuthorizationRequest)requestData);
                case TransactionType.Capture:
                    return this.capture.Execute((PaymentRequest)requestData);
                case TransactionType.SearchPayment:
                    return this.searchPayment.Execute((PaymentRequest)requestData);
                case TransactionType.Void:
                    return this.voidTransaction.Execute((PaymentRequest)requestData);
                default:
                    return default(BaseResponse);
            }
        }

        private void InitializeGlobalSettings(ApiEnvironment environment)
        {
            if(this.globalSettings == null)
                this.globalSettings = new Dictionary<string, string>();

            switch (environment)
            {
                case ApiEnvironment.Sandbox:
                    globalSettings.Add("authorizationUrl", "https://apisandbox.cieloecommerce.cielo.com.br/1/sales/");
                    globalSettings.Add("captureUrl", "https://apisandbox.cieloecommerce.cielo.com.br/1/sales/{0}/capture/");
                    globalSettings.Add("searchPaymentUrl", "https://apiquerysandbox.cieloecommerce.cielo.com.br/1/sales/{0}");
                    globalSettings.Add("voidUrl", "https://apisandbox.cieloecommerce.cielo.com.br/1/sales/{0}/void?amount={1}");
                    break;
                case ApiEnvironment.Production:
                    globalSettings.Add("authorizationUrl", "https://api.cieloecommerce.cielo.com.br/1/sales/");
                    globalSettings.Add("captureUrl", "https://api.cieloecommerce.cielo.com.br/1/sales/{0}/capture/");
                    globalSettings.Add("searchPaymentUrl", "https://apiquery.cieloecommerce.cielo.com.br/1/sales/{0}");
                    globalSettings.Add("voidUrl", "https://api.cieloecommerce.cielo.com.br/1/sales/{0}/void?amount={1}");
                    break;                
            }
        }
    }
}
