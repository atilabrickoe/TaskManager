﻿using MediatR;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Exceptions;
using TaskManagerDomain.Interfaces;
using Bogus;

namespace TaskManagerApplication.Tasks.Commands.Users.CreateRandomUsers
{
    public class CreateRandomUsersCommandHandler : IRequestHandler<CreateRandomUsersCommandRequest, CreateRandomUsersCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly Faker<User> _faker;


        public CreateRandomUsersCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _faker = new Faker<User>()
                            .RuleFor(u => u.Id, f => Guid.NewGuid())
                            .RuleFor(u => u.UserName, f => $"user_{f.Name.FirstName()}");
        }

        public async Task<CreateRandomUsersCommandResponse> Handle(CreateRandomUsersCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var response = new CreateRandomUsersCommandResponse()
                {
                    Users = new List<User>()
                };

                for (int i = 0; i < request.Amount; i++)
                {
                    User fakeUser = _faker.Generate();

                    fakeUser.IsValid();

                    response.Users.Add(await _userRepository.CreateAsync(fakeUser));
                }

                response.Success = true;

                return response;
            }
            catch (WrongRequiredInformation ex)
            {
                return new CreateRandomUsersCommandResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_INFORMATION,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new CreateRandomUsersCommandResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = ex.Message
                };
            }
        }
    }
}