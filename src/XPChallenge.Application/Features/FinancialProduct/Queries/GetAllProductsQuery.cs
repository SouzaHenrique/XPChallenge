using MediatR;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;

namespace XPChallenge.Application.Features.FinancialProduct.Queries;
public class GetAllProductsQuery : IRequest<IEnumerable<Domain.Entities.FinancialProduct>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public class GetAllProductsQueryHandler(IFinancialProductRepository financialProduct) : IRequestHandler<GetAllProductsQuery, IEnumerable<Domain.Entities.FinancialProduct>>
    {

        public async Task<IEnumerable<Domain.Entities.FinancialProduct>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await financialProduct.GetPagedReponse(request.Page, request.PageSize, cancellationToken);
        }
    }
}
