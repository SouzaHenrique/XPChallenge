namespace XPChallenge.Infrastructure.IntegrationTests;
public class CustomerRepositoryShould : IClassFixture<IntegrationTestShell>
{
    readonly IntegrationTestShell Shell;
    private readonly CustomerRepository SUT;

    public CustomerRepositoryShould(IntegrationTestShell shell)
    {
        Shell = shell;
        SUT = new CustomerRepository(Shell.mongoDatabase);
    }

    [Fact]
    public async Task GIVEN_NewCustomer_WHEN_Add_THEN_Create()
    {
        // Arrange
        var customer = Shell.FixtureContainer.Fixture.Create<Customer>();

        // Act
        await SUT.Add(customer, Shell.CTS.Token).ConfigureAwait(false);

        // Assert
        var allCustomers = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();
        allCustomers.Should().NotBeEmpty();
        allCustomers.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GIVEN_ExistingCustomer_WHEN_GetAll_THEN_ReturnExistingCustomer()
    {
        // Arrange
        var customer = Shell.FixtureContainer.Fixture.Create<Customer>();
        await SUT.Add(customer, Shell.CTS.Token).ConfigureAwait(false);

        // Act
        var allCustomers = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();

        // Assert
        allCustomers.Should().NotBeEmpty();
        allCustomers.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GIVEN_ExistingCustomer_WHEN_GetById_THEN_ReturnExistingCustomer()
    {
        // Arrange
        var customer = Shell.FixtureContainer.Fixture.Create<Customer>();
        await SUT.Add(customer, Shell.CTS.Token).ConfigureAwait(false);

        // Act
        var allCustomers = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();
        var customerById = await SUT.GetById(allCustomers.First().Id, Shell.CTS.Token).ConfigureAwait(false);

        // Assert
        customerById.Should().NotBeNull();
        customerById.Should().BeOfType<Customer>();
    }

    [Fact]
    public async Task GIVEN_ExistingCustomer_WHEN_Update_THEN_Update()
    {
        // Arrange
        var customer = Shell.FixtureContainer.Fixture.Create<Customer>();
        await SUT.Add(customer, Shell.CTS.Token).ConfigureAwait(false);

        // Act
        var allCustomers = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();
        var customerById = await SUT.GetById(allCustomers.First().Id, Shell.CTS.Token).ConfigureAwait(false);
        customerById.UpdateFirstName("UpdatedName");
        await SUT.Update(customerById, Shell.CTS.Token).ConfigureAwait(false);

        // Assert
        var updatedCustomer = await SUT.GetById(allCustomers.First().Id, Shell.CTS.Token).ConfigureAwait(false);
        updatedCustomer.FirstName.Should().Be("UpdatedName");
    }

    [Fact]
    public async Task GIVEN_ExistingCustomer_WHEN_Delete_THEN_Remove()
    {
        // Arrange
        var customer = Shell.FixtureContainer.Fixture.Create<Customer>();
        await SUT.Add(customer, Shell.CTS.Token).ConfigureAwait(false);

        // Act
        var allCustomers = (await SUT.GetAll(Shell.CTS.Token).ConfigureAwait(false)).ToList();
        await SUT.Delete(allCustomers.First().Id, Shell.CTS.Token).ConfigureAwait(false);

        // Assert
        var deletedCustomer = await SUT.GetById(allCustomers.First().Id, Shell.CTS.Token).ConfigureAwait(false);
        deletedCustomer.Should().BeNull();
    }
}
