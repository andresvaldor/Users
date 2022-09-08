using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Commands.DeleteUser;
using Users.Application.Queries.GetUserByUsername;
using Users.Domain.Aggregates.User;
using Users.Domain.Exceptions;

namespace Users.Test.UnitTests.Users.Application.Commands;

[TestClass]
public class DeleteUserHandlerTests
{
    private Mock<IUserRepository> repository = null!;
    private Mock<IMediator> mediator = null!;
    private readonly CancellationToken token = new();

    private DeleteUserHandler handler = null!;

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
        Assert.ThrowsException<ArgumentNullException>(() => new DeleteUserHandler(null, mediator.Object));
    }

    [TestMethod]
    public void Constructor_WithNullMediator_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => new DeleteUserHandler(repository.Object, null));
    }

    [TestMethod]
    public async Task Handle_WithCorrectUserDtoData_CallsDeleteMethodInRepository()
    {
        // Arrange
        DeleteUserDto userDto = new()
        {
            Username = "username"
        };

        DeleteUserCommand command = new(userDto);

        mediator.Setup(s => s.Send(It.IsAny<GetUserByUsernameQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new User("username", "firstName", "lastname", "email"));
        repository.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync(new User("username", "firstName", "lastname", "email"));

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        repository.Verify(s => s.Delete(It.Is<User>(u => u.Username == userDto.Username)), Times.Once);
    }

    [TestMethod]
    public async Task Handle_WithNotFoundUser_ThrowsUserNotFoundException()
    {
        // Arrange
        DeleteUserDto userDto = new()
        {
            Username = "username"
        };

        DeleteUserCommand command = new(userDto);

        User? user = null;

        mediator.Setup(s => s.Send(It.IsAny<GetUserByUsernameQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        repository.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync(new User("username", "firstName", "lastname", "email"));

        // Act && Assert
        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => handler.Handle(command, token));
    }
}