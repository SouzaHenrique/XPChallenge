using MediatR;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;

namespace XPChallenge.Application.Features.Customer.Commands;
public class PurchaseFinancialProductCommand : IRequest<string>
{
    public Guid CustomerId { get; set; }
    public Guid FinancialProductId { get; set; }
    public int Quantity { get; set; }

    public class PurchaseFinancialProductCommandHandler(ICustomerRepository customerRepository, IFinancialProductRepository financialProductRepository)
        : IRequestHandler<PurchaseFinancialProductCommand, string>
    {

        public async Task<string> Handle(PurchaseFinancialProductCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetById(request.CustomerId, cancellationToken);
            var financialProduct = await financialProductRepository.GetById(request.FinancialProductId, cancellationToken);

            customer.PurchaseFinancialProduct(financialProduct, request.Quantity);

            await customerRepository.Update(customer, cancellationToken);

            return "Product purchased successfully";
        }
    }

}
