using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using N5.API.Controllers;
using N5.Application.Commands.Permission.Modify;
using N5.Application.Commands.Permission.Request;
using N5.Application.DTOs.Permission;
using N5.Application.Queries.Permission.GetAll;

namespace N5.UnitTest.API
{
    public class PermissionsControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private PermissionsController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();

            _controller = new PermissionsController(_mediatorMock.Object);
        }

        [Test]
        public async Task WhenRequestPermission_ShouldReturnOk_WhenPermissionDtoIsValid()
        {
            // Arrange
            var permissionDto = new PermissionDto
            {
                EmployeeName = "John",
                EmployeeLastName = "Doe",
                PermissionDate = DateTime.Now,
                PermissionTypeId = 1
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<RequestPermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            // Act
            var result = await _controller.RequestPermission(permissionDto);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(1, okResult.Value);
        }

        [Test]
        public async Task WhenRequestPermission_ThrowsBadRequest_WhenPermissionDtoIsNull()
        {
            // Arrange
            PermissionDto permissionDto = null;

            // Act
            var result = await _controller.RequestPermission(permissionDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Permission data is required.", badRequestResult.Value);
        }

        [Test]
        public async Task WhenRequestPermission_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var permissionDto = new PermissionDto
            {
                EmployeeName = "John",
                EmployeeLastName = "Doe",
                PermissionDate = DateTime.Now,
                PermissionTypeId = 1
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<RequestPermissionCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.RequestPermission(permissionDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("An error occurred while processing your request.", objectResult.Value);
        }

        [Test]
        public async Task WhenModifyPermission_ShouldReturnOk_WhenPermissionDtoIsValid()
        {
            // Arrange
            var permissionDto = new PermissionDto
            {
                Id = 1,
                EmployeeName = "John",
                EmployeeLastName = "Doe",
                PermissionDate = DateTime.Now,
                PermissionTypeId = 1
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ModifyPermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.ModifyPermission(permissionDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(1, okResult.Value);
        }

        [Test]
        public async Task WhenModifyPermission_ThrowsBadRequest_WhenPermissionDtoIsNull()
        {
            // Arrange
            PermissionDto permissionDto = null;

            // Act
            var result = await _controller.ModifyPermission(permissionDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Valid permission data with ID is required for modification.", badRequestResult.Value);
        }

        [Test]
        public async Task WhenModifyPermission_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var permissionDto = new PermissionDto
            {
                Id = 1,
                EmployeeName = "John",
                EmployeeLastName = "Doe",
                PermissionDate = DateTime.Now,
                PermissionTypeId = 1
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ModifyPermissionCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.ModifyPermission(permissionDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("An error occurred while processing your request.", objectResult.Value);
        }

        [Test]
        public async Task WhenModifyPermission_ReturnNotFound_WhenPermissionDoesNotExist()
        {
            // Arrange
            var permissionDto = new PermissionDto
            {
                Id = 1,
                EmployeeName = "John",
                EmployeeLastName = "Doe",
                PermissionDate = DateTime.Now,
                PermissionTypeId = 1
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ModifyPermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.ModifyPermission(permissionDto);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task WhenGetPermissionsByEmployeeId_ShouldReturnOk_WhenPermissionsExist()
        {
            // Arrange
            int employeeId = 1;
            var permission = new PermissionDto { Id = 1, EmployeeName = "John", EmployeeLastName = "Doe", PermissionDate = DateTime.Now, PermissionTypeId = 1 }
            ;
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permission);

            // Act
            var result = await _controller.GetPermission(employeeId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(permission, okResult.Value);
        }
    }
}