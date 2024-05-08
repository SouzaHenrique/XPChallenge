using FluentAssertions;
using XPChallenge.Application.Features.Customer.Commands;

namespace XPChallenge.WebAPI.IntegrationTest;

public class PurchaseControllerShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PurchaseControllerShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact(Skip = "need further setup")]
    public async Task GIVEN_PurchaseController_WHEN_PostRequestReceived_THEN_ReturnsCreated()
    {
        // Arrange
        var client = _factory.CreateClient();

        PurchaseFinancialProductCommand request = new PurchaseFinancialProductCommand
        {
            CustomerId = new Guid("F9BA9FB5-DCCD-4997-A699-727482D0DF00"),
            FinancialProductId = new Guid("45678A57-35B1-4029-B794-F859719ABB67"),
            Quantity = 1
        };

        // Act
        var response = await client.PostAsJsonAsync("/purchases", request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

    }
}
