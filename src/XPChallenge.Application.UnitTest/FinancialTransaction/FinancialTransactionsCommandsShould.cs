using XPChallenge.Application.Features.FinancialTransaction.Commands;

namespace XPChallenge.Application.UnitTest.FinancialTransaction;

public class FinancialTransactionsCommandsShould
{
    private readonly Mock<IFinancialProductRepository> _financialProductRepository;
    private readonly List<Domain.Entities.FinancialTransaction> _financialTransactions = new();

    public FinancialTransactionsCommandsShould()
    {
        _financialProductRepository = new Mock<IFinancialProductRepository>();
    }

    [Fact]
    public async Task GIVEN_CreateFinancialTransactionCommand_WHEN_Handle_THEN_CreateFinancialTransaction()
    {
        //arrange
        var financialProduct = new Domain.Entities.FinancialProduct(Guid.NewGuid(), "Product", purchasePrice: 100, DateTime.Now);
        _financialProductRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                   .Returns(Task.FromResult(financialProduct));

        var command = new CreateFinancialTransactionCommand
        {
            FinancialProductID = financialProduct.Id,
            Quantity = 10,
            UnitPrice = financialProduct.CurrentPurchasePrice,
            TransactionType = Domain.Enums.TransactionTypeEnum.Purchase,
            CustomerId = Guid.NewGuid()
        };

        var transactionRepository = new Mock<IFinancialTransactionRepository>();
        transactionRepository.Setup(x => x.Add(It.IsAny<Domain.Entities.FinancialTransaction>(), It.IsAny<CancellationToken>()))
                             .Returns((Domain.Entities.FinancialTransaction transaction, CancellationToken cancellationToken) =>
                             {
                                 transaction.Id = Guid.NewGuid();
                                 _financialTransactions.Add(transaction);
                                 return Task.FromResult(transaction);
                             });

        var handler = new CreateFinancialTransactionCommand.CreateFinancialTransactionCommandHandler(transactionRepository.Object);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.Id.Should().NotBe(Guid.Empty);
        _financialTransactions.Should().NotBeEmpty();
        _financialTransactions.FirstOrDefault().Should().NotBeNull();
        _financialTransactions.FirstOrDefault()?.FinancialProductID.Should().Be(financialProduct.Id);
    }
}
