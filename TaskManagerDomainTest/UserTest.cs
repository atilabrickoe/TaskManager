using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;

namespace TaskManagerDomainTest
{
    public class UserTest
    {
        [Test]
        public void ShouldThrow_WhenUserNameIsNullOrEmpty()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "",
                PasswordHash = "hash"
            };

            Action act = () => user.IsValid();

            act.Should()
               .Throw<WrongRequiredInformation>()
               .WithMessage("UserName cannot be null or empty.");
        }

        [Test]
        public void ShouldThrow_WhenIdUserIsEmpty()
        {
            var user = new User
            {
                Id = Guid.Empty,
                UserName = "userTest",
                PasswordHash = "hash"
            };

            Action act = () => user.IsValid();

            act.Should()
               .Throw<WrongRequiredInformation>()
               .WithMessage("Id cannot be empty.");
        }

        [Test]
        public void ShouldNotThrow_WhenUserIsValid()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "userTest",
                PasswordHash = "hash"
            };

            Action act = () => user.IsValid();

            act.Should().NotThrow();
        }
    }
}
