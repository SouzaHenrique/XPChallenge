using AutoFixture;
using AutoFixture.Kernel;
using XPChallenge.Domain.Commom.Models;
using XPChallenge.Domain.Entities;

namespace XPChallenge.Tests.Shared.Specimens;

public class CustomerGenerator : ISpecimenBuilder
{
    private readonly IFixture _fixture;

    public CustomerGenerator(IFixture fixture)
    {
        _fixture = new Fixture() ;
    }

    public object Create(object request, ISpecimenContext context)
    {
        Type? type = (request as SeededRequest)?.Request as Type;

        if (type is not null && type != typeof(Customer))
            return new NoSpecimen();

        var output = new Customer(id: Guid.Empty, _fixture.Create<string>(), _fixture.Create<string>(), 0, Enumerable.Empty<PurchasedProductValueObject>());
        return output;
    }
}
