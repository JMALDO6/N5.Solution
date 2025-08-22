using Microsoft.Extensions.Logging;
using Moq;
using N5.Application.Commands.Permission.Modify;
using N5.Application.DTOs.Permission;
using N5.Application.Events;
using N5.Application.Interfaces;
using N5.Domain.Entities;
using N5.Domain.Interfaces;

namespace N5.UnitTest.Application.Commands
{
    public class ModifyPermissionHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IKafkaProducer> _kafkaProducerMock;
        private Mock<ILogger<ModifyPermissionHandler>> _loggerMock;
        private ModifyPermissionHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _kafkaProducerMock = new Mock<IKafkaProducer>();
            var permissionRepoMock = new Mock<IPermissionRepository>();

            _loggerMock = new Mock<ILogger<ModifyPermissionHandler>>();
            _handler = new ModifyPermissionHandler(_unitOfWorkMock.Object, _loggerMock.Object, _kafkaProducerMock.Object);

            _unitOfWorkMock.Setup(u => u.Permissions).Returns(permissionRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Permission { Id = 1, EmployeeForename = "fakeName", EmployeeSurname = "fakeLastName", PermissionDate = DateTime.Now, PermissionTypeId = 1 });
        }

        [Test]
        public async Task WhenCallHandle_ModifyIsValid_ShouldReturnUpdatedPermissionId()
        {
            // Arrange
            var command = new ModifyPermissionCommand { Permission = new PermissionDto { EmployeeLastName = "fakeLastName", EmployeeName = "fakeName", Id = 1, PermissionDate = DateTime.Now, PermissionTypeId = 1 } };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(r => r.Permissions.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _unitOfWorkMock.Verify(r => r.Permissions.Update(It.IsAny<Permission>()), Times.Once);
            _kafkaProducerMock.Verify(x => x.PublishAsync(It.IsAny<PermissionNotificationEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task WhenCallHandle_ReturnDefault_IfPermissionNotFound()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Permission)null);
            var command = new ModifyPermissionCommand { Permission = new PermissionDto { EmployeeLastName = "fakeLastName", EmployeeName = "fakeName", Id = 1, PermissionDate = DateTime.Now, PermissionTypeId = 1 } };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(r => r.Permissions.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _unitOfWorkMock.Verify(r => r.Permissions.Update(It.IsAny<Permission>()), Times.Never);
            _kafkaProducerMock.Verify(x => x.PublishAsync(It.IsAny<PermissionNotificationEvent>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void WhenCallHandle_ThenLogError_IfExceptionIsThrown()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Database error"));
            var command = new ModifyPermissionCommand { Permission = new PermissionDto { EmployeeLastName = "fakeLastName", EmployeeName = "fakeName", Id = 1, PermissionDate = DateTime.Now, PermissionTypeId = 1 } };

            // Act & Assert
            Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(command, CancellationToken.None));
        }
    }
}