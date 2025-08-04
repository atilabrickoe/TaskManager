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
               .WithMessage("Nome de usuário não pode ser nulo ou vazio.");
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
               .WithMessage("O ID não pode estar vazio.");
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
