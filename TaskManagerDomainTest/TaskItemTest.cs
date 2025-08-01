using FluentAssertions;
using NUnit.Framework;
using System.Reflection;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;

namespace TaskManagerDomainTest
{
    public class TaskItemTest
    {
        [Test]
        public void ShouldThrow_WhenTitleIsNullOrEmpty()
        {
            var task = new TaskItem
            {
                Title = "",
                DueDate = DateTime.Now.AddDays(1),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "testUser"
                }
            };

            Action act = () => task.IsValid();

            act.Should()
                .Throw<WrongRequiredInformation>()
                .WithMessage("Title cannot be null or empty.");
        }

        [Test]
        public void ShouldThrow_WhenDueDateIsInThePast()
        {
            var task = new TaskItem
            {
                Title = "test",
                DueDate = DateTime.Now.AddDays(-1),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "testUser"
                }
            };

            Action act = () => task.IsValid();

            act.Should()
                .Throw<WrongRequiredInformation>()
                .WithMessage("Due date cannot be in the past.");
        }

        [Test]
        public void ShouldNotThrow_WhenValidDataProvided()
        {
            var task = new TaskItem
            {
                Title = "test",
                DueDate = DateTime.Now.AddDays(5),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "testUser"
                }
            };

            Action act = () => task.IsValid();

            act.Should().NotThrow();
        }
        [Test]
        public void CanNotAssociate_WhenUserIsAlreadyAssociatedShouldThrow()
        {
            // Arrange
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                DueDate = DateTime.Now.AddDays(1),
                User = new User { Id = Guid.NewGuid(), UserName = "ExistingUser" }
            };

            // Use reflection to invoke private method
            var method = typeof(TaskItem)
                .GetMethod("CanAssociate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            Action act = () => method.Invoke(task, null);

            // Assert
            act.Should()
               .Throw<TargetInvocationException>() // reflection wraps the real exception
               .WithInnerException<TaskAlreadyAssociatedException>()
               .WithMessage("This task is already associated with a user.");
        }

        [Test]
        public void CanAssociate_ShouldNotThrow_WhenUserIsNull()
        {
            // Arrange
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                DueDate = DateTime.Now.AddDays(1),
                User = null
            };

            var method = typeof(TaskItem)
                .GetMethod("CanAssociate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            Action act = () => method.Invoke(task, null);

            // Assert
            act.Should().NotThrow();
        }
    }
}
