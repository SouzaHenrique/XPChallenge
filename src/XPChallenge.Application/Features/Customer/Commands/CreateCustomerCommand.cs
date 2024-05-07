using MediatR;
using XPChallenge.Application.Contracts.Infrastructure.Persistance;

namespace XPChallenge.Application.Features.Customer.Commands;
public class CreateCustomerCommand : IRequest<CreateCustomerResponse>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public decimal Balance { get; set; }

    public Domain.Entities.Customer MapToEntity()
    {
        return new Domain.Entities.Customer(FirstName, LastName, Balance, []);
    }

    public class CreateCustomerCommandHandler(ICustomerRepository customerRepoitory) : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = request.MapToEntity();

            if (customer.FirstName != string.Empty)
               customer = await customerRepoitory.Add(customer, cancellationToken);

            return new CreateCustomerResponse { Id = customer.Id };
        }
    }
}

