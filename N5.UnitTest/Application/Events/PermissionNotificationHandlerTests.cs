using Microsoft.Extensions.Logging;
using Moq;
using N5.Application.Commands.Permission.Modify;
using N5.Application.Events;
using N5.Application.Events.Handlers;
using N5.Application.Interfaces;
using N5.Domain.Entities;
using N5.Domain.Interfaces;

namespace N5.UnitTest.Application.Events
{
    public class PermissionNotificationHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IPermissionElasticService> _permissionElasticService;
        private Mock<ILogger<PermissionNotificationHandler>> _loggerMock;
        private PermissionNotificationHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _permissionElasticService = new Mock<IPermissionElasticService>();
            var permissionRepoMock = new Mock<IPermissionRepository>();

            _loggerMock = new Mock<ILogger<PermissionNotificationHandler>>();
            _handler = new PermissionNotificationHandler(_permissionElasticService.Object, _unitOfWorkMock.Object, _loggerMock.Object);

            _unitOfWorkMock.Setup(u => u.Permissions).Returns(permissionRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Permission { Id = 1, EmployeeForename = "fakeName", EmployeeSurname = "fakeLastName", PermissionDate = DateTime.Now, PermissionTypeId = 1 });
        }

        [Test]
        public async Task WhenCallHandle_CallIndex_IfOperationIsRequest()
        {
            // Arrange
            var notification = new PermissionNotificationEvent { PermissionId = 1, Operation = "request" };

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(r => r.Permissions.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _permissionElasticService.Verify(x => x.IndexAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Once);
            _permissionElasticService.Verify(x => x.UpdateAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task WhenCallHandle_CallUpdate_IfOperationIsModify()
        {
            // Arrange
            var notification = new PermissionNotificationEvent { PermissionId = 1, Operation = "modify" };

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(r => r.Permissions.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _permissionElasticService.Verify(x => x.IndexAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Never);
            _permissionElasticService.Verify(x => x.UpdateAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task WhenCallHandle_NeverCallElasticService_IfOperationIsGet()
        {
            // Arrange
            var notification = new PermissionNotificationEvent { PermissionId = 1, Operation = "get" };

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(r => r.Permissions.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _permissionElasticService.Verify(x => x.IndexAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Never);
            _permissionElasticService.Verify(x => x.UpdateAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task WhenCallHandle_ReturnDefault_IfPermissionNotFound()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Permission)null);
            var notification = new PermissionNotificationEvent { PermissionId = 1, Operation = "request" };

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(r => r.Permissions.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _permissionElasticService.Verify(x => x.IndexAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Never);
            _permissionElasticService.Verify(x => x.UpdateAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void WhenCallHandle_ThrowsException_IfErrorOccurs()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("Database error"));
            var notification = new PermissionNotificationEvent { PermissionId = 1, Operation = "request" };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _handler.Handle(notification, CancellationToken.None));
            _unitOfWorkMock.Verify(r => r.Permissions.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _permissionElasticService.Verify(x => x.IndexAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Never);
            _permissionElasticService.Verify(x => x.UpdateAsync(It.IsAny<PermissionDocument>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}