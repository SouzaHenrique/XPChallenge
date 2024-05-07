using MediatR;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;

namespace XPChallenge.Application.Features.Customer.Commands;
public class UpdateCustomerBalanceCommand : IRequest<string>
{
    public Guid Id  { get; set; }
    public decimal Amount { get; set; }

    public void MapToEntity(Domain.Entities.Customer customer)
    {
        customer.IncreaseBalanceIn(Amount);
    }

    public class UpdateCustomerBalanceCommandHandler(ICustomerRepository customerRepoitory) : IRequestHandler<UpdateCustomerBalanceCommand, string>
    {
        public async Task<string> Handle(UpdateCustomerBalanceCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepoitory.GetById(request.Id, cancellationToken);

            if (customer != null)
            {
                request.MapToEntity(customer);
                await customerRepoitory.Update(customer, cancellationToken);
            }

            return customer?.Id.ToString() ?? string.Empty;
        }
    }
}
