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
    [TestClass]
    class CieloSearchPaymentTransactionTest
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
        public void ShouldReturnAuthorizedStatusWhenSendSearchPaymentTransactionWithCreditCard()
        {
            //Arrange   
            paymentData.CreditCard = creditCardData;
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest()
            {
                MerchantOrderId = "12345",
                Customer = customerData,
                Payment = paymentData
            };
            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            CieloTransaction searchPaymentTransaction = new CieloTransaction(TransactionType.SearchPayment, credentials, ApiEnvironment.Sandbox);
            //Act
            BaseResponse searchPaymentResponse = searchPaymentTransaction.Execute(new PaymentRequest() { PaymentId = authorizationResponse.Payment.PaymentId });
            //Assert
            Assert.AreEqual(1, searchPaymentResponse.Status);
        }

        [TestMethod]
        public void ShouldReturnNotFoundStatusWhenSendSearchTransactionWithNotValidPaymentId()
        {
            //Arrange
            PaymentRequest searchRequest = new PaymentRequest() { PaymentId = Guid.NewGuid().ToString() };
            //Act
            CieloTransaction searchPaymentTransaction = new CieloTransaction(TransactionType.SearchPayment, credentials, ApiEnvironment.Sandbox);
            BaseResponse captureResponse = searchPaymentTransaction.Execute(searchRequest);
            //Assert
            Assert.AreEqual(404, captureResponse.ReturnCode);
        }

    }
}
