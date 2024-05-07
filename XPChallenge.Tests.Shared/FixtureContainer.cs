using AutoFixture;
using XPChallenge.Tests.Shared.Specimens;

namespace XPChallenge.Tests.Shared;

public class FixtureContainer
{
    public IFixture Fixture { get; }

    public FixtureContainer()
    {
        Fixture = new Fixture();
        Fixture.Customizations.Add(new CustomerGenerator(Fixture));
        Fixture.Customizations.Add(new FinancialProductGenerator(Fixture));
        Fixture.Customizations.Add(new FinancialTransactionGenerator(Fixture));
    }
}
