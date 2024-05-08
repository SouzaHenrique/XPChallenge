using FluentAssertions;
using XPChallenge.Application.Features.Customer.Commands;

namespace XPChallenge.WebAPI.IntegrationTest;

public class SalesControllerShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SalesControllerShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact(Skip = "need further setup")]
    public async Task GIVEN_SalesController_WHEN_PostRequestReceived_THEN_ReturnsCreated()
    {
        var client = _factory.CreateClient();

        SellFinancialProductCommand request = new SellFinancialProductCommand
        {
            CustomerId = new Guid("F9BA9FB5-DCCD-4997-A699-727482D0DF00"),
            FinancialProductId = new Guid("45678A57-35B1-4029-B794-F859719ABB67"),
            Quantity = 1
        };

        //act
        var response = await client.PostAsJsonAsync("/sales", request);

        //assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

    }
}
