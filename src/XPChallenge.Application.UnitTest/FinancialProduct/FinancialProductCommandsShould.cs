using XPChallenge.Application.Features.FinancialProduct.Commands;

namespace XPChallenge.Application.UnitTest.FinancialProduct;
public class FinancialProductCommandsShould
{
    private readonly Mock<IFinancialProductRepository> _financialProductRepository;

    public FinancialProductCommandsShould()
    {
        _financialProductRepository = new Mock<IFinancialProductRepository>();
    }

    [Fact]
    public async Task GIVEN_NewProduct_WHEN_CreatingProduct_Then_CreateProduct()
    {
        //arrange
        _financialProductRepository.Setup(x => x.Add(It.IsAny<Domain.Entities.FinancialProduct>(), It.IsAny<CancellationToken>()))
                                   .Returns((Domain.Entities.FinancialProduct product, CancellationToken cancellationToken) =>
                                   {
                                       product = new Domain.Entities.FinancialProduct(Guid.NewGuid(), product.Name, product.CurrentPurchasePrice, product.DueDate);
                                       return Task.FromResult(product);
                                   });

        var createProductCommand = new CreateProductCommand
        {
            Name = "Product",
            CurrentPurchasePrice = 100,
            DueDate = DateTime.Now
        };

        //act
        var createProductCommandHandler = new CreateProductCommand.CreateProductCommandHandler(_financialProductRepository.Object);
        var result = await createProductCommandHandler.Handle(createProductCommand, CancellationToken.None);

        //assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
    }
}
