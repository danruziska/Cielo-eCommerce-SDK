using Microsoft.VisualStudio.TestTools.UnitTesting;
using RLabs.Cielo.SDK.Formatters.Json.Transaction;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Test.Factory;
using RLabs.Cielo.SDK.Test.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Test.Unit.Formatters
{
    class AuthorizationJSONFormatterTest
    {
        private string expectedResultPath = string.Empty;//Path.GetDirectoryName(Path.GetDirectoryName(TestContext..TestDirectory)) + "\\Components\\IntegracaoCielo\\Unit\\Formatters\\ExpectedJsonResults";
        private CustomerCreator customerCreator;
        private PaymentCreator paymentCreator;
        private CreditCardCreator creditCardCreator;

        private Customer customerData;
        private Payment paymentData;
        private CreditCard creditCardData;

        [TestInitialize]
        public void Init()
        {
            RefreshTestData();
        }

        [TestMethod]
        public void ShouldReturnJsonMessageWithAllParametersProvided()
        {
            //Arrange
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest();
            authorizationRequestData.MerchantOrderId = "12345";
            authorizationRequestData.Customer = customerData;
            authorizationRequestData.Payment = paymentData;
            authorizationRequestData.Payment.CreditCard = creditCardData;
            AuthorizationTransactionFormatter authFormatter = new AuthorizationTransactionFormatter();
            //Act
            string requestFormatted = authFormatter.ParseRequestToMessage(authorizationRequestData);
            //Assert
            string expectedResult = new StreamReader(expectedResultPath + "\\AuthorizationRequest.json").ReadToEnd();
            Assert.AreEqual(TestUtil.RemoveLineEndings(expectedResult), TestUtil.RemoveLineEndings(requestFormatted));
        }

        [TestMethod]
        public void ShouldReturnJsonMessageWithoutCustomerFieldWhenNoCustomerDataIsProvided()
        {
            //Arrange
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest();
            authorizationRequestData.MerchantOrderId = "12345";
            authorizationRequestData.Payment = paymentData;
            authorizationRequestData.Payment.CreditCard = creditCardData;
            AuthorizationTransactionFormatter authFormatter = new AuthorizationTransactionFormatter();
            //Act
            string requestFormatted = authFormatter.ParseRequestToMessage(authorizationRequestData);
            //Assert
            string expectedResult = new StreamReader(expectedResultPath + "\\AuthorizationRequestWithoutCustomer.json").ReadToEnd();
            Assert.AreEqual(TestUtil.RemoveLineEndings(expectedResult), TestUtil.RemoveLineEndings(requestFormatted));
        }

        [TestMethod]
        public void ShouldReturnJsonMessageWithoutPaymentFieldWhenNoPaymentDataIsProvided()
        {
            //Arrange
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest();
            authorizationRequestData.MerchantOrderId = "12345";
            authorizationRequestData.Customer = customerData;
            AuthorizationTransactionFormatter authFormatter = new AuthorizationTransactionFormatter();
            //Act
            string requestFormatted = authFormatter.ParseRequestToMessage(authorizationRequestData);
            //Assert
            string expectedResult = new StreamReader(expectedResultPath + "\\AuthorizationRequestWithoutPayment.json").ReadToEnd();
            Assert.AreEqual(TestUtil.RemoveLineEndings(expectedResult), TestUtil.RemoveLineEndings(requestFormatted));
        }

        [TestMethod]
        public void ShouldReturnJsonMessageWithoutCreditCardFieldWhenNoCreditCardDataIsProvided()
        {
            //Arrange
            AuthorizationRequest authorizationRequestData = new AuthorizationRequest();
            authorizationRequestData.MerchantOrderId = "12345";
            authorizationRequestData.Customer = customerData;
            authorizationRequestData.Payment = paymentData;
            AuthorizationTransactionFormatter authFormatter = new AuthorizationTransactionFormatter();
            //Act
            string requestFormatted = authFormatter.ParseRequestToMessage(authorizationRequestData);
            //Assert
            string expectedResult = new StreamReader(expectedResultPath + "\\AuthorizationRequest.json").ReadToEnd();
            Assert.AreEqual(TestUtil.RemoveLineEndings(expectedResult), TestUtil.RemoveLineEndings(requestFormatted));
        }

        [TestMethod]
        public void ShouldReturnResponseObjectWithAllParametersFilled()
        {
            //Arrange
            string response = new StreamReader(expectedResultPath + "\\AuthorizationResponse.json").ReadToEnd();
            AuthorizationTransactionFormatter authFormatter = new AuthorizationTransactionFormatter();
            //Act            
            AuthorizationResponse authResponse = authFormatter.ParseMessageToResponse(response);
            //Assert
            Assert.IsNotNull(authResponse.Payment);
            Assert.AreEqual(2, authResponse.Payment.Status);
            Assert.AreNotEqual(string.Empty, authResponse.Payment.PaymentId);
            Assert.AreEqual(6, authResponse.Payment.ReturnCode);
            Assert.AreEqual("Operation Successful", authResponse.Payment.ReturnMessage);
            Assert.AreEqual("0305020554239", authResponse.Payment.TransactionId);
        }

        [TestMethod]
        public void ShouldReturnResponseObjectWithErrorMessageWhenResponseMessageIsIncomplete()
        {
            //Arrange
            string response = new StreamReader(expectedResultPath + "\\AuthorizationResponseIncomplete.json").ReadToEnd();
            AuthorizationTransactionFormatter authFormatter = new AuthorizationTransactionFormatter();
            //Act            
            AuthorizationResponse authResponse = authFormatter.ParseMessageToResponse(response);
            //Assert
            Assert.AreEqual("Ocorreu um erro ao formatar a mensagem de resposta na API da Cielo. Verifique o log de erro para mais detalhes", authResponse.ReturnMessage);
        }


        [TestMethod]
        public void ShouldReturnEmptyObjectWhenAuthorizationRequestIsNull()
        {
            //Arrange
            AuthorizationRequest authRequest = null;
            AuthorizationTransactionFormatter authFormatter = new AuthorizationTransactionFormatter();
            //Act            
            string requestMessage = authFormatter.ParseRequestToMessage(authRequest);
            //Assert
            Assert.AreEqual("{}", requestMessage);
        }
        private void RefreshTestData()
        {
            customerCreator = new CustomerCreator();
            paymentCreator = new PaymentCreator();
            creditCardCreator = new CreditCardCreator();

            customerData = customerCreator.Result;
            paymentData = paymentCreator.Result;
            creditCardData = creditCardCreator.Result;
        }
    }
}
