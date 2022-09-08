using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Users.API.Contracts.v1.Requests;
using Users.API.Contracts.v1.Responses;
using Users.Infrastructure.Data;
using Users.Infrastructure.Data.Models;
using Users.Test.IntegrationTests.Support;

namespace Users.Test.IntegrationTests;

[TestClass]
public class UserControllerTests
{
    private readonly Guid guid1 = Guid.NewGuid();
    private readonly Guid guid2 = Guid.NewGuid();

    [TestMethod]
    public async Task GetUser_WithEmptyUsernameProvided_ReturnsBadRequestResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB1");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username=")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(400, resultContent.Status);
        Assert.AreEqual("The Username field is required.", resultContent.Errors?.Username.First.Value);
    }

    [TestMethod]
    public async Task GetUser_WithNotExistingUsernameProvided_ReturnsNotFoundResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB2");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username=notExistingUser")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1001, resultContent.Code);
        Assert.AreEqual("User not found in the system", resultContent.Message);
    }

    [TestMethod]
    public async Task GetUser_WithExistingUsernameProvided_ReturnsUserInformation()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB3");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username=testUser")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<GetUserResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual("testUser", resultContent.Username);
        Assert.AreEqual("testFirstName", resultContent.Firstname);
        Assert.AreEqual("testLastName", resultContent.Lastname);
        Assert.AreEqual("test@testmail.com", resultContent.Email);
    }

    [TestMethod]
    public async Task CreateUser_WithUsernameNotProvided_ReturnsBadRequestResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB4");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        CreateUserRequest user = new()
        {
            Username = "",
            Firstname = "firstName",
            Lastname = "lastName",
            Email = "test@email.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1000, resultContent.Code);
        Assert.AreEqual("The username must be provided", resultContent.Message);
    }

    [TestMethod]
    public async Task CreateUser_WithFirstnameNotProvided_ReturnsBadRequestResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB5");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        CreateUserRequest user = new()
        {
            Username = "username",
            Firstname = "",
            Lastname = "lastName",
            Email = "test@email.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1002, resultContent.Code);
        Assert.AreEqual("The user first name must be provided", resultContent.Message);
    }

    [TestMethod]
    public async Task CreateUser_WithLastnameNotProvided_ReturnsBadRequestResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB6");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        CreateUserRequest user = new()
        {
            Username = "username",
            Firstname = "firstname",
            Lastname = "",
            Email = "test@email.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1003, resultContent.Code);
        Assert.AreEqual("The user last name must be provided", resultContent.Message);
    }

    [TestMethod]
    public async Task CreateUser_WithEmailNotProvided_ReturnsBadRequestResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB7");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        CreateUserRequest user = new()
        {
            Username = "username",
            Firstname = "firstname",
            Lastname = "lastname",
            Email = ""
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1004, resultContent.Code);
        Assert.AreEqual("The user email must be provided", resultContent.Message);
    }

    [TestMethod]
    public async Task CreateUser_WithWrongEmailFormat_ReturnsBadRequestResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB8");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        CreateUserRequest user = new()
        {
            Username = "username",
            Firstname = "firstname",
            Lastname = "lastname",
            Email = "wrongEmailFormat"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1004, resultContent.Code);
        Assert.AreEqual("The user email must be provided", resultContent.Message);
    }

    [TestMethod]
    public async Task CreateUser_WithCorrectNewUserProvided_ReturnsOkResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB9");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        CreateUserRequest user = new()
        {
            Username = "newUser",
            Firstname = "firstname",
            Lastname = "lastname",
            Email = "test@email.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        UserContext? context = factory.Services.GetService(typeof(UserContext)) as UserContext;
        var createdUser = context?.Users.FirstOrDefault(u => u.Username == user.Username);

        Assert.IsNotNull(createdUser);
    }

    [TestMethod]
    public async Task UpdateUser_WithNotIdProvided_ReturnsBadRequestResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB10");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        UpdateUserRequest user = new()
        {
            Id = Guid.Empty,
            Username = "username",
            Firstname = "firstname",
            Lastname = "lastname",
            Email = "test@email.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1005, resultContent.Code);
        Assert.AreEqual("The user Id must be provided", resultContent.Message);
    }

    [TestMethod]
    public async Task UpdateUser_WithNotExistingUserId_ReturnsNotFoundResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB11");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        UpdateUserRequest user = new()
        {
            Id = Guid.NewGuid(),
            Username = "username",
            Firstname = "firstname",
            Lastname = "lastname",
            Email = "test@email.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1001, resultContent.Code);
        Assert.AreEqual("User not found in the system", resultContent.Message);
    }

    [TestMethod]
    public async Task UpdateUser_WithIncorrectEmailFormat_ReturnsBadRequestResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB12");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        UpdateUserRequest user = new()
        {
            Id = guid1,
            Username = "username",
            Firstname = "firstname",
            Lastname = "lastname",
            Email = "wrongEmailFormat"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1004, resultContent.Code);
        Assert.AreEqual("The user email must be provided", resultContent.Message);
    }

    [TestMethod]
    public async Task UpdateUser_WithCorrectUsernameUpdated_ReturnsOkResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB13");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        UpdateUserRequest user = new()
        {
            Id = guid1,
            Username = "newUsername",
            Firstname = "testFirstName",
            Lastname = "testLastName",
            Email = "test@testmail.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        var getHttpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username=newUsername")
        };

        var updatedUserResponse = await client.SendAsync(getHttpRequest);
        var resultUser = JsonConvert.DeserializeObject<GetUserResponse>(await updatedUserResponse.Content.ReadAsStringAsync());
        Assert.AreEqual("newUsername", resultUser?.Username);
    }

    [TestMethod]
    public async Task UpdateUser_WithCorrectFirstnameUpdated_ReturnsOkResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB14");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        UpdateUserRequest user = new()
        {
            Id = guid1,
            Username = "testUser",
            Firstname = "newFirstname",
            Lastname = "testLastName",
            Email = "test@testmail.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        var getHttpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username=testUser")
        };

        var updatedUserResponse = await client.SendAsync(getHttpRequest);
        var resultUser = JsonConvert.DeserializeObject<GetUserResponse>(await updatedUserResponse.Content.ReadAsStringAsync());
        Assert.AreEqual("newFirstname", resultUser?.Firstname);
    }

    [TestMethod]
    public async Task UpdateUser_WithCorrectLastnameUpdated_ReturnsOkResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB15");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        UpdateUserRequest user = new()
        {
            Id = guid1,
            Username = "testUser",
            Firstname = "testFirstName",
            Lastname = "newLastName",
            Email = "test@testmail.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        var getHttpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username=testUser")
        };

        var updatedUserResponse = await client.SendAsync(getHttpRequest);
        var resultUser = JsonConvert.DeserializeObject<GetUserResponse>(await updatedUserResponse.Content.ReadAsStringAsync());
        Assert.AreEqual("newLastName", resultUser?.Lastname);
    }

    [TestMethod]
    public async Task UpdateUser_WithCorrectEmailUpdated_ReturnsOkResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB16");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        UpdateUserRequest user = new()
        {
            Id = guid1,
            Username = "testUser",
            Firstname = "testFirstName",
            Lastname = "testLastName",
            Email = "newmail@testmail.com"
        };

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user"),
            Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        var getHttpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username=testUser")
        };

        var updatedUserResponse = await client.SendAsync(getHttpRequest);
        var resultUser = JsonConvert.DeserializeObject<GetUserResponse>(await updatedUserResponse.Content.ReadAsStringAsync());
        Assert.AreEqual("newmail@testmail.com", resultUser?.Email);
    }

    [TestMethod]
    public async Task DeleteUser_WithUsernameNotProvided_ReturnsBadRequestResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB17");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username="),
            Content = new StringContent("{\"Username\":\"\"}", Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1000, resultContent.Code);
        Assert.AreEqual("The username must be provided", resultContent.Message);
    }

    [TestMethod]
    public async Task DeleteUser_WithUserNotFound_ReturnsNotFoundResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB18");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username="),
            Content = new StringContent("{\"Username\":\"notExistingUser\"}", Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        var resultContent = JsonConvert.DeserializeObject<ApiDomainErrorResponse>(await result.Content.ReadAsStringAsync());
        Assert.IsNotNull(resultContent, "Expected result content not to be null");
        Assert.AreEqual(1001, resultContent.Code);
        Assert.AreEqual("User not found in the system", resultContent.Message);
    }

    [TestMethod]
    public async Task DeleteUser_WithUserFound_ReturnsOkResponse()
    {
        var factory = new UsersWebApplicationFactory<Program>("TestDB19");
        var client = factory.CreateClient();

        await SeedDatabase(factory);

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username="),
            Content = new StringContent("{\"Username\":\"testUser\"}", Encoding.UTF8, "application/json")
        };

        // Act
        var result = await client.SendAsync(httpRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        var getHttpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost:5001/user-api/v1/user?Username=testUser")
        };

        var deletedUserResponse = await client.SendAsync(getHttpRequest);

        Assert.AreEqual(HttpStatusCode.NotFound, deletedUserResponse.StatusCode);
    }

    private async Task SeedDatabase(UsersWebApplicationFactory<Program> factory)
    {
        UserContext? context = factory.Services.GetService(typeof(UserContext)) as UserContext;

        List<UserDataModel> users = new()
        {
            new()
            {
                Id = guid1,
                Username = "testUser",
                Firstname = "testFirstName",
                Lastname = "testLastName",
                Email = "test@testmail.com"
            },
            new()
            {
                Id = guid2,
                Username = "testUser2",
                Firstname = "testFirstName2",
                Lastname = "testLastName2",
                Email = "test2@testmail.com"
            }
        };

        if (context is not null)
        {
            context.AddRange(users);
            await context.SaveChangesAsync();
        }
    }
}