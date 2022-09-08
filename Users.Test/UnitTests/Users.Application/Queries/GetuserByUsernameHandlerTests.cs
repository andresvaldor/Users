using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Queries.GetUserByUsername;
using Users.Domain.Aggregates.User;
using Users.Domain.Exceptions;

namespace Users.Test.UnitTests.Users.Application.Queries;

[TestClass]
public class GetUserByUsernameHandlerTests
{
    private Mock<IUserRepository> repository = null!;
    private readonly CancellationToken token = new();

    private GetUserByUserNameHandler handler = null!;

    [TestInitialize]
    public void Initialize()
    {
        repository = new();

        handler = new(repository.Object);
    }

    [TestMethod]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => new GetUserByUserNameHandler(null));
    }

    [TestMethod]
    public async Task Handle_WithNotFoundUser_ThrowsUserNotFoundException()
    {
        // Arrange
        GetUserByUsernameDto userDto = new()
        {
            Username = "username"
        };

        GetUserByUsernameQuery query = new(userDto);

        User? user = null;

        repository.Setup(s => s.GetByUsername(It.IsAny<string>())).ReturnsAsync(user);

        // Act && Assert
        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => handler.Handle(query, token));
    }

    [TestMethod]
    public async Task Handle_WithCorrectUserDtoData_CallsGetMethodInRepository()
    {
        // Arrange
        GetUserByUsernameDto userDto = new()
        {
            Username = "username"
        };

        GetUserByUsernameQuery query = new(userDto);

        repository.Setup(s => s.GetByUsername(It.Is<string>(s => s == userDto.Username))).ReturnsAsync(new User("username", "firstName", "lastname", "email"));

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        repository.Verify(s => s.GetByUsername(It.Is<string>(s => s == userDto.Username)), Times.Once);
    }
}