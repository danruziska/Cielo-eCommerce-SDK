using Microsoft.VisualStudio.TestTools.UnitTesting;
using RLabs.Cielo.SDK.Formatters.Json.Transaction;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Test.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Test.Unit.Formatters
{
    [TestClass]
    class PaymentJSONFormatterTest
    {
        private string expectedResultPath = string.Empty; // Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + "\\Components\\IntegracaoCielo\\Unit\\Formatters\\ExpectedJsonResults";

        [TestMethod]
        public void ShouldReturnPaymentJsonMessageWithAllParametersProvided()
        {
            //Arrange
            PaymentRequest paymentRequestData = new PaymentRequest();
            paymentRequestData.Amount = 32000;
            PaymentTransactionFormatter paymentFormatter = new PaymentTransactionFormatter();
            //Act
            string requestFormatted = paymentFormatter.ParseRequestToMessage(paymentRequestData);
            //Assert
            string expectedResult = new StreamReader(expectedResultPath + "\\PaymentRequest.json").ReadToEnd();
            Assert.AreEqual(TestUtil.RemoveLineEndings(expectedResult), TestUtil.RemoveLineEndings(requestFormatted));
        }

        [TestMethod]
        public void ShouldReturnPaymentResponseObjectWithAllParametersFilled()
        {
            //Arrange
            string response = new StreamReader(expectedResultPath + "\\PaymentResponse.json").ReadToEnd();
            PaymentTransactionFormatter paymentFormatter = new PaymentTransactionFormatter();
            //Act            
            BaseResponse paymentResponse = paymentFormatter.ParseMessageToResponse(response);
            //Assert
            Assert.AreEqual(4, paymentResponse.Status);
            Assert.AreEqual(9, paymentResponse.ReturnCode);
        }

        [TestMethod]
        public void ShouldReturnResponseObjectWithErrorMessageWhenResponseMessageIsIncomplete()
        {
            //Arrange
            string response = new StreamReader(expectedResultPath + "\\PaymentResponseIncomplete.json").ReadToEnd();
            PaymentTransactionFormatter paymentFormatter = new PaymentTransactionFormatter();
            //Act            
            BaseResponse paymentResponse = paymentFormatter.ParseMessageToResponse(response);
            //Assert
            Assert.AreEqual("Ocorreu um erro ao formatar a mensagem de resposta na API da Cielo. Verifique o log de erro para mais detalhes", paymentResponse.ReturnMessage);
        }

        [TestMethod]
        public void ShouldReturnEmptyObjectWhenPaymentRequestIsNull()
        {
            //Arrange
            PaymentRequest paymentRequest = null;
            PaymentTransactionFormatter paymentFormatter = new PaymentTransactionFormatter();
            //Act            
            string requestMessage = paymentFormatter.ParseRequestToMessage(paymentRequest);
            //Assert
            Assert.AreEqual("{}", requestMessage);
        }
    }
}
