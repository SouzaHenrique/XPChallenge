namespace XPChallenge.WebAPI.IntegrationTest;

public class FinancialProductControllerShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public FinancialProductControllerShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

}
