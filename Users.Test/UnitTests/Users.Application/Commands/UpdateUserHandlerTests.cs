using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Commands.UpdateUser;
using Users.Application.Queries.GetUserById;
using Users.Domain.Aggregates.User;
using Users.Domain.Exceptions;

namespace Users.Test.UnitTests.Users.Application.Commands;

[TestClass]
public class UpdateUserHandlerTests
{
    private Mock<IUserRepository> repository = null!;
    private Mock<IMediator> mediator = null!;
    private readonly CancellationToken token = new();

    private UpdateUserHandler handler = null!;

    [TestInitialize]
    public void Initialize()
    {
        repository = new();
        mediator = new();

        handler = new(repository.Object, mediator.Object);
    }

    [TestMethod]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => new UpdateUserHandler(null, mediator.Object));
    }

    [TestMethod]
    public void Constructor_WithNullMediator_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => new UpdateUserHandler(repository.Object, null));
    }

    [TestMethod]
    public async Task Handle_WithCorrectUserDtoData_CallsUpdateMethodInRepository()
    {
        // Arrange
        UpdateUserDto userDto = new()
        {
            Id = Guid.NewGuid(),
            Username = "username",
            Firstname = "firstname",
            Lastname = "lastname",
            Email = "email@email.com"
        };

        UpdateUserCommand command = new(userDto);

        mediator.Setup(s => s.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new User("username", "firstName", "lastname", "email"));
        repository.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync(new User("username", "firstName", "lastname", "email"));

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        repository.Verify(s => s.Update(It.Is<User>(
            u => u.Username == userDto.Username
                && u.Firstname == userDto.Firstname
                && u.Lastname == userDto.Lastname
                && u.Email == userDto.Email)), Times.Once);
    }

    [TestMethod]
    public async Task Handle_WithNotFoundUser_ThrowsUserNotFoundException()
    {
        // Arrange
        UpdateUserDto userDto = new()
        {
            Id = Guid.NewGuid(),
            Username = "username",
            Firstname = "firstname",
            Lastname = "lastname",
            Email = "email@email.com"
        };

        UpdateUserCommand command = new(userDto);

        User? user = null;

        mediator.Setup(s => s.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        repository.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync(new User("username", "firstName", "lastname", "email"));

        // Act && Assert
        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => handler.Handle(command, token));
    }
}