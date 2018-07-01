using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Service;
using RLabs.Cielo.SDK.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Test.Unit.Service
{
    [TestClass]
    class HttpSearchPaymentServiceTest
    {
        [TestMethod]
        public void ShouldCallGetMethodOnceWhenSendSearchPaymentTransaction()
        {
            //Arrange
            Mock<IHttpWrapper> httpWrapperMock = new Mock<IHttpWrapper>();
            httpWrapperMock.Setup(h => h.Get(It.IsAny<Credential>(), It.IsAny<Header>(), It.IsAny<string>(), It.IsAny<string>()));
            HttpSearchPaymentService searchPaymentService = new HttpSearchPaymentService(null, new Credential(), httpWrapperMock.Object, "urlFake");
            //Act
            searchPaymentService.Execute(new PaymentRequest());
            //Assert
            httpWrapperMock.Verify(h => h.Get(It.IsAny<Credential>(), It.IsAny<Header>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
