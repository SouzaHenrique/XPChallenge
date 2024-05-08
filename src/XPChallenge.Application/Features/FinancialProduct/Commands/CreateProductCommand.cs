using MediatR;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;

namespace XPChallenge.Application.Features.FinancialProduct.Commands;
public class CreateProductCommand : IRequest<CreateProductResponse>
{
    public string Name { get; set; }
    public double CurrentPurchasePrice { get; set; }
    public DateTime DueDate { get; set; }

    public Domain.Entities.FinancialProduct MapToEntity()
    {
        return new Domain.Entities.FinancialProduct(Name, CurrentPurchasePrice, DueDate);
    }

    public class CreateProductCommandHandler(IFinancialProductRepository financialProductRepository) : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.MapToEntity();
            product = await financialProductRepository.Add(product, cancellationToken);

            return new CreateProductResponse(product.Id);
        }
    }

}
