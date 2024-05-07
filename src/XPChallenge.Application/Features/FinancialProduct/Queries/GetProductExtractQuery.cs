using MediatR;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;
using XPChallenge.Domain.Commom.Models;

namespace XPChallenge.Application.Features.FinancialProduct.Queries;
public class GetProductExtractQuery : IRequest<ProductExtractValueObject>
{
    public Guid Id { get; set; }


    public class GetProductExtractQueryCommandHandler(IFinancialProductRepository financialProductRepository,
                                                      IFinancialTransactionRepository financialTransactionRepository)
        : IRequestHandler<GetProductExtractQuery, ProductExtractValueObject>
    {
        public async Task<ProductExtractValueObject> Handle(GetProductExtractQuery request, CancellationToken cancellationToken)
        {
            var product = await financialProductRepository.GetById(request.Id, cancellationToken);

            if (product is null)
            {
                return new();
            }

            var extract = financialTransactionRepository.GetProductExtractByProduct(product, cancellationToken);

            return extract;
        }
    }
}
