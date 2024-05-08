using System.Net;
using FluentAssertions;
using XPChallenge.Application.Features.FinancialProduct.Commands;

namespace XPChallenge.WebAPI.IntegrationTest;

public class FinancialProductControllerShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public FinancialProductControllerShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GIVEN_FinancialProductController_WHEN_RequestReceived_THEN_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        CreateProductCommand request = new CreateProductCommand
        {
            Name = "Test Product",
            CurrentPurchasePrice = 100,
            DueDate = DateTime.Now.AddDays(30),
        };

        // Act
        var response = await client.PostAsJsonAsync("/products", request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

}
