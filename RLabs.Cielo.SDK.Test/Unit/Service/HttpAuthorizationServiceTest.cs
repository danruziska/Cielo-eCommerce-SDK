using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Request;
using RLabs.Cielo.SDK.Service;
using RLabs.Cielo.SDK.Util;

namespace RLabs.Cielo.SDK.Test.Unit.Service
{
    [TestClass]
    class HttpAuthorizationServiceTest
    {
        [TestMethod]
        public void ShouldCallPostMethodWhenSendAuthorizationTransaction()
        {
            //Arrange
            Mock<IHttpWrapper> httpWrapperMock = new Mock<IHttpWrapper>();
            httpWrapperMock.Setup(h => h.Post(It.IsAny<Credential>(), It.IsAny<Header>(), It.IsAny<string>(), It.IsAny<string>()));
            HttpAuthorizationService authService = new HttpAuthorizationService(new Credential(), httpWrapperMock.Object, "urlFake");
            //Act
            authService.Execute(new AuthorizationRequest());
            //Assert
            httpWrapperMock.Verify(h => h.Post(It.IsAny<Credential>(), It.IsAny<Header>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
