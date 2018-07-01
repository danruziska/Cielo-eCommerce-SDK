using Microsoft.VisualStudio.TestTools.UnitTesting;
using RLabs.Cielo.SDK.Config;
using RLabs.Cielo.SDK.Enum;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Test.Factory;
using RLabs.Cielo.SDK.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Test.Integration
{
    public class CieloVoidTransactionTest
    {
        private Credential credentials;

        private CustomerCreator customerCreator;
        private PaymentCreator paymentCreator;
        private CreditCardCreator creditCardCreator;

        private Customer customerData;
        private Payment paymentData;
        private CreditCard creditCardData;

        [TestInitialize]
        public void Init()
        {
            credentials = new Credential();
            credentials.AddCredentialItem("MerchantId", "41d05cb1-9f41-4981-9cd2-25994f37cd4c");
            credentials.AddCredentialItem("MerchantKey", "BNFUCGHXSSKTFLMSMRSYIGPARRYXNYBTCYNQPTIH");

            customerCreator = new CustomerCreator();
            paymentCreator = new PaymentCreator();
            creditCardCreator = new CreditCardCreator();

            customerData = customerCreator.Result;
            paymentData = paymentCreator.Result;
            creditCardData = creditCardCreator.Result;
        }

        [TestMethod]
        public void ShouldReturnSuccesstatusWhenSendVoidTransaction()
        {
            //Arrange
            //É necessário fazer uma autorização e uma captura antes do cancelamento
            #region Autorização
            AuthorizationRequest authRequest = new AuthorizationRequest() { Customer = customerData, Payment = paymentData, MerchantOrderId = "12345" };
            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            AuthorizationResponse authResponse = (AuthorizationResponse)authorizationTransaction.Execute(authRequest);
            #endregion

            #region Captura            
            PaymentRequest captureRequest = new PaymentRequest() { PaymentId = authResponse.Payment.PaymentId };
            BaseResponse captureResponse = new CieloTransaction(TransactionType.Capture, credentials, ApiEnvironment.Sandbox).Execute(captureRequest);
            #endregion
            //Act
            PaymentRequest voidRequest = new PaymentRequest() { PaymentId = authResponse.Payment.PaymentId };
            CieloTransaction voidTransaction = new CieloTransaction(TransactionType.Void, credentials, ApiEnvironment.Sandbox);
            BaseResponse voidTransactionResponse = voidTransaction.Execute(voidRequest);
            //Assert
            Assert.AreEqual(10, voidTransactionResponse.Status);
            Assert.AreEqual(0, voidTransactionResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnNotFoundStatusWhenSendVoidTransactionWithNotValidPaymentId()
        {
            //Arrange
            PaymentRequest voidRequest = new PaymentRequest() { PaymentId = Guid.NewGuid().ToString(), Amount = 50000 };
            //Act
            CieloTransaction voidTransaction = new CieloTransaction(TransactionType.Void, credentials, ApiEnvironment.Sandbox);
            BaseResponse voidResponse = voidTransaction.Execute(voidRequest);
            //Assert
            Assert.AreEqual(404, voidResponse.ReturnCode);
        }
    }
}
