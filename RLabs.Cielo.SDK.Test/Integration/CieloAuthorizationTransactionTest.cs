using Microsoft.VisualStudio.TestTools.UnitTesting;
using RLabs.Cielo.SDK.Config;
using RLabs.Cielo.SDK.Enum;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Test.Factory;
using RLabs.Cielo.SDK.Transaction;
using PaymentClass = RLabs.Cielo.SDK.Model.Entity.Payment;

namespace RLabs.Cielo.SDK.Test.Integration
{
    [TestClass]
    class CieloAuthorizationTransactionTest
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
            RefreshTestData();
        }

        [TestMethod]
        public void ShouldReturnAuthorizedStatusWhenSendAuthorizationTransactionWithCreditCard()
        {
            //Arrange   
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest()
            {
                MerchantOrderId = "12345",
                Customer = customerData,
                Payment = paymentData
            };

            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            //Act
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            //Assert
            Assert.AreEqual(1, authorizationResponse.Status);
            Assert.AreEqual(4, authorizationResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnNotAuthorizedStatusWhenSendAuthorizationTransactionWithCreditCard()
        {
            //Arrange
            paymentData.CreditCard = creditCardCreator.WithCardNumber("0000000000000002").Result;
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest()
            {
                MerchantOrderId = "12345",
                Customer = customerData,
                Payment = paymentData
            };

            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            //Act
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            //Assert
            Assert.AreEqual(3, authorizationResponse.Status);
            Assert.AreEqual(5, authorizationResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnExpiredStatusWhenSendAuthorizationTransactionWithCreditCard()
        {
            //Arrange
            paymentData.CreditCard = creditCardCreator.WithCardNumber("0000000000000003").Result;
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest()
            {
                MerchantOrderId = "12345",
                Customer = customerData,
                Payment = paymentData
            };

            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            //Act
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            //Assert
            Assert.AreEqual(3, authorizationResponse.Status);
            Assert.AreEqual(57, authorizationResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnBlockedStatusWhenSendAuthorizationTransactionWithCreditCard()
        {
            //Arrange
            paymentData.CreditCard = creditCardCreator.WithCardNumber("0000000000000005").Result;
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest()
            {
                MerchantOrderId = "12345",
                Customer = customerData,
                Payment = paymentData
            };

            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            //Act
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            //Assert
            Assert.AreEqual(3, authorizationResponse.Status);
            Assert.AreEqual(78, authorizationResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnTimeOutStatusWhenSendAuthorizationTransactionWithCreditCard()
        {
            //Arrange
            paymentData.CreditCard = creditCardCreator.WithCardNumber("0000000000000006").Result;
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest()
            {
                MerchantOrderId = "12345",
                Customer = customerData,
                Payment = paymentData
            };

            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            //Act
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            //Assert
            Assert.AreEqual(0, authorizationResponse.Status);
            Assert.AreEqual(99, authorizationResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnCancelledStatusWhenSendAuthorizationTransactionWithCreditCard()
        {
            //Arrange
            paymentData.CreditCard = creditCardCreator.WithCardNumber("0000000000000007").Result;
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest()
            {
                MerchantOrderId = "12345",
                Customer = customerData,
                Payment = paymentData
            };

            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            //Act
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            //Assert
            Assert.AreEqual(3, authorizationResponse.Status);
            Assert.AreEqual(77, authorizationResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnProblemStatusWhenSendAuthorizationTransactionWithCreditCard()
        {
            //Arrange
            paymentData.CreditCard = creditCardCreator.WithCardNumber("0000000000000008").Result;
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest()
            {
                MerchantOrderId = "12345",
                Customer = customerData,
                Payment = paymentData
            };
            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            //Act
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            //Assert
            Assert.AreEqual(3, authorizationResponse.Status);
            Assert.AreEqual(70, authorizationResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSendAuthorizationTransactionWithAmountEqualToZero()
        {
            //Arrange   
            PaymentClass paymentWithAmountZero = paymentCreator.WithAmount(0).Result;
            paymentWithAmountZero.CreditCard = creditCardData;
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest()
            {
                MerchantOrderId = "12345",
                Customer = customerData,
                Payment = paymentWithAmountZero
            };

            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials, ApiEnvironment.Sandbox);
            //Act
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            //Assert
            Assert.AreEqual("Ocorreu um erro de validação na chamada da transação de autorização. Verifique o log de informação para mais detalhes", authorizationResponse.ReturnMessage);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSendAuthorizationTransactionWithNullRequest()
        {
            //Arrange               
            AuthorizationRequest authorizationRequestData = null;
            CieloTransaction authorizationTransaction = new CieloTransaction(TransactionType.Authorization, credentials);
            //Act
            AuthorizationResponse authorizationResponse = (AuthorizationResponse)authorizationTransaction.Execute(authorizationRequestData);
            //Assert
            Assert.AreEqual("Ocorreu um erro na chamada da transação de autorização. Verifique o log de erro para mais detalhes", authorizationResponse.ReturnMessage);
        }

        private void RefreshTestData()
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
    }
}
