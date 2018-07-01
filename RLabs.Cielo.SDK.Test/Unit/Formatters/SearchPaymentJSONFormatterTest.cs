using Microsoft.VisualStudio.TestTools.UnitTesting;
using RLabs.Cielo.SDK.Formatters.Json.Transaction;
using RLabs.Cielo.SDK.Model.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Test.Unit.Formatters
{
    [TestClass]
    class SearchPaymentJSONFormatterTest
    {
        private string expectedResultPath = string.Empty; // Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + "\\Components\\IntegracaoCielo\\Unit\\Formatters\\ExpectedJsonResults";

        [TestMethod]
        public void ShouldReturnSearchPaymentResponseObjectWithAllParametersFilled()
        {
            //Arrange
            string response = new StreamReader(expectedResultPath + "\\SearchPaymentResponse.json").ReadToEnd();
            SearchPaymentTransactionFormatter searchPaymentFormatter = new SearchPaymentTransactionFormatter();
            //Act            
            BaseResponse paymentResponse = searchPaymentFormatter.ParseMessageToResponse(response);
            //Assert
            Assert.AreEqual(1, paymentResponse.Status);
        }

        [TestMethod]
        public void ShouldReturnResponseObjectWithErrorMessageWhenResponseMessageIsIncomplete()
        {
            //Arrange
            string response = new StreamReader(expectedResultPath + "\\SearchPaymentResponseIncomplete.json").ReadToEnd();
            SearchPaymentTransactionFormatter searchPaymentFormatter = new SearchPaymentTransactionFormatter();
            //Act            
            BaseResponse searchPaymentResponse = searchPaymentFormatter.ParseMessageToResponse(response);
            //Assert
            Assert.AreEqual("Ocorreu um erro ao formatar a mensagem de resposta na API da Cielo. Verifique o log de erro para mais detalhes", searchPaymentResponse.ReturnMessage);
        }
    }
}
