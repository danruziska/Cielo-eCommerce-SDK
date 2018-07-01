using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RLabs.Cielo.SDK.Formatters;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Model.Response;
using RLabs.Cielo.SDK.Model.Validators;
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
    class AuthorizationTransactionTest
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        [TestMethod]
        public void ShouldCallAllFormatterAndServiceMethodsForAuthorizationTransaction()
        {
            //Arrange
            AuthorizationRequest authRequest = new AuthorizationRequest() { Payment = new Payment() { Amount = 10000 } };
            //Mock Formatter
            var formatterMock = new Mock<IFormatter<AuthorizationRequest, AuthorizationResponse, string>>();
            formatterMock.Setup(f => f.ParseMessageToResponse(It.IsAny<string>()));
            formatterMock.Setup(f => f.ParseRequestToMessage(It.IsAny<AuthorizationRequest>()));
            //Mock Service
            var serviceMock = new Mock<HttpServiceBase<AuthorizationRequest, HttpResponseMessage>>(MockBehavior.Strict, new Credential(), "urlFake");
            serviceMock.Setup(s => s.Execute(It.IsAny<AuthorizationRequest>())).Returns(new HttpResponseMessage() { Content = new StringContent("teste"), StatusCode = System.Net.HttpStatusCode.OK });
            //Mock Validator
            var validatorMock = new Mock<BaseValidator<AuthorizationRequest>>(logger);
            validatorMock.Setup(v => v.ValidateField(authRequest)).Returns(true);

            AuthorizationTransaction authTransaction = new AuthorizationTransaction(formatterMock.Object, serviceMock.Object, logger, validatorMock.Object);
            //Act
            authTransaction.Execute(authRequest);
            //Assert
            formatterMock.Verify(f => f.ParseMessageToResponse(It.IsAny<string>()), Times.Once);
            formatterMock.Verify(f => f.ParseRequestToMessage(It.IsAny<AuthorizationRequest>()), Times.Once);
            serviceMock.Verify(s => s.Execute(It.IsAny<AuthorizationRequest>()), Times.Once);
        }

        [TestMethod]
        public void ShouldNotCallParseMessageToResponseWhenStatusCodeIsNotSuccess()
        {
            //Arrange
            AuthorizationRequest authRequest = new AuthorizationRequest() { Payment = new Payment() { Amount = 10000 } };
            //Mock Formatter
            var formatterMock = new Mock<IFormatter<AuthorizationRequest, AuthorizationResponse, string>>();
            formatterMock.Setup(f => f.ParseMessageToResponse(It.IsAny<string>()));
            formatterMock.Setup(f => f.ParseRequestToMessage(It.IsAny<AuthorizationRequest>()));
            //Mock Service
            var serviceMock = new Mock<HttpServiceBase<AuthorizationRequest, HttpResponseMessage>>(MockBehavior.Strict, new Credential(), "urlFake");
            serviceMock.Setup(s => s.Execute(It.IsAny<AuthorizationRequest>())).Returns(new HttpResponseMessage() { Content = new StringContent("teste"), StatusCode = System.Net.HttpStatusCode.NotFound });
            //Mock Validator
            var validatorMock = new Mock<BaseValidator<AuthorizationRequest>>(logger);
            validatorMock.Setup(v => v.ValidateField(authRequest)).Returns(true);

            AuthorizationTransaction authTransaction = new AuthorizationTransaction(formatterMock.Object, serviceMock.Object, logger, validatorMock.Object);
            //Act
            authTransaction.Execute(authRequest);
            //Assert
            formatterMock.Verify(f => f.ParseMessageToResponse(It.IsAny<string>()), Times.Never);
            formatterMock.Verify(f => f.ParseRequestToMessage(It.IsAny<AuthorizationRequest>()), Times.Once);
            serviceMock.Verify(s => s.Execute(It.IsAny<AuthorizationRequest>()), Times.Once);
        }

        [TestMethod]
        public void ShouldReturnErrorMessageWhenPassingNullRequest()
        {
            //Arrange
            AuthorizationTransaction authTransaction = new AuthorizationTransaction(null, null, logger, null);
            //Act
            AuthorizationResponse response = authTransaction.Execute(null);
            //Assert
            Assert.AreEqual("Ocorreu um erro na chamada da transação de autorização. Verifique o log de erro para mais detalhes", response.ReturnMessage);
        }
    }
}
