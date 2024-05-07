using XPChallenge.Application.Features.FinancialProduct.Queries;

namespace XPChallenge.Application.UnitTest.FinancialProduct;
public class FinancialProductQueriesShould
{
    private List<Domain.Entities.FinancialTransaction> _transactionsCollection = new();

    private readonly Mock<IFinancialProductRepository> _financialProductRepository;
    private readonly Mock<IFinancialTransactionRepository> _financialTransactionRepository;

    public FinancialProductQueriesShould()
    {
        _financialProductRepository = new Mock<IFinancialProductRepository>();
        _financialTransactionRepository = new Mock<IFinancialTransactionRepository>();

        _financialTransactionRepository.Setup(x => x.GetProductExtractByProduct(It.IsAny<Domain.Entities.FinancialProduct>(),
                                                                                It.IsAny<CancellationToken>()))
                                       .Returns((Domain.Entities.FinancialProduct financialProduct,
                                                 CancellationToken cancellationToken) =>
                                       {
                                           var productExtract = _transactionsCollection
                                                                .Where(transaction => transaction.FinancialProductID == financialProduct.Id)
                                                                .GroupBy(transaction => new { transaction.FinancialProductID, transaction.TransactionType })
                                                                .Select(group => new
                                                                {
                                                                    TransactionType = group.Key.TransactionType,
                                                                    TotalQuantity = group.Sum(transaction => transaction.Quantity),
                                                                    TotalAmount = group.Sum(transaction => transaction.Quantity * transaction.UnitPrice)
                                                                })
                                                                .ToList(); // Execute query to avoid multiple executions of the same query

                                           var purchases = productExtract.FirstOrDefault(x => x.TransactionType == Domain.Enums.TransactionTypeEnum.Purchase);
                                           var sales = productExtract.FirstOrDefault(x => x.TransactionType == Domain.Enums.TransactionTypeEnum.Sale);

                                           return new ProductExtractValueObject
                                           {
                                               Name = financialProduct.Name,
                                               CurrentPrice = financialProduct.CurrentPurchasePrice,
                                               TotalQuantityPurchased = purchases?.TotalQuantity,
                                               TotalAmountPurchased = purchases?.TotalAmount,
                                               TotalQuantitySold = sales?.TotalQuantity,
                                               TotalAmountSold = sales?.TotalAmount
                                           };
                                       });
    }

    [Fact]
    public async Task GIVEN_Product_WHEN_GettingProductExtract_THEN_GetProductExtract()
    {
        //arrange
        var customerId = new Guid();

        var product = new Domain.Entities.FinancialProduct(Guid.NewGuid(), "Product", purchasePrice: 100, DateTime.Now);

        _financialProductRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                   .Returns(Task.FromResult(product));

        var purchases = Enumerable.Range(0, 10).Select(_ =>
        new Domain.Entities.FinancialTransaction(product.Id,
                                                 Domain.Enums.TransactionTypeEnum.Purchase,
                                                 quantity: 10,
                                                 product.CurrentPurchasePrice,
                                                 DateTime.Now,
                                                 customerId)).ToList();

        var sales = Enumerable.Range(0, 10).Select(_ =>
        new Domain.Entities.FinancialTransaction(product.Id,
                                                 Domain.Enums.TransactionTypeEnum.Sale,
                                                 quantity: 10,
                                                 product.CurrentPurchasePrice,
                                                 DateTime.Now,
                                                 customerId)).ToList();

        _transactionsCollection.AddRange(purchases);
        _transactionsCollection.AddRange(sales);

        var getProductExtractQuery = new GetProductExtractQuery { Id = product.Id };

        //act
        var getProductExtractQueryCommandHandler = new GetProductExtractQuery.
                                                       GetProductExtractQueryCommandHandler(_financialProductRepository.Object,
                                                                                            _financialTransactionRepository.Object);

        var result = await getProductExtractQueryCommandHandler.Handle(getProductExtractQuery, CancellationToken.None);

        //assert
        result.Should().NotBeNull();
        result.Name.Should().Be(product.Name);
        result.TotalQuantityPurchased.Should().Be(purchases.Sum(x => x.Quantity));
        result.TotalAmountPurchased.Should().Be(purchases.Sum(x => x.Quantity * x.UnitPrice));
        result.TotalQuantitySold.Should().Be(sales.Sum(x => x.Quantity));
        result.TotalAmountSold.Should().Be(sales.Sum(x => x.Quantity * x.UnitPrice));

    }

    [Fact]
    public async Task GIVEN_Products_WHEN_GettingAllProducts_THEN_ReturnAllRegisteredProductsPaged()
    {
        //arrange
        var products = Enumerable.Range(0, 100).Select(_ => new Domain.Entities.FinancialProduct(Guid.NewGuid(), "Product", purchasePrice: 100, DateTime.Now)).ToList();

        _financialProductRepository.Setup(x => x.GetPagedReponse(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                                   .Returns((int page, int pageSize, CancellationToken cancellationToken) =>
                                   {
                                       return Task.FromResult(products.Skip((page - 1) * pageSize).Take(pageSize).AsQueryable());
                                   });

        var getAllProductsQuery = new GetAllProductsQuery { Page = 1, PageSize = 20 };

        //act
        var getAllProductsQueryCommandHandler = new GetAllProductsQuery.
                                                    GetAllProductsQueryHandler(_financialProductRepository.Object);

        var result = await getAllProductsQueryCommandHandler.Handle(getAllProductsQuery, CancellationToken.None);

        //assert
        result.Should().NotBeNull();
        result.Count().Should().Be((getAllProductsQuery.Page * getAllProductsQuery.PageSize));
    }
}
