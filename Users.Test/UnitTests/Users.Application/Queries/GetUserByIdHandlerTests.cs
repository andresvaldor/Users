using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Queries.GetUserById;
using Users.Domain.Aggregates.User;
using Users.Domain.Exceptions;

namespace Users.Test.UnitTests.Users.Application.Queries;

[TestClass]
public class GetUserByIdHandlerTests
{
    private Mock<IUserRepository> repository = null!;
    private readonly CancellationToken token = new();

    private GetUserByIdHandler handler = null!;

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
        Assert.ThrowsException<ArgumentNullException>(() => new GetUserByIdHandler(null));
    }

    [TestMethod]
    public async Task Handle_WithNotFoundUser_ThrowsUserNotFoundException()
    {
        // Arrange
        GetUserByIdDto userDto = new()
        {
            Id = Guid.NewGuid()
        };

        GetUserByIdQuery query = new(userDto);

        User? user = null;

        repository.Setup(s => s.Get(It.IsAny<Guid>())).ReturnsAsync(user);

        // Act && Assert
        await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => handler.Handle(query, token));
    }

    [TestMethod]
    public async Task Handle_WithCorrectUserDtoData_CallsGetMethodInRepository()
    {
        // Arrange
        GetUserByIdDto userDto = new()
        {
            Id = Guid.NewGuid()
        };

        GetUserByIdQuery query = new(userDto);

        repository.Setup(s => s.Get(It.IsAny<Guid>())).ReturnsAsync(new User("username", "firstName", "lastname", "email"));

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        repository.Verify(s => s.Get(It.Is<Guid>(g => g == userDto.Id)), Times.Once);
    }
}