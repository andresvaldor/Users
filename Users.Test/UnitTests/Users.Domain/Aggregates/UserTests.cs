using Microsoft.VisualStudio.TestTools.UnitTesting;
using Users.Domain.Aggregates.User;
using Users.Domain.Exceptions;

namespace Users.Test.UnitTests.Users.Domain.Aggregates;

[TestClass]
public class UserTests
{
    [TestMethod]
    public void Constructor_WithNullUsername_ThrowUsernameNotFoundException()
    {
        // Act & Assert
        Assert.ThrowsException<UsernameNotProvidedException>(() => new User(null, "firstname", "lastname", "email"));
    }

    [TestMethod]
    public void Constructor_WithNullFirstname_ThrowFirstnameNotFoundException()
    {
        // Act & Assert
        Assert.ThrowsException<FirstnameNotProvidedException>(() => new User("username", null, "lastname", "email"));
    }

    [TestMethod]
    public void Constructor_WithNullLastname_ThrowLastnameNotFoundException()
    {
        Assert.ThrowsException<LastnameNotProvidedException>(() => new User("username", "firstname", null, "email"));
    }

    [TestMethod]
    public void Constructor_WithNullEmail_ThrowEmailNotFoundException()
    {
        Assert.ThrowsException<EmailNotProvidedException>(() => new User("username", "firstname", "lastname", null));
    }

    [TestMethod]
    public void Constructor_WithCorrectInputData_UserIsCreated()
    {
        // Act
        var user = new User("username", "firstname", "lastname", "email");

        // Assert
        Assert.AreEqual("username", user.Username);
        Assert.AreEqual("firstname", user.Firstname);
        Assert.AreEqual("lastname", user.Lastname);
        Assert.AreEqual("email", user.Email);
    }

    [TestMethod]
    public void Update_WithUpdatedUsername_OnlyUsernameIsUpdated()
    {
        // Arrange
        var user = new User("username", "firstname", "lastname", "email");

        // Act
        user.UpdateUser("newUsername", null, null, null);

        // Assert
        Assert.AreEqual("newUsername", user.Username);
        Assert.AreEqual("firstname", user.Firstname);
        Assert.AreEqual("lastname", user.Lastname);
        Assert.AreEqual("email", user.Email);
    }

    [TestMethod]
    public void Update_WithUpdatedFirstname_OnlyFirstnameIsUpdated()
    {
        // Arrange
        var user = new User("username", "firstname", "lastname", "email");

        // Act
        user.UpdateUser(null, "newFirstname", null, null);

        // Assert
        Assert.AreEqual("username", user.Username);
        Assert.AreEqual("newFirstname", user.Firstname);
        Assert.AreEqual("lastname", user.Lastname);
        Assert.AreEqual("email", user.Email);
    }

    [TestMethod]
    public void Update_WithUpdatedLastname_OnlyLastnameIsUpdated()
    {
        // Arrange
        var user = new User("username", "firstname", "lastname", "email");

        // Act
        user.UpdateUser(null, null, "newLastname", null);

        // Assert
        Assert.AreEqual("username", user.Username);
        Assert.AreEqual("firstname", user.Firstname);
        Assert.AreEqual("newLastname", user.Lastname);
        Assert.AreEqual("email", user.Email);
    }

    [TestMethod]
    public void Update_WithUpdatedEmail_OnlyEmailIsUpdated()
    {
        // Arrange
        var user = new User("username", "firstname", "lastname", "email");

        // Act
        user.UpdateUser(null, null, null, "newEmail");

        // Assert
        Assert.AreEqual("username", user.Username);
        Assert.AreEqual("firstname", user.Firstname);
        Assert.AreEqual("lastname", user.Lastname);
        Assert.AreEqual("newEmail", user.Email);
    }
}