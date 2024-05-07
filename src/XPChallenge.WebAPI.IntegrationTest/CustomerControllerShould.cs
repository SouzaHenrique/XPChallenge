namespace XPChallenge.WebAPI.IntegrationTest;

public class CustomerControllerShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CustomerControllerShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

}
