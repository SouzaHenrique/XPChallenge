using AutoFixture;
using AutoFixture.Kernel;
using XPChallenge.Domain.Entities;

namespace XPChallenge.Tests.Shared.Specimens;

public class FinancialProductGenerator : ISpecimenBuilder
{
    private readonly IFixture _fixture;

    public FinancialProductGenerator(IFixture fixture)
    {
        _fixture = new Fixture();
    }

    public object Create(object request, ISpecimenContext context)
    {
        Type? type = (request as SeededRequest)?.Request as Type;

        if (type is not null && type != typeof(FinancialProduct))
            return new NoSpecimen();

        var output = new FinancialProduct(Guid.Empty, _fixture.Create<string>(), _fixture.Create<decimal>(), DateTime.Now.AddDays(30));

        return output;
    }
}
