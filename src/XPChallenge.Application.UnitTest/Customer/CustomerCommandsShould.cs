namespace XPChallenge.Application.UnitTest.Customer;
public class CustomerCommandsShould
{
    private readonly Mock<ICustomerRepository> _customerRepository;
    private readonly Mock<IFinancialProductRepository> _financialProductRepository;

    public CustomerCommandsShould()
    {
        _customerRepository = new Mock<ICustomerRepository>();
        _financialProductRepository = new Mock<IFinancialProductRepository>();
    }

    [Fact]
    public async Task GIVEN_NewCustomer_WHEN_CreatingCustomer_Then_CreateCustomer()
    {
        //arrange
        _customerRepository.Setup(x => x.Add(It.IsAny<Domain.Entities.Customer>(), It.IsAny<CancellationToken>()))
                           .Returns((Domain.Entities.Customer customer, CancellationToken cancellationToken) =>
                           {
                               customer = new Domain.Entities.Customer(Guid.NewGuid(), customer.FirstName, customer.LastName, customer.Balance, []);
                               return Task.FromResult(customer);
                           });

        var createCustomerCommand = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Balance = 100
        };

        //act
        var createCustomerCommandHandler = new CreateCustomerCommand.CreateCustomerCommandHandler(_customerRepository.Object);
        var result = await createCustomerCommandHandler.Handle(createCustomerCommand, CancellationToken.None);

        //assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task GIVEN_Customer_WHEN_BalanceIncreased_THEN_UpdateCustomerBalance()
    {
        //arrange
        var customer = new Domain.Entities.Customer(Guid.NewGuid(), "John", "Doe", 100, []);
        _customerRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.FromResult(customer));

        var updateCustomerBalanceCommand = new UpdateCustomerBalanceCommand
        {
            Id = customer.Id,
            Amount = 100
        };

        //act
        var updateCustomerBalanceCommandHandler = new UpdateCustomerBalanceCommand.UpdateCustomerBalanceCommandHandler(_customerRepository.Object);
        var result = await updateCustomerBalanceCommandHandler.Handle(updateCustomerBalanceCommand, CancellationToken.None);

