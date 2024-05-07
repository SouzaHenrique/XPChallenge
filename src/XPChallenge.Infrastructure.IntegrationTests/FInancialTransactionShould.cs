using XPChallenge.Domain.Enums;

namespace XPChallenge.Infrastructure.IntegrationTests;
public class FInancialTransactionShould : IClassFixture<IntegrationTestShell>
{
    readonly IntegrationTestShell Shell;
    private readonly FinancialTransactionRepository SUT;

    public FInancialTransactionShould(IntegrationTestShell shell)
    {
        Shell = shell;
        SUT = new FinancialTransactionRepository(Shell.mongoDatabase);
    }

    [Fact]
    public async Task GIVEN_NewFinancialTransaction_WHEN_Add_THEN_Create()
    {
        // Arrange
        var financialTransaction = Shell.FixtureContainer.Fixture.Create<FinancialTransaction>();

        // Act
        await SUT.Add(financialTransaction, Shell.CTS.Token).ConfigureAwait(false);

        // Assert
        var allFinancialTransactions = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();
        allFinancialTransactions.Should().NotBeEmpty();
        allFinancialTransactions.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GIVEN_ExistingFinancialTransaction_WHEN_GetAll_THEN_ReturnExistingFinancialTransaction()
    {
        // Arrange
        var financialTransaction = Shell.FixtureContainer.Fixture.Create<FinancialTransaction>();
        await SUT.Add(financialTransaction, Shell.CTS.Token).ConfigureAwait(false);

        // Act
        var allFinancialTransactions = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();

        // Assert
        allFinancialTransactions.Should().NotBeEmpty();
        allFinancialTransactions.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GIVEN_ExistingFinancialTransaction_WHEN_GetById_THEN_Return()
    {
        // Arrange
        var financialTransaction = Shell.FixtureContainer.Fixture.Create<FinancialTransaction>();
        await SUT.Add(financialTransaction, Shell.CTS.Token).ConfigureAwait(false);

        // Act
        var allFinancialTransactions = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();
        var financialTransactionById = await SUT.GetById(allFinancialTransactions.First().Id, Shell.CTS.Token).ConfigureAwait(false);

        // Assert
        financialTransactionById.Should().NotBeNull();
        financialTransactionById.Id.Should().Be(allFinancialTransactions.First().Id);
    }

    [Fact]
    public async Task GIVEN_ExistingFinancialTransaction_WHEN_Update_THEN_Update()
    {
        // Arrange
        var financialTransaction = Shell.FixtureContainer.Fixture.Create<FinancialTransaction>();
        await SUT.Add(financialTransaction, Shell.CTS.Token).ConfigureAwait(false);

        // Act
        var allFinancialTransactions = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();
        var financialTransactionById = await SUT.GetById(allFinancialTransactions.First().Id, Shell.CTS.Token).ConfigureAwait(false);

        // Update the transaction type from Sale to Purchase
        financialTransactionById = new FinancialTransaction(financialTransactionById.Id, TransactionTypeEnum.Purchase,
                                                            financialTransactionById.Quantity, financialTransactionById.UnitPrice,
                                                            financialTransactionById.TransactionDate, financialTransactionById.CustomerID);

        await SUT.Update(financialTransactionById, Shell.CTS.Token).ConfigureAwait(false);

        // Assert
        var updatedFinancialTransaction = await SUT.GetById(allFinancialTransactions.First().Id, Shell.CTS.Token).ConfigureAwait(false);
        updatedFinancialTransaction.TransactionType.Should().Be(TransactionTypeEnum.Purchase);
    }

    [Fact]
    public async Task GIVEN_ExistingFinancialTransaction_WHEN_Delete_THEN_Delete()
    {
        // Arrange
        var financialTransaction = Shell.FixtureContainer.Fixture.Create<FinancialTransaction>();
        await SUT.Add(financialTransaction, Shell.CTS.Token).ConfigureAwait(false);

        // Act
        var allFinancialTransactions = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();
        await SUT.Delete(allFinancialTransactions.First().Id, Shell.CTS.Token).ConfigureAwait(false);

        // Assert
        var deletedFinancialTransaction = await SUT.GetById(allFinancialTransactions.First().Id, Shell.CTS.Token).ConfigureAwait(false);
        deletedFinancialTransaction.Should().BeNull();
    }

}
