using AutoFixture;
using AutoFixture.Kernel;
using XPChallenge.Domain.Entities;

namespace XPChallenge.Tests.Shared.Specimens;
public class FinancialTransactionGenerator : ISpecimenBuilder
{
    private IFixture _fixture;

    public FinancialTransactionGenerator(IFixture fixture)
    {
        _fixture = new Fixture();
    }


    public object Create(object request, ISpecimenContext context)
    {
        Type? type = (request as SeededRequest)?.Request as Type;


        if (type is not null && type != typeof(FinancialTransaction))
            return new NoSpecimen();

        var output = new FinancialTransaction(Guid.NewGuid(), transactionType: Domain.Enums.TransactionTypeEnum.Purchase,
                                             quantity: 10, unitPrice: 100, DateTime.Now, Guid.NewGuid());

        return output;
    }
}
