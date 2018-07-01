using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RLabs.Cielo.SDK.Formatters;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Service;
using RLabs.Cielo.SDK.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Test.Unit.Transaction
{
    [TestClass]
    class VoidTransactionTest
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        [TestMethod]
        public void ShouldCallAllFormatterAndServiceMethodsForVoidTransaction()
        {
            //Arrange            
            //Mock Formatter
            var formatterMock = new Mock<IFormatter<PaymentRequest, BaseResponse, string>>();
            formatterMock.Setup(f => f.ParseMessageToResponse(It.IsAny<string>()));
            formatterMock.Setup(f => f.ParseRequestToMessage(It.IsAny<PaymentRequest>()));
            //Mock Service
            var serviceMock = new Mock<HttpServiceBase<PaymentRequest, HttpResponseMessage>>(MockBehavior.Strict, new Credential(), "urlFake");
            serviceMock.Setup(s => s.Execute(It.IsAny<PaymentRequest>())).Returns(new HttpResponseMessage() { Content = new StringContent("teste"), StatusCode = System.Net.HttpStatusCode.OK });

            VoidTransaction voidTransaction = new VoidTransaction(formatterMock.Object, serviceMock.Object, logger);
            //Act
            voidTransaction.Execute(new PaymentRequest());
            //Assert
            formatterMock.Verify(f => f.ParseMessageToResponse(It.IsAny<string>()), Times.Once);
            formatterMock.Verify(f => f.ParseRequestToMessage(It.IsAny<PaymentRequest>()), Times.Once);
            serviceMock.Verify(s => s.Execute(It.IsAny<PaymentRequest>()), Times.Once);
        }

        [TestMethod]
        public void ShouldNotCallParseMessageToResponseWhenStatusCodeIsNotSuccess()
        {
            //Arrange            
            //Mock Formatter
            var formatterMock = new Mock<IFormatter<PaymentRequest, BaseResponse, string>>();
            formatterMock.Setup(f => f.ParseMessageToResponse(It.IsAny<string>()));
            formatterMock.Setup(f => f.ParseRequestToMessage(It.IsAny<PaymentRequest>()));
            //Mock Service
            var serviceMock = new Mock<HttpServiceBase<PaymentRequest, HttpResponseMessage>>(MockBehavior.Strict, new Credential(), "urlFake");
            serviceMock.Setup(s => s.Execute(It.IsAny<PaymentRequest>())).Returns(new HttpResponseMessage() { Content = new StringContent("teste"), StatusCode = System.Net.HttpStatusCode.NotFound });

            VoidTransaction voidTransaction = new VoidTransaction(formatterMock.Object, serviceMock.Object, logger);
            //Act
            voidTransaction.Execute(new PaymentRequest());
            //Assert
            formatterMock.Verify(f => f.ParseMessageToResponse(It.IsAny<string>()), Times.Never);
            formatterMock.Verify(f => f.ParseRequestToMessage(It.IsAny<PaymentRequest>()), Times.Once);
            serviceMock.Verify(s => s.Execute(It.IsAny<PaymentRequest>()), Times.Once);
        }

        [TestMethod]
        public void ShouldReturnErrorMessageWhenPassingNullRequest()
        {
            //Arrange
            VoidTransaction voidTransaction = new VoidTransaction(null, null, logger);
            //Act
            BaseResponse response = voidTransaction.Execute(null);
            //Assert
            Assert.AreEqual("Ocorreu um erro na chamada da transação de cancelamento. Verifique o log de erro para mais detalhes", response.ReturnMessage);
        }
    }
}
