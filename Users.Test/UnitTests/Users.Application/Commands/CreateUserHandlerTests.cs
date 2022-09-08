using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Users.Application.Commands.CreateUser;
using Users.Domain.Aggregates.User;

namespace Users.Test.UnitTests.Users.Application.Commands;

[TestClass]
public class CreateUserHandlerTests
{
    private Mock<IUserRepository> repository = null!;
    private readonly CancellationToken token = new();

    private CreateUserHandler handler = null!;

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
        Assert.ThrowsException<ArgumentNullException>(() => new CreateUserHandler(null));
    }

    [TestMethod]
    public async Task Handle_WithCorrectUserDtoData_CallsAddMethodInRepository()
    {
        // Arrange
        CreateUserDto userDto = new()
        {
            Username = "username",
            Firstname = "firstName",
            Lastname = "lastname",
            Email = "email"
        };

        CreateUserCommand command = new(userDto);

        repository.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync(new User("username", "firstName", "lastname", "email"));

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        repository.Verify(s => s.Add(It.Is<User>(
            u => u.Username == userDto.Username
                && u.Firstname == userDto.Firstname
                && u.Lastname == userDto.Lastname
                && u.Email == userDto.Email)), Times.Once);
    }

    [TestMethod]
    public async Task Handle_WithCorrectUserDtoData_ReturnsDomainModelUser()
    {
        // Arrange
        CreateUserDto userDto = new()
        {
            Username = "username",
            Firstname = "firstName",
            Lastname = "lastname",
            Email = "email"
        };

        CreateUserCommand command = new(userDto);

        repository.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync(new User("username", "firstName", "lastname", "email"));

        // Act
        var result = await handler.Handle(command, token);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userDto.Username, result.Username);
        Assert.AreEqual(userDto.Firstname, result.Firstname);
        Assert.AreEqual(userDto.Lastname, result.Lastname);
        Assert.AreEqual(userDto.Email, result.Email);
    }
}