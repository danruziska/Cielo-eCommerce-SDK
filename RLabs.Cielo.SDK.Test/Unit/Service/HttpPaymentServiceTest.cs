using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Service;
using RLabs.Cielo.SDK.Util;

namespace RLabs.Cielo.SDK.Test.Unit.Service
{
    [TestClass]
    class HttpPaymentServiceTest
    {
        [TestMethod]
        public void ShouldCallPutMethodOnceWhenSendPaymentTransaction()
        {
            //Arrange
            Mock<IHttpWrapper> httpWrapperMock = new Mock<IHttpWrapper>();
            httpWrapperMock.Setup(h => h.Put(It.IsAny<Credential>(), It.IsAny<Header>(), It.IsAny<string>(), It.IsAny<string>()));
            HttpPaymentService paymentService = new HttpPaymentService(new Credential(), httpWrapperMock.Object, "urlFake");
            //Act
            paymentService.Execute(new PaymentRequest());
            //Assert
            httpWrapperMock.Verify(h => h.Put(It.IsAny<Credential>(), It.IsAny<Header>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
