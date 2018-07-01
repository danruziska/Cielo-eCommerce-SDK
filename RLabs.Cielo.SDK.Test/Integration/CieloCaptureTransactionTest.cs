using Microsoft.VisualStudio.TestTools.UnitTesting;
using RLabs.Cielo.SDK.Config;
using RLabs.Cielo.SDK.Enum;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Test.Factory;
using RLabs.Cielo.SDK.Transaction;
using System;
using PaymentClass = RLabs.Cielo.SDK.Model.Entity.Payment;

namespace RLabs.Cielo.SDK.Test.Integration
{
    [TestClass]
    class CieloCaptureTransactionTest
    {
        private Credential credentials;

        private CustomerCreator customerCreator;
        private PaymentCreator paymentCreator;
        private CreditCardCreator creditCardCreator;

        private Customer customerData;
        private PaymentClass paymentData;
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
        public void ShouldReturnAuthorizedStatusWhenSendCaptureTransactionWithCreditCard()
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
            CieloTransaction captureTransaction = new CieloTransaction(TransactionType.Capture, credentials, ApiEnvironment.Sandbox);
            //Act
            BaseResponse captureResponse = captureTransaction.Execute(new PaymentRequest() { PaymentId = authorizationResponse.Payment.PaymentId });
            //Assert
            Assert.AreEqual(2, captureResponse.Status);
            Assert.AreEqual(6, captureResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnNotFoundStatusWhenSendCaptureTransactionWithNotValidPaymentId()
        {
            //Arrange
            PaymentRequest captureRequest = new PaymentRequest() { PaymentId = Guid.NewGuid().ToString(), Amount = 50000 };
            //Act
            CieloTransaction captureTransaction = new CieloTransaction(TransactionType.Capture, credentials, ApiEnvironment.Sandbox);
            BaseResponse captureResponse = captureTransaction.Execute(captureRequest);
            //Assert
            Assert.AreEqual(404, captureResponse.ReturnCode);
        }
    }
}
