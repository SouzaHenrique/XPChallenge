using System.Net;
using FluentAssertions;
using XPChallenge.Application.Features.Customer.Commands;

namespace XPChallenge.WebAPI.IntegrationTest;

public class CustomerControllerShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CustomerControllerShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GIVEN_CustomerController_WHEN_PostRequestReceived_THEN_ReturnsCreated()
    {
        // Arrange
        var client = _factory.CreateClient();

        CreateCustomerCommand request = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var response = await client.PostAsJsonAsync("/customers", request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

    }
}
