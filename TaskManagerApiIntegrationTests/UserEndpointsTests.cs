using Azure.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Interfaces;

namespace TaskManagerApiIntegrationTests
{
    [TestFixture]
    public class UserEndpointsTests
    {
        private HttpClient _client = null!;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly User _user;
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public UserEndpointsTests()
        {

            _user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "UserTeste",
            };
            var hasher = new PasswordHasher<User>();
            _user.PasswordHash = hasher.HashPassword(_user, "testpassword");

            _userRepositoryMock
            .Setup(r => r.GetByUsernameAsync("UserTeste"))
            .ReturnsAsync(_user);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:7015/")
            };
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnUnauthorized_WhenNotAuthenticated()
        {
            var response = await _client.GetAsync("/User/GetAllUsers/False");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}