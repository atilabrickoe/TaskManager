using FluentAssertions;
using Moq;
using NUnit.Framework;
using TaskManagerApplication;
using TaskManagerApplication.Tasks.Commands.Tasks.AssociateTaskToUser;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApplicationTest.Tasks.Commands
{
    public class AssociateTaskToUserCommandHandlerTest
    {
        private Mock<ITaskRepository> _taskRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<INotificationService> _notificationServiceMock;
        private AssociateTaskToUserCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _notificationServiceMock = new Mock<INotificationService>();

            _handler = new AssociateTaskToUserCommandHandler(
                _taskRepositoryMock.Object,
                _notificationServiceMock.Object,
                _userRepositoryMock.Object
            );
        }
        [Test]
        public async Task ShouldAssociateTaskSuccessfully()
        {
            var request = new AssociateTaskToUserComandRequest { UserId = Guid.NewGuid(), TaskId = Guid.NewGuid() };
            var user = new User { Id = request.UserId, UserName = "TestUser" };
            var task = new TaskItem { Id = request.TaskId, Title = "Test Task" };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(request.UserId)).ReturnsAsync(user);
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(request.TaskId)).ReturnsAsync(task);

            var result = await _handler.Handle(request, CancellationToken.None);

            //assert
            result.Success.Should().BeTrue();
            result.Task.Title.Should().Be("Test Task");
            _taskRepositoryMock.Verify(r => r.UpdateAsync(task), Times.Once);
            _notificationServiceMock.Verify(n => n.NotifyUserAsync(user.Id, It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnUserNotFound_WhenUserDoesNotExist()
        {
            var request = new AssociateTaskToUserComandRequest { UserId = Guid.NewGuid(), TaskId = Guid.NewGuid() };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(request.UserId)).ReturnsAsync((User)null);

            var result = await _handler.Handle(request, CancellationToken.None);

            //assert
            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be(ErrorCodes.USER_NOT_FOUND);
        }

        [Test]
        public async Task ShouldReturnTaskNotFound_WhenTaskDoesNotExist()
        {
            var request = new AssociateTaskToUserComandRequest { UserId = Guid.NewGuid(), TaskId = Guid.NewGuid() };
            var user = new User { Id = request.UserId, UserName = "TestUser" };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(request.UserId)).ReturnsAsync(user);
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(request.TaskId)).ReturnsAsync((TaskItem)null);

            var result = await _handler.Handle(request, CancellationToken.None);

            //assert
            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be(ErrorCodes.TASK_NOT_FOUND);
        }

        [Test]
        public async Task ShouldReturnError_WhenWrongRequiredInformationExceptionThrown()
        {
            var request = new AssociateTaskToUserComandRequest { UserId = Guid.NewGuid(), TaskId = Guid.NewGuid() };
            var user = new User { Id = request.UserId, UserName = "TestUser" };
            var task = new TaskItem { Id = request.TaskId, Title = "Test Task" };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(request.UserId)).ReturnsAsync(user);
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(request.TaskId)).ReturnsAsync(task);

            _taskRepositoryMock.Setup(r => r.UpdateAsync(task)).ThrowsAsync(new WrongRequiredInformation("error"));

            var result = await _handler.Handle(request, CancellationToken.None);

            //assert
            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be(ErrorCodes.MISSING_INFORMATION);
        }

        [Test]
        public async Task ShouldReturnError_WhenUnexpectedExceptionThrown()
        {
            var request = new AssociateTaskToUserComandRequest { UserId = Guid.NewGuid(), TaskId = Guid.NewGuid() };
            var user = new User { Id = request.UserId, UserName = "TestUser" };
            var task = new TaskItem { Id = request.TaskId, Title = "Test Task" };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(request.UserId)).ReturnsAsync(user);
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(request.TaskId)).ReturnsAsync(task);
            _taskRepositoryMock.Setup(r => r.UpdateAsync(task)).ThrowsAsync(new Exception("unexpected"));

            var result = await _handler.Handle(request, CancellationToken.None);

            //assert
            result.Success.Should().BeFalse();
            result.ErrorCode.Should().Be(ErrorCodes.INTERNAL_SERVER_ERROR);
        }
    }
}