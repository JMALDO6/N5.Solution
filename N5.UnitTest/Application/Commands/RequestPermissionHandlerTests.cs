using Microsoft.Extensions.Logging;
using Moq;
using N5.Application.Commands.Permission.Request;
using N5.Application.DTOs.Permission;
using N5.Application.Events;
using N5.Application.Interfaces;
using N5.Domain.Entities;
using N5.Domain.Interfaces;

namespace N5.UnitTest.Application.Commands
{
    public class RequestPermissionHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IKafkaProducer> _kafkaProducerMock;
        private Mock<ILogger<RequestPermissionHandler>> _loggerMock;
        private RequestPermissionHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _kafkaProducerMock = new Mock<IKafkaProducer>();
            var permissionRepoMock = new Mock<IPermissionRepository>();

            _loggerMock = new Mock<ILogger<RequestPermissionHandler>>();
            _handler = new RequestPermissionHandler(_unitOfWorkMock.Object, _loggerMock.Object, _kafkaProducerMock.Object);

            _unitOfWorkMock.Setup(u => u.Permissions).Returns(permissionRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Permission { Id = 1, EmployeeForename = "fakeName", EmployeeSurname = "fakeLastName", PermissionDate = DateTime.Now, PermissionTypeId = 1 });
        }

        [Test]
        public async Task WhenCallHandle_RequestIsValid_ShouldReturnCreatedPermissionId()
        {
            // Arrange
            var command = new RequestPermissionCommand { Permission = new PermissionDto { EmployeeLastName = "fakeLastName", EmployeeName = "fakeName", PermissionDate = DateTime.Now, PermissionTypeId = 1 } };
            
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            
            // Assert
            _unitOfWorkMock.Verify(r => r.Permissions.AddAsync(It.IsAny<Permission>()), Times.Once);
            _unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _kafkaProducerMock.Verify(x => x.PublishAsync(It.IsAny<PermissionNotificationEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void WhenCallHandle_ThrowException_ShouldLogError()
        {
            // Arrange
            var command = new RequestPermissionCommand { Permission = new PermissionDto { EmployeeLastName = "fakeLastName", EmployeeName = "fakeName", PermissionDate = DateTime.Now, PermissionTypeId = 1 } };
            _unitOfWorkMock.Setup(u => u.Permissions.AddAsync(It.IsAny<Permission>())).Throws(new Exception("Database error"));
            
            // Act & Assert
            var ex = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Error occurred while handling RequestPermissionCommand"));
        }
    }
}
