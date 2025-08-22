using Microsoft.Extensions.Logging;
using Moq;
using N5.Application.Interfaces;
using N5.Application.Queries.Permission.GetAll;
using N5.Domain.Entities;
using N5.Domain.Interfaces;

namespace N5.UnitTest.Application.Queries
{
    public class GetPermissionByIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IKafkaProducer> _kafkaProducerMock;
        private Mock<ILogger<GetPermissionByIdQueryHandler>> _loggerMock;
        private GetPermissionByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _kafkaProducerMock = new Mock<IKafkaProducer>();
            var permissionRepoMock = new Mock<IPermissionRepository>();

            _loggerMock = new Mock<ILogger<GetPermissionByIdQueryHandler>>();
            _handler = new GetPermissionByIdQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _kafkaProducerMock.Object);

            _unitOfWorkMock.Setup(u => u.Permissions).Returns(permissionRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Permission { Id = 1, EmployeeForename = "fakeName", EmployeeSurname = "fakeLastName", PermissionDate = DateTime.Now, PermissionTypeId = 1 });
        }

        [Test]
        public async Task WhenCallHandler_GetPermissionByIdQuery_ShouldReturnPermissionDto()
        {
            // Arrange
            var query = new GetPermissionByIdQuery(1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(r => r.Permissions.GetByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("fakeName", result.EmployeeName);
            Assert.AreEqual("fakeLastName", result.EmployeeLastName);
            Assert.AreEqual(1, result.PermissionTypeId);
        }

        [Test]
        public async Task WhenCallHandler_ReturnNull_IfPermissionNotFound()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Permission)null);
            var query = new GetPermissionByIdQuery(1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(r => r.Permissions.GetByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNull(result);
        }
    }
}