        //assert
        result.Should().NotBeNullOrEmpty();
        customer.Balance.Should().Be(200);
    }

    [Fact]
    public async Task GIVEN_Customer_WHEN_BalanceDecreased_THEN_UpdateCustomerBalance()
    {
        //arrange
        var customer = new Domain.Entities.Customer(Guid.NewGuid(), "John", "Doe", 100, []);
        _customerRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.FromResult(customer));

        var updateCustomerBalanceCommand = new UpdateCustomerBalanceCommand
        {
            Id = customer.Id,
            Amount = -100
        };

        //act
        var updateCustomerBalanceCommandHandler = new UpdateCustomerBalanceCommand.UpdateCustomerBalanceCommandHandler(_customerRepository.Object);
        var result = await updateCustomerBalanceCommandHandler.Handle(updateCustomerBalanceCommand, CancellationToken.None);

        //assert
        result.Should().NotBeNullOrEmpty();
        customer.Balance.Should().Be(0);
    }

    [Fact]
    public async Task GIVEN_Customer_WHEN_BalanceDecreasedBelowZero_THEN_DontUpdateCustomerBalance()
    {
        //arrange
        var customer = new Domain.Entities.Customer(Guid.NewGuid(), "John", "Doe", 0, []);
        _customerRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.FromResult(customer));

        var updateCustomerBalanceCommand = new UpdateCustomerBalanceCommand
        {
            Id = customer.Id,
            Amount = -100
        };

        //act
        var updateCustomerBalanceCommandHandler = new UpdateCustomerBalanceCommand.UpdateCustomerBalanceCommandHandler(_customerRepository.Object);
        var result = await updateCustomerBalanceCommandHandler.Handle(updateCustomerBalanceCommand, CancellationToken.None);

        //assert
        customer.Balance.Should().Be(0);
    }

    [Fact]
    public async Task GIVEN_Customer_WHEN_ProductPurchased_THEN_AddToPurchasedProductsAndUpdateBalance()
    {
        //arrange
        var customer = new Domain.Entities.Customer(Guid.NewGuid(), "John", "Doe", 100, []);
        _customerRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.FromResult(customer));

        var financialProduct = new Domain.Entities.FinancialProduct(Guid.NewGuid(), "Product", 100, DateTime.Now);
        _financialProductRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                  .Returns(Task.FromResult(financialProduct));


        var purchaseFinancialProductCommand = new PurchaseFinancialProductCommand
        {
            CustomerId = customer.Id,
            FinancialProductId = financialProduct.Id,
            Quantity = 1
        };

        //act
        var purchaseFinancialProductCommandHandler = new PurchaseFinancialProductCommand.
                                                         PurchaseFinancialProductCommandHandler(_customerRepository.Object, _financialProductRepository.Object);

        var result = await purchaseFinancialProductCommandHandler.Handle(purchaseFinancialProductCommand, CancellationToken.None);

        //assert
        result.Should().NotBeNullOrEmpty();
        customer.Balance.Should().Be(0);
        customer.PurchasedProducts.Should().NotBeNullOrEmpty();
        customer.PurchasedProducts.First().FinancialProductID.Should().Be(financialProduct.Id);
    }

    [Fact]
    public async Task GIVEN_Customer_WHEN_ProductSold_THEN_RemoveFromPurchasedProductsAndUpdateBalance()
    {
        //arrange
        var financialProduct = new Domain.Entities.FinancialProduct(Guid.NewGuid(), "Product", 100, DateTime.Now);
        var customer = new Domain.Entities.Customer(Guid.NewGuid(), "John", "Doe", 100, new List<PurchasedProductValueObject>
        {
            new(financialProduct, 1)
        });

        _customerRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.FromResult(customer));

        _financialProductRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                  .Returns(Task.FromResult(financialProduct));

        var sellFinancialProductCommand = new SellFinancialProductCommand
        {
            CustomerId = customer.Id,
            FinancialProductId = financialProduct.Id,
            Quantity = 1
        };

        //act
        var sellFinancialProductCommandHandler = new SellFinancialProductCommand.
                                                     SellFinancialProductCommandHandler(_customerRepository.Object, _financialProductRepository.Object);

        var result = await sellFinancialProductCommandHandler.Handle(sellFinancialProductCommand, CancellationToken.None);

        //assert
        result.Should().NotBeNullOrEmpty();
        customer.Balance.Should().Be(200);
        customer.PurchasedProducts.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task GIVEN_CustomerWithMultipleProducts_WHEN_ProductSold_THEN_RemoveFromPurchasedProductsAndUpdateBalance()
    {
        //arrange
        var financialProduct1 = new Domain.Entities.FinancialProduct(Guid.NewGuid(), "Product1", 100, DateTime.Now);
        var financialProduct2 = new Domain.Entities.FinancialProduct(Guid.NewGuid(), "Product2", 200, DateTime.Now);
        var customer = new Domain.Entities.Customer(Guid.NewGuid(), "John", "Doe", 300, new List<PurchasedProductValueObject>
        {
            new(financialProduct1, 1),
            new(financialProduct2, 1)
        });

        _customerRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.FromResult(customer));

        _financialProductRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                  .Returns(Task.FromResult(financialProduct1));

        var sellFinancialProductCommand = new SellFinancialProductCommand
        {
            CustomerId = customer.Id,
            FinancialProductId = financialProduct1.Id,
            Quantity = 1
        };

        //act
        var sellFinancialProductCommandHandler = new SellFinancialProductCommand.
                                                     SellFinancialProductCommandHandler(_customerRepository.Object, _financialProductRepository.Object);

        var result = await sellFinancialProductCommandHandler.Handle(sellFinancialProductCommand, CancellationToken.None);

        //assert
        result.Should().NotBeNullOrEmpty();
        customer.Balance.Should().Be(400);
        customer.PurchasedProducts.Should().HaveCount(1);
    }
}
