namespace XPChallenge.Infrastructure.IntegrationTests;

#pragma warning disable CA2007

public class FinancialProductShould : IClassFixture<IntegrationTestShell>
{
    readonly IntegrationTestShell Shell;
    private readonly FinancialProductRepository SUT;

    public FinancialProductShould(IntegrationTestShell shell)
    {
        Shell = shell;
        SUT = new FinancialProductRepository(Shell.mongoDatabase);
    }

    [Fact]
    public async Task GIVEN_NewFinancialProduct_WHEN_Add_THEN_Create()
    {
        // Arrange
        var financialProduct = Shell.FixtureContainer.Fixture.Create<FinancialProduct>();

        // Act
        await SUT.Add(financialProduct, Shell.CTS.Token);

        // Assert
        var allFinancialProducts = (await SUT.GetAll(Shell.CTS.Token)).ToList();
        allFinancialProducts.Should().NotBeEmpty();
        allFinancialProducts.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GIVEN_ExistingFinancialProduct_WHEN_GetAll_THEN_ReturnExistingFinancialProduct()
    {
        // Arrange
        var financialProduct = Shell.FixtureContainer.Fixture.Create<FinancialProduct>();
        await SUT.Add(financialProduct, Shell.CTS.Token);

        // Act
        var allFinancialProducts = (await SUT.GetAll(Shell.CTS.Token)).ToList();

        // Assert
        allFinancialProducts.Should().NotBeEmpty();
        allFinancialProducts.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GIVEN_ExistingFinancialProduct_WHEN_GetById_THEN_Return()
    {
        // Arrange
        var financialProduct = Shell.FixtureContainer.Fixture.Create<FinancialProduct>();
        await SUT.Add(financialProduct, Shell.CTS.Token);

        // Act
        var allFinancialProducts = (await SUT.GetAll(Shell.CTS.Token)).ToList();
        var financialProductById = await SUT.GetById(allFinancialProducts.First().Id, Shell.CTS.Token);

        // Assert
        financialProductById.Should().NotBeNull();
        financialProductById.Should().BeOfType<FinancialProduct>();
    }

    [Fact]
    public async Task GIVEN_ExistingFinancialProduct_WHEN_Update_THEN_Update()
    {
        // Arrange
        var financialProduct = Shell.FixtureContainer.Fixture.Create<FinancialProduct>();
        await SUT.Add(financialProduct, Shell.CTS.Token);

        // Act
        var allFinancialProducts = (await SUT.GetAll(Shell.CTS.Token)).ToList();
        var financialProductById = await SUT.GetById(allFinancialProducts.First().Id, Shell.CTS.Token);
        financialProductById.UpdateName("Updated Name");
        await SUT.Update(financialProductById, Shell.CTS.Token);

        // Assert
        var updatedFinancialProduct = await SUT.GetById(allFinancialProducts.First().Id, Shell.CTS.Token);
        updatedFinancialProduct.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task GIVEN_ExistingFinancialProduct_WHEN_Delete_THEN_Delete()
    {
        // Arrange
        var financialProduct = Shell.FixtureContainer.Fixture.Create<FinancialProduct>();
        await SUT.Add(financialProduct, Shell.CTS.Token);

        // Act
        var allFinancialProducts = (await SUT.GetAll(Shell.CTS.Token)).ToList();
        await SUT.Delete(allFinancialProducts.First().Id, Shell.CTS.Token);

        // Assert
        var deletedFinancialProduct = await SUT.GetById(allFinancialProducts.First().Id, Shell.CTS.Token);
        deletedFinancialProduct.Should().BeNull();
    }
}
