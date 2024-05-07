using MediatR;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;

namespace XPChallenge.Application.Features.Customer.Commands;
public class SellFinancialProductCommand : IRequest<string>
{
    public Guid CustomerId { get; set; }
    public Guid FinancialProductId { get; set; }
    public int Quantity { get; set; }

    public class SellFinancialProductCommandHandler(ICustomerRepoitory customerRepository, IFinancialProductRepository financialProductRepository) : IRequestHandler<SellFinancialProductCommand, string>
    {

        public async Task<string> Handle(SellFinancialProductCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetById(request.CustomerId, cancellationToken);
            var financialProduct = await financialProductRepository.GetById(request.FinancialProductId, cancellationToken);

            if (customer is null)
            {
                return "Customer not found";
            }

            if (financialProduct is null)
            {
                return "Financial product not found";
            }

            customer.SellFinancialProduct(request.FinancialProductId, request.Quantity, financialProduct.CurrentPurchasePrice);

            await customerRepository.Update(customer, cancellationToken);

            return "Financial product sold successfully";
        }
    }

}
