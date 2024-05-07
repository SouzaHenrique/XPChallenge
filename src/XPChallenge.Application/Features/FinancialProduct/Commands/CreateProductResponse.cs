namespace XPChallenge.Application.Features.FinancialProduct.Commands;
public class CreateProductResponse
{
    public Guid Id { get; set; }

    public CreateProductResponse(Guid id)
    {
        Id = id;
    }
}
